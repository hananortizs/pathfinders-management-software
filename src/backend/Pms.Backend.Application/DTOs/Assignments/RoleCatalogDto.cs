using System.ComponentModel.DataAnnotations;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Domain.Enums;

namespace Pms.Backend.Application.DTOs.Assignments;

/// <summary>
/// Data Transfer Object for RoleCatalog entity
/// </summary>
public class RoleCatalogDto : BaseEntity
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
}

/// <summary>
/// Data Transfer Object for creating a new RoleCatalog
/// </summary>
public class CreateRoleCatalogDto
{
    /// <summary>
    /// Level where this role can be assigned
    /// </summary>
    [Required(ErrorMessage = "Level is required")]
    public RoleLevel Level { get; set; }

    /// <summary>
    /// Name of the role
    /// </summary>
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the role
    /// </summary>
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }

    /// <summary>
    /// Maximum number of people that can have this role per scope
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "MaxPerScope must be at least 1")]
    public int MaxPerScope { get; set; } = 1;

    /// <summary>
    /// Gender requirement for this role (null = no requirement)
    /// </summary>
    public MemberGender? GenderRequired { get; set; }

    /// <summary>
    /// Minimum age requirement for this role
    /// </summary>
    [Range(0, 120, ErrorMessage = "AgeMin must be between 0 and 120")]
    public int? AgeMin { get; set; }

    /// <summary>
    /// Maximum age requirement for this role
    /// </summary>
    [Range(0, 120, ErrorMessage = "AgeMax must be between 0 and 120")]
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
}

/// <summary>
/// Data Transfer Object for updating a RoleCatalog
/// </summary>
public class UpdateRoleCatalogDto
{
    /// <summary>
    /// Name of the role
    /// </summary>
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the role
    /// </summary>
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }

    /// <summary>
    /// Maximum number of people that can have this role per scope
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "MaxPerScope must be at least 1")]
    public int MaxPerScope { get; set; } = 1;

    /// <summary>
    /// Gender requirement for this role (null = no requirement)
    /// </summary>
    public MemberGender? GenderRequired { get; set; }

    /// <summary>
    /// Minimum age requirement for this role
    /// </summary>
    [Range(0, 120, ErrorMessage = "AgeMin must be between 0 and 120")]
    public int? AgeMin { get; set; }

    /// <summary>
    /// Maximum age requirement for this role
    /// </summary>
    [Range(0, 120, ErrorMessage = "AgeMax must be between 0 and 120")]
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
}
