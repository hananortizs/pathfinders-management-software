using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Infrastructure.Data.Configurations;

/// <summary>
/// Configuração do Entity Framework para a entidade Pastor
/// </summary>
public class PastorConfiguration : BaseEntityConfiguration<Pastor>
{
    /// <summary>
    /// Configura a entidade Pastor
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    protected override void ConfigureEntity(EntityTypeBuilder<Pastor> builder)
    {
        // Nome da tabela
        builder.ToTable("Pastors");

        // Propriedades obrigatórias
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(255);

        // Propriedades opcionais
        builder.Property(e => e.Phone)
            .HasMaxLength(20);

        builder.Property(e => e.OrdinationChurch)
            .HasMaxLength(200);

        builder.Property(e => e.Notes)
            .HasMaxLength(1000);

        // Índices
        builder.HasIndex(e => e.Email)
            .IsUnique()
            .HasDatabaseName("IX_Pastors_Email");

        builder.HasIndex(e => e.Name)
            .HasDatabaseName("IX_Pastors_Name");

        builder.HasIndex(e => e.IsActive)
            .HasDatabaseName("IX_Pastors_IsActive");

        builder.HasIndex(e => new { e.IsActive, e.StartDate, e.EndDate })
            .HasDatabaseName("IX_Pastors_ActivePeriod");

        // Check constraints
        builder.ToTable("Pastors", t =>
        {
            t.HasCheckConstraint("CK_Pastors_Email_Valid", "LENGTH(\"Email\") > 0 AND \"Email\" ~ '^[^@]+@[^@]+\\.[^@]+$'");
            t.HasCheckConstraint("CK_Pastors_Phone_Valid",
                "(\"Phone\" IS NULL) OR (LENGTH(\"Phone\") >= 10)");
            t.HasCheckConstraint("CK_Pastors_StartDate_NotFuture",
                "(\"StartDate\" IS NULL) OR (\"StartDate\" <= NOW())");
            t.HasCheckConstraint("CK_Pastors_EndDate_AfterStart",
                "(\"EndDate\" IS NULL) OR (\"StartDate\" IS NULL) OR (\"EndDate\" >= \"StartDate\")");
        });
    }
}
