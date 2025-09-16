using System.ComponentModel.DataAnnotations;

namespace Pms.Backend.Domain.Entities;

/// <summary>
/// Representa uma tag médica dinâmica para categorizar informações de saúde.
/// Permite categorizar alergias, medicamentos e condições médicas não listadas no sistema.
/// </summary>
public class MedicalTag : BaseEntity
{
    /// <summary>
    /// Nome da tag médica (ex: "Alergia a Frutos do Mar", "Intolerância à Lactose").
    /// </summary>
    [Required]
    [StringLength(100, ErrorMessage = "Nome da tag não pode exceder 100 caracteres")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Descrição detalhada da tag médica.
    /// </summary>
    [StringLength(500, ErrorMessage = "Descrição da tag não pode exceder 500 caracteres")]
    public string? Description { get; set; }

    /// <summary>
    /// Categoria da tag (Allergy, Medication, Condition, etc.)
    /// </summary>
    [Required]
    public MedicalTagCategory Category { get; set; }

    /// <summary>
    /// Se a tag é ativa (não deletada)
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Quantas vezes a tag foi usada
    /// </summary>
    public int UsageCount { get; set; } = 0;

    /// <summary>
    /// Tags associadas aos registros médicos
    /// </summary>
    public virtual ICollection<MedicalRecordTag> MedicalRecordTags { get; set; } = new List<MedicalRecordTag>();
}

/// <summary>
/// Categorias de tags médicas
/// </summary>
public enum MedicalTagCategory
{
    /// <summary>
    /// Categoria de alergia
    /// </summary>
    Allergy = 1,
    
    /// <summary>
    /// Categoria de medicamento
    /// </summary>
    Medication = 2,
    
    /// <summary>
    /// Categoria de condição médica
    /// </summary>
    Condition = 3,
    
    /// <summary>
    /// Categoria de deficiência
    /// </summary>
    Disability = 4,
    
    /// <summary>
    /// Outras categorias médicas
    /// </summary>
    Other = 5
}
