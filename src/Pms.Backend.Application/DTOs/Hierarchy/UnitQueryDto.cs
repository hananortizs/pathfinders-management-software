namespace Pms.Backend.Application.DTOs.Hierarchy;

/// <summary>
/// Data Transfer Object for Unit entity in hierarchy queries (without parent objects)
/// </summary>
public class UnitQueryDto
{
    /// <summary>
    /// Unit ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Unique code for the unit (≤5 chars, UPPERCASE A-Z0-9)
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Name of the unit
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the unit
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Parent club ID
    /// </summary>
    public Guid ClubId { get; set; }

    /// <summary>
    /// Unit gender type
    /// </summary>
    public UnitGender Gender { get; set; }

    /// <summary>
    /// Maximum number of members allowed in this unit
    /// </summary>
    public int MaxMembers { get; set; }

    /// <summary>
    /// Current number of members in this unit
    /// </summary>
    public int CurrentMemberCount { get; set; }

    /// <summary>
    /// Indicates if the unit has available capacity for new members
    /// </summary>
    public bool HasAvailableCapacity { get; set; }

    /// <summary>
    /// Code path (Division.Code.Union.Code.Association.Code.Region.Code.District.Code.Club.Code.Unit.Code)
    /// </summary>
    public string CodePath { get; set; } = string.Empty;

    /// <summary>
    /// Creation date
    /// </summary>
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime UpdatedAtUtc { get; set; }
}
