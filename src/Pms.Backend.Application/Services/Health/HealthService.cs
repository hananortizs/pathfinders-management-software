using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.Interfaces;
using System.Diagnostics;

namespace Pms.Backend.Application.Services.Health;

/// <summary>
/// Serviço para verificações de saúde da API
/// </summary>
public class HealthService : IHealthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<HealthService> _logger;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Inicializa uma nova instância do HealthService
    /// </summary>
    /// <param name="unitOfWork">Unit of Work para verificação do banco</param>
    /// <param name="logger">Logger</param>
    /// <param name="configuration">Configuração da aplicação</param>
    public HealthService(IUnitOfWork unitOfWork, ILogger<HealthService> logger, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Retorna o status básico de saúde da API
    /// </summary>
    /// <returns>Informações básicas de status</returns>
    public BaseResponse<HealthStatusDto> GetBasicHealthStatus()
    {
        try
        {
            var healthStatus = new HealthStatusDto
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Version = "1.0.0",
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown",
                Uptime = TimeSpan.FromMilliseconds(Environment.TickCount64)
            };

            return BaseResponse<HealthStatusDto>.SuccessResult(healthStatus, "API está funcionando normalmente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao verificar status de saúde da API");
            var errorStatus = new HealthStatusDto
            {
                Status = "Unhealthy",
                Timestamp = DateTime.UtcNow,
                Version = "1.0.0",
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown",
                Uptime = TimeSpan.FromMilliseconds(Environment.TickCount64)
            };
            return BaseResponse<HealthStatusDto>.ErrorResult("API com problemas", errorStatus);
        }
    }

    /// <summary>
    /// Retorna informações detalhadas de saúde da API incluindo verificações de serviços
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Status detalhado de saúde</returns>
    public async Task<BaseResponse<DetailedHealthStatusDto>> GetDetailedHealthStatusAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            var healthChecks = await PerformHealthChecksAsync(cancellationToken);
            stopwatch.Stop();

            var detailedStatus = new DetailedHealthStatusDto
            {
                Status = healthChecks.All(h => h.IsHealthy) ? "Healthy" : "Degraded",
                Timestamp = DateTime.UtcNow,
                Version = "1.0.0",
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown",
                Uptime = TimeSpan.FromMilliseconds(Environment.TickCount64),
                ResponseTime = stopwatch.ElapsedMilliseconds,
                Services = healthChecks.ToDictionary(h => h.ServiceName, h => new ServiceHealthDto
                {
                    Status = h.IsHealthy ? "Healthy" : "Unhealthy",
                    ResponseTime = h.ResponseTime,
                    Message = h.Message,
                    LastChecked = h.LastChecked
                }),
                Database = await GetDatabaseStatusAsync(cancellationToken),
                System = GetSystemStatus()
            };

            var isHealthy = healthChecks.All(h => h.IsHealthy);
            return BaseResponse<DetailedHealthStatusDto>.SuccessResult(detailedStatus, "Verificação de saúde concluída");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao verificar status detalhado de saúde da API");
            var errorStatus = new DetailedHealthStatusDto
            {
                Status = "Unhealthy",
                Timestamp = DateTime.UtcNow,
                Version = "1.0.0",
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown",
                Uptime = TimeSpan.FromMilliseconds(Environment.TickCount64),
                ResponseTime = 0,
                Services = new Dictionary<string, ServiceHealthDto>(),
                Database = new DatabaseStatusDto { Status = "Unhealthy", Message = ex.Message },
                System = GetSystemStatus()
            };
            return BaseResponse<DetailedHealthStatusDto>.ErrorResult("Erro na verificação de saúde", errorStatus);
        }
    }

    /// <summary>
    /// Retorna métricas básicas da API
    /// </summary>
    /// <returns>Métricas de performance</returns>
    public BaseResponse<ApiMetricsDto> GetApiMetrics()
    {
        try
        {
            var metrics = new ApiMetricsDto
            {
                Timestamp = DateTime.UtcNow,
                Uptime = TimeSpan.FromMilliseconds(Environment.TickCount64),
                MemoryUsage = GC.GetTotalMemory(false),
                ThreadCount = Process.GetCurrentProcess().Threads.Count,
                ProcessorTime = Process.GetCurrentProcess().TotalProcessorTime,
                WorkingSet = Process.GetCurrentProcess().WorkingSet64,
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown"
            };

            return BaseResponse<ApiMetricsDto>.SuccessResult(metrics, "Métricas obtidas com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter métricas da API");
            return BaseResponse<ApiMetricsDto>.ErrorResult($"Erro ao obter métricas: {ex.Message}");
        }
    }

    #region Private Methods

    /// <summary>
    /// Executa verificações de saúde dos serviços
    /// </summary>
    private async Task<List<HealthCheckResult>> PerformHealthChecksAsync(CancellationToken cancellationToken)
    {
        var healthChecks = new List<HealthCheckResult>();

        // Verificação do banco de dados
        healthChecks.Add(await CheckDatabaseHealthAsync(cancellationToken));

        // Verificação de memória
        healthChecks.Add(CheckMemoryHealth());

        // Verificação de configuração
        healthChecks.Add(CheckConfigurationHealth());

        return healthChecks;
    }

    /// <summary>
    /// Verifica a saúde do banco de dados
    /// </summary>
    private async Task<HealthCheckResult> CheckDatabaseHealthAsync(CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            // Tentar executar uma query simples
            var members = await _unitOfWork.Repository<Domain.Entities.Member>().GetAsync(cancellationToken: cancellationToken);
            var memberCount = members.Count();
            stopwatch.Stop();

            return new HealthCheckResult
            {
                ServiceName = "Database",
                IsHealthy = true,
                ResponseTime = stopwatch.ElapsedMilliseconds,
                Message = $"Banco de dados conectado. {memberCount} membros encontrados.",
                LastChecked = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return new HealthCheckResult
            {
                ServiceName = "Database",
                IsHealthy = false,
                ResponseTime = stopwatch.ElapsedMilliseconds,
                Message = $"Erro na conexão com o banco: {ex.Message}",
                LastChecked = DateTime.UtcNow
            };
        }
    }

    /// <summary>
    /// Verifica a saúde da memória
    /// </summary>
    private HealthCheckResult CheckMemoryHealth()
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var memoryUsage = GC.GetTotalMemory(false);
            var memoryUsageMB = memoryUsage / (1024 * 1024);
            var isHealthy = memoryUsageMB < 500; // Limite de 500MB

            stopwatch.Stop();

            return new HealthCheckResult
            {
                ServiceName = "Memory",
                IsHealthy = isHealthy,
                ResponseTime = stopwatch.ElapsedMilliseconds,
                Message = $"Uso de memória: {memoryUsageMB}MB",
                LastChecked = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return new HealthCheckResult
            {
                ServiceName = "Memory",
                IsHealthy = false,
                ResponseTime = stopwatch.ElapsedMilliseconds,
                Message = $"Erro ao verificar memória: {ex.Message}",
                LastChecked = DateTime.UtcNow
            };
        }
    }

    /// <summary>
    /// Verifica a saúde da configuração
    /// </summary>
    private HealthCheckResult CheckConfigurationHealth()
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var jwtSecret = _configuration["Jwt:Secret"];
            var isHealthy = !string.IsNullOrEmpty(connectionString) && !string.IsNullOrEmpty(jwtSecret);

            stopwatch.Stop();

            return new HealthCheckResult
            {
                ServiceName = "Configuration",
                IsHealthy = isHealthy,
                ResponseTime = stopwatch.ElapsedMilliseconds,
                Message = isHealthy ? "Configurações carregadas corretamente" : "Configurações essenciais não encontradas",
                LastChecked = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return new HealthCheckResult
            {
                ServiceName = "Configuration",
                IsHealthy = false,
                ResponseTime = stopwatch.ElapsedMilliseconds,
                Message = $"Erro ao verificar configuração: {ex.Message}",
                LastChecked = DateTime.UtcNow
            };
        }
    }

    /// <summary>
    /// Obtém o status do banco de dados
    /// </summary>
    private async Task<DatabaseStatusDto> GetDatabaseStatusAsync(CancellationToken cancellationToken)
    {
        try
        {
            var members = await _unitOfWork.Repository<Domain.Entities.Member>().GetAsync(cancellationToken: cancellationToken);
            var clubs = await _unitOfWork.Repository<Domain.Entities.Club>().GetAsync(cancellationToken: cancellationToken);
            var userCredentials = await _unitOfWork.Repository<Domain.Entities.UserCredential>().GetAsync(cancellationToken: cancellationToken);

            var memberCount = members.Count();
            var clubCount = clubs.Count();
            var userCredentialCount = userCredentials.Count();

            return new DatabaseStatusDto
            {
                Status = "Connected",
                Message = "Banco de dados conectado e funcionando",
                MemberCount = memberCount,
                ClubCount = clubCount,
                UserCredentialCount = userCredentialCount,
                LastChecked = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            return new DatabaseStatusDto
            {
                Status = "Disconnected",
                Message = $"Erro na conexão: {ex.Message}",
                MemberCount = 0,
                ClubCount = 0,
                UserCredentialCount = 0,
                LastChecked = DateTime.UtcNow
            };
        }
    }

    /// <summary>
    /// Obtém o status do sistema
    /// </summary>
    private SystemStatusDto GetSystemStatus()
    {
        var process = Process.GetCurrentProcess();
        return new SystemStatusDto
        {
            MachineName = Environment.MachineName,
            ProcessorCount = Environment.ProcessorCount,
            WorkingSet = process.WorkingSet64,
            PrivateMemorySize = process.PrivateMemorySize64,
            VirtualMemorySize = process.VirtualMemorySize64,
            ThreadCount = process.Threads.Count,
            HandleCount = process.HandleCount,
            StartTime = process.StartTime,
            TotalProcessorTime = process.TotalProcessorTime
        };
    }

    #endregion
}
