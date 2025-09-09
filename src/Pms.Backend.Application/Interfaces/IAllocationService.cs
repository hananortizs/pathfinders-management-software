using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Membership;

namespace Pms.Backend.Application.Interfaces;

/// <summary>
/// Interface para serviços de alocação de membros em unidades
/// </summary>
public interface IAllocationService
{
    /// <summary>
    /// Aloca automaticamente um membro em uma unidade baseado nas regras de negócio
    /// </summary>
    /// <param name="membershipId">ID da membership</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da alocação com opções se houver múltiplas</returns>
    Task<BaseResponse<AllocationResultDto>> AllocateMemberAsync(Guid membershipId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Aloca um membro em uma unidade específica
    /// </summary>
    /// <param name="membershipId">ID da membership</param>
    /// <param name="unitId">ID da unidade</param>
    /// <param name="reason">Motivo da alocação manual (opcional)</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da alocação</returns>
    Task<BaseResponse<MembershipDto>> AllocateToSpecificUnitAsync(Guid membershipId, Guid unitId, string? reason = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Realoca um membro para uma nova unidade
    /// </summary>
    /// <param name="membershipId">ID da membership</param>
    /// <param name="newUnitId">ID da nova unidade</param>
    /// <param name="reason">Motivo da realocação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da realocação</returns>
    Task<BaseResponse<MembershipDto>> ReallocateMemberAsync(Guid membershipId, Guid newUnitId, string? reason = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se um membro precisa ser realocado por aniversário
    /// </summary>
    /// <param name="membershipId">ID da membership</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da verificação</returns>
    Task<BaseResponse<ReallocationCheckResultDto>> CheckBirthdayReallocationAsync(Guid membershipId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém as unidades compatíveis para um membro
    /// </summary>
    /// <param name="memberId">ID do membro</param>
    /// <param name="clubId">ID do clube</param>
    /// <param name="referenceDate">Data de referência para cálculo de idade (padrão: 1º de junho do ano atual)</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de unidades compatíveis</returns>
    Task<BaseResponse<List<CompatibleUnitDto>>> GetCompatibleUnitsAsync(Guid memberId, Guid clubId, DateTime? referenceDate = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Valida se uma unidade tem capacidade disponível
    /// </summary>
    /// <param name="unitId">ID da unidade</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se tem capacidade, false caso contrário</returns>
    Task<bool> HasAvailableCapacityAsync(Guid unitId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém o número atual de membros em uma unidade
    /// </summary>
    /// <param name="unitId">ID da unidade</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Número de membros ativos</returns>
    Task<int> GetCurrentMemberCountAsync(Guid unitId, CancellationToken cancellationToken = default);
}
