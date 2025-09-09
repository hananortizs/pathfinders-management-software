namespace Pms.Backend.Domain.Entities;

/// <summary>
/// Represents a delegation of approval authority from one role to another
/// Used when the original approver is unavailable (vacation, leave, etc.)
/// </summary>
public class ApprovalDelegate : BaseEntity
{
    /// <summary>
    /// Role that is being delegated
    /// </summary>
    public string Role { get; set; } = string.Empty;

    /// <summary>
    /// Type of scope where the delegation applies
    /// </summary>
    public ScopeType ScopeType { get; set; }

    /// <summary>
    /// ID of the scope entity
    /// </summary>
    public Guid ScopeId { get; set; }

    /// <summary>
    /// When the delegation starts
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// When the delegation ends
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Foreign key to the assignment that receives the delegation
    /// </summary>
    public Guid DelegatedToAssignmentId { get; set; }

    /// <summary>
    /// Navigation property to the assignment that receives the delegation
    /// </summary>
    public Assignment DelegatedToAssignment { get; set; } = null!;

    /// <summary>
    /// Foreign key to the assignment that delegates the authority
    /// </summary>
    public Guid DelegatedFromAssignmentId { get; set; }

    /// <summary>
    /// Navigation property to the assignment that delegates the authority
    /// </summary>
    public Assignment DelegatedFromAssignment { get; set; } = null!;

    /// <summary>
    /// Reason for the delegation
    /// </summary>
    public string? Reason { get; set; }

    /// <summary>
    /// Additional notes for the delegation
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// When the delegation was ended
    /// </summary>
    public DateTime? EndedAtUtc { get; set; }

    /// <summary>
    /// Indicates if the delegation is currently active
    /// </summary>
    public bool IsActive => DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate;
}
