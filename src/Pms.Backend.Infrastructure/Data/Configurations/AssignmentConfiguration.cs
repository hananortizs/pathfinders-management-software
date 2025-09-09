using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration for Assignment entity
/// </summary>
public class AssignmentConfiguration : BaseEntityConfiguration<Assignment>
{
    /// <summary>
    /// Configures the Assignment entity
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    protected override void ConfigureEntity(EntityTypeBuilder<Assignment> builder)
    {
        // Properties
        builder.Property(e => e.ScopeType)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(e => e.ScopeId)
            .IsRequired();

        builder.Property(e => e.StartDate)
            .IsRequired();

        builder.Property(e => e.EndDate);

        builder.Property(e => e.EndReason)
            .HasMaxLength(500);

        builder.Property(e => e.Role)
            .IsRequired()
            .HasMaxLength(100);

        // Indexes
        // Index for member queries
        builder.HasIndex(e => e.MemberId);

        // Index for role queries
        builder.HasIndex(e => e.RoleId);

        // Index for scope queries
        builder.HasIndex(e => new { e.ScopeType, e.ScopeId });

        // Index for date range queries
        builder.HasIndex(e => e.StartDate);
        builder.HasIndex(e => e.EndDate);

        // Relationships
        builder.HasOne(e => e.Member)
            .WithMany(e => e.Assignments)
            .HasForeignKey(e => e.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.RoleCatalog)
            .WithMany(e => e.Assignments)
            .HasForeignKey(e => e.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.TimelineEntries)
            .WithOne(e => e.Assignment)
            .HasForeignKey(e => e.AssignmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.DelegatedToAssignments)
            .WithOne(e => e.DelegatedToAssignment)
            .HasForeignKey(e => e.DelegatedToAssignmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.DelegatedFromAssignments)
            .WithOne(e => e.DelegatedFromAssignment)
            .HasForeignKey(e => e.DelegatedFromAssignmentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Constraints
        builder.ToTable(t => t.HasCheckConstraint("CK_Assignment_DateRange", "\"EndDate\" IS NULL OR \"EndDate\" > \"StartDate\""));
    }
}
