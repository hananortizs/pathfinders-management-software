using Microsoft.AspNetCore.Mvc;
using Pms.Backend.Application.DTOs.Assignments;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Controller for managing approval delegations
/// </summary>
[ApiController]
[Route("pms/[controller]")]
public class ApprovalDelegateController : ControllerBase
{
    private readonly IApprovalDelegateService _approvalDelegateService;

    /// <summary>
    /// Initializes a new instance of the ApprovalDelegateController
    /// </summary>
    /// <param name="approvalDelegateService">The approval delegate service</param>
    public ApprovalDelegateController(IApprovalDelegateService approvalDelegateService)
    {
        _approvalDelegateService = approvalDelegateService;
    }

    /// <summary>
    /// Creates a new approval delegation
    /// </summary>
    /// <param name="request">The delegation creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created delegation</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApprovalDelegateDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDelegation([FromBody] CreateApprovalDelegateDto request, CancellationToken cancellationToken)
    {
        var result = await _approvalDelegateService.CreateDelegationAsync(request, cancellationToken);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return CreatedAtAction(nameof(GetDelegationById), new { id = result.Data?.Id }, result);
    }

    /// <summary>
    /// Gets a delegation by ID
    /// </summary>
    /// <param name="id">The delegation ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The delegation</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApprovalDelegateDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDelegationById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _approvalDelegateService.GetDelegationAsync(id, cancellationToken);
        if (!result.Success)
        {
            return NotFound(result);
        }
        return Ok(result);
    }

    /// <summary>
    /// Gets all delegations for a specific scope
    /// </summary>
    /// <param name="scopeType">The scope type</param>
    /// <param name="scopeId">The scope ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of delegations</returns>
    [HttpGet("scope/{scopeType}/{scopeId}")]
    [ProducesResponseType(typeof(IEnumerable<ApprovalDelegateDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDelegationsByScope(ScopeType scopeType, Guid scopeId, CancellationToken cancellationToken)
    {
        var result = await _approvalDelegateService.GetDelegationsByScopeAsync(scopeType, scopeId, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets all active delegations for a specific scope
    /// </summary>
    /// <param name="scopeType">The scope type</param>
    /// <param name="scopeId">The scope ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of active delegations</returns>
    [HttpGet("scope/{scopeType}/{scopeId}/active")]
    [ProducesResponseType(typeof(IEnumerable<ApprovalDelegateDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActiveDelegationsByScope(ScopeType scopeType, Guid scopeId, CancellationToken cancellationToken)
    {
        var result = await _approvalDelegateService.GetActiveDelegationsByScopeAsync(scopeType, scopeId, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets all delegations where a member is the delegate
    /// </summary>
    /// <param name="memberId">The member ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of delegations</returns>
    [HttpGet("delegate/{memberId}")]
    [ProducesResponseType(typeof(IEnumerable<ApprovalDelegateDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDelegationsByDelegate(Guid memberId, CancellationToken cancellationToken)
    {
        var result = await _approvalDelegateService.GetDelegationsByDelegateAsync(memberId, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets all delegations where a member is the delegator
    /// </summary>
    /// <param name="memberId">The member ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of delegations</returns>
    [HttpGet("delegator/{memberId}")]
    [ProducesResponseType(typeof(IEnumerable<ApprovalDelegateDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDelegationsByDelegator(Guid memberId, CancellationToken cancellationToken)
    {
        var result = await _approvalDelegateService.GetDelegationsByDelegatorAsync(memberId, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Updates a delegation
    /// </summary>
    /// <param name="id">The delegation ID</param>
    /// <param name="request">The update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated delegation</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApprovalDelegateDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateDelegation(Guid id, [FromBody] UpdateApprovalDelegateDto request, CancellationToken cancellationToken)
    {
        var result = await _approvalDelegateService.UpdateDelegationAsync(id, request, cancellationToken);
        if (!result.Success)
        {
            return result.Message?.Contains("not found") == true ? NotFound(result) : BadRequest(result);
        }
        return Ok(result);
    }

    /// <summary>
    /// Ends a delegation
    /// </summary>
    /// <param name="id">The delegation ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success result</returns>
    [HttpPut("{id}/end")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> EndDelegation(Guid id, CancellationToken cancellationToken)
    {
        var result = await _approvalDelegateService.EndDelegationAsync(id, cancellationToken);
        if (!result.Success)
        {
            return NotFound(result);
        }
        return Ok(result);
    }

    /// <summary>
    /// Deletes a delegation
    /// </summary>
    /// <param name="id">The delegation ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success result</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDelegation(Guid id, CancellationToken cancellationToken)
    {
        var result = await _approvalDelegateService.DeleteDelegationAsync(id, cancellationToken);
        if (!result.Success)
        {
            return NotFound(result);
        }
        return Ok(result);
    }

    /// <summary>
    /// Validates if a delegation can be created
    /// </summary>
    /// <param name="request">The delegation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Validation result</returns>
    [HttpPost("validate")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> ValidateDelegation([FromBody] CreateApprovalDelegateDto request, CancellationToken cancellationToken)
    {
        var result = await _approvalDelegateService.ValidateDelegationAsync(request, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets the effective approver for a role in a specific scope
    /// </summary>
    /// <param name="role">The role name</param>
    /// <param name="scopeType">The scope type</param>
    /// <param name="scopeId">The scope ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The effective assignment</returns>
    [HttpGet("effective-approver")]
    [ProducesResponseType(typeof(AssignmentDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEffectiveApprover(
        [FromQuery] string role,
        [FromQuery] ScopeType scopeType,
        [FromQuery] Guid scopeId,
        CancellationToken cancellationToken)
    {
        var result = await _approvalDelegateService.GetEffectiveApproverAsync(role, scopeType, scopeId, cancellationToken);
        return Ok(result);
    }
}
