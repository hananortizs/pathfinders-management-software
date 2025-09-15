using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration for TaskItem entity
/// </summary>
public class TaskItemConfiguration : BaseEntityConfiguration<TaskItem>
{
    /// <summary>
    /// Configures the TaskItem entity
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    protected override void ConfigureEntity(EntityTypeBuilder<TaskItem> builder)
    {
        // Properties
        builder.Property(e => e.Type)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

        builder.Property(e => e.Payload)
            .HasMaxLength(2000);

        builder.Property(e => e.Priority)
            .IsRequired()
            .HasConversion<string>()
            .HasDefaultValue(TaskPriority.Normal)
            .HasSentinel(TaskPriority.Low);

        builder.Property(e => e.DueDate);

        builder.Property(e => e.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasDefaultValue(Pms.Backend.Domain.Entities.TaskStatus.Open);

        builder.Property(e => e.CompletedAtUtc);

        builder.Property(e => e.CompletedBy)
            .HasMaxLength(100);

        builder.Property(e => e.CompletionNotes)
            .HasMaxLength(1000);

        // Indexes
        // Index for type queries
        builder.HasIndex(e => e.Type);

        // Index for status queries
        builder.HasIndex(e => e.Status);

        // Index for priority queries
        builder.HasIndex(e => e.Priority);

        // Index for due date queries
        builder.HasIndex(e => e.DueDate);

        // Index for club queries
        builder.HasIndex(e => e.ClubId);

        // Index for unit queries
        builder.HasIndex(e => e.UnitId);

        // Index for related member queries
        builder.HasIndex(e => e.RelatedMemberId);

        // Index for created by member queries
        builder.HasIndex(e => e.CreatedByMemberId);

        // Relationships
        builder.HasOne(e => e.CreatedByMember)
            .WithMany()
            .HasForeignKey(e => e.CreatedByMemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Club)
            .WithMany()
            .HasForeignKey(e => e.ClubId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Unit)
            .WithMany()
            .HasForeignKey(e => e.UnitId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.RelatedMember)
            .WithMany()
            .HasForeignKey(e => e.RelatedMemberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
