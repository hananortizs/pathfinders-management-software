namespace Pms.Backend.Domain.Entities.Hierarchy;

/// <summary>
/// Represents a Division in the organizational hierarchy
/// Division is the top-level entity in the hierarchy
/// </summary>
public class Division : BaseEntity
{
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
    /// Navigation property to child unions
    /// </summary>
    public ICollection<Union> Unions { get; set; } = new List<Union>();

    /// <summary>
    /// Gets the code path for this division (same as Code since it's root)
    /// </summary>
    public string CodePath => Code;
}
