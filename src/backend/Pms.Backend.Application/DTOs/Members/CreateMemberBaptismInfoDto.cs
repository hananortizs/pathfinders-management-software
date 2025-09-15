using System.ComponentModel.DataAnnotations;

namespace Pms.Backend.Application.DTOs.Members;

/// <summary>
/// DTO para informações de batismo
/// </summary>
public class CreateMemberBaptismInfoDto
{
    /// <summary>
    /// Data do batismo
    /// </summary>
    [Required(ErrorMessage = "Data do batismo é obrigatória")]
    public DateTime BaptismDate { get; set; }

    /// <summary>
    /// Igreja onde foi realizado o batismo
    /// </summary>
    [Required(ErrorMessage = "Igreja do batismo é obrigatória")]
    [StringLength(200, ErrorMessage = "Igreja do batismo não pode exceder 200 caracteres")]
    public string BaptismChurch { get; set; } = string.Empty;

    /// <summary>
    /// Pastor que realizou o batismo
    /// </summary>
    [Required(ErrorMessage = "Pastor do batismo é obrigatório")]
    [StringLength(200, ErrorMessage = "Pastor do batismo não pode exceder 200 caracteres")]
    public string BaptismPastor { get; set; } = string.Empty;

    /// <summary>
    /// Observações sobre o batismo
    /// </summary>
    [StringLength(500, ErrorMessage = "Observações do batismo não podem exceder 500 caracteres")]
    public string? Observations { get; set; }
}
