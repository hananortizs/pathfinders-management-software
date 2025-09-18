using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration for EventCoHost entity
/// </summary>
public class EventCoHostConfiguration : BaseEntityConfiguration<EventCoHost>
{
    /// <summary>
    /// Configures the EventCoHost entity
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    protected override void ConfigureEntity(EntityTypeBuilder<EventCoHost> builder)
    {
        // Properties
        builder.Property(e => e.Level)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(e => e.EntityId)
            .IsRequired();

        // Indexes
        builder.HasIndex(e => e.EventId);
        builder.HasIndex(e => e.Level);
        builder.HasIndex(e => e.EntityId);

        // Relationships
        builder.HasOne(e => e.Event)
            .WithMany(e => e.CoHosts)
            .HasForeignKey(e => e.EventId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
