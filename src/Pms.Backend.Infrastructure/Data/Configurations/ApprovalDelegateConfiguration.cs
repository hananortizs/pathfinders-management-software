using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration for ApprovalDelegate entity
/// </summary>
public class ApprovalDelegateConfiguration : BaseEntityConfiguration<ApprovalDelegate>
{
    /// <summary>
    /// Configures the ApprovalDelegate entity
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    protected override void ConfigureEntity(EntityTypeBuilder<ApprovalDelegate> builder)
    {
        // Properties
        builder.Property(e => e.Role)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.ScopeType)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(e => e.ScopeId)
            .IsRequired();

        builder.Property(e => e.StartDate)
            .IsRequired();

        builder.Property(e => e.EndDate)
            .IsRequired();

        builder.Property(e => e.Reason)
            .HasMaxLength(500);

        builder.Property(e => e.Notes)
            .HasMaxLength(1000);

        builder.Property(e => e.EndedAtUtc);

        // Indexes
        // Index for scope queries
        builder.HasIndex(e => new { e.ScopeType, e.ScopeId });

        // Index for date range queries
        builder.HasIndex(e => e.StartDate);
        builder.HasIndex(e => e.EndDate);

        // Index for active delegations
        builder.HasIndex(e => new { e.StartDate, e.EndDate })
            .HasFilter("\"IsDeleted\" = false");

        // Relationships
        builder.HasOne(e => e.DelegatedToAssignment)
            .WithMany(e => e.DelegatedToAssignments)
            .HasForeignKey(e => e.DelegatedToAssignmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.DelegatedFromAssignment)
            .WithMany(e => e.DelegatedFromAssignments)
            .HasForeignKey(e => e.DelegatedFromAssignmentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Constraints
        builder.ToTable(t => t.HasCheckConstraint("CK_ApprovalDelegate_DateRange", "\"EndDate\" > \"StartDate\""));
    }
}
