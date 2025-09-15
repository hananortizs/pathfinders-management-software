using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pms.Backend.Application.DTOs.Assignments;
using Pms.Backend.Application.Helpers;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Domain.Exceptions;

namespace Pms.Backend.Application.Services.Assignments;

/// <summary>
/// Serviço para gerenciar delegações de aprovação
/// Gerencia a delegação de autoridade de aprovação quando os aprovadores originais não estão disponíveis
/// </summary>
public class ApprovalDelegateService : IApprovalDelegateService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Inicializa uma nova instância do ApprovalDelegateService
    /// </summary>
    /// <param name="unitOfWork">Unidade de trabalho</param>
    /// <param name="mapper">Instância do AutoMapper</param>
    public ApprovalDelegateService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    #region Operações CRUD de Delegação

    /// <summary>
    /// Cria uma nova delegação de aprovação
    /// </summary>
    /// <param name="request">Solicitação de criação da delegação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>A delegação criada</returns>
    public async Task<ApprovalDelegateDto> CreateDelegationAsync(CreateApprovalDelegateDto request, CancellationToken cancellationToken = default)
    {
        // Validar delegação
        await ValidateDelegationAsync(request, cancellationToken);

        // Verificar delegações sobrepostas
        var overlappingDelegation = await _unitOfWork.Repository<ApprovalDelegate>().GetFirstOrDefaultAsync(
            d => d.Role == request.Role &&
                 d.ScopeType == request.ScopeType &&
                 d.ScopeId == request.ScopeId &&
                 d.DelegatedFromAssignmentId == request.DelegatedFromAssignmentId &&
                 d.IsActive &&
                 ((d.StartDate <= request.StartDate && d.EndDate >= request.StartDate) ||
                  (d.StartDate <= request.EndDate && d.EndDate >= request.EndDate) ||
                  (d.StartDate >= request.StartDate && d.EndDate <= request.EndDate)),
            cancellationToken);

        if (overlappingDelegation != null)
        {
            ExceptionHelper.ThrowBusinessRuleException(
                "DELEGATION_OVERLAP",
                "Já existe uma delegação ativa para esta função e escopo no período especificado",
                new Dictionary<string, object>
                {
                    ["Role"] = request.Role,
                    ["ScopeType"] = request.ScopeType,
                    ["ScopeId"] = request.ScopeId,
                    ["OverlappingDelegationId"] = overlappingDelegation.Id
                });
        }

        // Criar delegação
        var delegation = _mapper.Map<ApprovalDelegate>(request);
        delegation.Id = Guid.NewGuid();
        delegation.CreatedAtUtc = DateTime.UtcNow;
        delegation.UpdatedAtUtc = DateTime.UtcNow;

        await _unitOfWork.Repository<ApprovalDelegate>().AddAsync(delegation, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Obter a delegação criada com propriedades de navegação
        var createdDelegation = await _unitOfWork.Repository<ApprovalDelegate>()
            .GetQueryable(d => d.Id == delegation.Id)
            .Include(d => d.DelegatedToAssignment)
                .ThenInclude(a => a.Member)
            .Include(d => d.DelegatedToAssignment)
                .ThenInclude(a => a.RoleCatalog)
            .Include(d => d.DelegatedFromAssignment)
                .ThenInclude(a => a.Member)
            .Include(d => d.DelegatedFromAssignment)
                .ThenInclude(a => a.RoleCatalog)
            .FirstOrDefaultAsync(cancellationToken);

        return _mapper.Map<ApprovalDelegateDto>(createdDelegation);
    }

    /// <summary>
    /// Obtém uma delegação por ID
    /// </summary>
    /// <param name="id">ID da delegação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>A delegação</returns>
    public async Task<ApprovalDelegateDto> GetDelegationAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var delegation = await _unitOfWork.Repository<ApprovalDelegate>()
            .GetQueryable(d => d.Id == id)
            .Include(d => d.DelegatedToAssignment)
                .ThenInclude(a => a.Member)
            .Include(d => d.DelegatedToAssignment)
                .ThenInclude(a => a.RoleCatalog)
            .Include(d => d.DelegatedFromAssignment)
                .ThenInclude(a => a.Member)
            .Include(d => d.DelegatedFromAssignment)
                .ThenInclude(a => a.RoleCatalog)
            .FirstOrDefaultAsync(cancellationToken);

        if (delegation == null)
        {
            ExceptionHelper.ThrowNotFoundException<ApprovalDelegate>(id, "Delegação não encontrada");
        }

        return _mapper.Map<ApprovalDelegateDto>(delegation);
    }

    /// <summary>
    /// Obtém todas as delegações para um escopo específico
    /// </summary>
    /// <param name="scopeType">Tipo do escopo</param>
    /// <param name="scopeId">ID do escopo</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de delegações</returns>
    public async Task<IEnumerable<ApprovalDelegateDto>> GetDelegationsByScopeAsync(ScopeType scopeType, Guid scopeId, CancellationToken cancellationToken = default)
    {
        var delegations = await _unitOfWork.Repository<ApprovalDelegate>()
            .GetQueryable(d => d.ScopeType == scopeType && d.ScopeId == scopeId)
            .Include(d => d.DelegatedToAssignment)
                .ThenInclude(a => a.Member)
            .Include(d => d.DelegatedToAssignment)
                .ThenInclude(a => a.RoleCatalog)
            .Include(d => d.DelegatedFromAssignment)
                .ThenInclude(a => a.Member)
            .Include(d => d.DelegatedFromAssignment)
                .ThenInclude(a => a.RoleCatalog)
            .OrderBy(d => d.StartDate)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<ApprovalDelegateDto>>(delegations);
    }

    /// <summary>
    /// Obtém todas as delegações ativas para um escopo específico
    /// </summary>
    /// <param name="scopeType">Tipo do escopo</param>
    /// <param name="scopeId">ID do escopo</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de delegações ativas</returns>
    public async Task<IEnumerable<ApprovalDelegateDto>> GetActiveDelegationsByScopeAsync(ScopeType scopeType, Guid scopeId, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var delegations = await _unitOfWork.Repository<ApprovalDelegate>()
            .GetQueryable(d => d.ScopeType == scopeType &&
                              d.ScopeId == scopeId &&
                              d.IsActive &&
                              d.StartDate <= now &&
                              d.EndDate >= now)
            .Include(d => d.DelegatedToAssignment)
                .ThenInclude(a => a.Member)
            .Include(d => d.DelegatedToAssignment)
                .ThenInclude(a => a.RoleCatalog)
            .Include(d => d.DelegatedFromAssignment)
                .ThenInclude(a => a.Member)
            .Include(d => d.DelegatedFromAssignment)
                .ThenInclude(a => a.RoleCatalog)
            .OrderBy(d => d.StartDate)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<ApprovalDelegateDto>>(delegations);
    }

    /// <summary>
    /// Obtém todas as delegações onde um membro é o delegado
    /// </summary>
    /// <param name="memberId">ID do membro</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de delegações</returns>
    public async Task<IEnumerable<ApprovalDelegateDto>> GetDelegationsByDelegateAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        var delegations = await _unitOfWork.Repository<ApprovalDelegate>()
            .GetQueryable(d => d.DelegatedToAssignment.MemberId == memberId)
            .Include(d => d.DelegatedToAssignment)
                .ThenInclude(a => a.Member)
            .Include(d => d.DelegatedToAssignment)
                .ThenInclude(a => a.RoleCatalog)
            .Include(d => d.DelegatedFromAssignment)
                .ThenInclude(a => a.Member)
            .Include(d => d.DelegatedFromAssignment)
                .ThenInclude(a => a.RoleCatalog)
            .OrderBy(d => d.StartDate)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<ApprovalDelegateDto>>(delegations);
    }

    /// <summary>
    /// Obtém todas as delegações onde um membro é o delegante
    /// </summary>
    /// <param name="memberId">ID do membro</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de delegações</returns>
    public async Task<IEnumerable<ApprovalDelegateDto>> GetDelegationsByDelegatorAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        var delegations = await _unitOfWork.Repository<ApprovalDelegate>()
            .GetQueryable(d => d.DelegatedFromAssignment.MemberId == memberId)
            .Include(d => d.DelegatedToAssignment)
                .ThenInclude(a => a.Member)
            .Include(d => d.DelegatedToAssignment)
                .ThenInclude(a => a.RoleCatalog)
            .Include(d => d.DelegatedFromAssignment)
                .ThenInclude(a => a.Member)
            .Include(d => d.DelegatedFromAssignment)
                .ThenInclude(a => a.RoleCatalog)
            .OrderBy(d => d.StartDate)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<ApprovalDelegateDto>>(delegations);
    }

    /// <summary>
    /// Atualiza uma delegação
    /// </summary>
    /// <param name="id">ID da delegação</param>
    /// <param name="request">Solicitação de atualização</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>A delegação atualizada</returns>
    public async Task<ApprovalDelegateDto> UpdateDelegationAsync(Guid id, UpdateApprovalDelegateDto request, CancellationToken cancellationToken = default)
    {
        var delegation = await _unitOfWork.Repository<ApprovalDelegate>()
            .GetFirstOrDefaultAsync(d => d.Id == id, cancellationToken);

        if (delegation == null)
        {
            ExceptionHelper.ThrowNotFoundException<ApprovalDelegate>(id, "Delegação não encontrada");
        }

        // Verificar se a delegação pode ser atualizada
        if (delegation?.IsActive == true && delegation.EndDate < DateTime.UtcNow)
        {
            ExceptionHelper.ThrowBusinessRuleException(
                "DELEGATION_EXPIRED",
                "Não é possível atualizar uma delegação expirada",
                new Dictionary<string, object>
                {
                    ["DelegationId"] = id,
                    ["EndDate"] = delegation.EndDate
                });
        }

        // Atualizar propriedades
        _mapper.Map(request, delegation);
        delegation!.UpdatedAtUtc = DateTime.UtcNow;

        await _unitOfWork.Repository<ApprovalDelegate>().UpdateAsync(delegation, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Obter a delegação atualizada com propriedades de navegação
        var updatedDelegation = await _unitOfWork.Repository<ApprovalDelegate>()
            .GetQueryable(d => d.Id == id)
            .Include(d => d.DelegatedToAssignment)
                .ThenInclude(a => a.Member)
            .Include(d => d.DelegatedToAssignment)
                .ThenInclude(a => a.RoleCatalog)
            .Include(d => d.DelegatedFromAssignment)
                .ThenInclude(a => a.Member)
            .Include(d => d.DelegatedFromAssignment)
                .ThenInclude(a => a.RoleCatalog)
            .FirstOrDefaultAsync(cancellationToken);

        return _mapper.Map<ApprovalDelegateDto>(updatedDelegation);
    }

    /// <summary>
    /// Finaliza uma delegação
    /// </summary>
    /// <param name="id">ID da delegação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado do sucesso</returns>
    public async Task<bool> EndDelegationAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var delegation = await _unitOfWork.Repository<ApprovalDelegate>()
            .GetFirstOrDefaultAsync(d => d.Id == id, cancellationToken);

        if (delegation == null)
        {
            ExceptionHelper.ThrowNotFoundException<ApprovalDelegate>(id, "Delegação não encontrada");
        }

        if (delegation?.IsActive != true)
        {
            ExceptionHelper.ThrowBusinessRuleException(
                "DELEGATION_ALREADY_INACTIVE",
                "A delegação já está inativa",
                new Dictionary<string, object>
                {
                    ["DelegationId"] = id,
                    ["IsActive"] = delegation!.IsActive
                });
        }

        delegation!.EndedAtUtc = DateTime.UtcNow;
        delegation!.UpdatedAtUtc = DateTime.UtcNow;

        await _unitOfWork.Repository<ApprovalDelegate>().UpdateAsync(delegation, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    /// <summary>
    /// Exclui uma delegação
    /// </summary>
    /// <param name="id">ID da delegação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado do sucesso</returns>
    public async Task<bool> DeleteDelegationAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var delegation = await _unitOfWork.Repository<ApprovalDelegate>()
            .GetFirstOrDefaultAsync(d => d.Id == id, cancellationToken);

        if (delegation == null)
        {
            ExceptionHelper.ThrowNotFoundException<ApprovalDelegate>(id, "Delegação não encontrada");
        }

        // Verificar se a delegação pode ser excluída
        if (delegation?.IsActive == true && delegation.StartDate <= DateTime.UtcNow)
        {
            ExceptionHelper.ThrowBusinessRuleException(
                "DELEGATION_ACTIVE",
                "Não é possível excluir uma delegação ativa",
                new Dictionary<string, object>
                {
                    ["DelegationId"] = id,
                    ["IsActive"] = delegation.IsActive,
                    ["StartDate"] = delegation.StartDate
                });
        }

        // Soft delete
        delegation!.IsDeleted = true;
        delegation!.DeletedAtUtc = DateTime.UtcNow;
        delegation!.UpdatedAtUtc = DateTime.UtcNow;

        await _unitOfWork.Repository<ApprovalDelegate>().UpdateAsync(delegation, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    #endregion

    #region Validação e Aprovação Efetiva

    /// <summary>
    /// Valida se uma delegação pode ser criada
    /// </summary>
    /// <param name="request">Solicitação da delegação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da validação</returns>
    public async Task<bool> ValidateDelegationAsync(CreateApprovalDelegateDto request, CancellationToken cancellationToken = default)
    {
        // Verificar se o delegante existe e tem a função
        var delegatorAssignment = await _unitOfWork.Repository<Assignment>()
            .GetFirstOrDefaultAsync(a => a.Id == request.DelegatedFromAssignmentId, cancellationToken);

        if (delegatorAssignment == null)
        {
            ExceptionHelper.ThrowNotFoundException<Assignment>(request.DelegatedFromAssignmentId, "Atribuição do delegante não encontrada");
        }

        // Verificar se o delegado existe e pode receber a delegação
        var delegateAssignment = await _unitOfWork.Repository<Assignment>()
            .GetFirstOrDefaultAsync(a => a.Id == request.DelegatedToAssignmentId, cancellationToken);

        if (delegateAssignment == null)
        {
            ExceptionHelper.ThrowNotFoundException<Assignment>(request.DelegatedToAssignmentId, "Atribuição do delegado não encontrada");
        }

        // Verificar se o delegado não é o mesmo que o delegante
        if (request.DelegatedFromAssignmentId == request.DelegatedToAssignmentId)
        {
            ExceptionHelper.ThrowValidationException("DelegatedToAssignmentId", "O delegado não pode ser o mesmo que o delegante");
        }

        // Verificar se o delegante tem a função no escopo
        if (delegatorAssignment?.Role != request.Role ||
            delegatorAssignment?.ScopeType != request.ScopeType ||
            delegatorAssignment?.ScopeId != request.ScopeId)
        {
            ExceptionHelper.ThrowBusinessRuleException(
                "INVALID_DELEGATOR_ROLE",
                "O delegante não possui a função especificada no escopo",
                new Dictionary<string, object>
                {
                    ["DelegatorRole"] = delegatorAssignment!.Role,
                    ["RequestedRole"] = request.Role,
                    ["DelegatorScopeType"] = delegatorAssignment!.ScopeType,
                    ["RequestedScopeType"] = request.ScopeType
                });
        }

        // Verificar se o delegado está ativo
        if (delegateAssignment?.IsActive != true)
        {
            ExceptionHelper.ThrowBusinessRuleException(
                "DELEGATE_INACTIVE",
                "O delegado deve estar ativo para receber a delegação",
                new Dictionary<string, object>
                {
                    ["DelegateAssignmentId"] = request.DelegatedToAssignmentId,
                    ["IsActive"] = delegateAssignment!.IsActive
                });
        }

        return true;
    }

    /// <summary>
    /// Obtém o aprovador efetivo para uma função em um escopo específico
    /// </summary>
    /// <param name="role">Nome da função</param>
    /// <param name="scopeType">Tipo do escopo</param>
    /// <param name="scopeId">ID do escopo</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>A atribuição efetiva</returns>
    public async Task<AssignmentDto> GetEffectiveApproverAsync(string role, ScopeType scopeType, Guid scopeId, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        // Primeiro, verificar se há uma delegação ativa
        var activeDelegation = await _unitOfWork.Repository<ApprovalDelegate>()
            .GetFirstOrDefaultAsync(d => d.Role == role &&
                                        d.ScopeType == scopeType &&
                                        d.ScopeId == scopeId &&
                                        d.IsActive &&
                                        d.StartDate <= now &&
                                        d.EndDate >= now,
                                    cancellationToken);

        if (activeDelegation != null)
        {
            // Retornar o delegado
            var delegateAssignment = await _unitOfWork.Repository<Assignment>()
                .GetQueryable(a => a.Id == activeDelegation.DelegatedToAssignmentId)
                .Include(a => a.Member)
                .Include(a => a.RoleCatalog)
                .FirstOrDefaultAsync(cancellationToken);

            if (delegateAssignment != null)
            {
                return _mapper.Map<AssignmentDto>(delegateAssignment);
            }
        }

        // Se não há delegação ativa, retornar o aprovador original
        var originalAssignment = await _unitOfWork.Repository<Assignment>()
            .GetFirstOrDefaultAsync(a => a.Role == role &&
                                        a.ScopeType == scopeType &&
                                        a.ScopeId == scopeId &&
                                        a.IsActive,
                                    cancellationToken);

        if (originalAssignment == null)
        {
            ExceptionHelper.ThrowNotFoundException("Assignment",
                $"Atribuição não encontrada para a função '{role}' no escopo {scopeType}:{scopeId}");
        }

        return _mapper.Map<AssignmentDto>(originalAssignment);
    }

    #endregion
}
