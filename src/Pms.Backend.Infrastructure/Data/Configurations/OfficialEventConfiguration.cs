using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration for OfficialEvent entity
/// </summary>
public class OfficialEventConfiguration : BaseEntityConfiguration<OfficialEvent>
{
    /// <summary>
    /// Configures the OfficialEvent entity
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    protected override void ConfigureEntity(EntityTypeBuilder<OfficialEvent> builder)
    {
        // Properties
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

        builder.Property(e => e.StartDate)
            .IsRequired();

        builder.Property(e => e.EndDate)
            .IsRequired();

        builder.Property(e => e.Location)
            .HasMaxLength(500);

        builder.Property(e => e.OrganizerLevel)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(e => e.OrganizerId)
            .IsRequired();

        builder.Property(e => e.FeeAmount)
            .HasPrecision(10, 2);

        builder.Property(e => e.FeeCurrency)
            .IsRequired()
            .HasMaxLength(3)
            .HasDefaultValue("BRL");

        builder.Property(e => e.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // Indexes
        // Index for organizer queries
        builder.HasIndex(e => new { e.OrganizerLevel, e.OrganizerId });

        // Index for date range queries
        builder.HasIndex(e => e.StartDate);
        builder.HasIndex(e => e.EndDate);

        // Index for active events
        builder.HasIndex(e => e.IsActive)
            .HasFilter("\"IsActive\" = true AND \"IsDeleted\" = false");

        // Relationships
        builder.HasMany(e => e.Participations)
            .WithOne(e => e.Event)
            .HasForeignKey(e => e.EventId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.TimelineEntries)
            .WithOne(e => e.Event)
            .HasForeignKey(e => e.EventId)
            .OnDelete(DeleteBehavior.Restrict);

        // Constraints
        builder.ToTable(t => t.HasCheckConstraint("CK_OfficialEvent_DateRange", "\"EndDate\" > \"StartDate\""));
        builder.ToTable(t => t.HasCheckConstraint("CK_OfficialEvent_FeeAmount", "\"FeeAmount\" IS NULL OR \"FeeAmount\" >= 0"));
    }
}
