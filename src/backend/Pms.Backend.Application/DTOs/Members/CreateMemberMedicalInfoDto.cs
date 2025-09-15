using System.ComponentModel.DataAnnotations;

namespace Pms.Backend.Application.DTOs.Members;

/// <summary>
/// DTO para informações médicas
/// </summary>
public class CreateMemberMedicalInfoDto
{
    // Informações básicas de saúde
    /// <summary>
    /// Indica se possui plano de saúde
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
    /// Tipo sanguíneo
    /// </summary>
    public string? BloodType { get; set; }

    // Histórico de doenças
    /// <summary>
    /// Indica se teve catapora
    /// </summary>
    public bool HasChickenpox { get; set; } = false;

    /// <summary>
    /// Indica se teve meningite
    /// </summary>
    public bool HasMeningitis { get; set; } = false;

    /// <summary>
    /// Indica se teve hepatite
    /// </summary>
    public bool HasHepatitis { get; set; } = false;

    /// <summary>
    /// Indica se teve dengue
    /// </summary>
    public bool HasDengue { get; set; } = false;

    /// <summary>
    /// Indica se teve cólera
    /// </summary>
    public bool HasCholera { get; set; } = false;

    /// <summary>
    /// Indica se teve rubéola
    /// </summary>
    public bool HasRubella { get; set; } = false;

    /// <summary>
    /// Indica se teve sarampo
    /// </summary>
    public bool HasMeasles { get; set; } = false;

    /// <summary>
    /// Indica se teve tétano
    /// </summary>
    public bool HasTetanus { get; set; } = false;

    /// <summary>
    /// Indica se teve varíola
    /// </summary>
    public bool HasSmallpox { get; set; } = false;

    /// <summary>
    /// Indica se teve coqueluche
    /// </summary>
    public bool HasWhoopingCough { get; set; } = false;

    /// <summary>
    /// Indica se teve difteria
    /// </summary>
    public bool HasDiphtheria { get; set; } = false;

    /// <summary>
    /// Indica se teve caxumba
    /// </summary>
    public bool HasMumps { get; set; } = false;

    /// <summary>
    /// Indica se teve transfusão de sangue
    /// </summary>
    public bool HasBloodTransfusion { get; set; } = false;

    // Condições crônicas
    /// <summary>
    /// Indica se tem problemas cardíacos
    /// </summary>
    public bool HasHeartProblems { get; set; } = false;

    /// <summary>
    /// Medicamentos para problemas cardíacos
    /// </summary>
    public string? HeartMedications { get; set; }

    /// <summary>
    /// Indica se tem diabetes
    /// </summary>
    public bool HasDiabetes { get; set; } = false;

    /// <summary>
    /// Medicamentos para diabetes
    /// </summary>
    public string? DiabetesMedications { get; set; }

    /// <summary>
    /// Indica se tem problemas renais
    /// </summary>
    public bool HasKidneyProblems { get; set; } = false;

    /// <summary>
    /// Medicamentos para problemas renais
    /// </summary>
    public string? KidneyMedications { get; set; }

    /// <summary>
    /// Indica se tem problemas psicológicos
    /// </summary>
    public bool HasPsychologicalProblems { get; set; } = false;

    /// <summary>
    /// Medicamentos para problemas psicológicos
    /// </summary>
    public string? PsychologicalMedications { get; set; }

    // Alergias específicas
    /// <summary>
    /// Indica se tem alergia de pele
    /// </summary>
    public bool HasSkinAllergy { get; set; } = false;

    /// <summary>
    /// Indica se tem alergia alimentar
    /// </summary>
    public bool HasFoodAllergy { get; set; } = false;

    /// <summary>
    /// Indica se tem alergia a medicamentos
    /// </summary>
    public bool HasDrugAllergy { get; set; } = false;

    /// <summary>
    /// Indica se tem rinite
    /// </summary>
    public bool HasRhinitis { get; set; } = false;

    /// <summary>
    /// Indica se tem bronquite
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
    /// Lesões recentes
    /// </summary>
    public string? RecentInjury { get; set; }

    /// <summary>
    /// Fraturas recentes
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
    /// Hospitalizações
    /// </summary>
    public string? Hospitalization { get; set; }

    // Deficiências
    /// <summary>
    /// Indica se tem deficiência física
    /// </summary>
    public bool HasPhysicalDisability { get; set; } = false;

    /// <summary>
    /// Indica se tem deficiência visual
    /// </summary>
    public bool HasVisualDisability { get; set; } = false;

    /// <summary>
    /// Indica se tem deficiência auditiva
    /// </summary>
    public bool HasAuditoryDisability { get; set; } = false;

    /// <summary>
    /// Indica se tem deficiência de fala
    /// </summary>
    public bool HasSpeechDisability { get; set; } = false;

    /// <summary>
    /// Indica se tem deficiência intelectual
    /// </summary>
    public bool HasIntellectualDisability { get; set; } = false;

    /// <summary>
    /// Indica se tem deficiência psicológica
    /// </summary>
    public bool HasPsychologicalDisability { get; set; } = false;

    /// <summary>
    /// Indica se tem autismo
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
    /// Alergias gerais
    /// </summary>
    public string? Allergies { get; set; }

    /// <summary>
    /// Medicamentos gerais
    /// </summary>
    public string? Medications { get; set; }

    // Campos de texto livre para informações não estruturadas
    /// <summary>
    /// Alergias específicas detalhadas
    /// </summary>
    [StringLength(2000, ErrorMessage = "Alergias específicas não podem exceder 2000 caracteres")]
    public string? SpecificAllergies { get; set; }

    /// <summary>
    /// Medicamentos específicos detalhados
    /// </summary>
    [StringLength(2000, ErrorMessage = "Medicamentos específicos não podem exceder 2000 caracteres")]
    public string? SpecificMedications { get; set; }

    /// <summary>
    /// Condições específicas detalhadas
    /// </summary>
    [StringLength(2000, ErrorMessage = "Condições específicas não podem exceder 2000 caracteres")]
    public string? SpecificConditions { get; set; }

    /// <summary>
    /// Observações gerais
    /// </summary>
    [StringLength(2000, ErrorMessage = "Observações gerais não podem exceder 2000 caracteres")]
    public string? GeneralObservations { get; set; }

    /// <summary>
    /// Tags médicas associadas
    /// </summary>
    public List<CreateMemberMedicalTagDto>? MedicalTags { get; set; }
}
