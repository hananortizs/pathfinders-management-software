using Pms.Backend.Application.DTOs.Members;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Application.DTOs.Assignments;

/// <summary>
/// DTO for Assignment entity
/// </summary>
public class AssignmentDto : BaseEntity
{
    /// <summary>
    /// Member information
    /// </summary>
    public MemberDto Member { get; set; } = null!;

    /// <summary>
    /// Role catalog information
    /// </summary>
    public RoleCatalogDto RoleCatalog { get; set; } = null!;

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
    public bool IsActive { get; set; }
}

/// <summary>
/// DTO for creating a new assignment
/// </summary>
public class CreateAssignmentDto
{
    /// <summary>
    /// Member ID
    /// </summary>
    public Guid MemberId { get; set; }

    /// <summary>
    /// Role catalog ID
    /// </summary>
    public Guid RoleId { get; set; }

    /// <summary>
    /// Type of scope where the role is assigned
    /// </summary>
    public ScopeType ScopeType { get; set; }

    /// <summary>
    /// ID of the scope entity
    /// </summary>
    public Guid ScopeId { get; set; }

    /// <summary>
    /// When the assignment starts
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// When the assignment ends (optional)
    /// </summary>
    public DateTime? EndDate { get; set; }
}

/// <summary>
/// DTO for updating an assignment
/// </summary>
public class UpdateAssignmentDto
{
    /// <summary>
    /// When the assignment ends
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Reason for ending the assignment
    /// </summary>
    public string? EndReason { get; set; }

    /// <summary>
    /// Indicates if the assignment is active
    /// </summary>
    public bool IsActive { get; set; }
}

/// <summary>
/// DTO for RoleCatalog entity
/// </summary>
public class RoleCatalogDto : BaseEntity
{
    /// <summary>
    /// Role name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Role description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Hierarchy level where this role can be assigned
    /// </summary>
    public string HierarchyLevel { get; set; } = string.Empty;

    /// <summary>
    /// Indicates if this role requires spiritual requirements (Batismo + Lenço)
    /// </summary>
    public bool RequiresSpiritualRequirements { get; set; }

    /// <summary>
    /// Indicates if this role is active
    /// </summary>
    public bool IsActive { get; set; }
}
