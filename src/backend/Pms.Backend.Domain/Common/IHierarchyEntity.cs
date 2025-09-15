namespace Pms.Backend.Domain.Common;

/// <summary>
/// Interface for hierarchy entities with common properties
/// Defines the standard properties that all hierarchy entities should have
/// </summary>
public interface IHierarchyEntity
{
    /// <summary>
    /// Unique code for the entity (letters and numbers - will be converted to uppercase)
    /// </summary>
    string Code { get; set; }

    /// <summary>
    /// Name of the entity
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// Description of the entity
    /// </summary>
    string? Description { get; set; }

    /// <summary>
    /// Gets the hierarchical code path for this entity
    /// </summary>
    string CodePath { get; }
}
