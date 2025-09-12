using Microsoft.AspNetCore.Mvc;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.Interfaces;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Health check controller for monitoring API status
/// </summary>
[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    private readonly IHealthService _healthService;

    /// <summary>
    /// Inicializa uma nova instância do HealthController
    /// </summary>
    /// <param name="healthService">Serviço de verificação de saúde</param>
    public HealthController(IHealthService healthService)
    {
        _healthService = healthService;
    }

    /// <summary>
    /// Retorna o status básico de saúde da API
    /// </summary>
    /// <returns>Informações básicas de status</returns>
    [HttpGet]
    [ProducesResponseType(typeof(HealthStatusDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(HealthStatusDto), StatusCodes.Status503ServiceUnavailable)]
    public IActionResult Get()
    {
        var result = _healthService.GetBasicHealthStatus();

        if (!result.IsSuccess)
        {
            return StatusCode(503, result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Retorna informações detalhadas de saúde da API incluindo verificações de serviços
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Status detalhado de saúde</returns>
    [HttpGet("detailed")]
    [ProducesResponseType(typeof(DetailedHealthStatusDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(DetailedHealthStatusDto), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetDetailed(CancellationToken cancellationToken = default)
    {
        var result = await _healthService.GetDetailedHealthStatusAsync(cancellationToken);

        if (!result.IsSuccess)
        {
            return StatusCode(503, result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Retorna métricas básicas da API
    /// </summary>
    /// <returns>Métricas de performance</returns>
    [HttpGet("metrics")]
    [ProducesResponseType(typeof(ApiMetricsDto), StatusCodes.Status200OK)]
    public IActionResult GetMetrics()
    {
        var result = _healthService.GetApiMetrics();

        if (!result.IsSuccess)
        {
            return StatusCode(500, result);
        }

        return Ok(result);
    }
}
