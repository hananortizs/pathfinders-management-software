using System.ComponentModel.DataAnnotations;

namespace Pms.Backend.Application.DTOs.Timeline;

/// <summary>
/// DTO para entrada da timeline
/// </summary>
public class TimelineEntryDto
{
    /// <summary>
    /// ID da entrada
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// ID do membro relacionado
    /// </summary>
    [Required]
    public Guid MemberId { get; set; }

    /// <summary>
    /// Nome do membro
    /// </summary>
    public string? MemberName { get; set; }

    /// <summary>
    /// Tipo da entrada
    /// </summary>
    [Required]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Título da entrada
    /// </summary>
    [Required]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Descrição da entrada
    /// </summary>
    [Required]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Dados adicionais (JSON)
    /// </summary>
    public string? Data { get; set; }

    /// <summary>
    /// Data do evento (UTC)
    /// </summary>
    [Required]
    public DateTime EventDateUtc { get; set; }

    /// <summary>
    /// Data do evento (BRT)
    /// </summary>
    public DateTime EventDateBrt { get; set; }

    /// <summary>
    /// ID do membership relacionado
    /// </summary>
    public Guid? MembershipId { get; set; }

    /// <summary>
    /// Nome do clube do membership
    /// </summary>
    public string? ClubName { get; set; }

    /// <summary>
    /// ID da atribuição relacionada
    /// </summary>
    public Guid? AssignmentId { get; set; }

    /// <summary>
    /// Nome do cargo da atribuição
    /// </summary>
    public string? RoleName { get; set; }

    /// <summary>
    /// ID do evento relacionado
    /// </summary>
    public Guid? EventId { get; set; }

    /// <summary>
    /// Nome do evento
    /// </summary>
    public string? EventName { get; set; }

    /// <summary>
    /// Indica se é entrada manual
    /// </summary>
    public bool IsManual { get; set; }

    /// <summary>
    /// Data de criação
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Data da última atualização
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// DTO para busca de timeline
/// </summary>
public class SearchTimelineDto
{
    /// <summary>
    /// Termo de busca
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Filtro por tipo de entrada
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// Filtro por data de início (a partir de)
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Filtro por data de fim (até)
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Filtro por clube
    /// </summary>
    public Guid? ClubId { get; set; }

    /// <summary>
    /// Filtro por cargo
    /// </summary>
    public string? RoleName { get; set; }

    /// <summary>
    /// Filtro por evento
    /// </summary>
    public Guid? EventId { get; set; }

    /// <summary>
    /// Filtro por entrada manual
    /// </summary>
    public bool? IsManual { get; set; }

    /// <summary>
    /// Página atual
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Tamanho da página
    /// </summary>
    public int PageSize { get; set; } = 20;

    /// <summary>
    /// Campo para ordenação
    /// </summary>
    public string? SortBy { get; set; }

    /// <summary>
    /// Direção da ordenação (asc/desc)
    /// </summary>
    public string? SortDirection { get; set; } = "desc";
}

/// <summary>
/// DTO para resultado de busca de timeline
/// </summary>
public class SearchTimelineResultDto
{
    /// <summary>
    /// Lista de entradas da timeline
    /// </summary>
    public List<TimelineEntryDto> Entries { get; set; } = new List<TimelineEntryDto>();

    /// <summary>
    /// Total de registros
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Página atual
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Tamanho da página
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total de páginas
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Filtros aplicados
    /// </summary>
    public Dictionary<string, object> AppliedFilters { get; set; } = new Dictionary<string, object>();
}

/// <summary>
/// DTO para criação de entrada manual
/// </summary>
public class CreateTimelineEntryDto
{
    /// <summary>
    /// ID do membro relacionado
    /// </summary>
    [Required(ErrorMessage = "ID do membro é obrigatório")]
    public Guid MemberId { get; set; }

    /// <summary>
    /// Tipo da entrada
    /// </summary>
    [Required(ErrorMessage = "Tipo é obrigatório")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Título da entrada
    /// </summary>
    [Required(ErrorMessage = "Título é obrigatório")]
    [StringLength(200, ErrorMessage = "Título deve ter no máximo 200 caracteres")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Descrição da entrada
    /// </summary>
    [Required(ErrorMessage = "Descrição é obrigatória")]
    [StringLength(1000, ErrorMessage = "Descrição deve ter no máximo 1000 caracteres")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Dados adicionais (JSON)
    /// </summary>
    [StringLength(2000, ErrorMessage = "Dados devem ter no máximo 2000 caracteres")]
    public string? Data { get; set; }

    /// <summary>
    /// Data do evento (UTC)
    /// </summary>
    [Required(ErrorMessage = "Data do evento é obrigatória")]
    public DateTime EventDateUtc { get; set; }

    /// <summary>
    /// ID do membership relacionado (opcional)
    /// </summary>
    public Guid? MembershipId { get; set; }

    /// <summary>
    /// ID da atribuição relacionada (opcional)
    /// </summary>
    public Guid? AssignmentId { get; set; }

    /// <summary>
    /// ID do evento relacionado (opcional)
    /// </summary>
    public Guid? EventId { get; set; }
}

/// <summary>
/// DTO para estatísticas da timeline
/// </summary>
public class TimelineStatsDto
{
    /// <summary>
    /// Total de entradas
    /// </summary>
    public int TotalEntries { get; set; }

    /// <summary>
    /// Entradas por tipo
    /// </summary>
    public Dictionary<string, int> EntriesByType { get; set; } = new Dictionary<string, int>();

    /// <summary>
    /// Entradas por período
    /// </summary>
    public Dictionary<string, int> EntriesByPeriod { get; set; } = new Dictionary<string, int>();

    /// <summary>
    /// Entradas manuais
    /// </summary>
    public int ManualEntries { get; set; }

    /// <summary>
    /// Entradas automáticas
    /// </summary>
    public int AutomaticEntries { get; set; }

    /// <summary>
    /// Período mais ativo
    /// </summary>
    public string? MostActivePeriod { get; set; }

    /// <summary>
    /// Tipo mais comum
    /// </summary>
    public string? MostCommonType { get; set; }

    /// <summary>
    /// Data da última entrada
    /// </summary>
    public DateTime? LastEntryDate { get; set; }

    /// <summary>
    /// Data da primeira entrada
    /// </summary>
    public DateTime? FirstEntryDate { get; set; }
}
