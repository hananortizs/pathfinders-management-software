using Pms.Backend.Domain.Entities.Hierarchy;
using Pms.Backend.Domain.Enums;

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
    // Phone and Email fields removed - now using centralized Contact entity

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

    /// <summary>
    /// Navigation property to contacts
    /// </summary>
    public ICollection<Contact> Contacts { get; set; } = new List<Contact>();

    /// <summary>
    /// Gets the primary email contact for display
    /// </summary>
    public string? PrimaryEmail => Contacts
        .Where(c => c.Type == ContactType.Email && c.IsActive && c.IsPrimary)
        .Select(c => c.FormattedValue)
        .FirstOrDefault();

    /// <summary>
    /// Gets the primary phone contact for display
    /// </summary>
    public string? PrimaryPhone => Contacts
        .Where(c => (c.Type == ContactType.Landline || c.Type == ContactType.Mobile) && c.IsActive && c.IsPrimary)
        .Select(c => c.FormattedValue)
        .FirstOrDefault();
}
