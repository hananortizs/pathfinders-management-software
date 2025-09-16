using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Domain.Helpers;

namespace Pms.Backend.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration for Member entity
/// </summary>
public class MemberConfiguration : BaseEntityConfiguration<Member>
{
    /// <summary>
    /// Configures the Member entity
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    protected override void ConfigureEntity(EntityTypeBuilder<Member> builder)
    {
        // Properties
        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(100)
            .HasConversion(
                v => NormalizeNameForDatabase(v) ?? string.Empty,
                v => v);

        builder.Property(e => e.MiddleNames)
            .HasMaxLength(200)
            .HasConversion(
                v => NormalizeNameForDatabase(v),
                v => v);

        builder.Property(e => e.SocialName)
            .HasMaxLength(150)
            .HasConversion(
                v => NormalizeNameForDatabase(v),
                v => v);

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(100)
            .HasConversion(
                v => NormalizeNameForDatabase(v) ?? string.Empty,
                v => v);

        // Email and Phone are now handled through Contact entity

        builder.Property(e => e.Gender)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(e => e.Cpf)
            .HasMaxLength(11)
            .HasConversion(
                v => NormalizeCpfForDatabase(v),
                v => v);

        builder.Property(e => e.Rg)
            .HasMaxLength(9)
            .HasConversion(
                v => NormalizeRgForDatabase(v),
                v => v);

        builder.Property(e => e.DateOfBirth)
            .IsRequired();

        // Address fields removed - now using centralized Address entity

        builder.Property(e => e.Baptized)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(e => e.BaptizedAt);

        builder.Property(e => e.BaptizedPlace)
            .HasMaxLength(200);

        builder.Property(e => e.ScarfInvested)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(e => e.ScarfInvestedAt);

        // Indexes
        // Email uniqueness is now handled through Contact entity

        // Index for age queries
        builder.HasIndex(e => e.DateOfBirth);

        // Index for gender queries
        builder.HasIndex(e => e.Gender);

        // Index for baptism status
        builder.HasIndex(e => e.Baptized);

        // Index for scarf status
        builder.HasIndex(e => e.ScarfInvested);

        // Relationships
        builder.HasOne(e => e.UserCredential)
            .WithOne(e => e.Member)
            .HasForeignKey<UserCredential>(e => e.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Memberships)
            .WithOne(e => e.Member)
            .HasForeignKey(e => e.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Assignments)
            .WithOne(e => e.Member)
            .HasForeignKey(e => e.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.TimelineEntries)
            .WithOne(e => e.Member)
            .HasForeignKey(e => e.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relationship with Contacts (polymorphic)
        builder.HasMany(e => e.Contacts)
            .WithOne()
            .HasForeignKey(c => c.EntityId)
            .HasPrincipalKey(e => e.Id)
            .OnDelete(DeleteBehavior.Cascade);

        // Constraints
        builder.ToTable(t => t.HasCheckConstraint("CK_Member_Baptism", "\"Baptized\" = false OR (\"BaptizedAt\" IS NOT NULL AND \"BaptizedPlace\" IS NOT NULL)"));
        builder.ToTable(t => t.HasCheckConstraint("CK_Member_Scarf", "\"ScarfInvested\" = false OR \"ScarfInvestedAt\" IS NOT NULL"));
    }

    /// <summary>
    /// Normalizes email for database storage (lowercase, trimmed)
    /// </summary>
    /// <param name="email">Email input</param>
    /// <returns>Normalized email for database</returns>
    private static string? NormalizeEmailForDatabase(string? email)
    {
        return EmailHelper.NormalizeEmail(email);
    }

    /// <summary>
    /// Normalizes phone for database storage (digits only with country code)
    /// </summary>
    /// <param name="phone">Phone input</param>
    /// <returns>Normalized phone for database</returns>
    private static string? NormalizePhoneForDatabase(string? phone)
    {
        return PhoneHelper.NormalizePhone(phone);
    }

    /// <summary>
    /// Normalizes CPF for database storage (digits only)
    /// </summary>
    /// <param name="cpf">CPF input</param>
    /// <returns>Normalized CPF for database</returns>
    private static string? NormalizeCpfForDatabase(string? cpf)
    {
        return CpfHelper.NormalizeCpf(cpf);
    }

    /// <summary>
    /// Normalizes RG for database storage (digits and X only)
    /// </summary>
    /// <param name="rg">RG input</param>
    /// <returns>Normalized RG for database</returns>
    private static string? NormalizeRgForDatabase(string? rg)
    {
        return RgHelper.NormalizeRg(rg);
    }

    /// <summary>
    /// Normalizes name for database storage (proper case, trimmed)
    /// </summary>
    /// <param name="name">Name input</param>
    /// <returns>Normalized name for database</returns>
    private static string? NormalizeNameForDatabase(string? name)
    {
        return NameHelper.NormalizeName(name);
    }
}
