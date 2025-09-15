using System.ComponentModel.DataAnnotations;
using Pms.Backend.Application.DTOs.Common;

namespace Pms.Backend.Application.DTOs.Hierarchy;

/// <summary>
/// Data Transfer Object for District entity
/// </summary>
public class DistrictDto : AddressableEntityDtoBase
{
    /// <summary>
    /// Entity type for address relationships
    /// </summary>
    public override string EntityType => "District";

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
    /// Parent region ID
    /// </summary>
    public Guid RegionId { get; set; }


    /// <summary>
    /// Code path (Division.Code.Union.Code.Association.Code.Region.Code.District.Code)
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
    /// Child clubs
    /// </summary>
    public ICollection<ClubDto> Clubs { get; set; } = new List<ClubDto>();

    /// <summary>
    /// Child churches
    /// </summary>
    public ICollection<ChurchDto> Churches { get; set; } = new List<ChurchDto>();
}

/// <summary>
/// Data Transfer Object for creating a new District
/// </summary>
public class CreateDistrictDto : CreateHierarchyDtoBase
{
    /// <summary>
    /// Unique code for the district (≤5 chars, letters and numbers - will be converted to uppercase)
    /// </summary>
    [StringLength(5, MinimumLength = 1, ErrorMessage = "District code must be between 1 and 5 characters")]
    public new string Code { get; set; } = string.Empty;

    /// <summary>
    /// Parent region ID
    /// </summary>
    [Required(ErrorMessage = "Region ID is required")]
    public Guid RegionId { get; set; }
}

/// <summary>
/// Data Transfer Object for updating a District
/// </summary>
public class UpdateDistrictDto : UpdateHierarchyDtoBase
{
    /// <summary>
    /// Unique code for the district (≤5 chars, letters and numbers - will be converted to uppercase)
    /// </summary>
    [StringLength(5, MinimumLength = 1, ErrorMessage = "District code must be between 1 and 5 characters")]
    public new string Code { get; set; } = string.Empty;
}
