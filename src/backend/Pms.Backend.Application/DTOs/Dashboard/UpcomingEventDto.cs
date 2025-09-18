using System.ComponentModel.DataAnnotations;

namespace Pms.Backend.Application.DTOs.Dashboard;

/// <summary>
/// DTO para eventos próximos na dashboard
/// </summary>
public class UpcomingEventDto
{
    /// <summary>
    /// ID do evento
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Título do evento
    /// </summary>
    [Required]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Data do evento
    /// </summary>
    [Required]
    public DateTime Date { get; set; }

    /// <summary>
    /// Local do evento
    /// </summary>
    [Required]
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// Número de participantes confirmados
    /// </summary>
    [Required]
    public int Participants { get; set; }

    /// <summary>
    /// Capacidade máxima do evento
    /// </summary>
    [Required]
    public int MaxParticipants { get; set; }

    /// <summary>
    /// Tipo do evento (Meeting, Camp, Ceremony, etc.)
    /// </summary>
    [Required]
    public string EventType { get; set; } = string.Empty;

    /// <summary>
    /// Status do evento (Scheduled, Confirmed, Cancelled)
    /// </summary>
    [Required]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Nome do clube organizador
    /// </summary>
    public string? ClubName { get; set; }

    /// <summary>
    /// Nome do distrito organizador
    /// </summary>
    public string? DistrictName { get; set; }

    /// <summary>
    /// Descrição do evento
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Indica se o evento requer inscrição
    /// </summary>
    [Required]
    public bool RequiresRegistration { get; set; }

    /// <summary>
    /// Data limite para inscrição
    /// </summary>
    public DateTime? RegistrationDeadline { get; set; }
}
