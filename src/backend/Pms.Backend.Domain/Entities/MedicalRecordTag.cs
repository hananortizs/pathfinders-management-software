using System.ComponentModel.DataAnnotations;

namespace Pms.Backend.Domain.Entities;

/// <summary>
/// Representa o relacionamento entre um registro médico e uma tag médica.
/// Permite associar múltiplas tags a um registro médico específico.
/// </summary>
public class MedicalRecordTag : BaseEntity
{
    /// <summary>
    /// Identificador único do registro médico associado.
    /// </summary>
    [Required]
    public Guid MedicalRecordId { get; set; }

    /// <summary>
    /// Navegação para o registro médico associado.
    /// </summary>
    public virtual MedicalRecord MedicalRecord { get; set; } = null!;

    /// <summary>
    /// ID da tag médica
    /// </summary>
    [Required]
    public Guid MedicalTagId { get; set; }

    /// <summary>
    /// Navegação para a tag médica
    /// </summary>
    public virtual MedicalTag MedicalTag { get; set; } = null!;

    /// <summary>
    /// Descrição específica da tag para este registro
    /// </summary>
    [StringLength(1000, ErrorMessage = "Descrição específica não pode exceder 1000 caracteres")]
    public string? SpecificDescription { get; set; }

    /// <summary>
    /// Severidade da condição (para alergias, por exemplo)
    /// </summary>
    public MedicalSeverity? Severity { get; set; }

    /// <summary>
    /// Data de início da condição
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Data de fim da condição (se aplicável)
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Se a condição está ativa
    /// </summary>
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// Níveis de severidade médica
/// </summary>
public enum MedicalSeverity
{
    /// <summary>
    /// Severidade leve
    /// </summary>
    Mild = 1,
    
    /// <summary>
    /// Severidade moderada
    /// </summary>
    Moderate = 2,
    
    /// <summary>
    /// Severidade grave
    /// </summary>
    Severe = 3,
    
    /// <summary>
    /// Severidade crítica
    /// </summary>
    Critical = 4
}
