namespace Pms.Backend.Domain.Entities;

/// <summary>
/// Represents an assignment of a member to a role within a specific scope
/// </summary>
public class Assignment : BaseEntity
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
    /// Foreign key to the role catalog
    /// </summary>
    public Guid RoleId { get; set; }

    /// <summary>
    /// Navigation property to the role catalog
    /// </summary>
    public RoleCatalog RoleCatalog { get; set; } = null!;

    /// <summary>
    /// Type of scope where the role is assigned
    /// </summary>
    public ScopeType ScopeType { get; set; }

    /// <summary>
    /// ID of the scope entity
    /// </summary>
    public Guid ScopeId { get; set; }

    /// <summary>
    /// When the assignment started
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// When the assignment ended (null if still active)
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Reason for ending the assignment
    /// </summary>
    public string? EndReason { get; set; }

    /// <summary>
    /// Indicates if the assignment is currently active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Navigation property to timeline entries
    /// </summary>
    public ICollection<TimelineEntry> TimelineEntries { get; set; } = new List<TimelineEntry>();

    /// <summary>
    /// Navigation property to approval delegates (where this assignment is the delegate)
    /// </summary>
    public ICollection<ApprovalDelegate> DelegatedToAssignments { get; set; } = new List<ApprovalDelegate>();

    /// <summary>
    /// Navigation property to approval delegates (where this assignment is the delegator)
    /// </summary>
    public ICollection<ApprovalDelegate> DelegatedFromAssignments { get; set; } = new List<ApprovalDelegate>();
}

/// <summary>
/// Enumeration for scope types
/// </summary>
public enum ScopeType
{
    /// <summary>
    /// Union scope
    /// </summary>
    Union,

    /// <summary>
    /// Association scope
    /// </summary>
    Association,

    /// <summary>
    /// Region scope
    /// </summary>
    Region,

    /// <summary>
    /// District scope
    /// </summary>
    District,

    /// <summary>
    /// Club scope
    /// </summary>
    Club,

    /// <summary>
    /// Unit scope
    /// </summary>
    Unit
}
