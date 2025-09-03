using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration for MemberEventParticipation entity
/// </summary>
public class MemberEventParticipationConfiguration : BaseEntityConfiguration<MemberEventParticipation>
{
    /// <summary>
    /// Configures the MemberEventParticipation entity
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    protected override void ConfigureEntity(EntityTypeBuilder<MemberEventParticipation> builder)
    {
        // Properties
        builder.Property(e => e.RegisteredAtUtc)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(e => e.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasDefaultValue(ParticipationStatus.Registered);

        builder.Property(e => e.FeePaid)
            .HasPrecision(10, 2);

        builder.Property(e => e.FeeCurrency)
            .HasMaxLength(3);

        builder.Property(e => e.FeePaidAtUtc);

        builder.Property(e => e.Notes)
            .HasMaxLength(1000);

        // Indexes
        // Unique constraint: One participation per member per event
        builder.HasIndex(e => new { e.MemberId, e.EventId })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");

        // Index for member queries
        builder.HasIndex(e => e.MemberId);

        // Index for event queries
        builder.HasIndex(e => e.EventId);

        // Index for status queries
        builder.HasIndex(e => e.Status);

        // Relationships
        builder.HasOne(e => e.Member)
            .WithMany()
            .HasForeignKey(e => e.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Event)
            .WithMany(e => e.Participations)
            .HasForeignKey(e => e.EventId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.TimelineEntries)
            .WithOne()
            .HasForeignKey(e => e.EventId)
            .OnDelete(DeleteBehavior.Restrict);

        // Constraints
        builder.ToTable(t => t.HasCheckConstraint("CK_MemberEventParticipation_FeePaid", "\"FeePaid\" IS NULL OR \"FeePaid\" >= 0"));
    }
}
