using Pms.Backend.Application.DTOs.Auth;
using System.ComponentModel.DataAnnotations;

namespace Pms.Backend.Application.DTOs.Members;

/// <summary>
/// DTO para atualização de contatos do usuário autenticado
/// </summary>
public class UpdateMyContactsRequestDto
{
    /// <summary>
    /// Token de autenticação
    /// </summary>
    [Required(ErrorMessage = "Token é obrigatório")]
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Lista de contatos atualizados
    /// </summary>
    [Required(ErrorMessage = "A lista de contatos não pode ser nula.")]
    public IEnumerable<MemberContactDto> Contacts { get; set; } = new List<MemberContactDto>();
}
