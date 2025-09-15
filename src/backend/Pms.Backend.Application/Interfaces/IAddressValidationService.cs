namespace Pms.Backend.Application.Interfaces;

/// <summary>
/// Service interface for Address validation operations
/// </summary>
public interface IAddressValidationService
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
    /// Validates if an entity can have addresses
    /// </summary>
    /// <param name="entityType">Entity type</param>
    /// <returns>True if entity can have addresses, false otherwise</returns>
    bool ValidateEntityCanHaveAddresses(string entityType);

    /// <summary>
    /// Gets the table name for an entity type
    /// </summary>
    /// <param name="entityType">Entity type</param>
    /// <returns>Table name or null if invalid</returns>
    string? GetTableName(string entityType);
}
