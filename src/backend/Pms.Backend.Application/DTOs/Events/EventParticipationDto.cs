using System.ComponentModel.DataAnnotations;

namespace Pms.Backend.Application.DTOs.Events;

/// <summary>
/// DTO para registro de participação em evento
/// </summary>
public class RegisterParticipationDto
{
    /// <summary>
    /// ID do membro
    /// </summary>
    [Required(ErrorMessage = "ID do membro é obrigatório")]
    public Guid MemberId { get; set; }

    /// <summary>
    /// Observações sobre a participação
    /// </summary>
    [StringLength(500, ErrorMessage = "Observações devem ter no máximo 500 caracteres")]
    public string? Notes { get; set; }
}

/// <summary>
/// DTO para exibição de participação em evento
/// </summary>
public class EventParticipationDto
{
    /// <summary>
    /// ID da participação
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// ID do evento
    /// </summary>
    [Required]
    public Guid EventId { get; set; }

    /// <summary>
    /// Nome do evento
    /// </summary>
    public string? EventName { get; set; }

    /// <summary>
    /// ID do membro
    /// </summary>
    [Required]
    public Guid MemberId { get; set; }

    /// <summary>
    /// Nome do membro
    /// </summary>
    public string? MemberName { get; set; }

    /// <summary>
    /// Data de registro da participação (UTC)
    /// </summary>
    [Required]
    public DateTime RegisteredAtUtc { get; set; }

    /// <summary>
    /// Status da participação
    /// </summary>
    [Required]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Taxa paga (se houver)
    /// </summary>
    public decimal? FeePaid { get; set; }

    /// <summary>
    /// Moeda da taxa paga
    /// </summary>
    public string? FeeCurrency { get; set; }

    /// <summary>
    /// Data do pagamento da taxa (UTC)
    /// </summary>
    public DateTime? FeePaidAtUtc { get; set; }

    /// <summary>
    /// Observações sobre a participação
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Data de check-in (UTC)
    /// </summary>
    public DateTime? CheckedInAtUtc { get; set; }

    /// <summary>
    /// Indica se fez check-in
    /// </summary>
    public bool HasCheckedIn { get; set; }

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
/// DTO para busca de participações
/// </summary>
public class SearchParticipationsDto
{
    /// <summary>
    /// Termo de busca
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Filtro por status
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Filtro por membro
    /// </summary>
    public Guid? MemberId { get; set; }

    /// <summary>
    /// Filtro por data de registro (a partir de)
    /// </summary>
    public DateTime? RegisteredFrom { get; set; }

    /// <summary>
    /// Filtro por data de registro (até)
    /// </summary>
    public DateTime? RegisteredTo { get; set; }

    /// <summary>
    /// Filtro por check-in
    /// </summary>
    public bool? HasCheckedIn { get; set; }

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
/// DTO para resultado de busca de participações
/// </summary>
public class SearchParticipationsResultDto
{
    /// <summary>
    /// Lista de participações
    /// </summary>
    public List<EventParticipationDto> Participations { get; set; } = new List<EventParticipationDto>();

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

/// <summary>
/// DTO para métricas de evento
/// </summary>
public class EventMetricsDto
{
    /// <summary>
    /// ID do evento
    /// </summary>
    [Required]
    public Guid EventId { get; set; }

    /// <summary>
    /// Nome do evento
    /// </summary>
    public string? EventName { get; set; }

    /// <summary>
    /// Capacidade máxima
    /// </summary>
    public int? Capacity { get; set; }

    /// <summary>
    /// Número de participantes registrados
    /// </summary>
    public int RegisteredCount { get; set; }

    /// <summary>
    /// Número de participantes na lista de espera
    /// </summary>
    public int WaitlistedCount { get; set; }

    /// <summary>
    /// Número de participantes que fizeram check-in
    /// </summary>
    public int CheckedInCount { get; set; }

    /// <summary>
    /// Capacidade restante
    /// </summary>
    public int? CapacityRemaining { get; set; }

    /// <summary>
    /// Taxa de ocupação (0-100)
    /// </summary>
    public decimal? OccupancyRate { get; set; }

    /// <summary>
    /// Taxa de check-in (0-100)
    /// </summary>
    public decimal? CheckInRate { get; set; }

    /// <summary>
    /// Indica se está na capacidade máxima
    /// </summary>
    public bool IsAtCapacity { get; set; }

    /// <summary>
    /// Indica se as inscrições estão abertas
    /// </summary>
    public bool IsRegistrationOpen { get; set; }

    /// <summary>
    /// Data da última atualização das métricas
    /// </summary>
    public DateTime LastUpdated { get; set; }
}

/// <summary>
/// DTO para atualização de audiência de evento
/// </summary>
public class UpdateEventAudienceDto
{
    /// <summary>
    /// Modo de audiência
    /// </summary>
    public string? AudienceMode { get; set; }

    /// <summary>
    /// Permite líderes acima do nível do host
    /// </summary>
    public bool? AllowLeadersAboveHost { get; set; }

    /// <summary>
    /// Lista de permissão customizada (JSON)
    /// </summary>
    public string? AllowList { get; set; }

    /// <summary>
    /// Motivo da mudança
    /// </summary>
    [StringLength(500, ErrorMessage = "Motivo deve ter no máximo 500 caracteres")]
    public string? Reason { get; set; }

    /// <summary>
    /// Executar dry-run (apenas simular)
    /// </summary>
    public bool DryRun { get; set; } = false;

    /// <summary>
    /// Aplicar mudanças restritivas (MVP-2)
    /// </summary>
    public bool HardEnforce { get; set; } = false;
}

/// <summary>
/// DTO para resultado de atualização de audiência
/// </summary>
public class UpdateEventAudienceResultDto
{
    /// <summary>
    /// Indica se a operação foi bem-sucedida
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Mensagem de resultado
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Avisos sobre a mudança
    /// </summary>
    public List<string> Warnings { get; set; } = new List<string>();

    /// <summary>
    /// Lista de participantes afetados (amostral)
    /// </summary>
    public List<AffectedParticipantDto> AffectedParticipants { get; set; } = new List<AffectedParticipantDto>();

    /// <summary>
    /// Dados do evento atualizado
    /// </summary>
    public EventDto? Event { get; set; }
}

/// <summary>
/// DTO para participante afetado por mudança de audiência
/// </summary>
public class AffectedParticipantDto
{
    /// <summary>
    /// ID da participação
    /// </summary>
    public Guid ParticipationId { get; set; }

    /// <summary>
    /// Nome do membro
    /// </summary>
    public string MemberName { get; set; } = string.Empty;

    /// <summary>
    /// Motivo da afetação
    /// </summary>
    public string Reason { get; set; } = string.Empty;
}
