using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration for Club entity
/// </summary>
public class ClubConfiguration : BaseEntityConfiguration<Club>
{
    /// <summary>
    /// Configures the Club entity
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    protected override void ConfigureEntity(EntityTypeBuilder<Club> builder)
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

        builder.Property(e => e.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // Indexes
        builder.HasIndex(e => e.Code)
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");

        // Unique constraint: Code must be unique within the same District
        builder.HasIndex(e => new { e.Code, e.DistrictId })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");

        // Index for active clubs
        builder.HasIndex(e => e.IsActive)
            .HasFilter("\"IsActive\" = true AND \"IsDeleted\" = false");

        // Relationships
        builder.HasOne(e => e.District)
            .WithMany(e => e.Clubs)
            .HasForeignKey(e => e.DistrictId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Church)
            .WithOne(e => e.Club)
            .HasForeignKey<Club>(e => e.ChurchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Units)
            .WithOne(e => e.Club)
            .HasForeignKey(e => e.ClubId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Memberships)
            .WithOne(e => e.Club)
            .HasForeignKey(e => e.ClubId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
