using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Infrastructure.Data.Configurations;

/// <summary>
/// Base configuration for all entities
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    /// <summary>
    /// Configures the entity
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        // Primary key
        builder.HasKey(e => e.Id);

        // Properties
        builder.Property(e => e.Id)
            .IsRequired();

        builder.Property(e => e.CreatedAtUtc)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(e => e.UpdatedAtUtc)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(e => e.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false);

        // Indexes
        builder.HasIndex(e => e.CreatedAtUtc);
        builder.HasIndex(e => e.UpdatedAtUtc);
        builder.HasIndex(e => e.IsDeleted);

        // Soft delete index (partial)
        builder.HasIndex(e => e.IsDeleted)
            .HasFilter("\"IsDeleted\" = false");

        // Configure entity-specific properties
        ConfigureEntity(builder);
    }

    /// <summary>
    /// Configure entity-specific properties
    /// </summary>
    /// <param name="builder">Entity type builder</param>
    protected abstract void ConfigureEntity(EntityTypeBuilder<T> builder);
}
