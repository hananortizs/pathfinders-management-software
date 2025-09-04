namespace Pms.Backend.Application.DTOs.Hierarchy;

/// <summary>
/// Lightweight Data Transfer Object for Region listing operations
/// Contains only essential information without related entities
/// </summary>
public class RegionSummaryDto
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
    /// Hierarchical code path (e.g., "DSA.UCB.ASA.RSP")
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
}
