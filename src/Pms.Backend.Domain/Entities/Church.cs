using Pms.Backend.Domain.Entities.Hierarchy;

namespace Pms.Backend.Domain.Entities;

/// <summary>
/// Represents a Church in the system
/// Each Church belongs to a District and can have at most one Club
/// </summary>
public class Church : BaseEntity
{
    /// <summary>
    /// Name of the church
    /// </summary>
    public string Name { get; set; } = string.Empty;

    // Address fields removed - now using centralized Address entity

    /// <summary>
    /// Phone number of the church
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Email of the church
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Foreign key to the parent district
    /// </summary>
    public Guid DistrictId { get; set; }

    /// <summary>
    /// Navigation property to parent district
    /// </summary>
    public District District { get; set; } = null!;

    /// <summary>
    /// Navigation property to the club linked to this church (1:1 relationship)
    /// </summary>
    public Club? Club { get; set; }
}
