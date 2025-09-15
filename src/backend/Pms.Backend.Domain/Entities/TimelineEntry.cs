namespace Pms.Backend.Domain.Entities;

/// <summary>
/// Represents a timeline entry for tracking member activities and changes
/// Timeline is append-only and provides audit trail
/// </summary>
public class TimelineEntry : BaseEntity
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
    /// Type of timeline entry
    /// </summary>
    public TimelineEntryType Type { get; set; }

    /// <summary>
    /// Title of the timeline entry
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Description of the timeline entry
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Additional data related to the entry (JSON)
    /// </summary>
    public string? Data { get; set; }

    /// <summary>
    /// When the event occurred (UTC)
    /// </summary>
    public DateTime EventDateUtc { get; set; }

    /// <summary>
    /// Foreign key to related membership (if applicable)
    /// </summary>
    public Guid? MembershipId { get; set; }

    /// <summary>
    /// Navigation property to related membership
    /// </summary>
    public Membership? Membership { get; set; }

    /// <summary>
    /// Foreign key to related assignment (if applicable)
    /// </summary>
    public Guid? AssignmentId { get; set; }

    /// <summary>
    /// Navigation property to related assignment
    /// </summary>
    public Assignment? Assignment { get; set; }

    /// <summary>
    /// Foreign key to related event (if applicable)
    /// </summary>
    public Guid? EventId { get; set; }

    /// <summary>
    /// Navigation property to related event
    /// </summary>
    public OfficialEvent? Event { get; set; }
}

/// <summary>
/// Enumeration for timeline entry types
/// </summary>
public enum TimelineEntryType
{
    /// <summary>
    /// Membership started
    /// </summary>
    MembershipStarted,

    /// <summary>
    /// Membership ended
    /// </summary>
    MembershipEnded,

    /// <summary>
    /// Unit allocated
    /// </summary>
    UnitAllocated,

    /// <summary>
    /// Unit reallocated
    /// </summary>
    UnitReallocated,

    /// <summary>
    /// Unit override (manual allocation)
    /// </summary>
    UnitOverride,

    /// <summary>
    /// Role assigned
    /// </summary>
    RoleAssigned,

    /// <summary>
    /// Role removed
    /// </summary>
    RoleRemoved,

    /// <summary>
    /// Event participation
    /// </summary>
    EventParticipation,

    /// <summary>
    /// Scarf invested
    /// </summary>
    ScarfInvested,

    /// <summary>
    /// Auto-promotion (e.g., Conselheiro Associado to Conselheiro at 18)
    /// </summary>
    AutoPromotion,

    /// <summary>
    /// Manual promotion
    /// </summary>
    ManualPromotion,

    /// <summary>
    /// Manual demotion
    /// </summary>
    ManualDemotion
}
