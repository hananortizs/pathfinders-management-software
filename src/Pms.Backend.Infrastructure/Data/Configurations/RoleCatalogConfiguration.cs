using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration for RoleCatalog entity
/// </summary>
public class RoleCatalogConfiguration : BaseEntityConfiguration<RoleCatalog>
{
    /// <summary>
    /// Configures the RoleCatalog entity
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    protected override void ConfigureEntity(EntityTypeBuilder<RoleCatalog> builder)
    {
        // Properties
        builder.Property(e => e.Level)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Description)
            .HasMaxLength(500);

        builder.Property(e => e.MaxPerScope)
            .IsRequired()
            .HasDefaultValue(1);

        builder.Property(e => e.GenderRequired)
            .HasConversion<string>();

        builder.Property(e => e.AgeMin);

        builder.Property(e => e.AgeMax);

        builder.Property(e => e.RequiresBaptism)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(e => e.RequiresScarf)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(e => e.IsLeadership)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(e => e.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // Indexes
        builder.HasIndex(e => e.Level);
        builder.HasIndex(e => e.IsActive)
            .HasFilter("\"IsActive\" = true AND \"IsDeleted\" = false");

        // Relationships
        builder.HasMany(e => e.Assignments)
            .WithOne(e => e.RoleCatalog)
            .HasForeignKey(e => e.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        // Constraints
        builder.ToTable(t => t.HasCheckConstraint("CK_RoleCatalog_AgeRange", "\"AgeMin\" IS NULL OR \"AgeMax\" IS NULL OR \"AgeMin\" <= \"AgeMax\""));
        builder.ToTable(t => t.HasCheckConstraint("CK_RoleCatalog_MaxPerScope", "\"MaxPerScope\" > 0"));
    }
}
