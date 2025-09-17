using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Validation;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Application.Interfaces.Validation;

/// <summary>
/// Interface para o serviço de validação de gating por lenço
/// </summary>
public interface IScarfGatingService
{
    /// <summary>
    /// Valida se o membro pode realizar operações de progresso baseado no lenço
    /// </summary>
    /// <param name="member">Membro a ser validado</param>
    /// <param name="operationType">Tipo de operação (classe, especialidade, mestrado, etc.)</param>
    /// <returns>Resultado da validação</returns>
    BaseResponse<bool> ValidateScarfRequirement(Member member, string operationType);

    /// <summary>
    /// Valida se o membro pode assumir cargos de liderança baseado no lenço
    /// </summary>
    /// <param name="member">Membro a ser validado</param>
    /// <param name="roleName">Nome do cargo de liderança</param>
    /// <returns>Resultado da validação</returns>
    BaseResponse<bool> ValidateLeadershipScarfRequirement(Member member, string roleName);

    /// <summary>
    /// Verifica se o membro possui lenço investido
    /// </summary>
    /// <param name="member">Membro a ser verificado</param>
    /// <returns>True se possui lenço, False caso contrário</returns>
    bool HasScarfInvestiture(Member member);

    /// <summary>
    /// Obtém informações sobre a investidura de lenço do membro
    /// </summary>
    /// <param name="member">Membro a ser consultado</param>
    /// <returns>Informações da investidura ou null se não possuir</returns>
    ScarfInvestitureInfo? GetScarfInvestitureInfo(Member member);
}
