namespace Pms.Backend.Domain.Entities;

/// <summary>
/// Represents a role catalog entry defining available roles in the system
/// </summary>
public class RoleCatalog : BaseEntity
{
    /// <summary>
    /// Level where this role can be assigned
    /// </summary>
    public RoleLevel Level { get; set; }

    /// <summary>
    /// Name of the role
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the role
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Maximum number of people that can have this role per scope
    /// </summary>
    public int MaxPerScope { get; set; } = 1;

    /// <summary>
    /// Gender requirement for this role (null = no requirement)
    /// </summary>
    public MemberGender? GenderRequired { get; set; }

    /// <summary>
    /// Minimum age requirement for this role
    /// </summary>
    public int? AgeMin { get; set; }

    /// <summary>
    /// Maximum age requirement for this role
    /// </summary>
    public int? AgeMax { get; set; }

    /// <summary>
    /// Indicates if this role requires baptism
    /// </summary>
    public bool RequiresBaptism { get; set; }

    /// <summary>
    /// Indicates if this role requires scarf (lenço)
    /// </summary>
    public bool RequiresScarf { get; set; }

    /// <summary>
    /// Indicates if this role is a leadership role
    /// </summary>
    public bool IsLeadership { get; set; }

    /// <summary>
    /// Indicates if this role is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Navigation property to assignments
    /// </summary>
    public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
}

/// <summary>
/// Enumeration for role levels
/// </summary>
public enum RoleLevel
{
    /// <summary>
    /// Union level role
    /// </summary>
    Union,

    /// <summary>
    /// Association level role
    /// </summary>
    Association,

    /// <summary>
    /// Region level role
    /// </summary>
    Region,

    /// <summary>
    /// District level role
    /// </summary>
    District,

    /// <summary>
    /// Club level role
    /// </summary>
    Club,

    /// <summary>
    /// Unit level role
    /// </summary>
    Unit
}
