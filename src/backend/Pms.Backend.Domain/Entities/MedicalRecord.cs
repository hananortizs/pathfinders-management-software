using System.ComponentModel.DataAnnotations;

namespace Pms.Backend.Domain.Entities;

/// <summary>
/// Representa o registro médico de um membro do clube de desbravadores.
/// Cada membro pode ter no máximo um registro médico.
/// </summary>
public class MedicalRecord : BaseEntity
{
    /// <summary>
    /// Identificador único do membro associado a este registro médico.
    /// </summary>
    [Required]
    public Guid MemberId { get; set; }

    /// <summary>
    /// Navegação para o membro associado a este registro médico.
    /// </summary>
    public virtual Member Member { get; set; } = null!;

    // Informações básicas de saúde
    public bool HasHealthPlan { get; set; } = false;
    public string? HealthPlanName { get; set; }
    public string? HealthCardNumber { get; set; }
    public string? BloodType { get; set; }

    // Histórico de doenças
    public bool HasChickenpox { get; set; } = false;
    public bool HasMeningitis { get; set; } = false;
    public bool HasHepatitis { get; set; } = false;
    public bool HasDengue { get; set; } = false;
    public bool HasCholera { get; set; } = false;
    public bool HasRubella { get; set; } = false;
    public bool HasMeasles { get; set; } = false;
    public bool HasTetanus { get; set; } = false;
    public bool HasSmallpox { get; set; } = false;
    public bool HasWhoopingCough { get; set; } = false;
    public bool HasDiphtheria { get; set; } = false;
    public bool HasMumps { get; set; } = false;
    public bool HasBloodTransfusion { get; set; } = false;

    // Condições crônicas
    public bool HasHeartProblems { get; set; } = false;
    public string? HeartMedications { get; set; }
    public bool HasDiabetes { get; set; } = false;
    public string? DiabetesMedications { get; set; }
    public bool HasKidneyProblems { get; set; } = false;
    public string? KidneyMedications { get; set; }
    public bool HasPsychologicalProblems { get; set; } = false;
    public string? PsychologicalMedications { get; set; }

    // Alergias específicas
    public bool HasSkinAllergy { get; set; } = false;
    public bool HasFoodAllergy { get; set; } = false;
    public bool HasDrugAllergy { get; set; } = false;
    public bool HasRhinitis { get; set; } = false;
    public bool HasBronchitis { get; set; } = false;

    // Problemas recentes
    public string? RecentProblems { get; set; }
    public string? RecentMedications { get; set; }

    // Lesões e procedimentos
    public string? RecentInjury { get; set; }
    public string? RecentFracture { get; set; }
    public string? ImmobilizationTime { get; set; }
    public string? Surgeries { get; set; }
    public string? Hospitalization { get; set; }

    // Deficiências
    public bool HasPhysicalDisability { get; set; } = false;
    public bool HasVisualDisability { get; set; } = false;
    public bool HasAuditoryDisability { get; set; } = false;
    public bool HasSpeechDisability { get; set; } = false;
    public bool HasIntellectualDisability { get; set; } = false;
    public bool HasPsychologicalDisability { get; set; } = false;
    public bool HasAutism { get; set; } = false;
    public string? DisabilityObservations { get; set; }

    // Outros problemas
    public string? OtherProblems { get; set; }
    public string? OtherMedications { get; set; }

    // Informações gerais
    public string? MedicalInfo { get; set; }
    public string? Allergies { get; set; }
    public string? Medications { get; set; }

    // Campos de texto livre para informações não estruturadas
    [StringLength(2000, ErrorMessage = "Alergias específicas não podem exceder 2000 caracteres")]
    public string? SpecificAllergies { get; set; }

    [StringLength(2000, ErrorMessage = "Medicamentos específicos não podem exceder 2000 caracteres")]
    public string? SpecificMedications { get; set; }

    [StringLength(2000, ErrorMessage = "Condições específicas não podem exceder 2000 caracteres")]
    public string? SpecificConditions { get; set; }

