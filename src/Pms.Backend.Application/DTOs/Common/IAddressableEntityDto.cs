namespace Pms.Backend.Application.DTOs.Common;

/// <summary>
/// Interface for DTOs that can have addresses
/// Provides the EntityType property for address relationships
/// </summary>
public interface IAddressableEntityDto
{
    /// <summary>
    /// Unique identifier of the entity
    /// </summary>
    Guid Id { get; set; }

    /// <summary>
    /// Entity type for address relationships
    /// This property should be implemented as a getter that returns the entity type name
    /// </summary>
    string EntityType { get; }
}
