using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Members;

namespace Pms.Backend.Application.Interfaces;

/// <summary>
/// Interface para serviço de validação e ativação de membros
/// </summary>
public interface IMemberActivationService
{
    /// <summary>
    /// Valida se um membro pode ser ativado
    /// </summary>
    /// <param name="memberId">ID do membro</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da validação</returns>
    Task<BaseResponse<MemberActivationValidationDto>> ValidateMemberActivationAsync(
        string memberId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Ativa um membro se todas as validações passarem
    /// </summary>
    /// <param name="memberId">ID do membro</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da ativação</returns>
    Task<BaseResponse<MemberDto>> ActivateMemberAsync(
        string memberId,
        CancellationToken cancellationToken = default);
}
