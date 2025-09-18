using Pms.Backend.Application.DTOs.Dashboard;

namespace Pms.Backend.Application.Interfaces.Activities;

/// <summary>
/// Interface para serviços de atividades recentes
/// </summary>
public interface IRecentActivitiesService
{
    /// <summary>
    /// Obtém atividades recentes baseadas no escopo do usuário
    /// </summary>
    /// <param name="userId">ID do usuário</param>
    /// <param name="userScopes">Scopes do usuário (ex: ["Club:123", "Region:456"])</param>
    /// <param name="limit">Limite de atividades (padrão: 10)</param>
    /// <returns>Lista de atividades recentes</returns>
    Task<List<RecentActivityDto>> GetRecentActivitiesAsync(
        Guid userId,
        List<string> userScopes,
        int limit = 10);
}
