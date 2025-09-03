namespace Pms.Backend.Domain.Entities.Hierarchy;

/// <summary>
/// Represents a Region in the organizational hierarchy
/// Region belongs to an Association
/// </summary>
public class Region : BaseEntity
{
    /// <summary>
    /// Unique code for the region (≤5 chars, UPPERCASE A-Z0-9)
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Name of the region
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the region
    /// </summary>
    public string? Description { get; set; }

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
    /// Gets the code path for this region (Division.Code.Union.Code.Association.Code.Region.Code)
    /// </summary>
    public string CodePath => $"{Association.CodePath}.{Code}";
}
