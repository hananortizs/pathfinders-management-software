using Microsoft.AspNetCore.Mvc;
using Pms.Backend.Application.DTOs.Membership;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Controller for managing memberships and unit allocations
/// </summary>
[ApiController]
[Route("[controller]")]
public class MembershipController : ControllerBase
{
    private readonly IMembershipService _membershipService;

    /// <summary>
    /// Initializes a new instance of the MembershipController
    /// </summary>
    /// <param name="membershipService">The membership service</param>
    public MembershipController(IMembershipService membershipService)
    {
        _membershipService = membershipService;
    }

    #region Membership CRUD Operations

    /// <summary>
    /// Creates a new membership for a member in a club
    /// </summary>
    /// <param name="memberId">The member ID</param>
    /// <param name="clubId">The club ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result of the membership creation</returns>
    [HttpPost("create")]
    public async Task<IActionResult> CreateMembership(
        [FromQuery] Guid memberId,
        [FromQuery] Guid clubId,
        CancellationToken cancellationToken = default)
    {
        var result = await _membershipService.CreateMembershipAsync(memberId, clubId, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return CreatedAtAction(nameof(GetMembership), new { id = result.Data!.Id }, result);
    }

    /// <summary>
    /// Gets a membership by ID
    /// </summary>
    /// <param name="id">The membership ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The membership data</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMembership(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _membershipService.GetMembershipAsync(id, cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Gets all memberships for a specific member
    /// </summary>
    /// <param name="memberId">The member ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of memberships</returns>
    [HttpGet("member/{memberId}")]
    public async Task<IActionResult> GetMembershipsByMember(Guid memberId, CancellationToken cancellationToken = default)
    {
        var result = await _membershipService.GetMembershipsByMemberAsync(memberId, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Gets all memberships for a specific club
    /// </summary>
    /// <param name="clubId">The club ID</param>
    /// <param name="pageNumber">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of memberships</returns>
    [HttpGet("club/{clubId}")]
    public async Task<IActionResult> GetMembershipsByClub(
        Guid clubId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _membershipService.GetMembershipsByClubAsync(clubId, pageNumber, pageSize, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Updates a membership
    /// </summary>
    /// <param name="id">The membership ID</param>
    /// <param name="dto">The updated membership data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result of the update</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMembership(Guid id, [FromBody] UpdateMembershipDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _membershipService.UpdateMembershipAsync(id, dto, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Deactivates a membership
    /// </summary>
    /// <param name="id">The membership ID</param>
    /// <param name="reason">Reason for deactivation</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result of the deactivation</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeactivateMembership(Guid id, [FromQuery] string reason, CancellationToken cancellationToken = default)
    {
        var result = await _membershipService.DeactivateMembershipAsync(id, reason, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    #endregion

    #region Unit Allocation Operations

    /// <summary>
    /// Allocates a member to a unit based on age and gender
    /// </summary>
    /// <param name="membershipId">The membership ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result of the allocation</returns>
    [HttpPost("{membershipId}/allocate")]
    public async Task<IActionResult> AllocateToUnit(Guid membershipId, CancellationToken cancellationToken = default)
    {
        var result = await _membershipService.AllocateToUnitAsync(membershipId, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Manually allocates a member to a specific unit
    /// </summary>
    /// <param name="membershipId">The membership ID</param>
    /// <param name="unitId">The unit ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result of the manual allocation</returns>
    [HttpPost("{membershipId}/allocate/{unitId}")]
    public async Task<IActionResult> AllocateToSpecificUnit(Guid membershipId, Guid unitId, CancellationToken cancellationToken = default)
    {
        var result = await _membershipService.AllocateToSpecificUnitAsync(membershipId, unitId, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Reallocates a member to a different unit
    /// </summary>
    /// <param name="membershipId">The membership ID</param>
    /// <param name="newUnitId">The new unit ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result of the reallocation</returns>
    [HttpPut("{membershipId}/reallocate/{newUnitId}")]
    public async Task<IActionResult> ReallocateToUnit(Guid membershipId, Guid newUnitId, CancellationToken cancellationToken = default)
    {
        var result = await _membershipService.ReallocateToUnitAsync(membershipId, newUnitId, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Removes a member from their current unit
    /// </summary>
    /// <param name="membershipId">The membership ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result of the removal</returns>
    [HttpDelete("{membershipId}/unit")]
    public async Task<IActionResult> RemoveFromUnit(Guid membershipId, CancellationToken cancellationToken = default)
    {
        var result = await _membershipService.RemoveFromUnitAsync(membershipId, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    #endregion

    #region Age and Eligibility Operations

    /// <summary>
    /// Gets the age of a member on June 1st of the specified year
    /// </summary>
    /// <param name="memberId">The member ID</param>
    /// <param name="year">The year to calculate age for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The age on June 1st</returns>
    [HttpGet("member/{memberId}/age/{year}")]
    public async Task<IActionResult> GetAgeOnJuneFirst(Guid memberId, int year, CancellationToken cancellationToken = default)
    {
        var result = await _membershipService.GetAgeOnJuneFirstAsync(memberId, year, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Checks if a member is eligible for membership (≥10 years old)
    /// </summary>
    /// <param name="memberId">The member ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Eligibility status</returns>
    [HttpGet("member/{memberId}/eligible")]
    public async Task<IActionResult> IsEligibleForMembership(Guid memberId, CancellationToken cancellationToken = default)
    {
        var result = await _membershipService.IsEligibleForMembershipAsync(memberId, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Gets all members who need unit allocation for a specific club
    /// </summary>
    /// <param name="clubId">The club ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of members needing allocation</returns>
    [HttpGet("club/{clubId}/needing-allocation")]
    public async Task<IActionResult> GetMembersNeedingAllocation(Guid clubId, CancellationToken cancellationToken = default)
    {
        var result = await _membershipService.GetMembersNeedingAllocationAsync(clubId, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    #endregion

    #region Club Capacity Operations

    /// <summary>
    /// Gets the current capacity status of all units in a club
    /// </summary>
    /// <param name="clubId">The club ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Capacity status for each unit</returns>
    [HttpGet("club/{clubId}/capacity")]
    public async Task<IActionResult> GetClubCapacityStatus(Guid clubId, CancellationToken cancellationToken = default)
    {
        var result = await _membershipService.GetClubCapacityStatusAsync(clubId, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Checks if a unit has available capacity
    /// </summary>
    /// <param name="unitId">The unit ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Capacity availability status</returns>
    [HttpGet("unit/{unitId}/capacity-available")]
    public async Task<IActionResult> IsUnitCapacityAvailable(Guid unitId, CancellationToken cancellationToken = default)
    {
        var result = await _membershipService.IsUnitCapacityAvailableAsync(unitId, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    #endregion

    #region Gender Change Operations

    /// <summary>
    /// Handles gender change for a member and reallocates to appropriate unit
    /// </summary>
    /// <param name="memberId">The member ID</param>
    /// <param name="newGender">The new gender</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result of the gender change and reallocation</returns>
    [HttpPut("member/{memberId}/gender/{newGender}")]
    public async Task<IActionResult> HandleGenderChange(Guid memberId, MemberGender newGender, CancellationToken cancellationToken = default)
    {
        var result = await _membershipService.HandleGenderChangeAsync(memberId, newGender, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    #endregion
}
