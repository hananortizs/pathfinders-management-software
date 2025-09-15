using Pms.Backend.Domain.Enums;

namespace Pms.Backend.Application.DTOs.Contacts;

/// <summary>
/// DTO para exibição de contato
/// </summary>
public class ContactDto
{
    /// <summary>
    /// ID do contato
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Tipo do contato
    /// </summary>
    public ContactType Type { get; set; }

    /// <summary>
    /// Categoria do contato
    /// </summary>
    public ContactCategory Category { get; set; }

    /// <summary>
    /// Valor do contato
    /// </summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Rótulo do contato
    /// </summary>
    public string? Label { get; set; }

    /// <summary>
    /// Indica se é o contato principal
    /// </summary>
    public bool IsPrimary { get; set; }

    /// <summary>
    /// Indica se está ativo
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Notas adicionais
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// ID da entidade
    /// </summary>
    public Guid EntityId { get; set; }

    /// <summary>
    /// Tipo da entidade
    /// </summary>
    public string EntityType { get; set; } = string.Empty;

    /// <summary>
    /// Prioridade do contato
    /// </summary>
    public int Priority { get; set; }

    /// <summary>
    /// Data da última verificação
    /// </summary>
    public DateTime? LastVerifiedAt { get; set; }

    /// <summary>
    /// Indica se foi verificado
    /// </summary>
    public bool IsVerified { get; set; }

    /// <summary>
    /// Valor formatado para exibição
    /// </summary>
    public string FormattedValue { get; set; } = string.Empty;

    /// <summary>
    /// Nome de exibição do tipo
    /// </summary>
    public string TypeDisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Nome de exibição da categoria
    /// </summary>
    public string CategoryDisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Nome completo do contato
    /// </summary>
    public string FullDisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Indica se é rede social
    /// </summary>
    public bool IsSocialMedia { get; set; }

    /// <summary>
    /// Indica se é telefônico
    /// </summary>
    public bool IsPhone { get; set; }

    /// <summary>
    /// Indica se é digital
    /// </summary>
    public bool IsDigital { get; set; }

    /// <summary>
    /// Indica se é de emergência
    /// </summary>
    public bool IsEmergency { get; set; }

    /// <summary>
    /// Indica se é pessoal
    /// </summary>
    public bool IsPersonal { get; set; }

    /// <summary>
    /// Indica se é da igreja
    /// </summary>
    public bool IsChurch { get; set; }

    /// <summary>
    /// Indica se é de responsável legal
    /// </summary>
    public bool IsLegalGuardian { get; set; }

    /// <summary>
    /// Data de criação
    /// </summary>
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Data de atualização
    /// </summary>
    public DateTime UpdatedAtUtc { get; set; }
}
