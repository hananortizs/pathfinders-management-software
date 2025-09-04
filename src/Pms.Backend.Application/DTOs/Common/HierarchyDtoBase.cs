using System.ComponentModel.DataAnnotations;

namespace Pms.Backend.Application.DTOs.Common;

/// <summary>
/// Base class for hierarchy DTOs with common properties and validation
/// Provides standardized implementation for Code, Name, Description with consistent validation rules
/// </summary>
public abstract class HierarchyDtoBase
{
    /// <summary>
    /// Unique code for the entity (letters and numbers - will be converted to uppercase)
    /// </summary>
    [Required(ErrorMessage = "Code is required")]
    [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "Code must contain only letters and numbers")]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Name of the entity
    /// </summary>
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the entity
    /// </summary>
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }
}

/// <summary>
/// Base class for creating hierarchy entities with common properties and validation
/// </summary>
public abstract class CreateHierarchyDtoBase : HierarchyDtoBase
{
    // Inherits all properties from HierarchyDtoBase
    // Can be extended with additional properties specific to creation operations
}

/// <summary>
/// Base class for updating hierarchy entities with common properties and validation
/// </summary>
public abstract class UpdateHierarchyDtoBase : HierarchyDtoBase
{
    // Inherits all properties from HierarchyDtoBase
    // Can be extended with additional properties specific to update operations
}
