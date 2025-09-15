using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Infrastructure.Data.Configurations;

/// <summary>
/// Configuração do Entity Framework para a entidade MedicalRecord.
/// </summary>
public class MedicalRecordConfiguration : IEntityTypeConfiguration<MedicalRecord>
{
    public void Configure(EntityTypeBuilder<MedicalRecord> builder)
    {
        // Configuração da tabela
        builder.ToTable("MedicalRecords");

        // Configuração da chave primária
        builder.HasKey(mr => mr.Id);

        // Configuração do campo Id
        builder.Property(mr => mr.Id)
            .IsRequired();

        // Configuração do campo MemberId
        builder.Property(mr => mr.MemberId)
            .IsRequired();

        // Configuração dos campos de string
        builder.Property(mr => mr.HealthPlanName)
            .HasMaxLength(100);

        builder.Property(mr => mr.HealthCardNumber)
            .HasMaxLength(50);

        builder.Property(mr => mr.BloodType)
            .HasMaxLength(10);

        builder.Property(mr => mr.HeartMedications)
            .HasMaxLength(500);

        builder.Property(mr => mr.DiabetesMedications)
            .HasMaxLength(500);

        builder.Property(mr => mr.KidneyMedications)
            .HasMaxLength(500);

        builder.Property(mr => mr.PsychologicalMedications)
            .HasMaxLength(500);

        builder.Property(mr => mr.RecentProblems)
            .HasMaxLength(1000);

        builder.Property(mr => mr.RecentMedications)
            .HasMaxLength(1000);

        builder.Property(mr => mr.RecentInjury)
            .HasMaxLength(1000);

        builder.Property(mr => mr.RecentFracture)
            .HasMaxLength(1000);

        builder.Property(mr => mr.ImmobilizationTime)
            .HasMaxLength(200);

        builder.Property(mr => mr.Surgeries)
            .HasMaxLength(1000);

        builder.Property(mr => mr.Hospitalization)
            .HasMaxLength(1000);

        builder.Property(mr => mr.DisabilityObservations)
            .HasMaxLength(1000);

        builder.Property(mr => mr.OtherProblems)
            .HasMaxLength(1000);

        builder.Property(mr => mr.OtherMedications)
            .HasMaxLength(1000);

        builder.Property(mr => mr.MedicalInfo)
            .HasMaxLength(2000);

        builder.Property(mr => mr.Allergies)
            .HasMaxLength(1000);

        builder.Property(mr => mr.Medications)
            .HasMaxLength(1000);

        builder.Property(mr => mr.SpecificAllergies)
            .HasMaxLength(2000);

        builder.Property(mr => mr.SpecificMedications)
            .HasMaxLength(2000);

        builder.Property(mr => mr.SpecificConditions)
            .HasMaxLength(2000);

        builder.Property(mr => mr.GeneralObservations)
            .HasMaxLength(2000);

        // Configuração dos campos de data (herdados de BaseEntity)
        builder.Property(mr => mr.CreatedAtUtc)
            .IsRequired();

        builder.Property(mr => mr.UpdatedAtUtc)
            .IsRequired();

        builder.Property(mr => mr.IsCompleteUpdatedAt);

        // Configuração dos campos booleanos
        builder.Property(mr => mr.HasHealthPlan)
            .HasDefaultValue(false);

        builder.Property(mr => mr.HasChickenpox)
            .HasDefaultValue(false);

        builder.Property(mr => mr.HasMeningitis)
            .HasDefaultValue(false);

        builder.Property(mr => mr.HasHepatitis)
            .HasDefaultValue(false);

        builder.Property(mr => mr.HasDengue)
            .HasDefaultValue(false);

        builder.Property(mr => mr.HasCholera)
            .HasDefaultValue(false);

        builder.Property(mr => mr.HasRubella)
            .HasDefaultValue(false);

        builder.Property(mr => mr.HasMeasles)
            .HasDefaultValue(false);

        builder.Property(mr => mr.HasTetanus)
            .HasDefaultValue(false);

        builder.Property(mr => mr.HasSmallpox)
            .HasDefaultValue(false);

        builder.Property(mr => mr.HasWhoopingCough)
            .HasDefaultValue(false);

        builder.Property(mr => mr.HasDiphtheria)
            .HasDefaultValue(false);

        builder.Property(mr => mr.HasMumps)
            .HasDefaultValue(false);

        builder.Property(mr => mr.HasBloodTransfusion)
            .HasDefaultValue(false);

        builder.Property(mr => mr.HasHeartProblems)
            .HasDefaultValue(false);

        builder.Property(mr => mr.HasDiabetes)
            .HasDefaultValue(false);

        builder.Property(mr => mr.HasKidneyProblems)
            .HasDefaultValue(false);

        builder.Property(mr => mr.HasPsychologicalProblems)
            .HasDefaultValue(false);

        builder.Property(mr => mr.HasSkinAllergy)
            .HasDefaultValue(false);

        builder.Property(mr => mr.HasFoodAllergy)
            .HasDefaultValue(false);

        builder.Property(mr => mr.HasDrugAllergy)
            .HasDefaultValue(false);

        builder.Property(mr => mr.HasRhinitis)
            .HasDefaultValue(false);

        builder.Property(mr => mr.HasBronchitis)
            .HasDefaultValue(false);

        builder.Property(mr => mr.HasPhysicalDisability)
            .HasDefaultValue(false);

        builder.Property(mr => mr.HasVisualDisability)
            .HasDefaultValue(false);

        builder.Property(mr => mr.HasAuditoryDisability)
            .HasDefaultValue(false);

        builder.Property(mr => mr.HasSpeechDisability)
            .HasDefaultValue(false);

        builder.Property(mr => mr.HasIntellectualDisability)
            .HasDefaultValue(false);

        builder.Property(mr => mr.HasPsychologicalDisability)
            .HasDefaultValue(false);

        builder.Property(mr => mr.HasAutism)
            .HasDefaultValue(false);

        builder.Property(mr => mr.IsComplete)
            .HasDefaultValue(false);

        // Configuração do relacionamento com Member
        builder.HasOne(mr => mr.Member)
            .WithOne(m => m.MedicalRecord)
            .HasForeignKey<MedicalRecord>(mr => mr.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuração do relacionamento com MedicalRecordTags
        builder.HasMany(mr => mr.MedicalRecordTags)
            .WithOne(mrt => mrt.MedicalRecord)
            .HasForeignKey(mrt => mrt.MedicalRecordId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuração de índices
        builder.HasIndex(mr => mr.MemberId)
            .IsUnique();

        builder.HasIndex(mr => mr.IsComplete);

        builder.HasIndex(mr => mr.CreatedAtUtc);
    }
}
