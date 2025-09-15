using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Assignments;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Application.Interfaces;

/// <summary>
/// Interface for managing role catalog operations
/// </summary>
public interface IRoleCatalogService
{
    /// <summary>
    /// Creates a new role in the catalog
    /// </summary>
    /// <param name="request">The role creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created role</returns>
    Task<BaseResponse<RoleCatalogDto>> CreateRoleAsync(CreateRoleCatalogDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a role by ID
    /// </summary>
    /// <param name="id">The role ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The role</returns>
    Task<BaseResponse<RoleCatalogDto>> GetRoleByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all roles with pagination
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of roles</returns>
    Task<BaseResponse<PaginatedResponse<IEnumerable<RoleCatalogDto>>>> GetAllRolesAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets roles by hierarchy level
    /// </summary>
    /// <param name="level">The hierarchy level</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of roles for the level</returns>
    Task<BaseResponse<IEnumerable<RoleCatalogDto>>> GetRolesByLevelAsync(RoleLevel level, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets active roles only
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of active roles</returns>
    Task<BaseResponse<IEnumerable<RoleCatalogDto>>> GetActiveRolesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing role
    /// </summary>
    /// <param name="id">The role ID</param>
    /// <param name="request">The update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated role</returns>
    Task<BaseResponse<RoleCatalogDto>> UpdateRoleAsync(Guid id, UpdateRoleCatalogDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Activates a role
    /// </summary>
    /// <param name="id">The role ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success result</returns>
    Task<BaseResponse<bool>> ActivateRoleAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deactivates a role
    /// </summary>
    /// <param name="id">The role ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success result</returns>
    Task<BaseResponse<bool>> DeactivateRoleAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Soft deletes a role
    /// </summary>
    /// <param name="id">The role ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success result</returns>
    Task<BaseResponse<bool>> DeleteRoleAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates if a role can be created with the given parameters
    /// </summary>
    /// <param name="request">The role creation request</param>
    /// <returns>Validation result</returns>
    BaseResponse<bool> ValidateRole(CreateRoleCatalogDto request);
}
