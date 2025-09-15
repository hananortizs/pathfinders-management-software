using Microsoft.AspNetCore.Mvc;
using Pms.Backend.Application.DTOs.Members;
using Pms.Backend.Application.Interfaces;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Second part of MemberController with remaining operations
/// </summary>
public partial class MemberController : BaseController
{
    #region Member Status Operations

    /// <summary>
    /// Activates a member account
    /// </summary>
    /// <param name="id">Member ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Activation result</returns>
    [HttpPost("{id}/activate")]
    public async Task<IActionResult> ActivateMemberAccount(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _memberService.ActivateMemberAccountAsync(id, cancellationToken);
        return ProcessResponse(result);
    }

    /// <summary>
    /// Deactivates a member account
    /// </summary>
    /// <param name="id">Member ID</param>
    /// <param name="reason">Deactivation reason</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Deactivation result</returns>
    [HttpPost("{id}/deactivate")]
    public async Task<IActionResult> DeactivateMemberAccount(Guid id, [FromBody] string reason, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(reason))
        {
            return BadRequest("Deactivation reason is required");
        }

        var result = await _memberService.DeactivateMemberAccountAsync(id, reason, cancellationToken);
        return ProcessResponse(result);
    }

    /// <summary>
    /// Locks a member account
    /// </summary>
    /// <param name="id">Member ID</param>
    /// <param name="reason">Lock reason</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Lock result</returns>
    [HttpPost("{id}/lock")]
    public async Task<IActionResult> LockMemberAccount(Guid id, [FromBody] string reason, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(reason))
        {
            return BadRequest("Lock reason is required");
        }

        var result = await _memberService.LockMemberAccountAsync(id, reason, cancellationToken);
        return ProcessResponse(result);
    }

    /// <summary>
    /// Unlocks a member account
    /// </summary>
    /// <param name="id">Member ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Unlock result</returns>
    [HttpPost("{id}/unlock")]
    public async Task<IActionResult> UnlockMemberAccount(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _memberService.UnlockMemberAccountAsync(id, cancellationToken);
        return ProcessResponse(result);
    }

    #endregion

    #region Member Search and Validation

    /// <summary>
    /// Searches for members
    /// </summary>
    /// <param name="searchTerm">Search term</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Search results</returns>
    [HttpGet("search")]
    public async Task<IActionResult> SearchMembers(
        [FromQuery] string searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return BadRequest("Search term is required");
        }

        var result = await _memberService.SearchMembersAsync(searchTerm, pageNumber, pageSize, cancellationToken);
        return ProcessResponse(result);
    }

    /// <summary>
    /// Validates if an email is available
    /// </summary>
    /// <param name="email">Email to validate</param>
    /// <param name="excludeMemberId">Optional member ID to exclude</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Email availability result</returns>
    [HttpGet("validate/email")]
    public async Task<IActionResult> ValidateEmail(
        [FromQuery] string email,
        [FromQuery] Guid? excludeMemberId = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return BadRequest("Email is required");
        }

        var result = await _memberService.IsEmailAvailableAsync(email, excludeMemberId, cancellationToken);
        return ProcessResponse(result);
    }

    /// <summary>
    /// Validates if a CPF is available
    /// </summary>
    /// <param name="cpf">CPF to validate</param>
    /// <param name="excludeMemberId">Optional member ID to exclude</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>CPF availability result</returns>
    [HttpGet("validate/cpf")]
    public async Task<IActionResult> ValidateCpf(
        [FromQuery] string cpf,
        [FromQuery] Guid? excludeMemberId = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(cpf))
        {
            return BadRequest("CPF is required");
        }

        var result = await _memberService.IsCpfAvailableAsync(cpf, excludeMemberId, cancellationToken);
        return ProcessResponse(result);
    }

    #endregion
}
