using Microsoft.AspNetCore.Mvc;
using Pms.Backend.Application.DTOs.Assignments;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Controller for managing role catalog operations
/// </summary>
[ApiController]
[Route("[controller]")]
public class RoleCatalogController : ControllerBase
{
    private readonly IRoleCatalogService _roleCatalogService;

    /// <summary>
    /// Initializes a new instance of the RoleCatalogController
    /// </summary>
    /// <param name="roleCatalogService">The role catalog service</param>
    public RoleCatalogController(IRoleCatalogService roleCatalogService)
    {
        _roleCatalogService = roleCatalogService;
    }

    /// <summary>
    /// Creates a new role in the catalog
    /// </summary>
    /// <param name="request">The role creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created role</returns>
    [HttpPost]
    [ProducesResponseType(typeof(RoleCatalogDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleCatalogDto request, CancellationToken cancellationToken)
    {
        var result = await _roleCatalogService.CreateRoleAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetRoleById), new { id = result.Data?.Id }, result);
    }

    /// <summary>
    /// Gets a role by ID
    /// </summary>
    /// <param name="id">The role ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The role</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RoleCatalogDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRoleById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _roleCatalogService.GetRoleByIdAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Gets all roles with pagination
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of roles</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RoleCatalogDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllRoles(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await _roleCatalogService.GetAllRolesAsync(pageNumber, pageSize, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets roles by hierarchy level
    /// </summary>
    /// <param name="level">The hierarchy level</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of roles for the level</returns>
    [HttpGet("level/{level}")]
    [ProducesResponseType(typeof(IEnumerable<RoleCatalogDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRolesByLevel(RoleLevel level, CancellationToken cancellationToken)
    {
        var result = await _roleCatalogService.GetRolesByLevelAsync(level, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets active roles only
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of active roles</returns>
    [HttpGet("active")]
    [ProducesResponseType(typeof(IEnumerable<RoleCatalogDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActiveRoles(CancellationToken cancellationToken)
    {
        var result = await _roleCatalogService.GetActiveRolesAsync(cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Updates an existing role
    /// </summary>
    /// <param name="id">The role ID</param>
    /// <param name="request">The update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated role</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(RoleCatalogDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateRole(Guid id, [FromBody] UpdateRoleCatalogDto request, CancellationToken cancellationToken)
    {
        var result = await _roleCatalogService.UpdateRoleAsync(id, request, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Activates a role
    /// </summary>
    /// <param name="id">The role ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success result</returns>
    [HttpPut("{id}/activate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ActivateRole(Guid id, CancellationToken cancellationToken)
    {
        var result = await _roleCatalogService.ActivateRoleAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Deactivates a role
    /// </summary>
    /// <param name="id">The role ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success result</returns>
    [HttpPut("{id}/deactivate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeactivateRole(Guid id, CancellationToken cancellationToken)
    {
        var result = await _roleCatalogService.DeactivateRoleAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Soft deletes a role
    /// </summary>
    /// <param name="id">The role ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success result</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRole(Guid id, CancellationToken cancellationToken)
    {
        var result = await _roleCatalogService.DeleteRoleAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Validates if a role can be created with the given parameters
    /// </summary>
    /// <param name="request">The role creation request</param>
    /// <returns>Validation result</returns>
    [HttpPost("validate")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public IActionResult ValidateRole([FromBody] CreateRoleCatalogDto request)
    {
        var result = _roleCatalogService.ValidateRole(request);
        return Ok(result);
    }
}
