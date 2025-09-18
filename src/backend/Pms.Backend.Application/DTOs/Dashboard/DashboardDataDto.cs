using System.ComponentModel.DataAnnotations;

namespace Pms.Backend.Application.DTOs.Dashboard;

/// <summary>
/// DTO principal para dados da dashboard
/// </summary>
public class DashboardDataDto
{
    /// <summary>
    /// Estatísticas gerais
    /// </summary>
    [Required]
    public DashboardStatsDto Stats { get; set; } = new();

    /// <summary>
    /// Atividades recentes
    /// </summary>
    [Required]
    public List<RecentActivityDto> RecentActivities { get; set; } = new();

    /// <summary>
    /// Próximos eventos
    /// </summary>
    [Required]
    public List<UpcomingEventDto> UpcomingEvents { get; set; } = new();

    /// <summary>
    /// Nível de acesso do usuário
    /// </summary>
    [Required]
    public string UserAccessLevel { get; set; } = string.Empty;

    /// <summary>
    /// Escopo do usuário (association, region, district, club)
    /// </summary>
    [Required]
    public string UserScope { get; set; } = string.Empty;

    /// <summary>
    /// ID do escopo do usuário
    /// </summary>
    public Guid? UserScopeId { get; set; }

    /// <summary>
    /// Nome do escopo do usuário
    /// </summary>
    public string? UserScopeName { get; set; }

    /// <summary>
    /// Data de última atualização dos dados
    /// </summary>
    [Required]
    public DateTime LastUpdated { get; set; }
}
