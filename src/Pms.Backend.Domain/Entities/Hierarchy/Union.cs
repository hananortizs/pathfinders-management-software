namespace Pms.Backend.Domain.Entities.Hierarchy;

/// <summary>
/// Represents a Union in the organizational hierarchy
/// Union belongs to a Division
/// </summary>
public class Union : BaseEntity
{
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
    /// Foreign key to the parent division
    /// </summary>
    public Guid DivisionId { get; set; }

    /// <summary>
    /// Navigation property to parent division
    /// </summary>
    public Division Division { get; set; } = null!;

    /// <summary>
    /// Navigation property to child associations
    /// </summary>
    public ICollection<Association> Associations { get; set; } = new List<Association>();

    /// <summary>
    /// Navigation property to contacts
    /// </summary>
    public ICollection<Contact> Contacts { get; set; } = new List<Contact>();

    /// <summary>
    /// Gets the code path for this union (Division.Code.Union.Code)
    /// </summary>
    public string CodePath => $"{Division.Code.Trim()}.{Code.Trim()}";
}
