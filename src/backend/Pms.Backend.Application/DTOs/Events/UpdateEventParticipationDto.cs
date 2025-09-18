using System.ComponentModel.DataAnnotations;

namespace Pms.Backend.Application.DTOs.Events;

/// <summary>
/// DTO para atualizar participação em evento
/// </summary>
public class UpdateEventParticipationDto
{
    /// <summary>
    /// Status da participação
    /// </summary>
    public string? Status { get; set; }

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

    /// <summary>
    /// Taxa paga pelo participante
    /// </summary>
    public decimal? FeePaid { get; set; }

    /// <summary>
    /// Moeda da taxa paga
    /// </summary>
    public string? FeeCurrency { get; set; }
}
