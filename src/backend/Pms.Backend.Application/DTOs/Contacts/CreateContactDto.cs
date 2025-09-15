using System.ComponentModel.DataAnnotations;
using Pms.Backend.Application.Validators;
using Pms.Backend.Domain.Enums;

namespace Pms.Backend.Application.DTOs.Contacts;

/// <summary>
/// DTO para criação de contato
/// </summary>
public class CreateContactDto
{
    /// <summary>
    /// Tipo do contato
    /// </summary>
    [Required(ErrorMessage = "Tipo do contato é obrigatório")]
    public ContactType Type { get; set; }

    /// <summary>
    /// Categoria do contato
    /// </summary>
    [Required(ErrorMessage = "Categoria do contato é obrigatória")]
    public ContactCategory Category { get; set; }

    /// <summary>
    /// Valor do contato
    /// </summary>
    [Required(ErrorMessage = "Valor do contato é obrigatório")]
    [StringLength(500, ErrorMessage = "Valor do contato não pode exceder 500 caracteres")]
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Rótulo do contato
    /// </summary>
    [StringLength(100, ErrorMessage = "Rótulo não pode exceder 100 caracteres")]
    public string? Label { get; set; }

    /// <summary>
    /// Indica se é o contato principal
    /// </summary>
    public bool IsPrimary { get; set; } = false;

    /// <summary>
    /// Indica se está ativo
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Notas adicionais
    /// </summary>
    [StringLength(1000, ErrorMessage = "Notas não podem exceder 1000 caracteres")]
    public string? Notes { get; set; }

    /// <summary>
    /// ID da entidade
    /// </summary>
    [Required(ErrorMessage = "EntityId é obrigatório")]
    [ContactEntityExists(ErrorMessage = "A entidade especificada não existe")]
    public Guid EntityId { get; set; }

    /// <summary>
    /// Tipo da entidade
    /// </summary>
    [Required(ErrorMessage = "EntityType é obrigatório")]
    [ValidContactEntityType(ErrorMessage = "Tipo de entidade inválido")]
    public string EntityType { get; set; } = string.Empty;

    /// <summary>
    /// Prioridade do contato
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "Prioridade deve ser um valor não negativo")]
    public int Priority { get; set; } = 0;
}
