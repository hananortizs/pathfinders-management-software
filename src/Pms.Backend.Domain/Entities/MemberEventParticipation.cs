namespace Pms.Backend.Domain.Entities;

/// <summary>
/// Represents a member's participation in an official event
/// </summary>
public class MemberEventParticipation : BaseEntity
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
    /// Foreign key to the event
    /// </summary>
    public Guid EventId { get; set; }

    /// <summary>
    /// Navigation property to the event
    /// </summary>
    public OfficialEvent Event { get; set; } = null!;

    /// <summary>
    /// When the participation was registered
    /// </summary>
    public DateTime RegisteredAtUtc { get; set; }

    /// <summary>
    /// Status of the participation
    /// </summary>
    public ParticipationStatus Status { get; set; } = ParticipationStatus.Registered;

    /// <summary>
    /// Fee paid for the event (if any)
    /// </summary>
    public decimal? FeePaid { get; set; }

    /// <summary>
    /// Currency of the fee paid
    /// </summary>
    public string? FeeCurrency { get; set; }

    /// <summary>
    /// When the fee was paid
    /// </summary>
    public DateTime? FeePaidAtUtc { get; set; }

    /// <summary>
    /// Notes about the participation
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Navigation property to timeline entries
    /// </summary>
    public ICollection<TimelineEntry> TimelineEntries { get; set; } = new List<TimelineEntry>();
}

/// <summary>
/// Enumeration for participation status
/// </summary>
public enum ParticipationStatus
{
    /// <summary>
    /// Registered for the event
    /// </summary>
    Registered,

    /// <summary>
    /// Confirmed participation
    /// </summary>
    Confirmed,

    /// <summary>
    /// Attended the event
    /// </summary>
    Attended,

    /// <summary>
    /// Cancelled participation
    /// </summary>
    Cancelled,

    /// <summary>
    /// No-show (registered but didn't attend)
    /// </summary>
    NoShow
}
