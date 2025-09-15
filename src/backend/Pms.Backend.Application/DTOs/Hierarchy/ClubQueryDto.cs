namespace Pms.Backend.Application.DTOs.Hierarchy;

/// <summary>
/// Data Transfer Object for Club entity in hierarchy queries (without parent objects)
/// </summary>
public class ClubQueryDto
{
    /// <summary>
    /// Club ID
    /// </summary>
    public Guid Id { get; set; }

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
    /// Parent church ID (optional)
    /// </summary>
    public Guid? ChurchId { get; set; }

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
    public ICollection<UnitQueryDto> Units { get; set; } = new List<UnitQueryDto>();
}
