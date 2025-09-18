using Pms.Backend.Application.DTOs.Dashboard;

namespace Pms.Backend.Application.Interfaces;

/// <summary>
/// Interface para serviços de dashboard
/// </summary>
public interface IDashboardService
{
    /// <summary>
    /// Obtém dados da dashboard para um usuário específico baseado em suas permissões
    /// </summary>
    /// <param name="userId">ID do usuário</param>
    /// <param name="userRoles">Roles do usuário</param>
    /// <param name="userScopes">Scopes do usuário</param>
    /// <returns>Dados da dashboard</returns>
    Task<DashboardDataDto> GetDashboardDataAsync(Guid userId, List<string> userRoles, List<string> userScopes);

    /// <summary>
    /// Obtém estatísticas gerais baseadas no escopo do usuário
    /// </summary>
    /// <param name="userId">ID do usuário</param>
    /// <param name="userScopes">Scopes do usuário</param>
    /// <returns>Estatísticas da dashboard</returns>
    Task<DashboardStatsDto> GetDashboardStatsAsync(Guid userId, List<string> userScopes);

    /// <summary>
    /// Obtém atividades recentes baseadas no escopo do usuário
    /// </summary>
    /// <param name="userId">ID do usuário</param>
    /// <param name="userScopes">Scopes do usuário</param>
    /// <param name="limit">Limite de atividades (padrão: 10)</param>
    /// <returns>Lista de atividades recentes</returns>
    Task<List<RecentActivityDto>> GetRecentActivitiesAsync(Guid userId, List<string> userScopes, int limit = 10);

    /// <summary>
    /// Obtém próximos eventos baseados no escopo do usuário
    /// </summary>
    /// <param name="userId">ID do usuário</param>
    /// <param name="userScopes">Scopes do usuário</param>
    /// <param name="limit">Limite de eventos (padrão: 5)</param>
    /// <returns>Lista de próximos eventos</returns>
    Task<List<UpcomingEventDto>> GetUpcomingEventsAsync(Guid userId, List<string> userScopes, int limit = 5);

    /// <summary>
    /// Obtém dados específicos para administradores de sistema
    /// </summary>
    /// <returns>Dados administrativos globais</returns>
    Task<DashboardDataDto> GetSystemAdminDashboardAsync();

    /// <summary>
    /// Obtém dados específicos para administradores de associação
    /// </summary>
    /// <param name="associationId">ID da associação</param>
    /// <returns>Dados da associação</returns>
    Task<DashboardDataDto> GetAssociationAdminDashboardAsync(Guid associationId);

    /// <summary>
    /// Obtém dados específicos para administradores de região
    /// </summary>
    /// <param name="regionId">ID da região</param>
    /// <returns>Dados da região</returns>
    Task<DashboardDataDto> GetRegionAdminDashboardAsync(Guid regionId);

    /// <summary>
    /// Obtém dados específicos para administradores de distrito
    /// </summary>
    /// <param name="districtId">ID do distrito</param>
    /// <returns>Dados do distrito</returns>
    Task<DashboardDataDto> GetDistrictAdminDashboardAsync(Guid districtId);

    /// <summary>
    /// Obtém dados específicos para administradores de clube
    /// </summary>
    /// <param name="clubId">ID do clube</param>
    /// <returns>Dados do clube</returns>
    Task<DashboardDataDto> GetClubAdminDashboardAsync(Guid clubId);

    /// <summary>
    /// Obtém dados específicos para membros
    /// </summary>
    /// <param name="memberId">ID do membro</param>
    /// <returns>Dados pessoais do membro</returns>
    Task<DashboardDataDto> GetMemberDashboardAsync(Guid memberId);
}
