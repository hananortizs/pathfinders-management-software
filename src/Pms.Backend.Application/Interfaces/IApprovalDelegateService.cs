using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Assignments;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Application.Interfaces;

/// <summary>
/// Interface for approval delegation operations
/// Handles delegation of approval authority when original approvers are unavailable
/// </summary>
public interface IApprovalDelegateService
{
    /// <summary>
    /// Creates a new approval delegation
    /// </summary>
    /// <param name="request">The delegation creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created delegation</returns>
    Task<BaseResponse<ApprovalDelegateDto>> CreateDelegationAsync(CreateApprovalDelegateDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a delegation by ID
    /// </summary>
    /// <param name="id">The delegation ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The delegation</returns>
    Task<BaseResponse<ApprovalDelegateDto>> GetDelegationAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all delegations for a specific scope
    /// </summary>
    /// <param name="scopeType">The scope type</param>
    /// <param name="scopeId">The scope ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of delegations</returns>
    Task<BaseResponse<IEnumerable<ApprovalDelegateDto>>> GetDelegationsByScopeAsync(ScopeType scopeType, Guid scopeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all active delegations for a specific scope
    /// </summary>
    /// <param name="scopeType">The scope type</param>
    /// <param name="scopeId">The scope ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of active delegations</returns>
    Task<BaseResponse<IEnumerable<ApprovalDelegateDto>>> GetActiveDelegationsByScopeAsync(ScopeType scopeType, Guid scopeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all delegations where a member is the delegate
    /// </summary>
    /// <param name="memberId">The member ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of delegations</returns>
    Task<BaseResponse<IEnumerable<ApprovalDelegateDto>>> GetDelegationsByDelegateAsync(Guid memberId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all delegations where a member is the delegator
    /// </summary>
    /// <param name="memberId">The member ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of delegations</returns>
    Task<BaseResponse<IEnumerable<ApprovalDelegateDto>>> GetDelegationsByDelegatorAsync(Guid memberId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a delegation
    /// </summary>
    /// <param name="id">The delegation ID</param>
    /// <param name="request">The update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated delegation</returns>
    Task<BaseResponse<ApprovalDelegateDto>> UpdateDelegationAsync(Guid id, UpdateApprovalDelegateDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ends a delegation
    /// </summary>
    /// <param name="id">The delegation ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success result</returns>
    Task<BaseResponse<bool>> EndDelegationAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a delegation
    /// </summary>
    /// <param name="id">The delegation ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success result</returns>
    Task<BaseResponse<bool>> DeleteDelegationAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates if a delegation can be created
    /// </summary>
    /// <param name="request">The delegation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Validation result</returns>
    Task<BaseResponse<bool>> ValidateDelegationAsync(CreateApprovalDelegateDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the effective approver for a role in a specific scope
    /// </summary>
    /// <param name="role">The role name</param>
    /// <param name="scopeType">The scope type</param>
    /// <param name="scopeId">The scope ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The effective assignment</returns>
    Task<BaseResponse<AssignmentDto?>> GetEffectiveApproverAsync(string role, ScopeType scopeType, Guid scopeId, CancellationToken cancellationToken = default);
}
