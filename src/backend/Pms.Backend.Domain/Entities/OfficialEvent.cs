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
    /// Co-organizers of the event (MVP-2 feature)
    /// </summary>
    public ICollection<EventCoHost> CoHosts { get; set; } = new List<EventCoHost>();

    /// <summary>
    /// Minimum age requirement for participation (null = no restriction)
    /// </summary>
    public int? MinAge { get; set; }

    /// <summary>
    /// Maximum age requirement for participation (null = no restriction)
    /// </summary>
    public int? MaxAge { get; set; }

    /// <summary>
    /// Indicates if medical information is required for participation
    /// </summary>
    public bool RequiresMedicalInfo { get; set; } = false;

    /// <summary>
    /// Indicates if scarf investment is required for participation
    /// </summary>
    public bool RequiresScarfInvested { get; set; } = false;

    /// <summary>
    /// Visibility level of the event
    /// </summary>
    public EventVisibility Visibility { get; set; } = EventVisibility.Public;

    /// <summary>
    /// Audience mode for participation eligibility
    /// </summary>
    public EventAudienceMode AudienceMode { get; set; } = EventAudienceMode.Subtree;

    /// <summary>
    /// Allow leaders above host level to participate
    /// </summary>
    public bool AllowLeadersAboveHost { get; set; } = true;

    /// <summary>
    /// Custom allow list for participation (JSON)
    /// </summary>
    public string? AllowList { get; set; }

    /// <summary>
    /// When registration opens (UTC)
    /// </summary>
    public DateTime? RegistrationOpenAtUtc { get; set; }

    /// <summary>
    /// When registration closes (UTC)
    /// </summary>
    public DateTime? RegistrationCloseAtUtc { get; set; }

    /// <summary>
    /// Maximum capacity for the event (null = unlimited)
    /// </summary>
    public int? Capacity { get; set; }

    /// <summary>
    /// Current number of registered participants
    /// </summary>
    public int RegisteredCount { get; set; } = 0;

    /// <summary>
    /// Current number of waitlisted participants
    /// </summary>
    public int WaitlistedCount { get; set; } = 0;

    /// <summary>
    /// Current number of checked-in participants
    /// </summary>
    public int CheckedInCount { get; set; } = 0;

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

    /// <summary>
    /// Checks if registration is currently open
    /// </summary>
    public bool IsRegistrationOpen
    {
        get
        {
            var now = DateTime.UtcNow;
            return (RegistrationOpenAtUtc == null || now >= RegistrationOpenAtUtc) &&
                   (RegistrationCloseAtUtc == null || now <= RegistrationCloseAtUtc);
        }
    }

    /// <summary>
    /// Gets the remaining capacity
    /// </summary>
    public int? CapacityRemaining => Capacity.HasValue ? Capacity.Value - RegisteredCount : null;

    /// <summary>
    /// Checks if the event is at capacity
    /// </summary>
    public bool IsAtCapacity => Capacity.HasValue && RegisteredCount >= Capacity.Value;
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

/// <summary>
/// Enumeration for event visibility levels
/// </summary>
public enum EventVisibility
{
    /// <summary>
    /// Public event - visible to everyone
    /// </summary>
    Public,

    /// <summary>
    /// Leaders only - visible to leaders and above
    /// </summary>
    LeadersOnly,

    /// <summary>
    /// Directors only - visible to directors and above
    /// </summary>
    DirectorsOnly,

    /// <summary>
    /// Custom roles - visible to specific roles
    /// </summary>
    CustomRoles
}

/// <summary>
/// Enumeration for event audience modes
/// </summary>
public enum EventAudienceMode
{
    /// <summary>
    /// Subtree - all entities under the organizer
    /// </summary>
    Subtree,

    /// <summary>
    /// Host only - only the organizing entity
    /// </summary>
    HostOnly,

    /// <summary>
    /// Custom allow list - specific entities in AllowList
    /// </summary>
    CustomAllowList
}

/// <summary>
/// Represents a co-host of an event
/// </summary>
public class EventCoHost : BaseEntity
{
    /// <summary>
    /// Foreign key to the event
    /// </summary>
    public Guid EventId { get; set; }

    /// <summary>
    /// Navigation property to the event
    /// </summary>
    public OfficialEvent Event { get; set; } = null!;

    /// <summary>
    /// Level of the co-host entity
    /// </summary>
    public EventLevel Level { get; set; }

    /// <summary>
    /// ID of the co-host entity
    /// </summary>
    public Guid EntityId { get; set; }
}

/// <summary>
/// Enumeration for event levels (used in co-hosts)
/// </summary>
public enum EventLevel
{
    /// <summary>
    /// Division level
    /// </summary>
    Division,

    /// <summary>
    /// Union level
    /// </summary>
    Union,

    /// <summary>
    /// Association level
    /// </summary>
    Association,

    /// <summary>
    /// Region level
    /// </summary>
    Region,

    /// <summary>
    /// District level
    /// </summary>
    District,

    /// <summary>
    /// Club level
    /// </summary>
    Club
}
