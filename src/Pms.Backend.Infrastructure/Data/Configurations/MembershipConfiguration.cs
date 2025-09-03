using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration for Membership entity
/// </summary>
public class MembershipConfiguration : BaseEntityConfiguration<Membership>
{
    /// <summary>
    /// Configures the Membership entity
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    protected override void ConfigureEntity(EntityTypeBuilder<Membership> builder)
    {
        // Properties
        builder.Property(e => e.StartDate)
            .IsRequired();

        builder.Property(e => e.EndDate);

        builder.Property(e => e.EndReason)
            .HasMaxLength(500);

        // Indexes
        // Unique constraint: Only one active membership per member
        builder.HasIndex(e => e.MemberId)
            .HasFilter("\"EndDate\" IS NULL AND \"IsDeleted\" = false");

        // Index for club queries
        builder.HasIndex(e => e.ClubId);

        // Index for unit queries
        builder.HasIndex(e => e.UnitId);

        // Index for date range queries
        builder.HasIndex(e => e.StartDate);
        builder.HasIndex(e => e.EndDate);

        // Relationships
        builder.HasOne(e => e.Member)
            .WithMany(e => e.Memberships)
            .HasForeignKey(e => e.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Club)
            .WithMany(e => e.Memberships)
            .HasForeignKey(e => e.ClubId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Unit)
            .WithMany(e => e.Memberships)
            .HasForeignKey(e => e.UnitId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.TimelineEntries)
            .WithOne(e => e.Membership)
            .HasForeignKey(e => e.MembershipId)
            .OnDelete(DeleteBehavior.Restrict);

        // Constraints
        builder.ToTable(t => t.HasCheckConstraint("CK_Membership_DateRange", "\"EndDate\" IS NULL OR \"EndDate\" > \"StartDate\""));
    }
}
