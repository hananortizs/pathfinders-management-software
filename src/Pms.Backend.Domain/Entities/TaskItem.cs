namespace Pms.Backend.Domain.Entities;

/// <summary>
/// Represents a task item in the system
/// Tasks are created automatically for various scenarios (allocation, capacity, etc.)
/// </summary>
public class TaskItem : BaseEntity
{
    /// <summary>
    /// Type of the task
    /// </summary>
    public TaskType Type { get; set; }

    /// <summary>
    /// Title of the task
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Description of the task
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// JSON payload with task-specific data
    /// </summary>
    public string? Payload { get; set; }

    /// <summary>
    /// Priority of the task
    /// </summary>
    public TaskPriority Priority { get; set; } = TaskPriority.Normal;

    /// <summary>
    /// Due date for the task (optional)
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Status of the task
    /// </summary>
    public TaskStatus Status { get; set; } = TaskStatus.Open;

    /// <summary>
    /// Foreign key to the member who created the task (if applicable)
    /// </summary>
    public Guid? CreatedByMemberId { get; set; }

    /// <summary>
    /// Navigation property to the member who created the task
    /// </summary>
    public Member? CreatedByMember { get; set; }

    /// <summary>
    /// Foreign key to the club where the task is assigned (if applicable)
    /// </summary>
    public Guid? ClubId { get; set; }

    /// <summary>
    /// Navigation property to the club
    /// </summary>
    public Club? Club { get; set; }

    /// <summary>
    /// Foreign key to the unit where the task is assigned (if applicable)
    /// </summary>
    public Guid? UnitId { get; set; }

    /// <summary>
    /// Navigation property to the unit
    /// </summary>
    public Unit? Unit { get; set; }

    /// <summary>
    /// Foreign key to the member the task is about (if applicable)
    /// </summary>
    public Guid? RelatedMemberId { get; set; }

    /// <summary>
    /// Navigation property to the related member
    /// </summary>
    public Member? RelatedMember { get; set; }

    /// <summary>
    /// When the task was completed
    /// </summary>
    public DateTime? CompletedAtUtc { get; set; }

    /// <summary>
    /// Who completed the task
    /// </summary>
    public string? CompletedBy { get; set; }

    /// <summary>
    /// Notes about task completion
    /// </summary>
    public string? CompletionNotes { get; set; }

    /// <summary>
    /// Checks if the task is overdue
    /// </summary>
    public bool IsOverdue => DueDate.HasValue && DueDate.Value < DateTime.UtcNow && Status != TaskStatus.Done;
}

/// <summary>
/// Enumeration for task types
/// </summary>
public enum TaskType
{
    /// <summary>
    /// Allocate member to unit
    /// </summary>
    AllocateUnit,

    /// <summary>
    /// Choose unit for member (multiple options available)
    /// </summary>
    ChooseUnit,

    /// <summary>
    /// Unit capacity exceeded
    /// </summary>
    UnitCapacityExceeded,

    /// <summary>
    /// Member reallocation needed (multiple options)
    /// </summary>
    MemberReallocation,

    /// <summary>
    /// Pending approval
    /// </summary>
    PendingApproval,

    /// <summary>
    /// Validate leadership investiture (no pastor witness)
    /// </summary>
    ValidateLeadershipInvestiture
}

/// <summary>
/// Enumeration for task priority
/// </summary>
public enum TaskPriority
{
    /// <summary>
    /// Low priority
    /// </summary>
    Low,

    /// <summary>
    /// Normal priority
    /// </summary>
    Normal,

    /// <summary>
    /// High priority
    /// </summary>
    High
}

/// <summary>
/// Enumeration for task status
/// </summary>
public enum TaskStatus
{
    /// <summary>
    /// Task is open
    /// </summary>
    Open,

    /// <summary>
    /// Task is in progress
    /// </summary>
    InProgress,

    /// <summary>
    /// Task is blocked
    /// </summary>
    Blocked,

    /// <summary>
    /// Task is done
    /// </summary>
    Done
}
