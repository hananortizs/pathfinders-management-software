using Pms.Backend.Application.DTOs;

namespace Pms.Backend.Application.Interfaces;

/// <summary>
/// Interface para serviços de relatórios
/// </summary>
public interface IReportsService
{
    /// <summary>
    /// Gera relatório de membros por clube com estatísticas básicas
    /// </summary>
    /// <param name="clubId">ID do clube</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Relatório de membros do clube</returns>
    Task<BaseResponse<ClubMembersReportDto>> GetClubMembersReportAsync(Guid clubId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gera relatório de capacidade das unidades de um clube
    /// </summary>
    /// <param name="clubId">ID do clube</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Relatório de capacidade das unidades</returns>
    Task<BaseResponse<ClubCapacityReportDto>> GetClubCapacityReportAsync(Guid clubId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gera relatório de membros por faixa etária
    /// </summary>
    /// <param name="clubId">ID do clube</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Relatório de membros por faixa etária</returns>
    Task<BaseResponse<AgeGroupReportDto>> GetAgeGroupReportAsync(Guid clubId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gera relatório de membros ativos/inativos
    /// </summary>
    /// <param name="clubId">ID do clube</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Relatório de status dos membros</returns>
    Task<BaseResponse<MemberStatusReportDto>> GetMemberStatusReportAsync(Guid clubId, CancellationToken cancellationToken = default);
}