    [StringLength(2000, ErrorMessage = "Observações gerais não podem exceder 2000 caracteres")]
    public string? GeneralObservations { get; set; }

    /// <summary>
    /// Tags associadas ao registro médico
    /// </summary>
    public virtual ICollection<MedicalRecordTag> MedicalRecordTags { get; set; } = new List<MedicalRecordTag>();

    /// <summary>
    /// Campo computado para indicar se a ficha médica está completa
    /// Atualizado automaticamente via trigger ou método de negócio
    /// </summary>
    public bool IsComplete { get; set; } = false;

    /// <summary>
    /// Data da última atualização do campo IsComplete
    /// </summary>
    public DateTime? IsCompleteUpdatedAt { get; set; }

    /// <summary>
    /// Verifica se a ficha médica está completa para ativação
    /// MÉTODO OTIMIZADO: Usa o campo IsComplete quando disponível
    /// </summary>
    public bool IsCompleteForActivation()
    {
        // Se o campo IsComplete está atualizado, usa ele (mais performático)
        if (IsCompleteUpdatedAt.HasValue && IsCompleteUpdatedAt.Value > UpdatedAtUtc)
        {
            return IsComplete;
        }

        // Caso contrário, calcula e atualiza o campo
        var isComplete = CalculateCompleteness();
        UpdateCompletenessStatus(isComplete);
        return isComplete;
    }

    /// <summary>
    /// Calcula se a ficha médica está completa (método otimizado)
    /// </summary>
    private bool CalculateCompleteness()
    {
        // Verifica campos de texto primeiro (mais prováveis de ter conteúdo)
        if (!string.IsNullOrWhiteSpace(MedicalInfo) ||
            !string.IsNullOrWhiteSpace(Allergies) ||
            !string.IsNullOrWhiteSpace(Medications) ||
            !string.IsNullOrWhiteSpace(SpecificAllergies) ||
            !string.IsNullOrWhiteSpace(SpecificMedications) ||
            !string.IsNullOrWhiteSpace(SpecificConditions) ||
            !string.IsNullOrWhiteSpace(GeneralObservations))
        {
            return true;
        }

        // Verifica campos booleanos em lotes (mais eficiente)
        var hasBasicInfo = HasHealthPlan || !string.IsNullOrWhiteSpace(BloodType);
        if (hasBasicInfo) return true;

        var hasDiseaseHistory = HasChickenpox || HasMeningitis || HasHepatitis || HasDengue ||
                               HasCholera || HasRubella || HasMeasles || HasTetanus ||
                               HasSmallpox || HasWhoopingCough || HasDiphtheria || HasMumps ||
                               HasBloodTransfusion;
        if (hasDiseaseHistory) return true;

        var hasChronicConditions = HasHeartProblems || HasDiabetes || HasKidneyProblems || HasPsychologicalProblems;
        if (hasChronicConditions) return true;

        var hasAllergies = HasSkinAllergy || HasFoodAllergy || HasDrugAllergy || HasRhinitis || HasBronchitis;
        if (hasAllergies) return true;

        var hasDisabilities = HasPhysicalDisability || HasVisualDisability || HasAuditoryDisability ||
                             HasSpeechDisability || HasIntellectualDisability || HasPsychologicalDisability || HasAutism;
        if (hasDisabilities) return true;

        // Verifica tags apenas se necessário (mais custoso)
        return MedicalRecordTags.Any(t => t.IsActive);
    }

    /// <summary>
    /// Atualiza o status de completude da ficha médica
    /// </summary>
    public void UpdateCompletenessStatus(bool isComplete)
    {
        IsComplete = isComplete;
        IsCompleteUpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Força o recálculo do status de completude
    /// </summary>
    public void RecalculateCompleteness()
    {
        var isComplete = CalculateCompleteness();
        UpdateCompletenessStatus(isComplete);
    }
}
