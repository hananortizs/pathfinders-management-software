using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration for InvestitureWitness entity
/// </summary>
public class InvestitureWitnessConfiguration : BaseEntityConfiguration<InvestitureWitness>
{
    /// <summary>
    /// Configures the InvestitureWitness entity
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    protected override void ConfigureEntity(EntityTypeBuilder<InvestitureWitness> builder)
    {
        // Properties
        builder.Property(e => e.Type)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(e => e.NameText)
            .HasMaxLength(200);

        builder.Property(e => e.RoleText)
            .HasMaxLength(100);

        builder.Property(e => e.OrgText)
            .HasMaxLength(120);

        // Configure RoleSnapshot as owned entity
        builder.OwnsOne(e => e.RoleSnapshot, roleSnapshot =>
        {
            roleSnapshot.Property(rs => rs.Role)
                .HasMaxLength(100);

            roleSnapshot.Property(rs => rs.ScopeType)
                .HasConversion<string>();

            roleSnapshot.Property(rs => rs.ScopeCodePath)
                .HasMaxLength(200);

            roleSnapshot.Property(rs => rs.EffectiveDate);
        });

        // Indexes
        // Index for investiture queries
        builder.HasIndex(e => e.InvestitureId);

        // Index for member queries (when structured)
        builder.HasIndex(e => e.MemberId);

        // Relationships
        builder.HasOne(e => e.Investiture)
            .WithMany(e => e.Witnesses)
            .HasForeignKey(e => e.InvestitureId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Member)
            .WithMany()
            .HasForeignKey(e => e.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        // Constraints
        builder.ToTable(t => t.HasCheckConstraint("CK_InvestitureWitness_Structured",
            "\"Type\" != 'Structured' OR \"MemberId\" IS NOT NULL"));

        builder.ToTable(t => t.HasCheckConstraint("CK_InvestitureWitness_Text",
            "\"Type\" != 'Text' OR (\"NameText\" IS NOT NULL AND \"RoleText\" IS NOT NULL)"));
    }
}
