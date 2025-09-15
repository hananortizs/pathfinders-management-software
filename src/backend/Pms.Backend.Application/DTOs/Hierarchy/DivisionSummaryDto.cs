namespace Pms.Backend.Application.DTOs.Hierarchy;

/// <summary>
/// Lightweight Data Transfer Object for Division listing operations
/// Contains only essential information without related entities
/// </summary>
public class DivisionSummaryDto
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
}
