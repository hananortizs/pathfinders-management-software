using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Domain.Entities;

/// <summary>
/// Represents a membership of a member in a club
/// Each member can have at most one active membership
/// </summary>
public class Membership : BaseEntity
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
    /// Foreign key to the club
    /// </summary>
    public Guid ClubId { get; set; }

    /// <summary>
    /// Navigation property to the club
    /// </summary>
    public Club Club { get; set; } = null!;

    /// <summary>
    /// Foreign key to the unit (optional - can be null if not allocated)
    /// </summary>
    public Guid? UnitId { get; set; }

    /// <summary>
    /// Navigation property to the unit
    /// </summary>
    public Unit? Unit { get; set; }

    /// <summary>
    /// When the membership started
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// When the membership ended (null if still active)
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Reason for ending the membership
    /// </summary>
    public string? EndReason { get; set; }

    /// <summary>
    /// Indicates if the membership is currently active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Age of the member on June 1st of the membership year
    /// Used for the "1º de junho" rule
    /// </summary>
    public int AgeOnJuneFirst => Member.GetAgeOnJuneFirst(StartDate.Year);

    /// <summary>
    /// Navigation property to timeline entries
    /// </summary>
    public ICollection<TimelineEntry> TimelineEntries { get; set; } = new List<TimelineEntry>();
}
