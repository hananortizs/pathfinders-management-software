using System.ComponentModel.DataAnnotations;
using Pms.Backend.Application.DTOs.Common;

namespace Pms.Backend.Application.DTOs.Hierarchy;

/// <summary>
/// Data Transfer Object for Region entity
/// </summary>
public class RegionDto : AddressableEntityDtoBase
{
    /// <summary>
    /// Entity type for address relationships
    /// </summary>
    public override string EntityType => "Region";

    /// <summary>
    /// Unique code for the region (≤7 chars, letters and numbers - will be converted to uppercase)
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
    public ICollection<DistrictDto> Districts { get; set; } = new List<DistrictDto>();
}

/// <summary>
/// Data Transfer Object for creating a new Region
/// </summary>
public class CreateRegionDto : CreateHierarchyDtoBase
{
    /// <summary>
    /// Unique code for the region (≤7 chars, letters and numbers - will be converted to uppercase)
    /// </summary>
    [StringLength(7, MinimumLength = 1, ErrorMessage = "Region code must be between 1 and 7 characters")]
    public new string Code { get; set; } = string.Empty;

    /// <summary>
    /// Parent association ID
    /// </summary>
    [Required(ErrorMessage = "Association ID is required")]
    public Guid AssociationId { get; set; }
}

/// <summary>
/// Data Transfer Object for updating a Region
/// </summary>
public class UpdateRegionDto : UpdateHierarchyDtoBase
{
    /// <summary>
    /// Unique code for the region (≤7 chars, letters and numbers - will be converted to uppercase)
    /// </summary>
    [StringLength(7, MinimumLength = 1, ErrorMessage = "Region code must be between 1 and 7 characters")]
    public new string Code { get; set; } = string.Empty;
}
