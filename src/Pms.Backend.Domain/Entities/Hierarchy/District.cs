namespace Pms.Backend.Domain.Entities.Hierarchy;

/// <summary>
/// Represents a District in the organizational hierarchy
/// District belongs to a Region
/// </summary>
public class District : BaseEntity
{
    /// <summary>
    /// Unique code for the district (≤5 chars, UPPERCASE A-Z0-9)
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Name of the district
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the district
    /// </summary>
    public string? Description { get; set; }

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
    /// Gets the code path for this district (Division.Code.Union.Code.Association.Code.Region.Code.District.Code)
    /// </summary>
    public string CodePath => $"{Region.CodePath}.{Code}";
}
