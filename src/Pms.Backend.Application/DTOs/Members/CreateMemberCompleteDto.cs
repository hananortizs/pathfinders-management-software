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
    [StringLength(14, ErrorMessage = "CPF deve ter 11 dígitos")]
    public string? Cpf { get; set; }

    /// <summary>
    /// RG do membro
    /// </summary>
    [StringLength(20, ErrorMessage = "RG não pode exceder 20 caracteres")]
    public string? Rg { get; set; }

    /// <summary>
    /// Informações de endereço do membro
    /// </summary>
    public CreateMemberAddressDto? Address { get; set; }

    /// <summary>
    /// Informações de batismo do membro
    /// </summary>
    public CreateMemberBaptismInfoDto? BaptismInfo { get; set; }

    /// <summary>
    /// Informações de contato do membro
    /// </summary>
    public List<CreateMemberContactDto>? ContactInfo { get; set; }

    /// <summary>
    /// Informações médicas do membro
    /// </summary>
    public CreateMemberMedicalInfoDto? MedicalInfo { get; set; }

    /// <summary>
    /// Informações de login do membro
    /// </summary>
    public CreateMemberLoginInfoDto? LoginInfo { get; set; }
}

