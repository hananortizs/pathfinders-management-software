using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pms.Backend.Domain.Entities.Hierarchy;

namespace Pms.Backend.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration for Union entity
/// </summary>
public class UnionConfiguration : BaseEntityConfiguration<Union>
{
    /// <summary>
    /// Configures the Union entity
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    protected override void ConfigureEntity(EntityTypeBuilder<Union> builder)
    {
        // Properties
        builder.Property(e => e.Code)
            .IsRequired()
            .HasMaxLength(5)
            .IsFixedLength();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

        // Indexes
        builder.HasIndex(e => e.Code)
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");

        // Unique constraint: Code must be unique within the same Division
        builder.HasIndex(e => new { e.Code, e.DivisionId })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");

        // Foreign key configuration
        builder.Property(e => e.DivisionId)
            .IsRequired();

        // Relationships
        builder.HasOne(e => e.Division)
            .WithMany(e => e.Unions)
            .HasForeignKey(e => e.DivisionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Associations)
            .WithOne(e => e.Union)
            .HasForeignKey(e => e.UnionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
