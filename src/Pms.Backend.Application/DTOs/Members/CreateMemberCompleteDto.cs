using System.ComponentModel.DataAnnotations;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Domain.Enums;

namespace Pms.Backend.Application.DTOs.Members;

/// <summary>
/// DTO para cadastro completo de membros com informações aninhadas
/// </summary>
public class CreateMemberCompleteDto
{
    // Campos mínimos obrigatórios
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome não pode exceder 100 caracteres")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Sobrenome é obrigatório")]
    [StringLength(100, ErrorMessage = "Sobrenome não pode exceder 100 caracteres")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Data de nascimento é obrigatória")]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Gênero é obrigatório")]
    public MemberGender Gender { get; set; }

    // Campos opcionais básicos
    [StringLength(200, ErrorMessage = "Nomes do meio não podem exceder 200 caracteres")]
    public string? MiddleNames { get; set; }

    [StringLength(100, ErrorMessage = "Nome social não pode exceder 100 caracteres")]
    public string? SocialName { get; set; }

    [StringLength(14, ErrorMessage = "CPF deve ter 11 dígitos")]
    public string? Cpf { get; set; }

    [StringLength(20, ErrorMessage = "RG não pode exceder 20 caracteres")]
    public string? Rg { get; set; }

    // Informações aninhadas (opcionais)
    public AddressDto? Address { get; set; }
    public BaptismInfoDto? BaptismInfo { get; set; }
    public List<ContactDto>? ContactInfo { get; set; }
    public MedicalInfoDto? MedicalInfo { get; set; }
    public LoginInfoDto? LoginInfo { get; set; }
}

/// <summary>
/// DTO para informações de endereço
/// </summary>
public class AddressDto
{
    [Required(ErrorMessage = "CEP é obrigatório")]
    [StringLength(9, ErrorMessage = "CEP deve ter 8 dígitos")]
    public string PostalCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Logradouro é obrigatório")]
    [StringLength(200, ErrorMessage = "Logradouro não pode exceder 200 caracteres")]
    public string Street { get; set; } = string.Empty;

    [Required(ErrorMessage = "Número é obrigatório")]
    [StringLength(20, ErrorMessage = "Número não pode exceder 20 caracteres")]
    public string Number { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Complemento não pode exceder 100 caracteres")]
    public string? Complement { get; set; }

    [Required(ErrorMessage = "Bairro é obrigatório")]
    [StringLength(100, ErrorMessage = "Bairro não pode exceder 100 caracteres")]
    public string Neighborhood { get; set; } = string.Empty;

    [Required(ErrorMessage = "Cidade é obrigatória")]
    [StringLength(100, ErrorMessage = "Cidade não pode exceder 100 caracteres")]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "Estado é obrigatório")]
    [StringLength(2, ErrorMessage = "Estado deve ter 2 caracteres")]
    public string State { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "País não pode exceder 100 caracteres")]
    public string? Country { get; set; } = "Brasil";

    public bool IsPrimary { get; set; } = true;
}

/// <summary>
/// DTO para informações de batismo
/// </summary>
public class BaptismInfoDto
{
    [Required(ErrorMessage = "Data do batismo é obrigatória")]
    public DateTime BaptismDate { get; set; }

    [Required(ErrorMessage = "Igreja do batismo é obrigatória")]
    [StringLength(200, ErrorMessage = "Igreja do batismo não pode exceder 200 caracteres")]
    public string BaptismChurch { get; set; } = string.Empty;

    [Required(ErrorMessage = "Pastor do batismo é obrigatório")]
    [StringLength(200, ErrorMessage = "Pastor do batismo não pode exceder 200 caracteres")]
    public string BaptismPastor { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Observações do batismo não podem exceder 500 caracteres")]
    public string? Observations { get; set; }
}

/// <summary>
/// DTO para informações de contato
/// </summary>
public class ContactDto
{
    [Required(ErrorMessage = "Tipo de contato é obrigatório")]
    public ContactType Type { get; set; }

    [Required(ErrorMessage = "Valor do contato é obrigatório")]
    [StringLength(255, ErrorMessage = "Valor do contato não pode exceder 255 caracteres")]
    public string Value { get; set; } = string.Empty;

    public bool IsPrimary { get; set; } = false;

    [StringLength(500, ErrorMessage = "Descrição do contato não pode exceder 500 caracteres")]
    public string? Description { get; set; }
}

/// <summary>
/// DTO para informações médicas
/// </summary>
public class MedicalInfoDto
{
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

    // Tags médicas
    public List<MedicalTagDto>? MedicalTags { get; set; }
}

/// <summary>
/// DTO para tags médicas
/// </summary>
public class MedicalTagDto
{
    [Required(ErrorMessage = "Nome da tag é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome da tag não pode exceder 100 caracteres")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Descrição da tag não pode exceder 500 caracteres")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Categoria da tag é obrigatória")]
    public MedicalTagCategory Category { get; set; }

    [StringLength(1000, ErrorMessage = "Descrição específica não pode exceder 1000 caracteres")]
    public string? SpecificDescription { get; set; }

    public MedicalSeverity? Severity { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// DTO para informações de login
/// </summary>
public class LoginInfoDto
{
    [Required(ErrorMessage = "Email é obrigatório para login")]
    [EmailAddress(ErrorMessage = "Email deve ter formato válido")]
    [StringLength(255, ErrorMessage = "Email não pode exceder 255 caracteres")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha é obrigatória")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Senha deve ter entre 6 e 100 caracteres")]
    public string Password { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Observações de login não podem exceder 500 caracteres")]
    public string? Observations { get; set; }
}
