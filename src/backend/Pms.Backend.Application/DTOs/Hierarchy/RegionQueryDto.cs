namespace Pms.Backend.Application.DTOs.Hierarchy;

/// <summary>
/// Data Transfer Object for Region entity in hierarchy queries (without parent objects)
/// </summary>
public class RegionQueryDto
{
    /// <summary>
    /// Region ID
    /// </summary>
    public Guid Id { get; set; }

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
    /// Parent association ID
    /// </summary>
    public Guid AssociationId { get; set; }

    /// <summary>
    /// Code path (Division.Code.Union.Code.Association.Code.Region.Code)
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

    /// <summary>
    /// Child districts
    /// </summary>
    public ICollection<DistrictQueryDto> Districts { get; set; } = new List<DistrictQueryDto>();
}
