using Pms.Backend.Application.DTOs;

namespace Pms.Backend.Application.Interfaces.Validation;

/// <summary>
/// Interface para o serviço de validação de status de clubes
/// </summary>
public interface IClubStatusService
{
    /// <summary>
    /// Verifica se um clube está ativo
    /// </summary>
    /// <param name="clubId">ID do clube</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se o clube está ativo, False caso contrário</returns>
    Task<bool> IsClubActiveAsync(Guid clubId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Valida se uma operação pode ser realizada considerando o status do clube
    /// </summary>
    /// <param name="clubId">ID do clube</param>
    /// <param name="operationType">Tipo de operação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da validação</returns>
    Task<BaseResponse<bool>> ValidateClubOperationAsync(Guid clubId, string operationType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Valida se um membro pode participar de eventos considerando o status do clube
    /// </summary>
    /// <param name="memberId">ID do membro</param>
    /// <param name="operationType">Tipo de operação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da validação</returns>
    Task<BaseResponse<bool>> ValidateMemberClubOperationAsync(Guid memberId, string operationType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza o status de ativo do clube baseado na presença de um diretor
    /// </summary>
    /// <param name="clubId">ID do clube</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da operação</returns>
    Task<BaseResponse<bool>> UpdateClubActiveStatusAsync(Guid clubId, CancellationToken cancellationToken = default);
}
