using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration for Member entity
/// </summary>
public class MemberConfiguration : BaseEntityConfiguration<Member>
{
    /// <summary>
    /// Configures the Member entity
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    protected override void ConfigureEntity(EntityTypeBuilder<Member> builder)
    {
        // Properties
        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.Phone)
            .HasMaxLength(20);

        builder.Property(e => e.Gender)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(e => e.DateOfBirth)
            .IsRequired();

        // Address fields removed - now using centralized Address entity

        builder.Property(e => e.Baptized)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(e => e.BaptizedAt);

        builder.Property(e => e.BaptizedPlace)
            .HasMaxLength(200);

        builder.Property(e => e.ScarfInvested)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(e => e.ScarfInvestedAt);

        // Indexes
        // Email must be unique globally (including soft-deleted records)
        builder.HasIndex(e => e.Email)
            .IsUnique();

        // Index for age queries
        builder.HasIndex(e => e.DateOfBirth);

        // Index for gender queries
        builder.HasIndex(e => e.Gender);

        // Index for baptism status
        builder.HasIndex(e => e.Baptized);

        // Index for scarf status
        builder.HasIndex(e => e.ScarfInvested);

        // Relationships
        builder.HasOne(e => e.UserCredential)
            .WithOne(e => e.Member)
            .HasForeignKey<UserCredential>(e => e.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Memberships)
            .WithOne(e => e.Member)
            .HasForeignKey(e => e.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Assignments)
            .WithOne(e => e.Member)
            .HasForeignKey(e => e.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.TimelineEntries)
            .WithOne(e => e.Member)
            .HasForeignKey(e => e.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        // Constraints
        builder.ToTable(t => t.HasCheckConstraint("CK_Member_Baptism", "\"Baptized\" = false OR (\"BaptizedAt\" IS NOT NULL AND \"BaptizedPlace\" IS NOT NULL)"));
        builder.ToTable(t => t.HasCheckConstraint("CK_Member_Scarf", "\"ScarfInvested\" = false OR \"ScarfInvestedAt\" IS NOT NULL"));
    }
}
