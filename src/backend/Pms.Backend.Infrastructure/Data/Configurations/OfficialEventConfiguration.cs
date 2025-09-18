using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration for OfficialEvent entity
/// </summary>
public class OfficialEventConfiguration : BaseEntityConfiguration<OfficialEvent>
{
    /// <summary>
    /// Configures the OfficialEvent entity
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    protected override void ConfigureEntity(EntityTypeBuilder<OfficialEvent> builder)
    {
        // Properties
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .HasMaxLength(1000);

        builder.Property(e => e.StartDate)
            .IsRequired();

        builder.Property(e => e.EndDate)
            .IsRequired();

        builder.Property(e => e.Location)
            .HasMaxLength(500);

        builder.Property(e => e.OrganizerLevel)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(e => e.OrganizerId)
            .IsRequired();

        builder.Property(e => e.FeeAmount)
            .HasPrecision(10, 2);

        builder.Property(e => e.FeeCurrency)
            .IsRequired()
            .HasMaxLength(3)
            .HasDefaultValue("BRL");

        builder.Property(e => e.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // New properties for enhanced event features
        builder.Property(e => e.MinAge)
            .HasDefaultValue(null);

        builder.Property(e => e.MaxAge)
            .HasDefaultValue(null);

        builder.Property(e => e.RequiresMedicalInfo)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(e => e.RequiresScarfInvested)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(e => e.Visibility)
            .IsRequired()
            .HasConversion<string>()
            .HasDefaultValue(EventVisibility.Public);

        builder.Property(e => e.AudienceMode)
            .IsRequired()
            .HasConversion<string>()
            .HasDefaultValue(EventAudienceMode.Subtree);

        builder.Property(e => e.AllowLeadersAboveHost)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(e => e.AllowList)
            .HasMaxLength(2000);

        builder.Property(e => e.RegistrationOpenAtUtc)
            .HasDefaultValue(null);

        builder.Property(e => e.RegistrationCloseAtUtc)
            .HasDefaultValue(null);

        builder.Property(e => e.Capacity)
            .HasDefaultValue(null);

        builder.Property(e => e.RegisteredCount)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(e => e.WaitlistedCount)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(e => e.CheckedInCount)
            .IsRequired()
            .HasDefaultValue(0);

        // Indexes
        // Index for organizer queries
        builder.HasIndex(e => new { e.OrganizerLevel, e.OrganizerId });

        // Index for date range queries
        builder.HasIndex(e => e.StartDate);
        builder.HasIndex(e => e.EndDate);

        // Index for active events
        builder.HasIndex(e => e.IsActive)
            .HasFilter("\"IsActive\" = true AND \"IsDeleted\" = false");

        // Index for visibility queries
        builder.HasIndex(e => e.Visibility);

        // Index for audience mode queries
        builder.HasIndex(e => e.AudienceMode);

        // Index for registration status queries
        builder.HasIndex(e => e.RegistrationOpenAtUtc);
        builder.HasIndex(e => e.RegistrationCloseAtUtc);

        // Index for capacity queries
        builder.HasIndex(e => e.Capacity);

        // Relationships
        builder.HasMany(e => e.Participations)
            .WithOne(e => e.Event)
            .HasForeignKey(e => e.EventId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.TimelineEntries)
            .WithOne(e => e.Event)
            .HasForeignKey(e => e.EventId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.CoHosts)
            .WithOne(e => e.Event)
            .HasForeignKey(e => e.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        // Constraints
        builder.ToTable(t => t.HasCheckConstraint("CK_OfficialEvent_DateRange", "\"EndDate\" > \"StartDate\""));
        builder.ToTable(t => t.HasCheckConstraint("CK_OfficialEvent_FeeAmount", "\"FeeAmount\" IS NULL OR \"FeeAmount\" >= 0"));
        builder.ToTable(t => t.HasCheckConstraint("CK_OfficialEvent_AgeRange", "\"MinAge\" IS NULL OR \"MaxAge\" IS NULL OR \"MinAge\" <= \"MaxAge\""));
        builder.ToTable(t => t.HasCheckConstraint("CK_OfficialEvent_RegistrationDates", "\"RegistrationOpenAtUtc\" IS NULL OR \"RegistrationCloseAtUtc\" IS NULL OR \"RegistrationOpenAtUtc\" < \"RegistrationCloseAtUtc\""));
        builder.ToTable(t => t.HasCheckConstraint("CK_OfficialEvent_Capacity", "\"Capacity\" IS NULL OR \"Capacity\" > 0"));
        builder.ToTable(t => t.HasCheckConstraint("CK_OfficialEvent_Counts", "\"RegisteredCount\" >= 0 AND \"WaitlistedCount\" >= 0 AND \"CheckedInCount\" >= 0"));
    }
}
