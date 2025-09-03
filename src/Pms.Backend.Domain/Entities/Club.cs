using Pms.Backend.Domain.Entities.Hierarchy;

namespace Pms.Backend.Domain.Entities;

/// <summary>
/// Represents a Club in the organizational hierarchy
/// Club belongs to a District and is linked to a Church
/// </summary>
public class Club : BaseEntity
{
    /// <summary>
    /// Unique code for the club (≤5 chars, UPPERCASE A-Z0-9)
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Name of the club
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the club
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Foreign key to the parent district
    /// </summary>
    public Guid DistrictId { get; set; }

    /// <summary>
    /// Navigation property to parent district
    /// </summary>
    public District District { get; set; } = null!;

    /// <summary>
    /// Foreign key to the linked church (1:1 relationship)
    /// </summary>
    public Guid ChurchId { get; set; }

    /// <summary>
    /// Navigation property to the linked church
    /// </summary>
    public Church Church { get; set; } = null!;

    /// <summary>
    /// Indicates if the club is active (has an active Director)
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Navigation property to child units
    /// </summary>
    public ICollection<Unit> Units { get; set; } = new List<Unit>();

    /// <summary>
    /// Navigation property to memberships
    /// </summary>
    public ICollection<Membership> Memberships { get; set; } = new List<Membership>();

    /// <summary>
    /// Gets the code path for this club (Division.Code.Union.Code.Association.Code.Region.Code.District.Code.Club.Code)
    /// </summary>
    public string CodePath => $"{District.CodePath}.{Code}";
}
