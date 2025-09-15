using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Assignments;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Application.Interfaces;

/// <summary>
/// Interface for assignment-related operations
/// Handles role assignments, spiritual requirements validation, and assignment lifecycle
/// </summary>
public interface IAssignmentService
{
    /// <summary>
    /// Creates a new assignment for a member
    /// </summary>
    /// <param name="request">The assignment creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created assignment</returns>
    Task<BaseResponse<AssignmentDto>> CreateAssignmentAsync(CreateAssignmentDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an assignment by ID
    /// </summary>
    /// <param name="id">The assignment ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The assignment</returns>
    Task<BaseResponse<AssignmentDto>> GetAssignmentAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all assignments for a member
    /// </summary>
    /// <param name="memberId">The member ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of assignments</returns>
    Task<BaseResponse<IEnumerable<AssignmentDto>>> GetAssignmentsByMemberAsync(Guid memberId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all assignments for a specific scope
    /// </summary>
    /// <param name="scopeType">The scope type</param>
    /// <param name="scopeId">The scope ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of assignments</returns>
    Task<BaseResponse<IEnumerable<AssignmentDto>>> GetAssignmentsByScopeAsync(ScopeType scopeType, Guid scopeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all active assignments for a specific scope
    /// </summary>
    /// <param name="scopeType">The scope type</param>
    /// <param name="scopeId">The scope ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of active assignments</returns>
    Task<BaseResponse<IEnumerable<AssignmentDto>>> GetActiveAssignmentsByScopeAsync(ScopeType scopeType, Guid scopeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an assignment
    /// </summary>
    /// <param name="id">The assignment ID</param>
    /// <param name="request">The update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated assignment</returns>
    Task<BaseResponse<AssignmentDto>> UpdateAssignmentAsync(Guid id, UpdateAssignmentDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ends an assignment
    /// </summary>
    /// <param name="id">The assignment ID</param>
    /// <param name="endReason">Reason for ending the assignment</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success result</returns>
    Task<BaseResponse<bool>> EndAssignmentAsync(Guid id, string? endReason = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an assignment
    /// </summary>
    /// <param name="id">The assignment ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success result</returns>
    Task<BaseResponse<bool>> DeleteAssignmentAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates if a member can be assigned to a role
    /// </summary>
    /// <param name="memberId">The member ID</param>
    /// <param name="roleId">The role ID</param>
    /// <param name="scopeType">The scope type</param>
    /// <param name="scopeId">The scope ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Validation result</returns>
    Task<BaseResponse<bool>> ValidateAssignmentAsync(Guid memberId, Guid roleId, ScopeType scopeType, Guid scopeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all role catalogs
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of role catalogs</returns>
    Task<BaseResponse<IEnumerable<RoleCatalogDto>>> GetRoleCatalogsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets role catalogs by hierarchy level
    /// </summary>
    /// <param name="hierarchyLevel">The hierarchy level</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of role catalogs</returns>
    Task<BaseResponse<IEnumerable<RoleCatalogDto>>> GetRoleCatalogsByLevelAsync(string hierarchyLevel, CancellationToken cancellationToken = default);
}
