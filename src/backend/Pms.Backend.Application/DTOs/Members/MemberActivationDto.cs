namespace Pms.Backend.Application.DTOs.Members;

/// <summary>
/// DTO para validação de ativação de membros.
/// Contém informações sobre se um membro pode ser ativado e os itens de validação.
/// </summary>
public class MemberActivationValidationDto
{
    /// <summary>
    /// Identificador único do membro.
    /// </summary>
    public string MemberId { get; set; } = string.Empty;
    
    /// <summary>
    /// Nome do membro.
    /// </summary>
    public string MemberName { get; set; } = string.Empty;
    
    /// <summary>
    /// Indica se o membro pode ser ativado.
    /// </summary>
    public bool CanBeActivated { get; set; }
    
    /// <summary>
    /// Data da validação.
    /// </summary>
    public DateTime ValidationDate { get; set; }

    /// <summary>
    /// Validação do endereço principal.
    /// </summary>
    public ValidationItemDto AddressValidation { get; set; } = new();
    
    /// <summary>
    /// Validação da ficha médica.
    /// </summary>
    public ValidationItemDto MedicalValidation { get; set; } = new();
    
    /// <summary>
    /// Validação dos contatos.
    /// </summary>
    public ValidationItemDto ContactValidation { get; set; } = new();
    
    /// <summary>
    /// Validação do batismo (para maiores de 16 anos).
    /// </summary>
    public ValidationItemDto BaptismValidation { get; set; } = new();
}

/// <summary>
/// DTO para itens de validação individual.
/// Representa o resultado da validação de um requisito específico.
/// </summary>
public class ValidationItemDto
{
    /// <summary>
    /// Indica se o item de validação está válido.
    /// </summary>
    public bool IsValid { get; set; }
    
    /// <summary>
    /// Mensagem descritiva do resultado da validação.
    /// </summary>
    public string Message { get; set; } = string.Empty;
}
