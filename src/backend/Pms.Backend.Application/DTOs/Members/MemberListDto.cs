using System.Text.Json.Serialization;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Application.DTOs.Members;

/// <summary>
/// DTO otimizado para listas de membros (sem campos repetitivos)
/// </summary>
public class MemberListDto
{
    /// <summary>
    /// ID único do membro
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nome do membro
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Nomes do meio do membro (opcional)
    /// </summary>
    public string? MiddleNames { get; set; }

    /// <summary>
    /// Sobrenome do membro
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Nome social do membro (nome preferido para identificação)
    /// </summary>
    public string? SocialName { get; set; }

    /// <summary>
    /// Nome completo do membro (propriedade calculada)
    /// </summary>
    public string FullName => $"{FirstName} {MiddleNames} {LastName}".Trim().Replace("  ", " ");


    /// <summary>
    /// Data de nascimento do membro
    /// </summary>
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Gênero do membro
    /// </summary>
    public MemberGender Gender { get; set; }

    /// <summary>
    /// CPF do membro
    /// </summary>
    public string? Cpf { get; set; }

    /// <summary>
    /// RG do membro
    /// </summary>
    public string? Rg { get; set; }

    /// <summary>
    /// Status do membro
    /// </summary>
    public MemberStatus Status { get; set; }

    /// <summary>
    /// Indica se o membro está ativo (Status == Active)
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Data de criação
    /// </summary>
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Data da última atualização
    /// </summary>
    public DateTime UpdatedAtUtc { get; set; }

    /// <summary>
    /// Data de ativação
    /// </summary>
    public DateTime? ActivatedAtUtc { get; set; }

    /// <summary>
    /// Data de desativação
    /// </summary>
    public DateTime? DeactivatedAtUtc { get; set; }

    /// <summary>
    /// Motivo da desativação
    /// </summary>
    public string? DeactivationReason { get; set; }

    /// <summary>
    /// E-mail principal do membro (calculado dos contatos)
    /// </summary>
    public string? PrimaryEmail { get; set; }

    /// <summary>
    /// Telefone principal do membro (calculado dos contatos)
    /// </summary>
    public string? PrimaryPhone { get; set; }
}

/// <summary>
/// DTO para metadados da lista de membros
/// </summary>
public class MemberListMetadata
{
    /// <summary>
    /// Tipo da entidade
    /// </summary>
    public string EntityType { get; set; } = "Member";

    /// <summary>
    /// Nome de exibição do tipo da entidade
    /// </summary>
    public string EntityTypeDisplayName { get; set; } = "Membro";

    /// <summary>
    /// Descrição do tipo da entidade
    /// </summary>
    public string EntityTypeDescription { get; set; } = "Membro do clube de desbravadores";
}

/// <summary>
/// DTO para resposta otimizada de lista de membros
/// </summary>
public class OptimizedMemberListResponse
{
    /// <summary>
    /// Lista de membros
    /// </summary>
    public IEnumerable<MemberListDto> Items { get; set; } = new List<MemberListDto>();

    /// <summary>
    /// Metadados da lista
    /// </summary>
    public MemberListMetadata Metadata { get; set; } = new();

    /// <summary>
    /// Número da página
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Tamanho da página
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total de itens
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Total de páginas
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Tem página anterior
    /// </summary>
    public bool HasPreviousPage { get; set; }

    /// <summary>
    /// Tem próxima página
    /// </summary>
    public bool HasNextPage { get; set; }
}
