namespace Pms.Backend.Application.DTOs.Hierarchy;

/// <summary>
/// Data Transfer Object for Division entity in hierarchy queries (without parent objects)
/// </summary>
public class DivisionQueryDto
{
    /// <summary>
    /// Division ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Unique code for the division (≤5 chars, UPPERCASE A-Z0-9)
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Name of the division
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the division
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Creation date
    /// </summary>
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime UpdatedAtUtc { get; set; }

    /// <summary>
    /// Child unions
    /// </summary>
    public ICollection<UnionQueryDto> Unions { get; set; } = new List<UnionQueryDto>();
}
