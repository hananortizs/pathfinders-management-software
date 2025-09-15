using System.ComponentModel.DataAnnotations;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Domain.Enums;

namespace Pms.Backend.Application.DTOs.Members;

/// <summary>
/// DTO para cadastro completo de membros com informações aninhadas
/// </summary>
public class CreateMemberCompleteDto
{
    /// <summary>
    /// Nome do membro
    /// </summary>
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome não pode exceder 100 caracteres")]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Sobrenome do membro
    /// </summary>
    [Required(ErrorMessage = "Sobrenome é obrigatório")]
    [StringLength(100, ErrorMessage = "Sobrenome não pode exceder 100 caracteres")]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Data de nascimento do membro
    /// </summary>
    [Required(ErrorMessage = "Data de nascimento é obrigatória")]
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Gênero do membro
    /// </summary>
    [Required(ErrorMessage = "Gênero é obrigatório")]
    public MemberGender Gender { get; set; }

    /// <summary>
    /// Nomes do meio do membro
    /// </summary>
    [StringLength(200, ErrorMessage = "Nomes do meio não podem exceder 200 caracteres")]
    public string? MiddleNames { get; set; }

    /// <summary>
    /// Nome social do membro
    /// </summary>
    [StringLength(100, ErrorMessage = "Nome social não pode exceder 100 caracteres")]
    public string? SocialName { get; set; }

    /// <summary>
    /// CPF do membro
    /// </summary>
    [Required(ErrorMessage = "CPF é obrigatório")]
    [StringLength(14, ErrorMessage = "CPF deve ter 11 dígitos")]
    public string Cpf { get; set; } = string.Empty;

    /// <summary>
    /// RG do membro
    /// </summary>
    [Required(ErrorMessage = "RG é obrigatório")]
    [StringLength(20, ErrorMessage = "RG não pode exceder 20 caracteres")]
    public string Rg { get; set; } = string.Empty;

    /// <summary>
    /// Informações de endereço do membro (obrigatório)
    /// </summary>
    [Required(ErrorMessage = "Informações de endereço são obrigatórias")]
    public CreateMemberAddressDto AddressInfo { get; set; } = new();

    /// <summary>
    /// Informações de login do membro (obrigatório)
    /// </summary>
    [Required(ErrorMessage = "Informações de login são obrigatórias")]
    public CreateMemberLoginInfoDto LoginInfo { get; set; } = new();

    /// <summary>
    /// Informações de contato do membro (obrigatório - pelo menos 1 Mobile)
    /// </summary>
    [Required(ErrorMessage = "Informações de contato são obrigatórias")]
    [MinLength(1, ErrorMessage = "Pelo menos um contato é obrigatório")]
    public List<CreateMemberContactDto> Contacts { get; set; } = new();

    /// <summary>
    /// Informações de batismo do membro (opcional)
    /// </summary>
    public CreateMemberBaptismInfoDto? BaptismInfo { get; set; }

    /// <summary>
    /// Informações médicas do membro (opcional)
    /// </summary>
    public CreateMemberMedicalInfoDto? MedicalInfo { get; set; }

    /// <summary>
    /// Investidura inicial do lenço (opcional)
    /// </summary>
    public InvestitureDto? InitialScarfInvestiture { get; set; }
}

