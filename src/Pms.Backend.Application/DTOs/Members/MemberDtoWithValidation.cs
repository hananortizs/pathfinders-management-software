using Pms.Backend.Domain.Entities;
using Pms.Backend.Application.DTOs.Common;
using Pms.Backend.Application.Validators;
 using Pms.Backend.Domain.Helpers;

namespace Pms.Backend.Application.DTOs.Members;

/// <summary>
/// Data Transfer Object for Member entity with validation attributes
/// </summary>
public class MemberDtoWithValidation : AddressableEntityDtoBase
{
    /// <summary>
    /// Entity type for address relationships
    /// </summary>
    public override string EntityType => "Member";

    /// <summary>
    /// Member's first name
    /// </summary>
    [ValidName(ErrorMessage = "Nome deve conter apenas letras, espaços, hífens e apostrofes")]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Member's middle name (optional)
    /// </summary>
    [ValidName(ErrorMessage = "Nome do meio deve conter apenas letras, espaços, hífens e apostrofes")]
    public string? MiddleName { get; set; }

    /// <summary>
    /// Member's last name
    /// </summary>
    [ValidName(ErrorMessage = "Sobrenome deve conter apenas letras, espaços, hífens e apostrofes")]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Member's full name (computed property)
    /// </summary>
    public string FullName => NameHelper.CombineFullName(FirstName, MiddleName, LastName);

    /// <summary>
    /// Member's email address
    /// </summary>
    [ValidEmail(ErrorMessage = "Email deve estar em um formato válido")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Member's phone number
    /// </summary>
    [ValidPhone(ErrorMessage = "Telefone deve estar no formato (11) 99999-9999")]
    public string? Phone { get; set; }

    /// <summary>
    /// Member's date of birth
    /// </summary>
    [ValidDateOfBirth(ErrorMessage = "Membro deve ter pelo menos 10 anos completos até 1º de junho do ano corrente")]
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Member's gender
    /// </summary>
    public MemberGender Gender { get; set; }

    /// <summary>
    /// Member's CPF (Brazilian tax ID)
    /// </summary>
    [ValidCpf(ErrorMessage = "CPF deve estar no formato 123.456.789-10")]
    public string? Cpf { get; set; }

    /// <summary>
    /// Member's RG (Brazilian ID)
    /// </summary>
    [ValidRg(ErrorMessage = "RG deve estar no formato 12.345.678-9")]
    public string? Rg { get; set; }

    /// <summary>
    /// Member's emergency contact name
    /// </summary>
    public string? EmergencyContactName { get; set; }

    /// <summary>
    /// Member's emergency contact phone
    /// </summary>
    [ValidPhone(ErrorMessage = "Telefone de contato de emergência deve estar no formato (11) 99999-9999")]
    public string? EmergencyContactPhone { get; set; }

    /// <summary>
    /// Member's emergency contact relationship
    /// </summary>
    public string? EmergencyContactRelationship { get; set; }

    /// <summary>
    /// Member's medical information
    /// </summary>
    public string? MedicalInfo { get; set; }

    /// <summary>
    /// Member's allergies
    /// </summary>
    public string? Allergies { get; set; }

    /// <summary>
    /// Member's medications
    /// </summary>
    public string? Medications { get; set; }

    /// <summary>
    /// Member's baptism date
    /// </summary>
    public DateTime? BaptismDate { get; set; }

    /// <summary>
    /// Member's baptism place
    /// </summary>
    public string? BaptismPlace { get; set; }

    /// <summary>
    /// Member's baptism status
    /// </summary>
    public bool Baptized { get; set; }

    /// <summary>
    /// Member's scarf investiture date
    /// </summary>
    public DateTime? ScarfInvestedAt { get; set; }

    /// <summary>
    /// Member's scarf investiture status
    /// </summary>
    public bool ScarfInvested { get; set; }

    /// <summary>
    /// Member's active status
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Member's deactivation date
    /// </summary>
    public DateTime? DeactivatedAt { get; set; }

    /// <summary>
    /// Member's deactivation reason
    /// </summary>
    public string? DeactivationReason { get; set; }

    // Formatted properties for display
    /// <summary>
    /// Email formatted for display
    /// </summary>
    public string? EmailFormatted { get; set; }

    /// <summary>
    /// Phone formatted for display
    /// </summary>
    public string? PhoneFormatted { get; set; }

    /// <summary>
    /// Emergency contact phone formatted for display
    /// </summary>
    public string? EmergencyContactPhoneFormatted { get; set; }

    /// <summary>
    /// CPF formatted for display
    /// </summary>
    public string? CpfFormatted { get; set; }

    /// <summary>
    /// RG formatted for display
    /// </summary>
    public string? RgFormatted { get; set; }

    /// <summary>
    /// First name formatted for display
    /// </summary>
    public string? FirstNameFormatted { get; set; }

    /// <summary>
    /// Middle name formatted for display
    /// </summary>
    public string? MiddleNameFormatted { get; set; }

    /// <summary>
    /// Last name formatted for display
    /// </summary>
    public string? LastNameFormatted { get; set; }

    /// <summary>
    /// Full name formatted for display
    /// </summary>
    public string? FullNameFormatted { get; set; }
}

