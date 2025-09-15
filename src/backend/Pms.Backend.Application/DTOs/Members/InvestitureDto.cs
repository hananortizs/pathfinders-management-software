using System.ComponentModel.DataAnnotations;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Application.DTOs.Members;

/// <summary>
/// DTO para investidura inicial do membro
/// </summary>
public class InvestitureDto
{
    /// <summary>
    /// Tipo de investidura
    /// </summary>
    [Required(ErrorMessage = "Tipo de investidura é obrigatório")]
    public InvestitureType Type { get; set; }

    /// <summary>
    /// Data da investidura
    /// </summary>
    [Required(ErrorMessage = "Data da investidura é obrigatória")]
    public DateTime Date { get; set; }

    /// <summary>
    /// Local da investidura
    /// </summary>
    [Required(ErrorMessage = "Local da investidura é obrigatório")]
    [StringLength(200, ErrorMessage = "Local não pode exceder 200 caracteres")]
    public string Place { get; set; } = string.Empty;

    /// <summary>
    /// URL do YouTube da cerimônia (opcional)
    /// </summary>
    [StringLength(500, ErrorMessage = "URL do YouTube não pode exceder 500 caracteres")]
    public string? YoutubeUrl { get; set; }

    /// <summary>
    /// Testemunhas da investidura
    /// </summary>
    [Required(ErrorMessage = "Pelo menos uma testemunha é obrigatória")]
    [MinLength(1, ErrorMessage = "Pelo menos uma testemunha é obrigatória")]
    public List<InvestitureWitnessDto> Witnesses { get; set; } = new();
}

/// <summary>
/// DTO para testemunha de investidura
/// </summary>
public class InvestitureWitnessDto
{
    /// <summary>
    /// Tipo de testemunha (Structured ou Text)
    /// </summary>
    [Required(ErrorMessage = "Tipo de testemunha é obrigatório")]
    public InvestitureWitnessType Type { get; set; }

    /// <summary>
    /// ID do membro (quando Type = Structured)
    /// </summary>
    public Guid? MemberId { get; set; }

    /// <summary>
    /// Nome da testemunha (quando Type = Text)
    /// </summary>
    [StringLength(200, ErrorMessage = "Nome não pode exceder 200 caracteres")]
    public string? NameText { get; set; }

    /// <summary>
    /// Cargo da testemunha (quando Type = Text)
    /// </summary>
    [StringLength(100, ErrorMessage = "Cargo não pode exceder 100 caracteres")]
    public string? RoleText { get; set; }

    /// <summary>
    /// Organização da testemunha (quando Type = Text)
    /// </summary>
    [StringLength(120, ErrorMessage = "Organização não pode exceder 120 caracteres")]
    public string? OrgText { get; set; }
}
