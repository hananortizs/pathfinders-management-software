using System.ComponentModel.DataAnnotations;

namespace Pms.Backend.Application.DTOs.Dashboard;

/// <summary>
/// DTO para estatísticas gerais da dashboard
/// </summary>
public class DashboardStatsDto
{
    /// <summary>
    /// Total de membros ativos
    /// </summary>
    [Required]
    public int TotalActiveMembers { get; set; }

    /// <summary>
    /// Total de membros pendentes (aguardando ativação)
    /// </summary>
    [Required]
    public int TotalPendingMembers { get; set; }

    /// <summary>
    /// Total de membros inativos
    /// </summary>
    [Required]
    public int TotalInactiveMembers { get; set; }

    /// <summary>
    /// Total de clubes ativos
    /// </summary>
    [Required]
    public int TotalActiveClubs { get; set; }

    /// <summary>
    /// Total de eventos agendados
    /// </summary>
    [Required]
    public int TotalUpcomingEvents { get; set; }

    /// <summary>
    /// Total de eventos realizados no mês atual
    /// </summary>
    [Required]
    public int TotalEventsThisMonth { get; set; }

    /// <summary>
    /// Taxa de participação (membros ativos / total de membros)
    /// </summary>
    [Required]
    public decimal ParticipationRate { get; set; }

    /// <summary>
    /// Crescimento de membros no último mês
    /// </summary>
    [Required]
    public int NewMembersThisMonth { get; set; }

    /// <summary>
    /// Total de especialidades conquistadas no mês
    /// </summary>
    [Required]
    public int SpecialtiesEarnedThisMonth { get; set; }

    /// <summary>
    /// Total de promoções de faixa no mês
    /// </summary>
    [Required]
    public int ScarfPromotionsThisMonth { get; set; }
}
