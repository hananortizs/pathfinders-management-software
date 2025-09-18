using Microsoft.Extensions.Logging;
using Pms.Backend.Application.DTOs.Dashboard;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Application.Interfaces.Activities;

namespace Pms.Backend.Application.Services;

/// <summary>
/// Serviço para gerenciar dados da dashboard
/// </summary>
public class DashboardService : IDashboardService
{
    private readonly ILogger<DashboardService> _logger;
    private readonly IRecentActivitiesService _recentActivitiesService;

    public DashboardService(
        ILogger<DashboardService> logger,
        IRecentActivitiesService recentActivitiesService)
    {
        _logger = logger;
        _recentActivitiesService = recentActivitiesService;
    }

    /// <summary>
    /// Obtém dados da dashboard para um usuário específico baseado em suas permissões
    /// </summary>
    public async Task<DashboardDataDto> GetDashboardDataAsync(Guid userId, List<string> userRoles, List<string> userScopes)
    {
        _logger.LogInformation("Obtendo dados da dashboard para usuário {UserId} com roles {Roles} e scopes {Scopes}",
            userId, string.Join(",", userRoles), string.Join(",", userScopes));

        try
        {
            // Determinar o nível de acesso baseado nas roles
            var accessLevel = DetermineAccessLevel(userRoles);

            // Obter dados baseados no nível de acesso
            var dashboardData = accessLevel switch
            {
                "system_admin" => await GetSystemAdminDashboardAsync(),
                "association_admin" => await GetAssociationAdminDashboardAsync(Guid.Empty),
                "region_admin" => await GetRegionAdminDashboardAsync(Guid.Empty),
                "district_admin" => await GetDistrictAdminDashboardAsync(Guid.Empty),
                "club_admin" => await GetClubAdminDashboardAsync(Guid.Empty),
                "member" => await GetMemberDashboardAsync(userId),
                _ => await GetBasicDashboardDataAsync()
            };

            dashboardData.UserAccessLevel = accessLevel;
            dashboardData.UserScope = userScopes.FirstOrDefault() ?? "unknown";
            dashboardData.LastUpdated = DateTime.UtcNow;

            return dashboardData;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter dados da dashboard para usuário {UserId}", userId);
            throw;
        }
    }

    /// <summary>
    /// Obtém estatísticas gerais baseadas no escopo do usuário
    /// </summary>
    public async Task<DashboardStatsDto> GetDashboardStatsAsync(Guid userId, List<string> userScopes)
    {
        _logger.LogInformation("Obtendo estatísticas da dashboard para usuário {UserId}", userId);

        // Retornar dados mockados por enquanto
        await Task.Delay(100); // Simular operação assíncrona

        return new DashboardStatsDto
        {
            TotalActiveMembers = 150,
            TotalPendingMembers = 25,
            TotalInactiveMembers = 10,
            TotalActiveClubs = 8,
            TotalUpcomingEvents = 12,
            TotalEventsThisMonth = 5,
            ParticipationRate = 85.5m,
            NewMembersThisMonth = 8,
            SpecialtiesEarnedThisMonth = 45,
            ScarfPromotionsThisMonth = 3
        };
    }

