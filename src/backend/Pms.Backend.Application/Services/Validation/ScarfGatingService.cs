using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Validation;
using Pms.Backend.Application.Interfaces.Validation;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Application.Services.Validation;

/// <summary>
/// Serviço responsável por validar o gating por lenço
/// Bloqueia operações de progresso quando o membro não possui lenço investido
/// </summary>
public class ScarfGatingService : IScarfGatingService
{
    /// <summary>
    /// Valida se o membro pode realizar operações de progresso baseado no lenço
    /// </summary>
    /// <param name="member">Membro a ser validado</param>
    /// <param name="operationType">Tipo de operação (classe, especialidade, mestrado, etc.)</param>
    /// <returns>Resultado da validação</returns>
    public BaseResponse<bool> ValidateScarfRequirement(Member member, string operationType)
    {
        try
        {
            // Verificar se o membro possui lenço investido
            if (!member.ScarfInvested)
            {
                return BaseResponse<bool>.ErrorResult(
                    $"Operação '{operationType}' requer investidura de lenço. " +
                    "O membro deve ter recebido o lenço antes de iniciar progressos.",
                    "ScarfRequired"
                );
            }

            return BaseResponse<bool>.SuccessResult(true, "Validação de lenço aprovada");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.InternalServerErrorResult(
                $"Erro ao validar requisito de lenço: {ex.Message}",
                true
            );
        }
    }

    /// <summary>
    /// Valida se o membro pode assumir cargos de liderança baseado no lenço
    /// </summary>
    /// <param name="member">Membro a ser validado</param>
    /// <param name="roleName">Nome do cargo de liderança</param>
    /// <returns>Resultado da validação</returns>
    public BaseResponse<bool> ValidateLeadershipScarfRequirement(Member member, string roleName)
    {
        try
        {
            // Verificar se o membro possui lenço investido
            if (!member.ScarfInvested)
            {
                return BaseResponse<bool>.ErrorResult(
                    $"Cargo de liderança '{roleName}' requer investidura de lenço. " +
                    "O membro deve ter recebido o lenço antes de assumir cargos de liderança.",
                    "ScarfRequired"
                );
            }

            return BaseResponse<bool>.SuccessResult(true, "Validação de lenço para liderança aprovada");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.InternalServerErrorResult(
                $"Erro ao validar requisito de lenço para liderança: {ex.Message}",
                true
            );
        }
    }

    /// <summary>
    /// Verifica se o membro possui lenço investido
    /// </summary>
    /// <param name="member">Membro a ser verificado</param>
    /// <returns>True se possui lenço, False caso contrário</returns>
    public bool HasScarfInvestiture(Member member)
    {
        return member.ScarfInvested;
    }

    /// <summary>
    /// Obtém informações sobre a investidura de lenço do membro
    /// </summary>
    /// <param name="member">Membro a ser consultado</param>
    /// <returns>Informações da investidura ou null se não possuir</returns>
    public ScarfInvestitureInfo? GetScarfInvestitureInfo(Member member)
    {
        if (!member.ScarfInvested)
            return null;

        return new ScarfInvestitureInfo
        {
            IsInvested = true,
            InvestedAt = member.ScarfInvestedAt,
            Church = member.ScarfChurch,
            Pastor = member.ScarfPastor,
            Date = member.ScarfDate
        };
    }
}

