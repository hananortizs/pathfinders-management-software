using System.ComponentModel.DataAnnotations;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Application.DTOs.Members;

/// <summary>
/// DTO para informações de endereço
/// </summary>
public class CreateMemberAddressDto
{
    /// <summary>
    /// CEP do endereço
    /// </summary>
    [Required(ErrorMessage = "CEP é obrigatório")]
    [StringLength(9, MinimumLength = 8, ErrorMessage = "CEP deve ter 8 ou 9 caracteres (com ou sem hífen)")]
    public string PostalCode { get; set; } = string.Empty;

    /// <summary>
    /// Logradouro do endereço
    /// </summary>
    [Required(ErrorMessage = "Logradouro é obrigatório")]
    [StringLength(200, ErrorMessage = "Logradouro não pode exceder 200 caracteres")]
    public string Street { get; set; } = string.Empty;

    /// <summary>
    /// Número do endereço
    /// </summary>
    [Required(ErrorMessage = "Número é obrigatório")]
    [StringLength(20, ErrorMessage = "Número não pode exceder 20 caracteres")]
    public string Number { get; set; } = string.Empty;

    /// <summary>
    /// Complemento do endereço
    /// </summary>
    [StringLength(100, ErrorMessage = "Complemento não pode exceder 100 caracteres")]
    public string? Complement { get; set; }

    /// <summary>
    /// Bairro do endereço
    /// </summary>
    [Required(ErrorMessage = "Bairro é obrigatório")]
    [StringLength(100, ErrorMessage = "Bairro não pode exceder 100 caracteres")]
    public string Neighborhood { get; set; } = string.Empty;

    /// <summary>
    /// Cidade do endereço
    /// </summary>
    [Required(ErrorMessage = "Cidade é obrigatória")]
    [StringLength(100, ErrorMessage = "Cidade não pode exceder 100 caracteres")]
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Estado do endereço
    /// </summary>
    [Required(ErrorMessage = "Estado é obrigatório")]
    [StringLength(2, ErrorMessage = "Estado deve ter 2 caracteres")]
    public string State { get; set; } = string.Empty;

    /// <summary>
    /// País do endereço
    /// </summary>
    [StringLength(100, ErrorMessage = "País não pode exceder 100 caracteres")]
    public string? Country { get; set; } = "Brasil";

    /// <summary>
    /// Indica se é o endereço principal
    /// </summary>
    public bool IsPrimary { get; set; } = true;
}
