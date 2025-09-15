using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration for Investiture entity
/// </summary>
public class InvestitureConfiguration : BaseEntityConfiguration<Investiture>
{
    /// <summary>
    /// Configures the Investiture entity
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    protected override void ConfigureEntity(EntityTypeBuilder<Investiture> builder)
    {
        // Properties
        builder.Property(e => e.Type)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(e => e.Date)
            .IsRequired();

        builder.Property(e => e.Place)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.YoutubeUrl)
            .HasMaxLength(500);

        builder.Property(e => e.RefItem)
            .HasMaxLength(2000);

        // Indexes
        // Index for member queries
        builder.HasIndex(e => e.MemberId);

        // Index for type queries
        builder.HasIndex(e => e.Type);

        // Index for date queries
        builder.HasIndex(e => e.Date);

        // Relationships
        builder.HasOne(e => e.Member)
            .WithMany(m => m.Investitures)
            .HasForeignKey(e => e.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Witnesses)
            .WithOne(e => e.Investiture)
            .HasForeignKey(e => e.InvestitureId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.TimelineEntries)
            .WithOne()
            .HasForeignKey(e => e.EventId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
