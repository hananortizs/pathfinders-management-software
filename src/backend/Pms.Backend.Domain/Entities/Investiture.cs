namespace Pms.Backend.Domain.Entities;

/// <summary>
/// Represents an investiture ceremony for a member
/// In MVP-0, only basic scarf investitures are supported
/// </summary>
public class Investiture : BaseEntity
{
    /// <summary>
    /// Foreign key to the member
    /// </summary>
    public Guid MemberId { get; set; }

    /// <summary>
    /// Navigation property to the member
    /// </summary>
    public Member Member { get; set; } = null!;

    /// <summary>
    /// Type of investiture
    /// </summary>
    public InvestitureType Type { get; set; }

    /// <summary>
    /// Date when the investiture occurred
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Place where the investiture occurred
    /// </summary>
    public string Place { get; set; } = string.Empty;

    /// <summary>
    /// YouTube URL of the investiture ceremony (optional)
    /// </summary>
    public string? YoutubeUrl { get; set; }

    /// <summary>
    /// Reference to the item being invested (JSON)
    /// For MVP-0, this will be null for scarf investitures
    /// </summary>
    public string? RefItem { get; set; }

    /// <summary>
    /// Navigation property to investiture witnesses
    /// </summary>
    public ICollection<InvestitureWitness> Witnesses { get; set; } = new List<InvestitureWitness>();

    /// <summary>
    /// Navigation property to timeline entries
    /// </summary>
    public ICollection<TimelineEntry> TimelineEntries { get; set; } = new List<TimelineEntry>();
}

/// <summary>
/// Enumeration for investiture types
/// </summary>
public enum InvestitureType
{
    /// <summary>
    /// Scarf investiture (lenço)
    /// </summary>
    Scarf,

    /// <summary>
    /// Class investiture (MVP-1)
    /// </summary>
    Class,

    /// <summary>
    /// Specialty investiture (MVP-1)
    /// </summary>
    Specialty,

    /// <summary>
    /// Mastery investiture (MVP-1)
    /// </summary>
    Mastery
}
