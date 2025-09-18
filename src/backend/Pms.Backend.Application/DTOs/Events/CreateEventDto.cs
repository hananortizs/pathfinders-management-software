using System.ComponentModel.DataAnnotations;

namespace Pms.Backend.Application.DTOs.Events;

/// <summary>
/// DTO para criação de evento
/// </summary>
public class CreateEventDto
{
    /// <summary>
    /// Nome do evento
    /// </summary>
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(200, ErrorMessage = "Nome deve ter no máximo 200 caracteres")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Descrição do evento
    /// </summary>
    [StringLength(1000, ErrorMessage = "Descrição deve ter no máximo 1000 caracteres")]
    public string? Description { get; set; }

    /// <summary>
    /// Data de início do evento (UTC)
    /// </summary>
    [Required(ErrorMessage = "Data de início é obrigatória")]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Data de fim do evento (UTC)
    /// </summary>
    [Required(ErrorMessage = "Data de fim é obrigatória")]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Local do evento
    /// </summary>
    [StringLength(200, ErrorMessage = "Local deve ter no máximo 200 caracteres")]
    public string? Location { get; set; }

    /// <summary>
    /// Nível do organizador
    /// </summary>
    [Required(ErrorMessage = "Nível do organizador é obrigatório")]
    public string OrganizerLevel { get; set; } = string.Empty;

    /// <summary>
    /// ID da entidade organizadora
    /// </summary>
    [Required(ErrorMessage = "ID do organizador é obrigatório")]
    public Guid OrganizerId { get; set; }

    /// <summary>
    /// Valor da taxa (null = gratuito)
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Valor da taxa deve ser positivo")]
    public decimal? FeeAmount { get; set; }

    /// <summary>
    /// Moeda da taxa (padrão: BRL)
    /// </summary>
    [StringLength(3, ErrorMessage = "Moeda deve ter 3 caracteres")]
    public string FeeCurrency { get; set; } = "BRL";

    /// <summary>
    /// Idade mínima para participação (null = sem restrição)
    /// </summary>
    [Range(0, 100, ErrorMessage = "Idade mínima deve estar entre 0 e 100")]
    public int? MinAge { get; set; }

    /// <summary>
    /// Idade máxima para participação (null = sem restrição)
    /// </summary>
    [Range(0, 100, ErrorMessage = "Idade máxima deve estar entre 0 e 100")]
    public int? MaxAge { get; set; }

    /// <summary>
    /// Indica se informações médicas são obrigatórias
    /// </summary>
    public bool RequiresMedicalInfo { get; set; } = false;

    /// <summary>
    /// Indica se investidura de lenço é obrigatória
    /// </summary>
    public bool RequiresScarfInvested { get; set; } = false;

    /// <summary>
    /// Nível de visibilidade do evento
    /// </summary>
    [Required(ErrorMessage = "Visibilidade é obrigatória")]
    public string Visibility { get; set; } = "Public";

    /// <summary>
    /// Modo de audiência para elegibilidade
    /// </summary>
    [Required(ErrorMessage = "Modo de audiência é obrigatório")]
    public string AudienceMode { get; set; } = "Subtree";

    /// <summary>
    /// Permite líderes acima do nível do host
    /// </summary>
    public bool AllowLeadersAboveHost { get; set; } = true;

    /// <summary>
    /// Lista de permissão customizada (JSON)
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
    /// Capacidade máxima do evento (null = ilimitado)
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Capacidade deve ser positiva")]
    public int? Capacity { get; set; }

    /// <summary>
    /// Co-organizadores do evento (MVP-2)
    /// </summary>
    public List<EventCoHostDto>? CoHosts { get; set; }
}

/// <summary>
/// DTO para co-organizador de evento
/// </summary>
public class EventCoHostDto
{
    /// <summary>
    /// Nível da entidade co-organizadora
    /// </summary>
    [Required(ErrorMessage = "Nível do co-organizador é obrigatório")]
    public string Level { get; set; } = string.Empty;

    /// <summary>
    /// ID da entidade co-organizadora
    /// </summary>
    [Required(ErrorMessage = "ID do co-organizador é obrigatório")]
    public Guid EntityId { get; set; }
}
