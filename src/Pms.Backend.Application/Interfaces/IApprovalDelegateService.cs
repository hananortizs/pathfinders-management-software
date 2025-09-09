using Pms.Backend.Application.DTOs.Assignments;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Application.Interfaces;

/// <summary>
/// Interface para operações de delegação de aprovação
/// Gerencia a delegação de autoridade de aprovação quando os aprovadores originais não estão disponíveis
/// </summary>
public interface IApprovalDelegateService
{
    /// <summary>
    /// Cria uma nova delegação de aprovação
    /// </summary>
    /// <param name="request">Solicitação de criação da delegação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>A delegação criada</returns>
    Task<ApprovalDelegateDto> CreateDelegationAsync(CreateApprovalDelegateDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém uma delegação por ID
    /// </summary>
    /// <param name="id">ID da delegação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>A delegação</returns>
    Task<ApprovalDelegateDto> GetDelegationAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém todas as delegações para um escopo específico
    /// </summary>
    /// <param name="scopeType">Tipo do escopo</param>
    /// <param name="scopeId">ID do escopo</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de delegações</returns>
    Task<IEnumerable<ApprovalDelegateDto>> GetDelegationsByScopeAsync(ScopeType scopeType, Guid scopeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém todas as delegações ativas para um escopo específico
    /// </summary>
    /// <param name="scopeType">Tipo do escopo</param>
    /// <param name="scopeId">ID do escopo</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de delegações ativas</returns>
    Task<IEnumerable<ApprovalDelegateDto>> GetActiveDelegationsByScopeAsync(ScopeType scopeType, Guid scopeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém todas as delegações onde um membro é o delegado
    /// </summary>
    /// <param name="memberId">ID do membro</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de delegações</returns>
    Task<IEnumerable<ApprovalDelegateDto>> GetDelegationsByDelegateAsync(Guid memberId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém todas as delegações onde um membro é o delegante
    /// </summary>
    /// <param name="memberId">ID do membro</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de delegações</returns>
    Task<IEnumerable<ApprovalDelegateDto>> GetDelegationsByDelegatorAsync(Guid memberId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza uma delegação
    /// </summary>
    /// <param name="id">ID da delegação</param>
    /// <param name="request">Solicitação de atualização</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>A delegação atualizada</returns>
    Task<ApprovalDelegateDto> UpdateDelegationAsync(Guid id, UpdateApprovalDelegateDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finaliza uma delegação
    /// </summary>
    /// <param name="id">ID da delegação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado do sucesso</returns>
    Task<bool> EndDelegationAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Exclui uma delegação
    /// </summary>
    /// <param name="id">ID da delegação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado do sucesso</returns>
    Task<bool> DeleteDelegationAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Valida se uma delegação pode ser criada
    /// </summary>
    /// <param name="request">Solicitação da delegação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da validação</returns>
    Task<bool> ValidateDelegationAsync(CreateApprovalDelegateDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém o aprovador efetivo para uma função em um escopo específico
    /// </summary>
    /// <param name="role">Nome da função</param>
    /// <param name="scopeType">Tipo do escopo</param>
    /// <param name="scopeId">ID do escopo</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>A atribuição efetiva</returns>
    Task<AssignmentDto> GetEffectiveApproverAsync(string role, ScopeType scopeType, Guid scopeId, CancellationToken cancellationToken = default);
}
