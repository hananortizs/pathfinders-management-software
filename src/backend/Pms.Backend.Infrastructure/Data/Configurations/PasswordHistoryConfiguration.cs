using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration for PasswordHistory entity
/// </summary>
public class PasswordHistoryConfiguration : BaseEntityConfiguration<PasswordHistory>
{
    /// <summary>
    /// Configures the PasswordHistory entity
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    protected override void ConfigureEntity(EntityTypeBuilder<PasswordHistory> builder)
    {
        // Properties
        builder.Property(e => e.PasswordHash)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.Salt)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.UsedAtUtc)
            .IsRequired();

        // Indexes
        builder.HasIndex(e => e.UserCredentialId);
        builder.HasIndex(e => e.UsedAtUtc);

        // Relationships
        builder.HasOne(e => e.UserCredential)
            .WithMany(e => e.PasswordHistory)
            .HasForeignKey(e => e.UserCredentialId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
