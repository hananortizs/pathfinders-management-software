using System.ComponentModel.DataAnnotations;

namespace Pms.Backend.Application.DTOs.Members;

/// <summary>
/// DTO para informações de login
/// </summary>
public class CreateMemberLoginInfoDto
{
    /// <summary>
    /// Email para login
    /// </summary>
    [Required(ErrorMessage = "Email é obrigatório para login")]
    [EmailAddress(ErrorMessage = "Email deve ter formato válido")]
    [StringLength(255, ErrorMessage = "Email não pode exceder 255 caracteres")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Senha para login
    /// </summary>
    [Required(ErrorMessage = "Senha é obrigatória")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Senha deve ter entre 6 e 100 caracteres")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Observações sobre o login
    /// </summary>
    [StringLength(500, ErrorMessage = "Observações de login não podem exceder 500 caracteres")]
    public string? Observations { get; set; }
}
