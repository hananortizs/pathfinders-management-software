using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pms.Backend.Domain.Entities.Hierarchy;

namespace Pms.Backend.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration for District entity
/// </summary>
public class DistrictConfiguration : BaseEntityConfiguration<District>
{
    /// <summary>
    /// Configures the District entity
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    protected override void ConfigureEntity(EntityTypeBuilder<District> builder)
    {
        // Properties
        builder.Property(e => e.Code)
            .IsRequired()
            .HasMaxLength(5);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

        // Indexes
        builder.HasIndex(e => e.Code)
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");

        // Unique constraint: Code must be unique within the same Region
        builder.HasIndex(e => new { e.Code, e.RegionId })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");

        // Relationships
        builder.HasOne(e => e.Region)
            .WithMany(e => e.Districts)
            .HasForeignKey(e => e.RegionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Clubs)
            .WithOne(e => e.District)
            .HasForeignKey(e => e.DistrictId)
            .OnDelete(DeleteBehavior.Restrict);

        // Pastor relationship (optional)
        builder.HasOne(e => e.Pastor)
            .WithMany(p => p.Districts)
            .HasForeignKey(e => e.PastorId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
