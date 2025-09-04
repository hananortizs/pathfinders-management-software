using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration for Address entity
/// </summary>
public class AddressConfiguration : BaseEntityConfiguration<Address>
{
    /// <summary>
    /// Configures the Address entity
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    protected override void ConfigureEntity(EntityTypeBuilder<Address> builder)
    {
        // Properties
        builder.Property(e => e.Street)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Number)
            .HasMaxLength(10);

        builder.Property(e => e.Complement)
            .HasMaxLength(100);

        builder.Property(e => e.Neighborhood)
            .HasMaxLength(100);

        builder.Property(e => e.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.State)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Country)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("Brasil");

        builder.Property(e => e.Cep)
            .HasMaxLength(10);

        builder.Property(e => e.Type)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(e => e.IsPrimary)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(e => e.Notes)
            .HasMaxLength(500);

        builder.Property(e => e.EntityId)
            .IsRequired();

        builder.Property(e => e.EntityType)
            .IsRequired()
            .HasMaxLength(50);

        // Indexes
        // Composite index for polymorphic relationship
        builder.HasIndex(e => new { e.EntityId, e.EntityType })
            .HasDatabaseName("IX_Addresses_EntityId_EntityType");

        // Index for primary addresses
        builder.HasIndex(e => new { e.EntityId, e.EntityType, e.IsPrimary })
            .HasDatabaseName("IX_Addresses_EntityId_EntityType_IsPrimary")
            .HasFilter("\"IsPrimary\" = true");

        // Index for CEP (for address lookups)
        builder.HasIndex(e => e.Cep)
            .HasDatabaseName("IX_Addresses_Cep")
            .HasFilter("\"Cep\" IS NOT NULL");

        // Index for city and state (for geographic queries)
        builder.HasIndex(e => new { e.City, e.State })
            .HasDatabaseName("IX_Addresses_City_State");

        // Note: FullAddress is computed in the application layer, not in the database
    }
}
