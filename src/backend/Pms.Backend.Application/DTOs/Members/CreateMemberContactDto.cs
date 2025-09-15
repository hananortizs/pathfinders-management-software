using System.ComponentModel.DataAnnotations;
using Pms.Backend.Domain.Enums;

namespace Pms.Backend.Application.DTOs.Members;

/// <summary>
/// DTO para informações de contato
/// </summary>
public class CreateMemberContactDto
{
    /// <summary>
    /// Tipo do contato (Email, Telefone, etc.)
    /// </summary>
    [Required(ErrorMessage = "Tipo de contato é obrigatório")]
    public ContactType Type { get; set; }

    /// <summary>
    /// Valor do contato
    /// </summary>
    [Required(ErrorMessage = "Valor do contato é obrigatório")]
    [StringLength(255, ErrorMessage = "Valor do contato não pode exceder 255 caracteres")]
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Indica se é o contato principal
    /// </summary>
    public bool IsPrimary { get; set; } = false;

    /// <summary>
    /// Descrição do contato
    /// </summary>
    [StringLength(500, ErrorMessage = "Descrição do contato não pode exceder 500 caracteres")]
    public string? Description { get; set; }
}
