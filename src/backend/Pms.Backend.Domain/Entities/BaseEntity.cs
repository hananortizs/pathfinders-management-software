namespace Pms.Backend.Domain.Entities;

/// <summary>
/// Base entity with common properties for all domain entities
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Unique identifier for the entity
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// When the entity was created (UTC)
    /// </summary>
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// When the entity was last updated (UTC)
    /// </summary>
    public DateTime UpdatedAtUtc { get; set; }

    /// <summary>
    /// Soft delete flag - when true, entity is considered deleted
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// When the entity was soft deleted (UTC)
    /// </summary>
    public DateTime? DeletedAtUtc { get; set; }
}
