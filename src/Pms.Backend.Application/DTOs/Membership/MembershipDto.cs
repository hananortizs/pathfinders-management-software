using Pms.Backend.Application.DTOs.Hierarchy;
using Pms.Backend.Application.DTOs.Members;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Application.DTOs.Membership;

/// <summary>
/// Data Transfer Object for Membership entity
/// </summary>
public class MembershipDto
{
    /// <summary>
    /// Unique identifier of the membership
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Associated member ID
    /// </summary>
    public Guid MemberId { get; set; }

    /// <summary>
    /// Associated club ID
    /// </summary>
    public Guid ClubId { get; set; }

    /// <summary>
    /// Associated unit ID (optional)
    /// </summary>
    public Guid? UnitId { get; set; }

    /// <summary>
    /// When the membership started
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// When the membership ended (null if still active)
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Reason for ending the membership
    /// </summary>
    public string? EndReason { get; set; }

    /// <summary>
    /// Indicates if the membership is currently active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Age of the member on June 1st of the membership year
    /// </summary>
    public int AgeOnJuneFirst { get; set; }

    /// <summary>
    /// Member information
    /// </summary>
    public MemberDto? Member { get; set; }

    /// <summary>
    /// Club information
    /// </summary>
    public ClubDto? Club { get; set; }

    /// <summary>
    /// Unit information (if allocated)
    /// </summary>
    public UnitDto? Unit { get; set; }

    /// <summary>
    /// Creation date
    /// </summary>
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime UpdatedAtUtc { get; set; }
}

/// <summary>
/// Data Transfer Object for creating a new membership
/// </summary>
public class CreateMembershipDto
{
    /// <summary>
    /// Associated member ID
    /// </summary>
    public Guid MemberId { get; set; }

    /// <summary>
    /// Associated club ID
    /// </summary>
    public Guid ClubId { get; set; }

    /// <summary>
    /// When the membership starts (defaults to current date)
    /// </summary>
    public DateTime? StartDate { get; set; }
}

/// <summary>
/// Data Transfer Object for updating a membership
/// </summary>
public class UpdateMembershipDto
{
    /// <summary>
    /// Associated unit ID (optional)
    /// </summary>
    public Guid? UnitId { get; set; }

    /// <summary>
    /// When the membership ends (null if still active)
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Reason for ending the membership
    /// </summary>
    public string? EndReason { get; set; }

    /// <summary>
    /// Indicates if the membership is currently active
    /// </summary>
    public bool IsActive { get; set; }
}

/// <summary>
/// Data Transfer Object for unit capacity information
/// </summary>
public class UnitCapacityDto
{
    /// <summary>
    /// Unit ID
    /// </summary>
    public Guid UnitId { get; set; }

    /// <summary>
    /// Unit name
    /// </summary>
    public string UnitName { get; set; } = string.Empty;

    /// <summary>
    /// Unit gender
    /// </summary>
    public Domain.Entities.UnitGender Gender { get; set; }

    /// <summary>
    /// Maximum capacity of the unit
    /// </summary>
    public int MaxCapacity { get; set; }

    /// <summary>
    /// Current number of members in the unit
    /// </summary>
    public int CurrentCount { get; set; }

    /// <summary>
    /// Available capacity
    /// </summary>
    public int AvailableCapacity => MaxCapacity - CurrentCount;

    /// <summary>
    /// Indicates if the unit has available capacity
    /// </summary>
    public bool HasAvailableCapacity => AvailableCapacity > 0;

    /// <summary>
    /// Capacity percentage (0-100)
    /// </summary>
    public double CapacityPercentage => MaxCapacity > 0 ? (double)CurrentCount / MaxCapacity * 100 : 0;
}

/// <summary>
/// Data Transfer Object for membership allocation request
/// </summary>
public class AllocateMembershipDto
{
    /// <summary>
    /// Membership ID
    /// </summary>
    public Guid MembershipId { get; set; }

    /// <summary>
    /// Specific unit ID (optional - if not provided, auto-allocation will be used)
    /// </summary>
    public Guid? UnitId { get; set; }

    /// <summary>
    /// Force allocation even if unit is at capacity
    /// </summary>
    public bool ForceAllocation { get; set; } = false;
}

/// <summary>
/// Data Transfer Object for membership reallocation request
/// </summary>
public class ReallocateMembershipDto
{
    /// <summary>
    /// Membership ID
    /// </summary>
    public Guid MembershipId { get; set; }

    /// <summary>
    /// New unit ID
    /// </summary>
    public Guid NewUnitId { get; set; }

    /// <summary>
    /// Reason for reallocation
    /// </summary>
    public string? Reason { get; set; }

    /// <summary>
    /// Force reallocation even if new unit is at capacity
    /// </summary>
    public bool ForceReallocation { get; set; } = false;
}
