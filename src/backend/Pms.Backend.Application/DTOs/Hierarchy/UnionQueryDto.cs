namespace Pms.Backend.Application.DTOs.Hierarchy;

/// <summary>
/// Data Transfer Object for Union entity in hierarchy queries (without parent objects)
/// </summary>
public class UnionQueryDto
{
    /// <summary>
    /// Union ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Unique code for the union (≤5 chars, UPPERCASE A-Z0-9)
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Name of the union
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the union
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Parent division ID
    /// </summary>
    public Guid DivisionId { get; set; }

    /// <summary>
    /// Code path (Division.Code.Union.Code)
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
    /// Child associations
    /// </summary>
    public ICollection<AssociationQueryDto> Associations { get; set; } = new List<AssociationQueryDto>();
}
