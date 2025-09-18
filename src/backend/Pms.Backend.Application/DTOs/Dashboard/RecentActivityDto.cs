using System.ComponentModel.DataAnnotations;

namespace Pms.Backend.Application.DTOs.Dashboard;

/// <summary>
/// DTO para atividades recentes na dashboard
/// </summary>
public class RecentActivityDto
{
    /// <summary>
    /// ID da atividade
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Tipo da atividade
    /// </summary>
    [Required]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Descrição da atividade
    /// </summary>
    [Required]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Data da atividade
    /// </summary>
    [Required]
    public DateTime Date { get; set; }

    /// <summary>
    /// Nome do membro relacionado (se aplicável)
    /// </summary>
    public string? MemberName { get; set; }

    /// <summary>
    /// Nome do clube relacionado (se aplicável)
    /// </summary>
    public string? ClubName { get; set; }

    /// <summary>
    /// Status da atividade
    /// </summary>
    [Required]
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Prioridade da atividade (High, Medium, Low)
    /// </summary>
    [Required]
    public string Priority { get; set; } = string.Empty;
}
