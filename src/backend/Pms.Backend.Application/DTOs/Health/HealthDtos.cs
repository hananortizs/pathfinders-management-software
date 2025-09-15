namespace Pms.Backend.Application.DTOs;

/// <summary>
/// DTO para status básico de saúde
/// </summary>
public class HealthStatusDto
{
    /// <summary>
    /// Status atual da API (Healthy/Unhealthy)
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// Timestamp da verificação
    /// </summary>
    public DateTime Timestamp { get; set; }
    
    /// <summary>
    /// Versão da API
    /// </summary>
    public string Version { get; set; } = string.Empty;
    
    /// <summary>
    /// Ambiente de execução
    /// </summary>
    public string Environment { get; set; } = string.Empty;
    
    /// <summary>
    /// Tempo de atividade da API
    /// </summary>
    public TimeSpan Uptime { get; set; }
}

/// <summary>
/// DTO para status detalhado de saúde
/// </summary>
public class DetailedHealthStatusDto
{
    /// <summary>
    /// Status geral da API (Healthy/Degraded/Unhealthy)
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// Timestamp da verificação
    /// </summary>
    public DateTime Timestamp { get; set; }
    
    /// <summary>
    /// Versão da API
    /// </summary>
    public string Version { get; set; } = string.Empty;
    
    /// <summary>
    /// Ambiente de execução
    /// </summary>
    public string Environment { get; set; } = string.Empty;
    
    /// <summary>
    /// Tempo de atividade da API
    /// </summary>
    public TimeSpan Uptime { get; set; }
    
    /// <summary>
    /// Tempo de resposta da verificação em milissegundos
    /// </summary>
    public long ResponseTime { get; set; }
    
    /// <summary>
    /// Status dos serviços verificados
    /// </summary>
    public Dictionary<string, ServiceHealthDto> Services { get; set; } = new();
    
    /// <summary>
    /// Status do banco de dados
    /// </summary>
    public DatabaseStatusDto Database { get; set; } = new();
    
    /// <summary>
    /// Status do sistema
    /// </summary>
    public SystemStatusDto System { get; set; } = new();
}

/// <summary>
/// DTO para status de serviço
/// </summary>
public class ServiceHealthDto
{
    /// <summary>
    /// Status do serviço (Healthy/Unhealthy)
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// Tempo de resposta do serviço em milissegundos
    /// </summary>
    public long ResponseTime { get; set; }
    
    /// <summary>
    /// Mensagem de status do serviço
    /// </summary>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// Data e hora da última verificação
    /// </summary>
    public DateTime LastChecked { get; set; }
}

/// <summary>
/// DTO para status do banco de dados
/// </summary>
public class DatabaseStatusDto
{
    /// <summary>
    /// Status da conexão (Connected/Disconnected)
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// Mensagem de status da conexão
    /// </summary>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// Número de membros no banco
    /// </summary>
    public int MemberCount { get; set; }
    
    /// <summary>
    /// Número de clubes no banco
    /// </summary>
    public int ClubCount { get; set; }
    
    /// <summary>
    /// Número de credenciais de usuário no banco
    /// </summary>
    public int UserCredentialCount { get; set; }
    
    /// <summary>
    /// Data e hora da última verificação
    /// </summary>
    public DateTime LastChecked { get; set; }
}

/// <summary>
/// DTO para status do sistema
/// </summary>
public class SystemStatusDto
{
    /// <summary>
    /// Nome da máquina
    /// </summary>
    public string MachineName { get; set; } = string.Empty;
    
    /// <summary>
    /// Número de processadores
    /// </summary>
    public int ProcessorCount { get; set; }
    
    /// <summary>
    /// Memória de trabalho em bytes
    /// </summary>
    public long WorkingSet { get; set; }
    
    /// <summary>
    /// Tamanho da memória privada em bytes
    /// </summary>
    public long PrivateMemorySize { get; set; }
    
    /// <summary>
    /// Tamanho da memória virtual em bytes
    /// </summary>
    public long VirtualMemorySize { get; set; }
    
    /// <summary>
    /// Número de threads
    /// </summary>
    public int ThreadCount { get; set; }
    
    /// <summary>
    /// Número de handles
    /// </summary>
    public int HandleCount { get; set; }
    
    /// <summary>
    /// Horário de início do processo
    /// </summary>
    public DateTime StartTime { get; set; }
    
    /// <summary>
    /// Tempo total de processador utilizado
    /// </summary>
    public TimeSpan TotalProcessorTime { get; set; }
}

/// <summary>
/// DTO para métricas da API
/// </summary>
public class ApiMetricsDto
{
    /// <summary>
    /// Timestamp da coleta de métricas
    /// </summary>
    public DateTime Timestamp { get; set; }
    
    /// <summary>
    /// Tempo de atividade da API
    /// </summary>
    public TimeSpan Uptime { get; set; }
    
    /// <summary>
    /// Uso de memória em bytes
    /// </summary>
    public long MemoryUsage { get; set; }
    
    /// <summary>
    /// Número de threads
    /// </summary>
    public int ThreadCount { get; set; }
    
    /// <summary>
    /// Tempo de processador utilizado
    /// </summary>
    public TimeSpan ProcessorTime { get; set; }
    
    /// <summary>
    /// Memória de trabalho em bytes
    /// </summary>
    public long WorkingSet { get; set; }
    
    /// <summary>
    /// Ambiente de execução
    /// </summary>
    public string Environment { get; set; } = string.Empty;
}

/// <summary>
/// Resultado de verificação de saúde
/// </summary>
public class HealthCheckResult
{
    /// <summary>
    /// Nome do serviço verificado
    /// </summary>
    public string ServiceName { get; set; } = string.Empty;
    
    /// <summary>
    /// Indica se o serviço está saudável
    /// </summary>
    public bool IsHealthy { get; set; }
    
    /// <summary>
    /// Tempo de resposta da verificação em milissegundos
    /// </summary>
    public long ResponseTime { get; set; }
    
    /// <summary>
    /// Mensagem de resultado da verificação
    /// </summary>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// Data e hora da verificação
    /// </summary>
    public DateTime LastChecked { get; set; }
}
