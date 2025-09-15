using System.ComponentModel.DataAnnotations;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Application.DTOs.Members;

/// <summary>
/// DTO para tags médicas
/// </summary>
public class CreateMemberMedicalTagDto
{
    /// <summary>
    /// Nome da tag médica
    /// </summary>
    [Required(ErrorMessage = "Nome da tag é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome da tag não pode exceder 100 caracteres")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Descrição da tag médica
    /// </summary>
    [StringLength(500, ErrorMessage = "Descrição da tag não pode exceder 500 caracteres")]
    public string? Description { get; set; }

    /// <summary>
    /// Categoria da tag médica
    /// </summary>
    [Required(ErrorMessage = "Categoria da tag é obrigatória")]
    public MedicalTagCategory Category { get; set; }

    /// <summary>
    /// Descrição específica da tag
    /// </summary>
    [StringLength(1000, ErrorMessage = "Descrição específica não pode exceder 1000 caracteres")]
    public string? SpecificDescription { get; set; }

    /// <summary>
    /// Severidade da condição médica
    /// </summary>
    public MedicalSeverity? Severity { get; set; }

    /// <summary>
    /// Data de início da condição
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Data de fim da condição
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Indica se a tag está ativa
    /// </summary>
    public bool IsActive { get; set; } = true;
}
