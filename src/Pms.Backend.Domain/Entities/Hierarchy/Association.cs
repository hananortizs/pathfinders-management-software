namespace Pms.Backend.Domain.Entities.Hierarchy;

/// <summary>
/// Represents an Association in the organizational hierarchy
/// Association belongs to a Union
/// </summary>
public class Association : BaseEntity
{
    /// <summary>
    /// Unique code for the association (≤5 chars, UPPERCASE A-Z0-9)
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Name of the association
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the association
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Foreign key to the parent union
    /// </summary>
    public Guid UnionId { get; set; }

    /// <summary>
    /// Navigation property to parent union
    /// </summary>
    public Union Union { get; set; } = null!;

    /// <summary>
    /// Navigation property to child regions
    /// </summary>
    public ICollection<Region> Regions { get; set; } = new List<Region>();

    /// <summary>
    /// Gets the code path for this association (Division.Code.Union.Code.Association.Code)
    /// </summary>
    public string CodePath => $"{Union.CodePath.Trim()}.{Code.Trim()}";
}
