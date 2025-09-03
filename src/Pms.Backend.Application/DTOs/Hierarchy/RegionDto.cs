namespace Pms.Backend.Application.DTOs.Hierarchy;

/// <summary>
/// Data Transfer Object for Region entity
/// </summary>
public class RegionDto
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
    /// Parent association
    /// </summary>
    public AssociationDto? Association { get; set; }

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
    public ICollection<DistrictDto> Districts { get; set; } = new List<DistrictDto>();
}

/// <summary>
/// Data Transfer Object for creating a new Region
/// </summary>
public class CreateRegionDto
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
    /// Parent association ID
    /// </summary>
    public Guid AssociationId { get; set; }
}

/// <summary>
/// Data Transfer Object for updating a Region
/// </summary>
public class UpdateRegionDto
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
}