    /// <summary>
    /// Obtém atividades recentes baseadas no escopo do usuário
    /// </summary>
    public async Task<List<RecentActivityDto>> GetRecentActivitiesAsync(Guid userId, List<string> userScopes, int limit = 10)
    {
        _logger.LogInformation("Obtendo atividades recentes para usuário {UserId}", userId);

        try
        {
            // Usar o serviço especializado de atividades recentes
            return await _recentActivitiesService.GetRecentActivitiesAsync(userId, userScopes, limit);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter atividades recentes para usuário {UserId}", userId);

            // Fallback para dados mockados em caso de erro
            return new List<RecentActivityDto>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Type = "SystemError",
                    Description = "Erro ao carregar atividades recentes",
                    Date = DateTime.UtcNow,
                    Status = "Error",
                    Priority = "High"
                }
            };
        }
    }

    /// <summary>
    /// Obtém próximos eventos baseados no escopo do usuário
    /// </summary>
    public async Task<List<UpcomingEventDto>> GetUpcomingEventsAsync(Guid userId, List<string> userScopes, int limit = 5)
    {
        _logger.LogInformation("Obtendo próximos eventos para usuário {UserId}", userId);

        // Retornar dados mockados por enquanto
        await Task.Delay(100); // Simular operação assíncrona

        return new List<UpcomingEventDto>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Reunião de Clube - Pássaro Celeste",
                Date = DateTime.UtcNow.AddDays(3),
                Location = "IASD Jardim Brasil",
                Participants = 25,
                MaxParticipants = 50,
                EventType = "Meeting",
                Status = "Scheduled",
                ClubName = "Pássaro Celeste",
                DistrictName = "Distrito de Vila Medeiros",
                Description = "Reunião semanal do clube Pássaro Celeste",
                RequiresRegistration = false
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Acampamento Regional",
                Date = DateTime.UtcNow.AddDays(7),
                Location = "Campo da Igreja Central",
                Participants = 120,
                MaxParticipants = 200,
                EventType = "Camp",
                Status = "Confirmed",
                ClubName = "Pássaro Celeste",
                DistrictName = "Distrito de Vila Medeiros",
                Description = "Acampamento regional da 3ª Região - Tigre",
                RequiresRegistration = true,
                RegistrationDeadline = DateTime.UtcNow.AddDays(5)
            }
        }.Take(limit).ToList();
    }

    /// <summary>
    /// Obtém dados específicos para administradores de sistema
    /// </summary>
    public async Task<DashboardDataDto> GetSystemAdminDashboardAsync()
    {
        _logger.LogInformation("Obtendo dados da dashboard para administrador de sistema");

        var stats = await GetDashboardStatsAsync(Guid.Empty, new List<string> { "system" });
        var activities = await GetRecentActivitiesAsync(Guid.Empty, new List<string> { "system" });
        var events = await GetUpcomingEventsAsync(Guid.Empty, new List<string> { "system" });

        return new DashboardDataDto
        {
            Stats = stats,
            RecentActivities = activities,
            UpcomingEvents = events,
            UserAccessLevel = "system_admin",
            UserScope = "system",
            LastUpdated = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Obtém dados específicos para administradores de associação
    /// </summary>
    public async Task<DashboardDataDto> GetAssociationAdminDashboardAsync(Guid associationId)
    {
        _logger.LogInformation("Obtendo dados da dashboard para administrador de associação {AssociationId}", associationId);

        var stats = await GetDashboardStatsAsync(Guid.Empty, new List<string> { "association" });
        var activities = await GetRecentActivitiesAsync(Guid.Empty, new List<string> { "association" });
        var events = await GetUpcomingEventsAsync(Guid.Empty, new List<string> { "association" });

        return new DashboardDataDto
        {
            Stats = stats,
            RecentActivities = activities,
            UpcomingEvents = events,
            UserAccessLevel = "association_admin",
            UserScope = "association",
            UserScopeId = associationId,
            LastUpdated = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Obtém dados específicos para administradores de região
    /// </summary>
    public async Task<DashboardDataDto> GetRegionAdminDashboardAsync(Guid regionId)
    {
        _logger.LogInformation("Obtendo dados da dashboard para administrador de região {RegionId}", regionId);

        var stats = await GetDashboardStatsAsync(Guid.Empty, new List<string> { "region" });
        var activities = await GetRecentActivitiesAsync(Guid.Empty, new List<string> { "region" });
        var events = await GetUpcomingEventsAsync(Guid.Empty, new List<string> { "region" });

        return new DashboardDataDto
        {
            Stats = stats,
            RecentActivities = activities,
            UpcomingEvents = events,
            UserAccessLevel = "region_admin",
            UserScope = "region",
            UserScopeId = regionId,
            LastUpdated = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Obtém dados específicos para administradores de distrito
    /// </summary>
    public async Task<DashboardDataDto> GetDistrictAdminDashboardAsync(Guid districtId)
    {
        _logger.LogInformation("Obtendo dados da dashboard para administrador de distrito {DistrictId}", districtId);

        var stats = await GetDashboardStatsAsync(Guid.Empty, new List<string> { "district" });
        var activities = await GetRecentActivitiesAsync(Guid.Empty, new List<string> { "district" });
        var events = await GetUpcomingEventsAsync(Guid.Empty, new List<string> { "district" });

        return new DashboardDataDto
        {
            Stats = stats,
            RecentActivities = activities,
            UpcomingEvents = events,
            UserAccessLevel = "district_admin",
            UserScope = "district",
            UserScopeId = districtId,
            LastUpdated = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Obtém dados específicos para administradores de clube
    /// </summary>
    public async Task<DashboardDataDto> GetClubAdminDashboardAsync(Guid clubId)
    {
        _logger.LogInformation("Obtendo dados da dashboard para administrador de clube {ClubId}", clubId);

        var stats = await GetDashboardStatsAsync(Guid.Empty, new List<string> { "club" });
        var activities = await GetRecentActivitiesAsync(Guid.Empty, new List<string> { "club" });
        var events = await GetUpcomingEventsAsync(Guid.Empty, new List<string> { "club" });

        return new DashboardDataDto
        {
            Stats = stats,
            RecentActivities = activities,
            UpcomingEvents = events,
            UserAccessLevel = "club_admin",
            UserScope = "club",
            UserScopeId = clubId,
            LastUpdated = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Obtém dados específicos para membros
    /// </summary>
    public async Task<DashboardDataDto> GetMemberDashboardAsync(Guid memberId)
    {
        _logger.LogInformation("Obtendo dados da dashboard para membro {MemberId}", memberId);

        var stats = await GetDashboardStatsAsync(memberId, new List<string> { "member" });
        var activities = await GetRecentActivitiesAsync(memberId, new List<string> { "member" });
        var events = await GetUpcomingEventsAsync(memberId, new List<string> { "member" });

        return new DashboardDataDto
        {
            Stats = stats,
            RecentActivities = activities,
            UpcomingEvents = events,
            UserAccessLevel = "member",
            UserScope = "member",
            UserScopeId = memberId,
            LastUpdated = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Obtém dados básicos da dashboard
    /// </summary>
    private async Task<DashboardDataDto> GetBasicDashboardDataAsync()
    {
        var stats = new DashboardStatsDto();
        var activities = new List<RecentActivityDto>();
        var events = new List<UpcomingEventDto>();

        return new DashboardDataDto
        {
            Stats = stats,
            RecentActivities = activities,
            UpcomingEvents = events,
            UserAccessLevel = "basic",
            UserScope = "unknown",
            LastUpdated = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Determina o nível de acesso baseado nas roles do usuário
    /// </summary>
    private string DetermineAccessLevel(List<string> userRoles)
    {
        if (userRoles.Contains("SystemAdmin")) return "system_admin";
        if (userRoles.Contains("AssociationAdmin")) return "association_admin";
        if (userRoles.Contains("RegionAdmin")) return "region_admin";
        if (userRoles.Contains("DistrictAdmin")) return "district_admin";
        if (userRoles.Contains("ClubAdmin")) return "club_admin";
        if (userRoles.Contains("Member")) return "member";
        return "basic";
    }

}
