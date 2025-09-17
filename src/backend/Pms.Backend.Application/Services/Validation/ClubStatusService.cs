using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Application.Interfaces.Validation;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Domain.Entities.Hierarchy;
using Pms.Backend.Domain.Enums;

namespace Pms.Backend.Application.Services.Validation;

/// <summary>
/// Serviço responsável por validar o status de clubes e aplicar bloqueios
/// </summary>
public class ClubStatusService : IClubStatusService
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Inicializa uma nova instância do ClubStatusService
    /// </summary>
    /// <param name="unitOfWork">Unit of work para acesso a dados</param>
    public ClubStatusService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Verifica se um clube está ativo
    /// </summary>
    /// <param name="clubId">ID do clube</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se o clube está ativo, False caso contrário</returns>
    public async Task<bool> IsClubActiveAsync(Guid clubId, CancellationToken cancellationToken = default)
    {
        try
        {
            var club = await _unitOfWork.Repository<Club>()
                .GetFirstOrDefaultAsync(c => c.Id == clubId, cancellationToken);

            return club?.IsActive ?? false;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Valida se uma operação pode ser realizada considerando o status do clube
    /// </summary>
    /// <param name="clubId">ID do clube</param>
    /// <param name="operationType">Tipo de operação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da validação</returns>
    public async Task<BaseResponse<bool>> ValidateClubOperationAsync(Guid clubId, string operationType, CancellationToken cancellationToken = default)
    {
        try
        {
            var isActive = await IsClubActiveAsync(clubId, cancellationToken);

            if (!isActive)
            {
                return BaseResponse<bool>.ErrorResult(
                    $"Operação '{operationType}' não pode ser realizada: o clube está inativo. " +
                    "É necessário nomear um Diretor para reativar o clube.",
                    "ClubInactive"
                );
            }

            return BaseResponse<bool>.SuccessResult(true, "Clube ativo - operação permitida");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.InternalServerErrorResult(
                $"Erro ao validar status do clube: {ex.Message}",
                true
            );
        }
    }

    /// <summary>
    /// Valida se um membro pode participar de eventos considerando o status do clube
    /// </summary>
    /// <param name="memberId">ID do membro</param>
    /// <param name="operationType">Tipo de operação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da validação</returns>
    public async Task<BaseResponse<bool>> ValidateMemberClubOperationAsync(Guid memberId, string operationType, CancellationToken cancellationToken = default)
    {
        try
        {
            // Buscar membership ativo do membro
            var membership = await _unitOfWork.Repository<Domain.Entities.Membership>()
                .GetFirstOrDefaultAsync(m => m.MemberId == memberId && m.IsActive, cancellationToken);

            if (membership == null)
            {
                // Membro sem membership ativo - verificar se é Regional/Distrital/Pastor
                var assignments = await _unitOfWork.Repository<Domain.Entities.Assignment>()
                    .GetAsync(a => a.MemberId == memberId && a.IsActive, cancellationToken);

                var hasLeadershipRole = assignments.Any(a =>
                    a.RoleCatalog.Name.Contains("Regional") ||
                    a.RoleCatalog.Name.Contains("Distrital") ||
                    a.RoleCatalog.Name.Contains("Pastor"));

                if (hasLeadershipRole)
                {
                    // Lideranças sem membership podem realizar operações
                    return BaseResponse<bool>.SuccessResult(true, "Liderança sem membership - operação permitida");
                }

                return BaseResponse<bool>.ErrorResult(
                    $"Operação '{operationType}' não pode ser realizada: membro não possui membership ativo.",
                    "NoActiveMembership"
                );
            }

            // Verificar se o clube do membership está ativo
            var clubValidation = await ValidateClubOperationAsync(membership.ClubId, operationType, cancellationToken);
            return clubValidation;
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.InternalServerErrorResult(
                $"Erro ao validar operação do membro no clube: {ex.Message}",
                true
            );
        }
    }

    /// <summary>
    /// Atualiza o status de ativo do clube baseado na presença de um diretor
    /// </summary>
    /// <param name="clubId">ID do clube</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da operação</returns>
    public async Task<BaseResponse<bool>> UpdateClubActiveStatusAsync(Guid clubId, CancellationToken cancellationToken = default)
    {
        try
        {
            var club = await _unitOfWork.Repository<Club>()
                .GetFirstOrDefaultAsync(c => c.Id == clubId, cancellationToken);

            if (club == null)
            {
                return BaseResponse<bool>.ErrorResult("Clube não encontrado");
            }

            // Verificar se existe um diretor ativo
            var hasActiveDirector = await _unitOfWork.Repository<Domain.Entities.Assignment>()
                .GetFirstOrDefaultAsync(a =>
                    a.ScopeType == ScopeType.Club &&
                    a.ScopeId == clubId &&
                    a.RoleCatalog.Name.Contains("Diretor") &&
                    a.IsActive,
                    cancellationToken) != null;

            var wasActive = club.IsActive;
            club.IsActive = hasActiveDirector;

            if (wasActive != club.IsActive)
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var statusMessage = club.IsActive ? "Clube reativado" : "Clube desativado";
                return BaseResponse<bool>.SuccessResult(true, $"{statusMessage} - Status atualizado baseado na presença de diretor");
            }

            return BaseResponse<bool>.SuccessResult(true, "Status do clube mantido");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.InternalServerErrorResult(
                $"Erro ao atualizar status do clube: {ex.Message}",
                true
            );
        }
    }
}
