using Microsoft.AspNetCore.Mvc;
using Pms.Backend.Application.DTOs.Auth;
using Pms.Backend.Application.DTOs.Members;
using Pms.Backend.Application.Interfaces;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Controller for member management operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public partial class MemberController : ControllerBase
{
    private readonly IMemberService _memberService;

    /// <summary>
    /// Initializes a new instance of the MemberController
    /// </summary>
    /// <param name="memberService">Member service for business logic</param>
    public MemberController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    #region Member CRUD Operations

    /// <summary>
    /// Gets a specific member by ID
    /// </summary>
    /// <param name="id">Member ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Member information</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMember(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _memberService.GetMemberAsync(id, cancellationToken);

                    if (!result.Success)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Gets a paginated list of members
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of members</returns>
    [HttpGet]
    public async Task<IActionResult> GetMembers(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _memberService.GetMembersAsync(pageNumber, pageSize, cancellationToken);

                    if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Gets members by club ID
    /// </summary>
    /// <param name="clubId">Club ID</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of members in the club</returns>
    [HttpGet("club/{clubId}")]
    public async Task<IActionResult> GetMembersByClub(
        Guid clubId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _memberService.GetMembersByClubAsync(clubId, pageNumber, pageSize, cancellationToken);

                    if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Creates a new member
    /// </summary>
    /// <param name="dto">Member creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created member information</returns>
    [HttpPost]
    public async Task<IActionResult> CreateMember([FromBody] CreateMemberDto dto, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _memberService.CreateMemberAsync(dto, cancellationToken);

                    if (!result.Success)
        {
            return BadRequest(result);
        }

        return CreatedAtAction(nameof(GetMember), new { id = result.Data!.Id }, result);
    }

    /// <summary>
    /// Updates an existing member
    /// </summary>
    /// <param name="id">Member ID</param>
    /// <param name="dto">Member update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated member information</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMember(Guid id, [FromBody] UpdateMemberDto dto, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _memberService.UpdateMemberAsync(id, dto, cancellationToken);

                    if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Deletes a member
    /// </summary>
    /// <param name="id">Member ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Deletion result</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMember(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _memberService.DeleteMemberAsync(id, cancellationToken);

                    if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    #endregion

    #region Authentication Operations

    /// <summary>
    /// Authenticates a user
    /// </summary>
    /// <param name="request">Login request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Authentication response with JWT token</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _memberService.LoginAsync(request, cancellationToken);

                    if (!result.Success)
        {
            return Unauthorized(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Changes user password
    /// </summary>
    /// <param name="memberId">Member ID</param>
    /// <param name="request">Password change request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Password change result</returns>
    [HttpPost("{memberId}/change-password")]
    public async Task<IActionResult> ChangePassword(Guid memberId, [FromBody] ChangePasswordRequestDto request, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _memberService.ChangePasswordAsync(memberId, request, cancellationToken);

                    if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Initiates password reset
    /// </summary>
    /// <param name="request">Password reset request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Password reset initiation result</returns>
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto request, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _memberService.ResetPasswordAsync(request, cancellationToken);

                    if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Confirms password reset
    /// </summary>
    /// <param name="request">Password reset confirmation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Password reset confirmation result</returns>
    [HttpPost("reset-password/confirm")]
    public async Task<IActionResult> ResetPasswordConfirm([FromBody] ResetPasswordConfirmDto request, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _memberService.ResetPasswordConfirmAsync(request, cancellationToken);

                    if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    #endregion

    #region Member Invitation and Activation

    /// <summary>
    /// Invites a new member
    /// </summary>
    /// <param name="request">Member invitation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Invitation result</returns>
    [HttpPost("invite")]
    public async Task<IActionResult> InviteMember([FromBody] InviteMemberRequestDto request, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _memberService.InviteMemberAsync(request, cancellationToken);

                    if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Activates a member account
    /// </summary>
    /// <param name="request">Member activation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Activation result with JWT token</returns>
    [HttpPost("activate")]
    public async Task<IActionResult> ActivateMember([FromBody] ActivateMemberRequestDto request, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _memberService.ActivateMemberAsync(request, cancellationToken);

                    if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Resends activation email
    /// </summary>
    /// <param name="memberId">Member ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Resend result</returns>
    [HttpPost("{memberId}/resend-activation")]
    public async Task<IActionResult> ResendActivationEmail(Guid memberId, CancellationToken cancellationToken = default)
    {
        var result = await _memberService.ResendActivationEmailAsync(memberId, cancellationToken);

                    if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    #endregion
}
