using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Infrastructure.Data.Configurations;

/// <summary>
/// Configuração do Entity Framework para a entidade MedicalTag.
/// </summary>
public class MedicalTagConfiguration : IEntityTypeConfiguration<MedicalTag>
{
    public void Configure(EntityTypeBuilder<MedicalTag> builder)
    {
        // Configuração da tabela
        builder.ToTable("MedicalTags");

        // Configuração da chave primária
        builder.HasKey(mt => mt.Id);

        // Configuração do campo Id
        builder.Property(mt => mt.Id)
            .IsRequired();

        // Configuração dos campos de string
        builder.Property(mt => mt.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(mt => mt.Description)
            .HasMaxLength(500);

        // Configuração dos campos de data (herdados de BaseEntity)
        builder.Property(mt => mt.CreatedAtUtc)
            .IsRequired();

        builder.Property(mt => mt.UpdatedAtUtc)
            .IsRequired();

        // Configuração dos campos booleanos
        builder.Property(mt => mt.IsActive)
            .HasDefaultValue(true);

        // Configuração dos enums
        builder.Property(mt => mt.Category)
            .IsRequired()
            .HasConversion<string>();

        // Configuração do relacionamento com MedicalRecordTags
        builder.HasMany(mt => mt.MedicalRecordTags)
            .WithOne(mrt => mrt.MedicalTag)
            .HasForeignKey(mrt => mrt.MedicalTagId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuração de índices
        builder.HasIndex(mt => mt.Name);

        builder.HasIndex(mt => mt.Category);

        builder.HasIndex(mt => mt.IsActive);

        builder.HasIndex(mt => mt.CreatedAtUtc);

        // Configuração de índice único para nome
        builder.HasIndex(mt => mt.Name)
            .IsUnique();
    }
}
