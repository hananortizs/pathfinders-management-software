using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Members;
using Pms.Backend.Application.DTOs.Membership;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Application.Interfaces;

/// <summary>
/// Interface for membership-related operations
/// Handles club memberships, unit allocation, and the "1º de junho" rule
/// </summary>
public interface IMembershipService
{
    #region Membership CRUD Operations

    /// <summary>
    /// Creates a new membership for a member in a club
    /// </summary>
    /// <param name="memberId">The member ID</param>
    /// <param name="clubId">The club ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result of the membership creation</returns>
    Task<BaseResponse<MembershipDto>> CreateMembershipAsync(Guid memberId, Guid clubId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a membership by ID
    /// </summary>
    /// <param name="id">The membership ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The membership data</returns>
    Task<BaseResponse<MembershipDto>> GetMembershipAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all memberships for a specific member
    /// </summary>
    /// <param name="memberId">The member ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of memberships</returns>
    Task<BaseResponse<IEnumerable<MembershipDto>>> GetMembershipsByMemberAsync(Guid memberId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all memberships for a specific club
    /// </summary>
    /// <param name="clubId">The club ID</param>
    /// <param name="pageNumber">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of memberships</returns>
    Task<BaseResponse<PaginatedResponse<IEnumerable<MembershipDto>>>> GetMembershipsByClubAsync(
        Guid clubId, 
        int pageNumber = 1, 
        int pageSize = 10, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a membership
    /// </summary>
    /// <param name="id">The membership ID</param>
    /// <param name="dto">The updated membership data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result of the update</returns>
    Task<BaseResponse<MembershipDto>> UpdateMembershipAsync(Guid id, UpdateMembershipDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deactivates a membership
    /// </summary>
    /// <param name="id">The membership ID</param>
    /// <param name="reason">Reason for deactivation</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result of the deactivation</returns>
    Task<BaseResponse<bool>> DeactivateMembershipAsync(Guid id, string reason, CancellationToken cancellationToken = default);

    #endregion

    #region Unit Allocation Operations

    /// <summary>
    /// Allocates a member to a unit based on age and gender
    /// </summary>
    /// <param name="membershipId">The membership ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result of the allocation</returns>
    Task<BaseResponse<MembershipDto>> AllocateToUnitAsync(Guid membershipId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Manually allocates a member to a specific unit
    /// </summary>
    /// <param name="membershipId">The membership ID</param>
    /// <param name="unitId">The unit ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result of the manual allocation</returns>
    Task<BaseResponse<MembershipDto>> AllocateToSpecificUnitAsync(Guid membershipId, Guid unitId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Reallocates a member to a different unit
    /// </summary>
    /// <param name="membershipId">The membership ID</param>
    /// <param name="newUnitId">The new unit ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result of the reallocation</returns>
    Task<BaseResponse<MembershipDto>> ReallocateToUnitAsync(Guid membershipId, Guid newUnitId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a member from their current unit
    /// </summary>
    /// <param name="membershipId">The membership ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result of the removal</returns>
    Task<BaseResponse<MembershipDto>> RemoveFromUnitAsync(Guid membershipId, CancellationToken cancellationToken = default);

    #endregion

    #region Age and Eligibility Operations

    /// <summary>
    /// Gets the age of a member on June 1st of the specified year
    /// </summary>
    /// <param name="memberId">The member ID</param>
    /// <param name="year">The year to calculate age for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The age on June 1st</returns>
    Task<BaseResponse<int>> GetAgeOnJuneFirstAsync(Guid memberId, int year, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a member is eligible for membership (≥10 years old)
    /// </summary>
    /// <param name="memberId">The member ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Eligibility status</returns>
    Task<BaseResponse<bool>> IsEligibleForMembershipAsync(Guid memberId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all members who need unit allocation for a specific club
    /// </summary>
    /// <param name="clubId">The club ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of members needing allocation</returns>
    Task<BaseResponse<IEnumerable<MemberDto>>> GetMembersNeedingAllocationAsync(Guid clubId, CancellationToken cancellationToken = default);

    #endregion

    #region Club Capacity Operations

    /// <summary>
    /// Gets the current capacity status of all units in a club
    /// </summary>
    /// <param name="clubId">The club ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Capacity status for each unit</returns>
    Task<BaseResponse<IEnumerable<UnitCapacityDto>>> GetClubCapacityStatusAsync(Guid clubId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a unit has available capacity
    /// </summary>
    /// <param name="unitId">The unit ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Capacity availability status</returns>
    Task<BaseResponse<bool>> IsUnitCapacityAvailableAsync(Guid unitId, CancellationToken cancellationToken = default);

    #endregion

    #region Gender Change Operations

    /// <summary>
    /// Handles gender change for a member and reallocates to appropriate unit
    /// </summary>
    /// <param name="memberId">The member ID</param>
    /// <param name="newGender">The new gender</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result of the gender change and reallocation</returns>
    Task<BaseResponse<MembershipDto>> HandleGenderChangeAsync(Guid memberId, MemberGender newGender, CancellationToken cancellationToken = default);

    #endregion
}
