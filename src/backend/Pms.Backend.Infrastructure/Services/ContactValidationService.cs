using Microsoft.EntityFrameworkCore;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Domain.Enums;
using Pms.Backend.Infrastructure.Data;

namespace Pms.Backend.Infrastructure.Services;

/// <summary>
/// Service for validating contact-related operations
/// </summary>
public class ContactValidationService : IContactValidationService
{
    private readonly PmsDbContext _context;

    /// <summary>
    /// Initializes a new instance of the ContactValidationService
    /// </summary>
    /// <param name="context">Database context</param>
    public ContactValidationService(PmsDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Validates if an entity exists in the database
    /// </summary>
    /// <param name="entityId">Entity ID</param>
    /// <param name="entityType">Entity type</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if entity exists, false otherwise</returns>
    public async Task<bool> ValidateEntityExistsAsync(Guid entityId, string entityType, CancellationToken cancellationToken = default)
    {
        if (!EntityTypeHelper.IsValidEntityType(entityType))
        {
            return false;
        }

        var parsedType = EntityTypeHelper.ParseEntityType(entityType);
        if (!parsedType.HasValue)
        {
            return false;
        }

        return parsedType.Value switch
        {
            EntityType.Member => await _context.Members.AnyAsync(m => m.Id == entityId && !m.IsDeleted, cancellationToken),
            EntityType.Church => await _context.Churches.AnyAsync(c => c.Id == entityId && !c.IsDeleted, cancellationToken),
            EntityType.Club => await _context.Clubs.AnyAsync(c => c.Id == entityId && !c.IsDeleted, cancellationToken),
            EntityType.Unit => await _context.Units.AnyAsync(u => u.Id == entityId && !u.IsDeleted, cancellationToken),
            EntityType.District => await _context.Districts.AnyAsync(d => d.Id == entityId && !d.IsDeleted, cancellationToken),
            EntityType.Association => await _context.Associations.AnyAsync(a => a.Id == entityId && !a.IsDeleted, cancellationToken),
            EntityType.Union => await _context.Unions.AnyAsync(u => u.Id == entityId && !u.IsDeleted, cancellationToken),
            EntityType.Division => await _context.Divisions.AnyAsync(d => d.Id == entityId && !d.IsDeleted, cancellationToken),
            EntityType.Region => await _context.Regions.AnyAsync(r => r.Id == entityId && !r.IsDeleted, cancellationToken),
            _ => false
        };
    }

    /// <summary>
    /// Validates if an entity can have contacts
    /// </summary>
    /// <param name="entityType">Entity type</param>
    /// <returns>True if entity can have contacts, false otherwise</returns>
    public bool ValidateEntityCanHaveContacts(string entityType)
    {
        return EntityTypeHelper.IsValidEntityType(entityType);
    }

    /// <summary>
    /// Gets the table name for an entity type
    /// </summary>
    /// <param name="entityType">Entity type</param>
    /// <returns>Table name or null if invalid</returns>
    public string? GetTableName(string entityType)
    {
        if (!EntityTypeHelper.IsValidEntityType(entityType))
        {
            return null;
        }

        var parsedType = EntityTypeHelper.ParseEntityType(entityType);
        if (!parsedType.HasValue)
        {
            return null;
        }

        return parsedType.Value switch
        {
            EntityType.Member => "Members",
            EntityType.Church => "Churches",
            EntityType.Club => "Clubs",
            EntityType.Unit => "Units",
            EntityType.District => "Districts",
            EntityType.Association => "Associations",
            EntityType.Union => "Unions",
            EntityType.Division => "Divisions",
            EntityType.Region => "Regions",
            _ => null
        };
    }

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
        public async Task<bool> ValidateContactValueUniqueAsync(Guid? contactId, Guid entityId, string entityType,
        ContactType contactType, string value, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return true; // Empty values are allowed
        }

        var query = _context.Contacts
            .Where(c => c.EntityId == entityId &&
                       c.EntityType == entityType &&
                       c.Type == contactType &&
                       c.Value == value &&
                       !c.IsDeleted);

        // Exclude current contact if updating
        if (contactId.HasValue)
        {
            query = query.Where(c => c.Id != contactId.Value);
        }

        return !await query.AnyAsync(cancellationToken);
    }

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
        public async Task<bool> ValidatePrimaryContactAsync(Guid? contactId, Guid entityId, string entityType,
        ContactCategory category, bool isPrimary, CancellationToken cancellationToken = default)
    {
        if (!isPrimary)
        {
            return true; // Non-primary contacts are always valid
        }

        var query = _context.Contacts
            .Where(c => c.EntityId == entityId &&
                       c.EntityType == entityType &&
                       c.Category == category &&
                       c.IsPrimary &&
                       !c.IsDeleted);

        // Exclude current contact if updating
        if (contactId.HasValue)
        {
            query = query.Where(c => c.Id != contactId.Value);
        }

        return !await query.AnyAsync(cancellationToken);
    }
}
