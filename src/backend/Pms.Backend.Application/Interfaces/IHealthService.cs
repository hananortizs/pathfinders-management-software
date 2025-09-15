using Pms.Backend.Application.DTOs;

namespace Pms.Backend.Application.Interfaces;

/// <summary>
/// Interface para serviços de verificação de saúde
/// </summary>
public interface IHealthService
{
    /// <summary>
    /// Retorna o status básico de saúde da API
    /// </summary>
    /// <returns>Informações básicas de status</returns>
    BaseResponse<HealthStatusDto> GetBasicHealthStatus();

    /// <summary>
    /// Retorna informações detalhadas de saúde da API incluindo verificações de serviços
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Status detalhado de saúde</returns>
    Task<BaseResponse<DetailedHealthStatusDto>> GetDetailedHealthStatusAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Retorna métricas básicas da API
    /// </summary>
    /// <returns>Métricas de performance</returns>
    BaseResponse<ApiMetricsDto> GetApiMetrics();
}
