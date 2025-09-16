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
    /// <summary>
    /// Indica se o membro possui plano de saúde
    /// </summary>
    public bool HasHealthPlan { get; set; } = false;

    /// <summary>
    /// Nome do plano de saúde
    /// </summary>
    public string? HealthPlanName { get; set; }

    /// <summary>
    /// Número da carteirinha do plano de saúde
    /// </summary>
    public string? HealthCardNumber { get; set; }

    /// <summary>
    /// Tipo sanguíneo do membro
    /// </summary>
    public string? BloodType { get; set; }

    // Histórico de doenças
    /// <summary>
    /// Indica se o membro já teve catapora
    /// </summary>
    public bool HasChickenpox { get; set; } = false;

    /// <summary>
    /// Indica se o membro já teve meningite
    /// </summary>
    public bool HasMeningitis { get; set; } = false;

    /// <summary>
    /// Indica se o membro já teve hepatite
    /// </summary>
    public bool HasHepatitis { get; set; } = false;

    /// <summary>
    /// Indica se o membro já teve dengue
    /// </summary>
    public bool HasDengue { get; set; } = false;

    /// <summary>
    /// Indica se o membro já teve cólera
    /// </summary>
    public bool HasCholera { get; set; } = false;

    /// <summary>
    /// Indica se o membro já teve rubéola
    /// </summary>
    public bool HasRubella { get; set; } = false;

    /// <summary>
    /// Indica se o membro já teve sarampo
    /// </summary>
    public bool HasMeasles { get; set; } = false;

    /// <summary>
    /// Indica se o membro já teve tétano
    /// </summary>
    public bool HasTetanus { get; set; } = false;

    /// <summary>
    /// Indica se o membro já teve varíola
    /// </summary>
    public bool HasSmallpox { get; set; } = false;

    /// <summary>
    /// Indica se o membro já teve coqueluche
    /// </summary>
    public bool HasWhoopingCough { get; set; } = false;

    /// <summary>
    /// Indica se o membro já teve difteria
    /// </summary>
    public bool HasDiphtheria { get; set; } = false;

    /// <summary>
    /// Indica se o membro já teve caxumba
    /// </summary>
    public bool HasMumps { get; set; } = false;

    /// <summary>
    /// Indica se o membro já teve transfusão de sangue
    /// </summary>
    public bool HasBloodTransfusion { get; set; } = false;

    // Condições crônicas
    /// <summary>
    /// Indica se o membro tem problemas cardíacos
    /// </summary>
    public bool HasHeartProblems { get; set; } = false;

    /// <summary>
    /// Medicamentos para problemas cardíacos
    /// </summary>
    public string? HeartMedications { get; set; }

    /// <summary>
    /// Indica se o membro tem diabetes
    /// </summary>
    public bool HasDiabetes { get; set; } = false;

    /// <summary>
    /// Medicamentos para diabetes
    /// </summary>
    public string? DiabetesMedications { get; set; }

    /// <summary>
    /// Indica se o membro tem problemas renais
    /// </summary>
    public bool HasKidneyProblems { get; set; } = false;

    /// <summary>
    /// Medicamentos para problemas renais
    /// </summary>
    public string? KidneyMedications { get; set; }

    /// <summary>
    /// Indica se o membro tem problemas psicológicos
    /// </summary>
    public bool HasPsychologicalProblems { get; set; } = false;

    /// <summary>
    /// Medicamentos para problemas psicológicos
    /// </summary>
    public string? PsychologicalMedications { get; set; }

    // Alergias específicas
    /// <summary>
    /// Indica se o membro tem alergia de pele
    /// </summary>
    public bool HasSkinAllergy { get; set; } = false;

    /// <summary>
    /// Indica se o membro tem alergia alimentar
    /// </summary>
    public bool HasFoodAllergy { get; set; } = false;

    /// <summary>
    /// Indica se o membro tem alergia a medicamentos
    /// </summary>
    public bool HasDrugAllergy { get; set; } = false;

    /// <summary>
    /// Indica se o membro tem rinite
    /// </summary>
    public bool HasRhinitis { get; set; } = false;

    /// <summary>
    /// Indica se o membro tem bronquite
    /// </summary>
    public bool HasBronchitis { get; set; } = false;

    // Problemas recentes
    /// <summary>
    /// Problemas de saúde recentes
    /// </summary>
    public string? RecentProblems { get; set; }

    /// <summary>
    /// Medicamentos recentes
    /// </summary>
    public string? RecentMedications { get; set; }

    // Lesões e procedimentos
    /// <summary>
    /// Lesões recentes do membro
    /// </summary>
    public string? RecentInjury { get; set; }

    /// <summary>
    /// Fraturas recentes do membro
    /// </summary>
    public string? RecentFracture { get; set; }

    /// <summary>
    /// Tempo de imobilização
    /// </summary>
    public string? ImmobilizationTime { get; set; }

    /// <summary>
    /// Cirurgias realizadas
    /// </summary>
    public string? Surgeries { get; set; }

    /// <summary>
    /// Histórico de hospitalizações
    /// </summary>
    public string? Hospitalization { get; set; }

    // Deficiências
    /// <summary>
    /// Indica se o membro tem deficiência física
    /// </summary>
    public bool HasPhysicalDisability { get; set; } = false;

    /// <summary>
    /// Indica se o membro tem deficiência visual
    /// </summary>
    public bool HasVisualDisability { get; set; } = false;

    /// <summary>
    /// Indica se o membro tem deficiência auditiva
    /// </summary>
    public bool HasAuditoryDisability { get; set; } = false;

    /// <summary>
    /// Indica se o membro tem deficiência de fala
    /// </summary>
    public bool HasSpeechDisability { get; set; } = false;

    /// <summary>
    /// Indica se o membro tem deficiência intelectual
    /// </summary>
    public bool HasIntellectualDisability { get; set; } = false;

    /// <summary>
    /// Indica se o membro tem deficiência psicológica
    /// </summary>
    public bool HasPsychologicalDisability { get; set; } = false;

    /// <summary>
    /// Indica se o membro tem autismo
    /// </summary>
    public bool HasAutism { get; set; } = false;

    /// <summary>
    /// Observações sobre deficiências
    /// </summary>
    public string? DisabilityObservations { get; set; }

    // Outros problemas
    /// <summary>
    /// Outros problemas de saúde
    /// </summary>
    public string? OtherProblems { get; set; }

    /// <summary>
    /// Outros medicamentos
    /// </summary>
    public string? OtherMedications { get; set; }

    // Informações gerais
    /// <summary>
    /// Informações médicas gerais
    /// </summary>
    public string? MedicalInfo { get; set; }

    /// <summary>
    /// Alergias do membro
    /// </summary>
    public string? Allergies { get; set; }

    /// <summary>
    /// Medicamentos do membro
    /// </summary>
    public string? Medications { get; set; }

    // Campos de texto livre para informações não estruturadas
    /// <summary>
    /// Alergias específicas do membro
    /// </summary>
    [StringLength(2000, ErrorMessage = "Alergias específicas não podem exceder 2000 caracteres")]
    public string? SpecificAllergies { get; set; }

    /// <summary>
    /// Medicamentos específicos do membro
    /// </summary>
    [StringLength(2000, ErrorMessage = "Medicamentos específicos não podem exceder 2000 caracteres")]
    public string? SpecificMedications { get; set; }

    /// <summary>
    /// Condições médicas específicas do membro
    /// </summary>
    [StringLength(2000, ErrorMessage = "Condições específicas não podem exceder 2000 caracteres")]
    public string? SpecificConditions { get; set; }

    /// <summary>
    /// Observações gerais sobre a saúde do membro
    /// </summary>
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
