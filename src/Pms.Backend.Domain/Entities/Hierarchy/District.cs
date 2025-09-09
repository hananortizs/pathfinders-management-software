using Pms.Backend.Domain.Common;

namespace Pms.Backend.Domain.Entities.Hierarchy;

/// <summary>
/// Represents a District in the organizational hierarchy
/// District belongs to a Region
/// </summary>
public class District : HierarchyEntityBase
{

    /// <summary>
    /// Foreign key to the parent region
    /// </summary>
    public Guid RegionId { get; set; }

    /// <summary>
    /// Navigation property to parent region
    /// </summary>
    public Region Region { get; set; } = null!;

    /// <summary>
    /// Navigation property to child clubs
    /// </summary>
    public ICollection<Club> Clubs { get; set; } = new List<Club>();

    /// <summary>
    /// Navigation property to child churches
    /// </summary>
    public ICollection<Church> Churches { get; set; } = new List<Church>();

    /// <summary>
    /// Navigation property to contacts
    /// </summary>
    public ICollection<Contact> Contacts { get; set; } = new List<Contact>();

    /// <summary>
    /// Foreign key to the pastor (optional)
    /// </summary>
    public Guid? PastorId { get; set; }

    /// <summary>
    /// Navigation property to pastor
    /// </summary>
    public Pastor? Pastor { get; set; }

    /// <summary>
    /// Gets the code path for this district (Division.Code.Union.Code.Association.Code.Region.Code.District.Code)
    /// </summary>
    public override string CodePath => $"{Region.CodePath.Trim()}.{Code.Trim()}";
}
