using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration for Church entity
/// </summary>
public class ChurchConfiguration : BaseEntityConfiguration<Church>
{
    /// <summary>
    /// Configures the Church entity
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    protected override void ConfigureEntity(EntityTypeBuilder<Church> builder)
    {
        // Properties
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        // Address fields removed - now using centralized Address entity

        builder.Property(e => e.Phone)
            .HasMaxLength(20);

        builder.Property(e => e.Email)
            .HasMaxLength(255);

        // Indexes
        // CEP uniqueness now handled in Address entity

        // Relationships
        // Church belongs to a District
        builder.HasOne(e => e.District)
            .WithMany(e => e.Churches)
            .HasForeignKey(e => e.DistrictId)
            .OnDelete(DeleteBehavior.Restrict);

        // Church can have one Club (1:1 relationship)
        builder.HasOne(e => e.Club)
            .WithOne(e => e.Church)
            .HasForeignKey<Club>(e => e.ChurchId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
