namespace Pms.Backend.Application.DTOs.Hierarchy;

/// <summary>
/// Data Transfer Object for District entity in hierarchy queries (without parent objects)
/// </summary>
public class DistrictQueryDto
{
    /// <summary>
    /// District ID
    /// </summary>
    public Guid Id { get; set; }

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
    public ICollection<ClubQueryDto> Clubs { get; set; } = new List<ClubQueryDto>();
}
