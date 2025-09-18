using System.ComponentModel.DataAnnotations;

namespace Pms.Backend.Application.DTOs.Events;

/// <summary>
/// DTO para criar participação em evento
/// </summary>
public class CreateEventParticipationDto
{
    /// <summary>
    /// ID do evento
    /// </summary>
    [Required(ErrorMessage = "EventId é obrigatório")]
    public Guid EventId { get; set; }

    /// <summary>
    /// ID do membro
    /// </summary>
    [Required(ErrorMessage = "MemberId é obrigatório")]
    public Guid MemberId { get; set; }

    /// <summary>
    /// Status da participação
    /// </summary>
    [Required(ErrorMessage = "Status é obrigatório")]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Observações da participação
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Dados adicionais da participação
    /// </summary>
    public string? Data { get; set; }

    /// <summary>
    /// Data de check-in
    /// </summary>
    public DateTime? CheckInDate { get; set; }

    /// <summary>
    /// Data de check-out
    /// </summary>
    public DateTime? CheckOutDate { get; set; }
}
