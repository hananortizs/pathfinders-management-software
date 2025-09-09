using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Domain.Enums;

namespace Pms.Backend.Infrastructure.Data.Configurations;

/// <summary>
/// Configuração do Entity Framework para a entidade BaptismRecord
/// </summary>
public class BaptismRecordConfiguration : BaseEntityConfiguration<BaptismRecord>
{
    /// <summary>
    /// Configura a entidade BaptismRecord
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    protected override void ConfigureEntity(EntityTypeBuilder<BaptismRecord> builder)
    {
        // Nome da tabela
        builder.ToTable("BaptismRecords");

        // Propriedades obrigatórias
        builder.Property(e => e.MemberId)
            .IsRequired();

        builder.Property(e => e.Type)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(e => e.Date)
            .IsRequired();

        // Propriedades opcionais
        builder.Property(e => e.PlaceText)
            .HasMaxLength(160);

        builder.Property(e => e.PastorText)
            .HasMaxLength(120);

        builder.Property(e => e.EvidenceUrl)
            .HasMaxLength(255);

        // Relacionamentos
        builder.HasOne(e => e.Member)
            .WithMany(m => m.BaptismRecords)
            .HasForeignKey(e => e.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Church)
            .WithMany()
            .HasForeignKey(e => e.ChurchId)
            .OnDelete(DeleteBehavior.SetNull);

        // Índices
        builder.HasIndex(e => e.MemberId)
            .HasDatabaseName("IX_BaptismRecords_MemberId");

        builder.HasIndex(e => new { e.MemberId, e.Type })
            .HasDatabaseName("IX_BaptismRecords_MemberId_Type");

        builder.HasIndex(e => e.Date)
            .HasDatabaseName("IX_BaptismRecords_Date");

        // Check constraints
        builder.ToTable("BaptismRecords", t => 
        {
            t.HasCheckConstraint("CK_BaptismRecords_Date_NotFuture", "\"Date\" <= NOW()");
            t.HasCheckConstraint("CK_BaptismRecords_Place_Required", 
                "(\"ChurchId\" IS NOT NULL) OR (\"PlaceText\" IS NOT NULL AND LENGTH(\"PlaceText\") > 0)");
        });
    }
}
