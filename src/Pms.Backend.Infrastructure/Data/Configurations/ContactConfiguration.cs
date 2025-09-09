using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration for Contact entity
/// </summary>
public class ContactConfiguration : BaseEntityConfiguration<Contact>
{
    /// <summary>
    /// Configures the Contact entity
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    protected override void ConfigureEntity(EntityTypeBuilder<Contact> builder)
    {
        // Table name and check constraints
        builder.ToTable("Contacts", t => 
        {
            t.HasCheckConstraint("CK_Contacts_Value_NotEmpty", "LENGTH(\"Value\") > 0");
            t.HasCheckConstraint("CK_Contacts_EntityType_NotEmpty", "LENGTH(\"EntityType\") > 0");
            t.HasCheckConstraint("CK_Contacts_Priority_NonNegative", "\"Priority\" >= 0");
        });

        // Properties configuration
        builder.Property(e => e.Type)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(e => e.Category)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(e => e.Value)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(e => e.Label)
            .HasMaxLength(100);

        builder.Property(e => e.Notes)
            .HasMaxLength(1000);

        builder.Property(e => e.EntityId)
            .IsRequired();

        builder.Property(e => e.EntityType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Priority)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(e => e.LastVerifiedAt)
            .IsRequired(false);

        builder.Property(e => e.IsPrimary)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(e => e.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(e => e.IsVerified)
            .IsRequired()
            .HasDefaultValue(false);

        // Indexes for performance
        builder.HasIndex(e => new { e.EntityId, e.EntityType })
            .HasDatabaseName("IX_Contacts_EntityId_EntityType");

        builder.HasIndex(e => new { e.EntityId, e.EntityType, e.Type })
            .HasDatabaseName("IX_Contacts_EntityId_EntityType_Type");

        builder.HasIndex(e => new { e.EntityId, e.EntityType, e.Category })
            .HasDatabaseName("IX_Contacts_EntityId_EntityType_Category");

        builder.HasIndex(e => new { e.EntityId, e.EntityType, e.IsPrimary })
            .HasDatabaseName("IX_Contacts_EntityId_EntityType_IsPrimary");

        builder.HasIndex(e => new { e.EntityId, e.EntityType, e.IsActive })
            .HasDatabaseName("IX_Contacts_EntityId_EntityType_IsActive");

        builder.HasIndex(e => e.Priority)
            .HasDatabaseName("IX_Contacts_Priority");
    }
}
