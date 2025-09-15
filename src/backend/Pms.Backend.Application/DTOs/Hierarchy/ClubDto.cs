using Pms.Backend.Application.DTOs.Common;

namespace Pms.Backend.Application.DTOs.Hierarchy;

/// <summary>
/// Data Transfer Object for Club entity
/// </summary>
public class ClubDto : AddressableEntityDtoBase
{
    /// <summary>
    /// Entity type for address relationships
    /// </summary>
    public override string EntityType => "Club";

    /// <summary>
    /// Unique code for the club (≤5 chars, UPPERCASE A-Z0-9)
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Name of the club
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the club
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Parent district ID
    /// </summary>
    public Guid DistrictId { get; set; }


    /// <summary>
    /// Linked church ID
    /// </summary>
    public Guid ChurchId { get; set; }


    /// <summary>
    /// Indicates if the club is active (has an active Director)
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Code path (Division.Code.Union.Code.Association.Code.Region.Code.District.Code.Club.Code)
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
    /// Child units
    /// </summary>
    public ICollection<UnitDto> Units { get; set; } = new List<UnitDto>();
}

/// <summary>
/// Data Transfer Object for creating a new Club
/// </summary>
public class CreateClubDto
{
    /// <summary>
    /// Unique code for the club (≤5 chars, UPPERCASE A-Z0-9)
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Name of the club
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the club
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Parent district ID
    /// </summary>
    public Guid DistrictId { get; set; }

    /// <summary>
    /// Linked church ID
    /// </summary>
    public Guid ChurchId { get; set; }
}

/// <summary>
/// Data Transfer Object for updating a Club
/// </summary>
public class UpdateClubDto
{
    /// <summary>
    /// Unique code for the club (≤5 chars, UPPERCASE A-Z0-9)
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Name of the club
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the club
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Linked church ID
    /// </summary>
    public Guid ChurchId { get; set; }
}
