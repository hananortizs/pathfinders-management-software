using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pms.Backend.Domain.Entities.Hierarchy;

namespace Pms.Backend.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration for Region entity
/// </summary>
public class RegionConfiguration : BaseEntityConfiguration<Region>
{
    /// <summary>
    /// Configures the Region entity
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    protected override void ConfigureEntity(EntityTypeBuilder<Region> builder)
    {
        // Properties
        builder.Property(e => e.Code)
            .IsRequired()
            .HasMaxLength(5)
            .IsFixedLength();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

        // Indexes
        builder.HasIndex(e => e.Code)
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");

        // Unique constraint: Code must be unique within the same Association
        builder.HasIndex(e => new { e.Code, e.AssociationId })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");

        // Relationships
        builder.HasOne(e => e.Association)
            .WithMany(e => e.Regions)
            .HasForeignKey(e => e.AssociationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Districts)
            .WithOne(e => e.Region)
            .HasForeignKey(e => e.RegionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
