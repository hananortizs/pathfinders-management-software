using Pms.Backend.Application.DTOs.Assignments;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Application.DTOs.Assignments;

/// <summary>
/// DTO for ApprovalDelegate entity
/// </summary>
public class ApprovalDelegateDto : BaseEntity
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
    /// Assignment that receives the delegation
    /// </summary>
    public AssignmentDto DelegatedToAssignment { get; set; } = null!;

    /// <summary>
    /// Assignment that delegates the authority
    /// </summary>
    public AssignmentDto DelegatedFromAssignment { get; set; } = null!;

    /// <summary>
    /// Reason for the delegation
    /// </summary>
    public string? Reason { get; set; }

    /// <summary>
    /// Indicates if the delegation is currently active
    /// </summary>
    public bool IsActive { get; set; }
}

/// <summary>
/// DTO for creating a new approval delegate
/// </summary>
public class CreateApprovalDelegateDto
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
    /// ID of the assignment that receives the delegation
    /// </summary>
    public Guid DelegatedToAssignmentId { get; set; }

    /// <summary>
    /// ID of the assignment that delegates the authority
    /// </summary>
    public Guid DelegatedFromAssignmentId { get; set; }

    /// <summary>
    /// Reason for the delegation
    /// </summary>
    public string? Reason { get; set; }
}

/// <summary>
/// DTO for updating an approval delegate
/// </summary>
public class UpdateApprovalDelegateDto
{
    /// <summary>
    /// When the delegation ends
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Reason for the delegation
    /// </summary>
    public string? Reason { get; set; }
}
