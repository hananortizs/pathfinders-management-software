using Microsoft.AspNetCore.Mvc;
using Pms.Backend.Application.DTOs.Assignments;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Controller for managing role assignments
/// </summary>
[ApiController]
[Route("[controller]")]
public class AssignmentController : ControllerBase
{
    private readonly IAssignmentService _assignmentService;

    /// <summary>
    /// Initializes a new instance of the AssignmentController
    /// </summary>
    /// <param name="assignmentService">The assignment service</param>
    public AssignmentController(IAssignmentService assignmentService)
    {
        _assignmentService = assignmentService;
    }

    /// <summary>
    /// Creates a new assignment for a member
    /// </summary>
    /// <param name="request">The assignment creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created assignment</returns>
    [HttpPost]
    [ProducesResponseType(typeof(AssignmentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAssignment([FromBody] CreateAssignmentDto request, CancellationToken cancellationToken)
    {
        var result = await _assignmentService.CreateAssignmentAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetAssignmentById), new { id = result.Data?.Id }, result);
    }

    /// <summary>
    /// Gets an assignment by ID
    /// </summary>
    /// <param name="id">The assignment ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The assignment</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AssignmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAssignmentById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _assignmentService.GetAssignmentAsync(id, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets all assignments for a member
    /// </summary>
    /// <param name="memberId">The member ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of assignments</returns>
    [HttpGet("member/{memberId}")]
    [ProducesResponseType(typeof(IEnumerable<AssignmentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAssignmentsByMember(Guid memberId, CancellationToken cancellationToken)
    {
        var result = await _assignmentService.GetAssignmentsByMemberAsync(memberId, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets all assignments for a specific scope
    /// </summary>
    /// <param name="scopeType">The scope type</param>
    /// <param name="scopeId">The scope ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of assignments</returns>
    [HttpGet("scope/{scopeType}/{scopeId}")]
    [ProducesResponseType(typeof(IEnumerable<AssignmentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAssignmentsByScope(ScopeType scopeType, Guid scopeId, CancellationToken cancellationToken)
    {
        var result = await _assignmentService.GetAssignmentsByScopeAsync(scopeType, scopeId, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets all active assignments for a specific scope
    /// </summary>
    /// <param name="scopeType">The scope type</param>
    /// <param name="scopeId">The scope ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of active assignments</returns>
    [HttpGet("scope/{scopeType}/{scopeId}/active")]
    [ProducesResponseType(typeof(IEnumerable<AssignmentDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActiveAssignmentsByScope(ScopeType scopeType, Guid scopeId, CancellationToken cancellationToken)
    {
        var result = await _assignmentService.GetActiveAssignmentsByScopeAsync(scopeType, scopeId, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Updates an assignment
    /// </summary>
    /// <param name="id">The assignment ID</param>
    /// <param name="request">The update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated assignment</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(AssignmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAssignment(Guid id, [FromBody] UpdateAssignmentDto request, CancellationToken cancellationToken)
    {
        var result = await _assignmentService.UpdateAssignmentAsync(id, request, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Ends an assignment
    /// </summary>
    /// <param name="id">The assignment ID</param>
    /// <param name="endReason">Reason for ending the assignment</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success result</returns>
    [HttpPut("{id}/end")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EndAssignment(Guid id, [FromQuery] string? endReason = null, CancellationToken cancellationToken = default)
    {
        var result = await _assignmentService.EndAssignmentAsync(id, endReason, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Deletes an assignment
    /// </summary>
    /// <param name="id">The assignment ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success result</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAssignment(Guid id, CancellationToken cancellationToken)
    {
        var result = await _assignmentService.DeleteAssignmentAsync(id, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Validates if a member can be assigned to a role
    /// </summary>
    /// <param name="memberId">The member ID</param>
    /// <param name="roleId">The role ID</param>
    /// <param name="scopeType">The scope type</param>
    /// <param name="scopeId">The scope ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Validation result</returns>
    [HttpGet("validate")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> ValidateAssignment(
        [FromQuery] Guid memberId,
        [FromQuery] Guid roleId,
        [FromQuery] ScopeType scopeType,
        [FromQuery] Guid scopeId,
        CancellationToken cancellationToken)
    {
        var result = await _assignmentService.ValidateAssignmentAsync(memberId, roleId, scopeType, scopeId, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets all role catalogs
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of role catalogs</returns>
    [HttpGet("roles")]
    [ProducesResponseType(typeof(IEnumerable<RoleCatalogDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRoleCatalogs(CancellationToken cancellationToken)
    {
        var result = await _assignmentService.GetRoleCatalogsAsync(cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets role catalogs by hierarchy level
    /// </summary>
    /// <param name="hierarchyLevel">The hierarchy level</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of role catalogs</returns>
    [HttpGet("roles/level/{hierarchyLevel}")]
    [ProducesResponseType(typeof(IEnumerable<RoleCatalogDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRoleCatalogsByLevel(string hierarchyLevel, CancellationToken cancellationToken)
    {
        var result = await _assignmentService.GetRoleCatalogsByLevelAsync(hierarchyLevel, cancellationToken);
        return Ok(result);
    }
}
