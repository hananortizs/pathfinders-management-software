using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Infrastructure.Data.Configurations;

/// <summary>
/// Configuração do Entity Framework para a entidade MedicalRecordTag.
/// </summary>
public class MedicalRecordTagConfiguration : IEntityTypeConfiguration<MedicalRecordTag>
{
    public void Configure(EntityTypeBuilder<MedicalRecordTag> builder)
    {
        // Configuração da tabela
        builder.ToTable("MedicalRecordTags");

        // Configuração da chave primária
        builder.HasKey(mrt => mrt.Id);

        // Configuração do campo Id
        builder.Property(mrt => mrt.Id)
            .IsRequired();

        // Configuração dos campos de chave estrangeira
        builder.Property(mrt => mrt.MedicalRecordId)
            .IsRequired();

        builder.Property(mrt => mrt.MedicalTagId)
            .IsRequired();

        // Configuração dos campos de string
        builder.Property(mrt => mrt.SpecificDescription)
            .HasMaxLength(1000);

        // Configuração dos campos de data (herdados de BaseEntity)
        builder.Property(mrt => mrt.CreatedAtUtc)
            .IsRequired();

        builder.Property(mrt => mrt.UpdatedAtUtc)
            .IsRequired();

        builder.Property(mrt => mrt.StartDate);

        builder.Property(mrt => mrt.EndDate);

        // Configuração dos campos booleanos
        builder.Property(mrt => mrt.IsActive)
            .HasDefaultValue(true);

        // Configuração dos enums
        builder.Property(mrt => mrt.Severity)
            .IsRequired()
            .HasConversion<string>();

        // Configuração do relacionamento com MedicalRecord
        builder.HasOne(mrt => mrt.MedicalRecord)
            .WithMany(mr => mr.MedicalRecordTags)
            .HasForeignKey(mrt => mrt.MedicalRecordId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuração do relacionamento com MedicalTag
        builder.HasOne(mrt => mrt.MedicalTag)
            .WithMany(mt => mt.MedicalRecordTags)
            .HasForeignKey(mrt => mrt.MedicalTagId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuração de índices
        builder.HasIndex(mrt => mrt.MedicalRecordId);

        builder.HasIndex(mrt => mrt.MedicalTagId);

        builder.HasIndex(mrt => mrt.Severity);

        builder.HasIndex(mrt => mrt.IsActive);

        builder.HasIndex(mrt => mrt.CreatedAtUtc);

        // Configuração de índice único composto
        builder.HasIndex(mrt => new { mrt.MedicalRecordId, mrt.MedicalTagId })
            .IsUnique();
    }
}
