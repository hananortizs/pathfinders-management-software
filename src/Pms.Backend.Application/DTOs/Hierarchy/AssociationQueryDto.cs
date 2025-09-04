namespace Pms.Backend.Application.DTOs.Hierarchy;

/// <summary>
/// Data Transfer Object for Association entity in hierarchy queries (without parent objects)
/// </summary>
public class AssociationQueryDto
{
    /// <summary>
    /// Association ID
    /// </summary>
    public Guid Id { get; set; }

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
    /// Parent union ID
    /// </summary>
    public Guid UnionId { get; set; }

    /// <summary>
    /// Code path (Division.Code.Union.Code.Association.Code)
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
    /// Child regions
    /// </summary>
    public ICollection<RegionQueryDto> Regions { get; set; } = new List<RegionQueryDto>();
}
