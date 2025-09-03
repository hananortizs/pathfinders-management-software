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

        builder.Property(e => e.Address)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(e => e.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.State)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Cep)
            .IsRequired()
            .HasMaxLength(10)
            .IsFixedLength();

        builder.Property(e => e.Phone)
            .HasMaxLength(20);

        builder.Property(e => e.Email)
            .HasMaxLength(255);

        // Indexes
        // CEP must be unique globally
        builder.HasIndex(e => e.Cep)
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");

        // Relationships
        builder.HasOne(e => e.Club)
            .WithOne(e => e.Church)
            .HasForeignKey<Club>(e => e.ChurchId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
