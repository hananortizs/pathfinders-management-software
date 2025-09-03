using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration for UserCredential entity
/// </summary>
public class UserCredentialConfiguration : BaseEntityConfiguration<UserCredential>
{
    /// <summary>
    /// Configures the UserCredential entity
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    protected override void ConfigureEntity(EntityTypeBuilder<UserCredential> builder)
    {
        // Properties
        builder.Property(e => e.PasswordHash)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.Salt)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.IsLockedOut)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(e => e.LockedOutUntilUtc);

        builder.Property(e => e.FailedLoginAttempts)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(e => e.LastFailedLoginAttemptUtc);

        builder.Property(e => e.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(e => e.LastActiveUtc);

        builder.Property(e => e.ActivationToken)
            .HasMaxLength(255);

        builder.Property(e => e.ActivationTokenExpiresUtc);

        builder.Property(e => e.PasswordResetToken)
            .HasMaxLength(255);

        builder.Property(e => e.PasswordResetTokenExpiresUtc);

        // Indexes
        builder.HasIndex(e => e.MemberId)
            .IsUnique()
            .HasFilter("\"IsDeleted\" = false");

        builder.HasIndex(e => e.IsActive)
            .HasFilter("\"IsActive\" = true AND \"IsDeleted\" = false");

        builder.HasIndex(e => e.IsLockedOut)
            .HasFilter("\"IsLockedOut\" = true AND \"IsDeleted\" = false");

        builder.HasIndex(e => e.ActivationToken)
            .HasFilter("\"ActivationToken\" IS NOT NULL AND \"IsDeleted\" = false");

        builder.HasIndex(e => e.PasswordResetToken)
            .HasFilter("\"PasswordResetToken\" IS NOT NULL AND \"IsDeleted\" = false");

        // Relationships
        builder.HasOne(e => e.Member)
            .WithOne(e => e.UserCredential)
            .HasForeignKey<UserCredential>(e => e.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.PasswordHistory)
            .WithOne(e => e.UserCredential)
            .HasForeignKey(e => e.UserCredentialId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
