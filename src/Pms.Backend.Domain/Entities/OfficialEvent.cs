namespace Pms.Backend.Domain.Entities;

/// <summary>
/// Represents an official event organized by any level of the hierarchy
/// </summary>
public class OfficialEvent : BaseEntity
{
    /// <summary>
    /// Name of the event
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the event
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// When the event starts
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// When the event ends
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Where the event takes place
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// Level of the organizer
    /// </summary>
    public OrganizerLevel OrganizerLevel { get; set; }

    /// <summary>
    /// ID of the organizing entity
    /// </summary>
    public Guid OrganizerId { get; set; }

    /// <summary>
    /// Fee amount for the event (null = free)
    /// </summary>
    public decimal? FeeAmount { get; set; }

    /// <summary>
    /// Currency for the fee (default: BRL)
    /// </summary>
    public string FeeCurrency { get; set; } = "BRL";

    /// <summary>
    /// Indicates if the event is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Navigation property to event participations
    /// </summary>
    public ICollection<MemberEventParticipation> Participations { get; set; } = new List<MemberEventParticipation>();

    /// <summary>
    /// Navigation property to timeline entries
    /// </summary>
    public ICollection<TimelineEntry> TimelineEntries { get; set; } = new List<TimelineEntry>();

    /// <summary>
    /// Checks if the event is currently happening
    /// </summary>
    public bool IsCurrentlyHappening => DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate;

    /// <summary>
    /// Checks if the event has ended
    /// </summary>
    public bool HasEnded => DateTime.UtcNow > EndDate;
}

/// <summary>
/// Enumeration for organizer levels
/// </summary>
public enum OrganizerLevel
{
    /// <summary>
    /// Organized by Division
    /// </summary>
    Division,

    /// <summary>
    /// Organized by Union
    /// </summary>
    Union,

    /// <summary>
    /// Organized by Association
    /// </summary>
    Association,

    /// <summary>
    /// Organized by Region
    /// </summary>
    Region,

    /// <summary>
    /// Organized by District
    /// </summary>
    District,

    /// <summary>
    /// Organized by Club
    /// </summary>
    Club
}
