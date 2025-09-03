using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration for TimelineEntry entity
/// </summary>
public class TimelineEntryConfiguration : BaseEntityConfiguration<TimelineEntry>
{
    /// <summary>
    /// Configures the TimelineEntry entity
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    protected override void ConfigureEntity(EntityTypeBuilder<TimelineEntry> builder)
    {
        // Properties
        builder.Property(e => e.Type)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(e => e.Data)
            .HasMaxLength(2000);

        builder.Property(e => e.EventDateUtc)
            .IsRequired();

        // Indexes
        // Index for member queries
        builder.HasIndex(e => e.MemberId);

        // Index for type queries
        builder.HasIndex(e => e.Type);

        // Index for event date queries
        builder.HasIndex(e => e.EventDateUtc);

        // Index for related entity queries
        builder.HasIndex(e => e.MembershipId);
        builder.HasIndex(e => e.AssignmentId);
        builder.HasIndex(e => e.EventId);

        // Relationships
        builder.HasOne(e => e.Member)
            .WithMany(e => e.TimelineEntries)
            .HasForeignKey(e => e.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Membership)
            .WithMany(e => e.TimelineEntries)
            .HasForeignKey(e => e.MembershipId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Assignment)
            .WithMany(e => e.TimelineEntries)
            .HasForeignKey(e => e.AssignmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Event)
            .WithMany(e => e.TimelineEntries)
            .HasForeignKey(e => e.EventId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
