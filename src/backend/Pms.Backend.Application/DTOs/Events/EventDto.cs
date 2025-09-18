using System.ComponentModel.DataAnnotations;

namespace Pms.Backend.Application.DTOs.Events;

/// <summary>
/// DTO para exibição de evento
/// </summary>
public class EventDto
{
    /// <summary>
    /// ID do evento
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Nome do evento
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Descrição do evento
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Data de início do evento (UTC)
    /// </summary>
    [Required]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Data de fim do evento (UTC)
    /// </summary>
    [Required]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Local do evento
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// Nível do organizador
    /// </summary>
    [Required]
    public string OrganizerLevel { get; set; } = string.Empty;

    /// <summary>
    /// ID da entidade organizadora
    /// </summary>
    [Required]
    public Guid OrganizerId { get; set; }

    /// <summary>
    /// Nome da entidade organizadora
    /// </summary>
    public string? OrganizerName { get; set; }

    /// <summary>
    /// Valor da taxa (null = gratuito)
    /// </summary>
    public decimal? FeeAmount { get; set; }

    /// <summary>
    /// Moeda da taxa
    /// </summary>
    public string FeeCurrency { get; set; } = "BRL";

    /// <summary>
    /// Indica se o evento está ativo
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Idade mínima para participação
    /// </summary>
    public int? MinAge { get; set; }

    /// <summary>
    /// Idade máxima para participação
    /// </summary>
    public int? MaxAge { get; set; }

    /// <summary>
    /// Indica se informações médicas são obrigatórias
    /// </summary>
    public bool RequiresMedicalInfo { get; set; }

    /// <summary>
    /// Indica se investidura de lenço é obrigatória
    /// </summary>
    public bool RequiresScarfInvested { get; set; }

    /// <summary>
    /// Nível de visibilidade do evento
    /// </summary>
    [Required]
    public string Visibility { get; set; } = string.Empty;

    /// <summary>
    /// Modo de audiência para elegibilidade
    /// </summary>
    [Required]
    public string AudienceMode { get; set; } = string.Empty;

    /// <summary>
    /// Permite líderes acima do nível do host
    /// </summary>
    public bool AllowLeadersAboveHost { get; set; }

    /// <summary>
    /// Lista de permissão customizada
    /// </summary>
    public string? AllowList { get; set; }

    /// <summary>
    /// Quando as inscrições abrem (UTC)
    /// </summary>
    public DateTime? RegistrationOpenAtUtc { get; set; }

    /// <summary>
    /// Quando as inscrições fecham (UTC)
    /// </summary>
    public DateTime? RegistrationCloseAtUtc { get; set; }

    /// <summary>
    /// Capacidade máxima do evento
    /// </summary>
    public int? Capacity { get; set; }

    /// <summary>
    /// Número atual de participantes registrados
    /// </summary>
    public int RegisteredCount { get; set; }

    /// <summary>
    /// Contagem total de participantes (alias para RegisteredCount)
    /// </summary>
    public int ParticipantCount => RegisteredCount;

    /// <summary>
    /// Número atual de participantes na lista de espera
    /// </summary>
    public int WaitlistedCount { get; set; }

    /// <summary>
    /// Número atual de participantes que fizeram check-in
    /// </summary>
    public int CheckedInCount { get; set; }

    /// <summary>
    /// Capacidade restante
    /// </summary>
    public int? CapacityRemaining { get; set; }

    /// <summary>
    /// Indica se o evento está na capacidade máxima
    /// </summary>
    public bool IsAtCapacity { get; set; }

    /// <summary>
    /// Indica se as inscrições estão abertas
    /// </summary>
    public bool IsRegistrationOpen { get; set; }

    /// <summary>
    /// Indica se o evento está acontecendo agora
    /// </summary>
    public bool IsCurrentlyHappening { get; set; }

    /// <summary>
    /// Indica se o evento já terminou
    /// </summary>
    public bool HasEnded { get; set; }

