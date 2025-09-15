using Microsoft.EntityFrameworkCore;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Domain.Enums;
using Pms.Backend.Infrastructure.Data;

namespace Pms.Backend.Infrastructure.Services;

/// <summary>
/// Service for validating address-related operations
/// </summary>
public class AddressValidationService : IAddressValidationService
{
    private readonly PmsDbContext _context;

    /// <summary>
    /// Initializes a new instance of the AddressValidationService
    /// </summary>
    /// <param name="context">Database context</param>
    public AddressValidationService(PmsDbContext context)
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
    /// Validates if an entity can have addresses
    /// </summary>
    /// <param name="entityType">Entity type</param>
    /// <returns>True if entity can have addresses, false otherwise</returns>
    public bool ValidateEntityCanHaveAddresses(string entityType)
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
}
