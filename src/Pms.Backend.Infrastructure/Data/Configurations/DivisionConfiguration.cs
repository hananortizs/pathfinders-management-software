using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pms.Backend.Domain.Entities.Hierarchy;

namespace Pms.Backend.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration for Division entity
/// </summary>
public class DivisionConfiguration : BaseEntityConfiguration<Division>
{
    /// <summary>
    /// Configures the Division entity
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    protected override void ConfigureEntity(EntityTypeBuilder<Division> builder)
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

        // Relationships
        builder.HasMany(e => e.Unions)
            .WithOne(e => e.Division)
            .HasForeignKey(e => e.DivisionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
