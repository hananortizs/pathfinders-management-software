using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Domain.Common;

/// <summary>
/// Base class for hierarchy entities with common properties and behavior
/// Provides standardized implementation for Code, Name, Description, and CodePath
/// </summary>
public abstract class HierarchyEntityBase : BaseEntity, IHierarchyEntity
{
    /// <summary>
    /// Unique code for the entity (letters and numbers - stored in uppercase without trailing spaces)
    /// </summary>
    private string _code = string.Empty;

    /// <summary>
    /// Gets or sets the entity code (automatically trimmed and stored without trailing spaces)
    /// </summary>
    public string Code
    {
        get => _code;
        set => _code = value?.Trim() ?? string.Empty;
    }

    /// <summary>
    /// Name of the entity
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the entity
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets the hierarchical code path for this entity
    /// Must be implemented by each hierarchy level to build the complete path
    /// </summary>
    public abstract string CodePath { get; }
}
