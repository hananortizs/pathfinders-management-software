using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pms.Backend.Application.DTOs.Dashboard;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Application.Interfaces.Activities;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Infrastructure.Data;

namespace Pms.Backend.Infrastructure.Services.Activities;

/// <summary>
/// Serviço especializado para gerenciar atividades recentes baseadas no nível hierárquico do usuário
/// </summary>
public class RecentActivitiesService : Pms.Backend.Application.Interfaces.Activities.IRecentActivitiesService
{
    private readonly PmsDbContext _context;
    private readonly ILogger<RecentActivitiesService> _logger;

    public RecentActivitiesService(
        PmsDbContext context,
        ILogger<RecentActivitiesService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<RecentActivityDto>> GetRecentActivitiesAsync(Guid userId, List<string> userScopes, int limit = 10)
    {
        _logger.LogInformation("Fetching recent activities for user {UserId} with scopes {UserScopes}", userId, string.Join(",", userScopes));

        // TODO: Implement real logic based on userScopes and TimelineEntry
        // For now, return mock data
        await Task.Delay(100); // Simulate async operation

        var activities = new List<RecentActivityDto>();

        // Example logic based on scope (to be replaced with actual DB queries)
        if (userScopes.Any(s => s.StartsWith("Club:")))
        {
            activities.Add(new RecentActivityDto
            {
                Id = Guid.NewGuid(),
                Type = "MemberRegistration",
                Description = "Novo membro: João Silva",
                Date = DateTime.UtcNow.AddHours(-2),
                MemberName = "João Silva",
                ClubName = "Pássaro Celeste",
                Status = "Active",
                Priority = "Medium"
            });
            activities.Add(new RecentActivityDto
            {
                Id = Guid.NewGuid(),
                Type = "SpecialtyCompleted",
                Description = "Maria Santos concluiu a especialidade de Culinária",
                Date = DateTime.UtcNow.AddHours(-5),
                MemberName = "Maria Santos",
                ClubName = "Pássaro Celeste",
                Status = "Completed",
                Priority = "Low"
            });
        }
        else if (userScopes.Any(s => s.StartsWith("Region:") || s.StartsWith("Association:")))
        {
            activities.Add(new RecentActivityDto
            {
                Id = Guid.NewGuid(),
                Type = "ClubEventCreated",
                Description = "Clube Águias Reais criou o evento 'Acampamento Regional'",
                Date = DateTime.UtcNow.AddDays(-1),
                ClubName = "Águias Reais",
                Status = "Scheduled",
                Priority = "High"
            });
            activities.Add(new RecentActivityDto
            {
                Id = Guid.NewGuid(),
                Type = "LeadershipChange",
                Description = "Novo Diretor Distrital nomeado para o Distrito Central",
                Date = DateTime.UtcNow.AddDays(-3),
                Status = "Completed",
                Priority = "Medium"
            });
        }
        else // SystemAdmin or higher
        {
            activities.Add(new RecentActivityDto
            {
                Id = Guid.NewGuid(),
                Type = "NewClubRegistered",
                Description = "Novo clube 'Estrelas do Amanhã' registrado na Associação XYZ",
                Date = DateTime.UtcNow.AddDays(-7),
                Status = "Active",
                Priority = "High"
            });
        }

        return activities.OrderByDescending(a => a.Date).Take(limit).ToList();
    }
}
