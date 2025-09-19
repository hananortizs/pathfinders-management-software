using Microsoft.AspNetCore.Mvc;
using Pms.Backend.Application.DTOs.Dashboard;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Application.DTOs.Auth;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Controller para gerenciar dados da dashboard
/// </summary>
[ApiController]
[Route("pms-loc/dashboard")]
public class DashboardController : BaseController
{
    private readonly IDashboardService _dashboardService;
    private readonly IAuthService _authService;
    private readonly ILogger<DashboardController> _logger;

    /// <summary>
    /// Inicializa uma nova instância da classe DashboardController
    /// </summary>
    /// <param name="dashboardService">Serviço de dashboard</param>
    /// <param name="authService">Serviço de autenticação</param>
    /// <param name="logger">Logger para registro de eventos</param>
    public DashboardController(
        IDashboardService dashboardService,
        IAuthService authService,
        ILogger<DashboardController> logger)
    {
        _dashboardService = dashboardService;
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Obtém dados da dashboard para o usuário autenticado
    /// </summary>
    /// <returns>Dados da dashboard</returns>
    [HttpPost("data")]
    public async Task<IActionResult> GetDashboardData([FromBody] TokenRequestDto request)
    {
        try
        {
            _logger.LogInformation("Solicitação de dados da dashboard recebida");

            // Validar token e obter informações do usuário
            var userInfo = _authService.GetUserInfoFromToken(request.Token);

            if (!userInfo.IsSuccess || userInfo.Data == null)
            {
                return BadRequest(new
                {
                    isSuccess = false,
                    message = "Token inválido ou expirado",
                    statusCode = 400
                });
            }

            var user = userInfo.Data;
            _logger.LogInformation("Obtendo dados da dashboard para usuário {UserId} com roles {Roles}",
                user.Id, string.Join(",", user.Roles));

            // Obter dados da dashboard
            var dashboardData = await _dashboardService.GetDashboardDataAsync(
                user.Id,
                user.Roles,
                user.Scopes
            );

            return Ok(new
            {
                isSuccess = true,
                message = "Dados da dashboard obtidos com sucesso",
                statusCode = 200,
                data = dashboardData
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter dados da dashboard");
            return StatusCode(500, new
            {
                isSuccess = false,
                message = "Erro interno do servidor",
                statusCode = 500
            });
        }
    }

    /// <summary>
    /// Obtém estatísticas da dashboard para o usuário autenticado
    /// </summary>
    /// <returns>Estatísticas da dashboard</returns>
    [HttpPost("stats")]
    public async Task<IActionResult> GetDashboardStats([FromBody] TokenRequestDto request)
    {
        try
        {
            _logger.LogInformation("Solicitação de estatísticas da dashboard recebida");

            // Validar token e obter informações do usuário
            var userInfo = _authService.GetUserInfoFromToken(request.Token);

            if (!userInfo.IsSuccess || userInfo.Data == null)
            {
                return BadRequest(new
                {
                    isSuccess = false,
                    message = "Token inválido ou expirado",
                    statusCode = 400
                });
            }

            var user = userInfo.Data;

            // Obter estatísticas da dashboard
            var stats = await _dashboardService.GetDashboardStatsAsync(user.Id, user.Scopes);

            return Ok(new
            {
                isSuccess = true,
                message = "Estatísticas obtidas com sucesso",
                statusCode = 200,
                data = stats
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter estatísticas da dashboard");
            return StatusCode(500, new
            {
                isSuccess = false,
                message = "Erro interno do servidor",
                statusCode = 500
            });
        }
    }

    /// <summary>
    /// Obtém atividades recentes para o usuário autenticado
    /// </summary>
    /// <param name="request">Token do usuário</param>
    /// <param name="limit">Limite de atividades (padrão: 10)</param>
    /// <returns>Lista de atividades recentes</returns>
    [HttpPost("activities")]
    public async Task<IActionResult> GetRecentActivities(
        [FromBody] TokenRequestDto request,
        [FromQuery] int limit = 10)
    {
        try
        {
            _logger.LogInformation("Solicitação de atividades recentes recebida");

            // Validar token e obter informações do usuário
            var userInfo = _authService.GetUserInfoFromToken(request.Token);

            if (!userInfo.IsSuccess || userInfo.Data == null)
            {
                return BadRequest(new
                {
                    isSuccess = false,
                    message = "Token inválido ou expirado",
                    statusCode = 400
                });
            }

            var user = userInfo.Data;

            // Obter atividades recentes
            var activities = await _dashboardService.GetRecentActivitiesAsync(
                user.Id,
                user.Scopes,
                limit
            );

            return Ok(new
            {
                isSuccess = true,
                message = "Atividades recentes obtidas com sucesso",
                statusCode = 200,
                data = activities
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter atividades recentes");
            return StatusCode(500, new
            {
                isSuccess = false,
                message = "Erro interno do servidor",
                statusCode = 500
            });
        }
    }

    /// <summary>
    /// Obtém próximos eventos para o usuário autenticado
    /// </summary>
    /// <param name="request">Token do usuário</param>
    /// <param name="limit">Limite de eventos (padrão: 5)</param>
    /// <returns>Lista de próximos eventos</returns>
    [HttpPost("events")]
    public async Task<IActionResult> GetUpcomingEvents(
        [FromBody] TokenRequestDto request,
        [FromQuery] int limit = 5)
    {
        try
        {
            _logger.LogInformation("Solicitação de próximos eventos recebida");

            // Validar token e obter informações do usuário
            var userInfo = _authService.GetUserInfoFromToken(request.Token);

            if (!userInfo.IsSuccess || userInfo.Data == null)
            {
                return BadRequest(new
                {
                    isSuccess = false,
                    message = "Token inválido ou expirado",
                    statusCode = 400
                });
            }

            var user = userInfo.Data;

            // Obter próximos eventos
            var events = await _dashboardService.GetUpcomingEventsAsync(
                user.Id,
                user.Scopes,
                limit
            );

            return Ok(new
            {
                isSuccess = true,
                message = "Próximos eventos obtidos com sucesso",
                statusCode = 200,
                data = events
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter próximos eventos");
            return StatusCode(500, new
            {
                isSuccess = false,
                message = "Erro interno do servidor",
                statusCode = 500
            });
        }
    }

    /// <summary>
    /// Obtém dados específicos para administradores de sistema
    /// </summary>
    /// <returns>Dados administrativos globais</returns>
    [HttpPost("system-admin")]
    public async Task<IActionResult> GetSystemAdminDashboard([FromBody] TokenRequestDto request)
    {
        try
        {
            _logger.LogInformation("Solicitação de dashboard de administrador de sistema recebida");

            // Validar token e obter informações do usuário
            var userInfo = _authService.GetUserInfoFromToken(request.Token);

            if (!userInfo.IsSuccess || userInfo.Data == null)
            {
                return BadRequest(new
                {
                    isSuccess = false,
                    message = "Token inválido ou expirado",
                    statusCode = 400
                });
            }

            var user = userInfo.Data;

            // Verificar se o usuário é administrador de sistema
            if (!user.Roles.Contains("SystemAdmin"))
            {
                return Forbid("Acesso negado. Apenas administradores de sistema podem acessar esta funcionalidade.");
            }

            // Obter dados da dashboard de administrador de sistema
            var dashboardData = await _dashboardService.GetSystemAdminDashboardAsync();

            return Ok(new
            {
                isSuccess = true,
                message = "Dados de administrador de sistema obtidos com sucesso",
                statusCode = 200,
                data = dashboardData
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter dados de administrador de sistema");
            return StatusCode(500, new
            {
                isSuccess = false,
                message = "Erro interno do servidor",
                statusCode = 500
            });
        }
    }
}