    /// <summary>
    /// Co-organizadores do evento
    /// </summary>
    public List<EventCoHostDto> CoHosts { get; set; } = new List<EventCoHostDto>();

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
/// DTO para atualização de evento
/// </summary>
public class UpdateEventDto
{
    /// <summary>
    /// Nome do evento
    /// </summary>
    [StringLength(200, ErrorMessage = "Nome deve ter no máximo 200 caracteres")]
    public string? Name { get; set; }

    /// <summary>
    /// Descrição do evento
    /// </summary>
    [StringLength(1000, ErrorMessage = "Descrição deve ter no máximo 1000 caracteres")]
    public string? Description { get; set; }

    /// <summary>
    /// Data de início do evento (UTC)
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Data de fim do evento (UTC)
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Local do evento
    /// </summary>
    [StringLength(200, ErrorMessage = "Local deve ter no máximo 200 caracteres")]
    public string? Location { get; set; }

    /// <summary>
    /// Valor da taxa (null = gratuito)
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Valor da taxa deve ser positivo")]
    public decimal? FeeAmount { get; set; }

    /// <summary>
    /// Moeda da taxa
    /// </summary>
    [StringLength(3, ErrorMessage = "Moeda deve ter 3 caracteres")]
    public string? FeeCurrency { get; set; }

    /// <summary>
    /// Indica se o evento está ativo
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// Idade mínima para participação
    /// </summary>
    [Range(0, 100, ErrorMessage = "Idade mínima deve estar entre 0 e 100")]
    public int? MinAge { get; set; }

    /// <summary>
    /// Idade máxima para participação
    /// </summary>
    [Range(0, 100, ErrorMessage = "Idade máxima deve estar entre 0 e 100")]
    public int? MaxAge { get; set; }

    /// <summary>
    /// Indica se informações médicas são obrigatórias
    /// </summary>
    public bool? RequiresMedicalInfo { get; set; }

    /// <summary>
    /// Indica se investidura de lenço é obrigatória
    /// </summary>
    public bool? RequiresScarfInvested { get; set; }

    /// <summary>
    /// Nível de visibilidade do evento
    /// </summary>
    public string? Visibility { get; set; }

    /// <summary>
    /// Capacidade máxima do evento
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Capacidade deve ser positiva")]
    public int? Capacity { get; set; }

    /// <summary>
    /// Quando as inscrições abrem (UTC)
    /// </summary>
    public DateTime? RegistrationOpenAtUtc { get; set; }

    /// <summary>
    /// Quando as inscrições fecham (UTC)
    /// </summary>
    public DateTime? RegistrationCloseAtUtc { get; set; }
}

/// <summary>
/// DTO para busca de eventos
/// </summary>
public class SearchEventsDto
{
    /// <summary>
    /// Termo de busca
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Filtro por organizador
    /// </summary>
    public Guid? OrganizerId { get; set; }

    /// <summary>
    /// Filtro por nível do organizador
    /// </summary>
    public string? OrganizerLevel { get; set; }

    /// <summary>
    /// Filtro por visibilidade
    /// </summary>
    public string? Visibility { get; set; }

    /// <summary>
    /// Filtro por data de início (a partir de)
    /// </summary>
    public DateTime? StartDateFrom { get; set; }

    /// <summary>
    /// Filtro por data de início (até)
    /// </summary>
    public DateTime? StartDateTo { get; set; }

    /// <summary>
    /// Filtro por status de inscrição
    /// </summary>
    public string? RegistrationStatus { get; set; }

    /// <summary>
    /// Filtro por capacidade
    /// </summary>
    public bool? HasCapacity { get; set; }

    /// <summary>
    /// Filtro por taxa
    /// </summary>
    public bool? IsFree { get; set; }

    /// <summary>
    /// Página atual
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Tamanho da página
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Campo para ordenação
    /// </summary>
    public string? SortBy { get; set; }

    /// <summary>
    /// Direção da ordenação (asc/desc)
    /// </summary>
    public string? SortDirection { get; set; } = "asc";
}

/// <summary>
/// DTO para resultado de busca de eventos
/// </summary>
public class SearchEventsResultDto
{
    /// <summary>
    /// Lista de eventos
    /// </summary>
    public List<EventDto> Events { get; set; } = new List<EventDto>();

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
}
