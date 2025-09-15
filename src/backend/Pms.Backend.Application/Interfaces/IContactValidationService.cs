using Pms.Backend.Domain.Enums;

namespace Pms.Backend.Application.Interfaces;

/// <summary>
/// Service interface for Contact validation operations
/// </summary>
public interface IContactValidationService
{
    /// <summary>
    /// Validates if an entity exists in the database
    /// </summary>
    /// <param name="entityId">Entity ID</param>
    /// <param name="entityType">Entity type</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if entity exists, false otherwise</returns>
    Task<bool> ValidateEntityExistsAsync(Guid entityId, string entityType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates if an entity can have contacts
    /// </summary>
    /// <param name="entityType">Entity type</param>
    /// <returns>True if entity can have contacts, false otherwise</returns>
    bool ValidateEntityCanHaveContacts(string entityType);

    /// <summary>
    /// Gets the table name for an entity type
    /// </summary>
    /// <param name="entityType">Entity type</param>
    /// <returns>Table name or null if invalid</returns>
    string? GetTableName(string entityType);

    /// <summary>
    /// Validates if a contact value is unique for the entity
    /// </summary>
    /// <param name="contactId">Contact ID (for updates, null for new contacts)</param>
    /// <param name="entityId">Entity ID</param>
    /// <param name="entityType">Entity type</param>
    /// <param name="contactType">Contact type</param>
    /// <param name="value">Contact value</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if unique, false otherwise</returns>
    Task<bool> ValidateContactValueUniqueAsync(Guid? contactId, Guid entityId, string entityType,
        ContactType contactType, string value, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates if there's only one primary contact per category for an entity
    /// </summary>
    /// <param name="contactId">Contact ID (for updates, null for new contacts)</param>
    /// <param name="entityId">Entity ID</param>
    /// <param name="entityType">Entity type</param>
    /// <param name="category">Contact category</param>
    /// <param name="isPrimary">Whether this contact should be primary</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if valid, false otherwise</returns>
    Task<bool> ValidatePrimaryContactAsync(Guid? contactId, Guid entityId, string entityType,
        ContactCategory category, bool isPrimary, CancellationToken cancellationToken = default);
}
