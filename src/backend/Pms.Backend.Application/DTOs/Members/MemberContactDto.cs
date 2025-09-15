using System.ComponentModel.DataAnnotations;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Domain.Enums;

namespace Pms.Backend.Application.DTOs.Members;

/// <summary>
/// Data Transfer Object for Member contact information
/// </summary>
public class MemberContactDto
{
    /// <summary>
    /// Contact type (Email, Phone, etc.)
    /// </summary>
    [Required(ErrorMessage = "Tipo de contato é obrigatório")]
    public ContactType Type { get; set; }

    /// <summary>
    /// Contact value (email address, phone number, etc.)
    /// </summary>
    [Required(ErrorMessage = "Valor do contato é obrigatório")]
    [StringLength(255, MinimumLength = 3, ErrorMessage = "Valor do contato deve ter entre 3 e 255 caracteres")]
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Indicates if this is the primary contact of this type
    /// </summary>
    public bool IsPrimary { get; set; } = false;

    /// <summary>
    /// Additional notes about this contact
    /// </summary>
    [StringLength(500, ErrorMessage = "Observações não podem exceder 500 caracteres")]
    public string? Notes { get; set; }
}

/// <summary>
/// Data Transfer Object for creating a Member with contact information
/// </summary>
public class CreateMemberWithContactsDto
{
    /// <summary>
    /// Member's first name
    /// </summary>
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 e 100 caracteres")]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Member's middle names (optional) - can contain multiple middle names
    /// </summary>
    [StringLength(200, ErrorMessage = "Sobrenomes do meio não podem exceder 200 caracteres")]
    public string? MiddleNames { get; set; }

    /// <summary>
    /// Member's last name
    /// </summary>
    [Required(ErrorMessage = "Sobrenome é obrigatório")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Sobrenome deve ter entre 2 e 100 caracteres")]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Member's social name (preferred name for identification)
    /// </summary>
    [StringLength(150, ErrorMessage = "Nome social não pode exceder 150 caracteres")]
    public string? SocialName { get; set; }

    /// <summary>
    /// Member's date of birth
    /// </summary>
    [Required(ErrorMessage = "Data de nascimento é obrigatória")]
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Member's gender
    /// </summary>
    [Required(ErrorMessage = "Gênero é obrigatório")]
    public MemberGender Gender { get; set; }

    /// <summary>
    /// Member's CPF (Brazilian tax ID)
    /// </summary>
    [StringLength(14, ErrorMessage = "CPF deve ter no máximo 14 caracteres")]
    public string? Cpf { get; set; }

    /// <summary>
    /// Member's RG (Brazilian ID)
    /// </summary>
    [StringLength(20, ErrorMessage = "RG deve ter no máximo 20 caracteres")]
    public string? Rg { get; set; }

    /// <summary>
    /// Member's emergency contact name
    /// </summary>
    [StringLength(150, ErrorMessage = "Nome do contato de emergência não pode exceder 150 caracteres")]
    public string? EmergencyContactName { get; set; }

    /// <summary>
    /// Member's emergency contact phone
    /// </summary>
    [StringLength(20, ErrorMessage = "Telefone do contato de emergência não pode exceder 20 caracteres")]
    public string? EmergencyContactPhone { get; set; }

    /// <summary>
    /// Member's emergency contact relationship
    /// </summary>
    [StringLength(50, ErrorMessage = "Parentesco do contato de emergência não pode exceder 50 caracteres")]
    public string? EmergencyContactRelationship { get; set; }

    /// <summary>
    /// Member's medical information
    /// </summary>
    [StringLength(1000, ErrorMessage = "Informações médicas não podem exceder 1000 caracteres")]
    public string? MedicalInfo { get; set; }

    /// <summary>
    /// Member's allergies
    /// </summary>
    [StringLength(500, ErrorMessage = "Alergias não podem exceder 500 caracteres")]
    public string? Allergies { get; set; }

    /// <summary>
    /// Member's medications
    /// </summary>
    [StringLength(500, ErrorMessage = "Medicamentos não podem exceder 500 caracteres")]
    public string? Medications { get; set; }

    /// <summary>
    /// Member's baptism date
    /// </summary>
    public DateTime? BaptismDate { get; set; }

    /// <summary>
    /// Member's baptism church
    /// </summary>
    [StringLength(200, ErrorMessage = "Igreja de batismo não pode exceder 200 caracteres")]
    public string? BaptismChurch { get; set; }

    /// <summary>
    /// Member's baptism pastor
    /// </summary>
    [StringLength(150, ErrorMessage = "Pastor de batismo não pode exceder 150 caracteres")]
    public string? BaptismPastor { get; set; }

    /// <summary>
    /// Member's scarf date
    /// </summary>
    public DateTime? ScarfDate { get; set; }

    /// <summary>
    /// Member's scarf church
    /// </summary>
    [StringLength(200, ErrorMessage = "Igreja do lenço não pode exceder 200 caracteres")]
    public string? ScarfChurch { get; set; }

    /// <summary>
    /// Member's scarf pastor
    /// </summary>
    [StringLength(150, ErrorMessage = "Pastor do lenço não pode exceder 150 caracteres")]
    public string? ScarfPastor { get; set; }

    /// <summary>
    /// Member's contact information
    /// </summary>
    [Required(ErrorMessage = "Pelo menos um contato é obrigatório")]
    [MinLength(1, ErrorMessage = "Pelo menos um contato é obrigatório")]
    public List<MemberContactDto> Contacts { get; set; } = new();
}
