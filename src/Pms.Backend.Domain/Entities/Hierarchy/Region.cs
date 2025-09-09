using Pms.Backend.Domain.Common;

namespace Pms.Backend.Domain.Entities.Hierarchy;

/// <summary>
/// Represents a Region in the organizational hierarchy
/// Region belongs to an Association
/// </summary>
public class Region : HierarchyEntityBase
{

    /// <summary>
    /// Foreign key to the parent association
    /// </summary>
    public Guid AssociationId { get; set; }

    /// <summary>
    /// Navigation property to parent association
    /// </summary>
    public Association Association { get; set; } = null!;

    /// <summary>
    /// Navigation property to child districts
    /// </summary>
    public ICollection<District> Districts { get; set; } = new List<District>();

    /// <summary>
    /// Navigation property to contacts
    /// </summary>
    public ICollection<Contact> Contacts { get; set; } = new List<Contact>();

    /// <summary>
    /// Gets the code path for this region (Division.Code.Union.Code.Association.Code.Region.Code)
    /// </summary>
    public override string CodePath => $"{Association.CodePath.Trim()}.{Code.Trim()}";
}
