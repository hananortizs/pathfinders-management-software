using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pms.Backend.Domain.Entities.Hierarchy;

namespace Pms.Backend.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration for Association entity
/// </summary>
public class AssociationConfiguration : BaseEntityConfiguration<Association>
{
    /// <summary>
    /// Configures the Association entity
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    protected override void ConfigureEntity(EntityTypeBuilder<Association> builder)
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

        // Unique constraint: Code must be unique within the same Union
        builder.HasIndex(e => new { e.Code, e.UnionId })
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");

        // Relationships
        builder.HasOne(e => e.Union)
            .WithMany(e => e.Associations)
            .HasForeignKey(e => e.UnionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Regions)
            .WithOne(e => e.Association)
            .HasForeignKey(e => e.AssociationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
