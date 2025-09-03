using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration for Unit entity
/// </summary>
public class UnitConfiguration : BaseEntityConfiguration<Unit>
{
    /// <summary>
    /// Configures the Unit entity
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    protected override void ConfigureEntity(EntityTypeBuilder<Unit> builder)
    {
        // Properties
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

        builder.Property(e => e.Gender)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(e => e.AgeMin)
            .IsRequired();

        builder.Property(e => e.AgeMax)
            .IsRequired();

        builder.Property(e => e.Capacity);

        // Indexes
        // Unique constraint: Name must be unique within the same Club
        builder.HasIndex(e => new { e.Name, e.ClubId })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");

        // Index for gender and age range queries
        builder.HasIndex(e => new { e.ClubId, e.Gender, e.AgeMin, e.AgeMax });

        // Relationships
        builder.HasOne(e => e.Club)
            .WithMany(e => e.Units)
            .HasForeignKey(e => e.ClubId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Memberships)
            .WithOne(e => e.Unit)
            .HasForeignKey(e => e.UnitId)
            .OnDelete(DeleteBehavior.Restrict);

        // Constraints
        builder.ToTable(t => t.HasCheckConstraint("CK_Unit_AgeRange", "\"AgeMin\" <= \"AgeMax\""));
        builder.ToTable(t => t.HasCheckConstraint("CK_Unit_Capacity", "\"Capacity\" IS NULL OR \"Capacity\" > 0"));
    }
}
