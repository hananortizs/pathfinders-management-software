using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Domain.Enums;

namespace Pms.Backend.Infrastructure.Data.Configurations;

/// <summary>
/// Configuração do Entity Framework para a entidade DisciplineRecord
/// </summary>
public class DisciplineRecordConfiguration : BaseEntityConfiguration<DisciplineRecord>
{
    /// <summary>
    /// Configura a entidade DisciplineRecord
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    protected override void ConfigureEntity(EntityTypeBuilder<DisciplineRecord> builder)
    {
        // Nome da tabela
        builder.ToTable("DisciplineRecords");

        // Propriedades obrigatórias
        builder.Property(e => e.MemberId)
            .IsRequired();

        builder.Property(e => e.Type)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(e => e.StartDate)
            .IsRequired();

        // Propriedades opcionais
        builder.Property(e => e.EndDate);

        builder.Property(e => e.PlaceText)
            .HasMaxLength(160);

        builder.Property(e => e.Notes)
            .HasMaxLength(500);

        // Relacionamentos
        builder.HasOne(e => e.Member)
            .WithMany(m => m.DisciplineRecords)
            .HasForeignKey(e => e.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Church)
            .WithMany()
            .HasForeignKey(e => e.ChurchId)
            .OnDelete(DeleteBehavior.SetNull);

        // Índices
        builder.HasIndex(e => e.MemberId)
            .HasDatabaseName("IX_DisciplineRecords_MemberId");

        builder.HasIndex(e => new { e.MemberId, e.Type })
            .HasDatabaseName("IX_DisciplineRecords_MemberId_Type");

        builder.HasIndex(e => e.StartDate)
            .HasDatabaseName("IX_DisciplineRecords_StartDate");

        builder.HasIndex(e => new { e.MemberId, e.StartDate, e.EndDate })
            .HasDatabaseName("IX_DisciplineRecords_MemberId_Dates");

        // Índice para disciplinas ativas
        builder.HasIndex(e => new { e.MemberId, e.Type, e.StartDate, e.EndDate })
            .HasFilter("\"IsDeleted\" = false")
            .HasDatabaseName("IX_DisciplineRecords_Active");

        // Check constraints
        builder.ToTable("DisciplineRecords", t => 
        {
            t.HasCheckConstraint("CK_DisciplineRecords_StartDate_NotFuture", "\"StartDate\" <= NOW()");
            t.HasCheckConstraint("CK_DisciplineRecords_EndDate_AfterStart", 
                "(\"EndDate\" IS NULL) OR (\"EndDate\" >= \"StartDate\")");
            t.HasCheckConstraint("CK_DisciplineRecords_Place_Required", 
                "(\"ChurchId\" IS NOT NULL) OR (\"PlaceText\" IS NOT NULL AND LENGTH(\"PlaceText\") > 0)");
        });
    }
}
