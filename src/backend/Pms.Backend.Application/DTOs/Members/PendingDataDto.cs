using System.ComponentModel.DataAnnotations;

namespace Pms.Backend.Application.DTOs.Members;

/// <summary>
/// DTO para dados pendentes do membro no login response
/// </summary>
public class PendingDataDto
{
    /// <summary>
    /// Lista de itens obrigatórios para ativação
    /// </summary>
    [Required]
    public List<string> ActivationRequired { get; set; } = new();

    /// <summary>
    /// Lista de itens obrigatórios para operações de negócio
    /// </summary>
    [Required]
    public List<string> OperationRequired { get; set; } = new();

    /// <summary>
    /// Lista de itens opcionais
    /// </summary>
    [Required]
    public List<string> Optional { get; set; } = new();

    /// <summary>
    /// Indica se escritas de domínio estão bloqueadas
    /// </summary>
    [Required]
    public bool BlockingWrites { get; set; }
}

/// <summary>
/// DTO para informações básicas do membro no login
/// </summary>
public class MemberBasicInfoDto
{
    /// <summary>
    /// ID do membro
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Status do membro
    /// </summary>
    [Required]
    public string Status { get; set; } = string.Empty;
}
