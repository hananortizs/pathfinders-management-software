using Pms.Backend.Application.DTOs.Timeline;
using Pms.Backend.Application.DTOs.BaseResponse;

namespace Pms.Backend.Application.Interfaces.Timeline;

/// <summary>
/// Interface para serviços de timeline
/// </summary>
public interface ITimelineService
{
    /// <summary>
    /// Obtém timeline de um membro
    /// </summary>
    /// <param name="memberId">ID do membro</param>
    /// <param name="request">Filtros de busca</param>
    /// <returns>Timeline do membro</returns>
    Task<ServiceResult<SearchTimelineResultDto>> GetMemberTimelineAsync(Guid memberId, SearchTimelineDto request);

    /// <summary>
    /// Obtém timeline de um clube
    /// </summary>
    /// <param name="clubId">ID do clube</param>
    /// <param name="request">Filtros de busca</param>
    /// <returns>Timeline do clube</returns>
    Task<ServiceResult<SearchTimelineResultDto>> GetClubTimelineAsync(Guid clubId, SearchTimelineDto request);

    /// <summary>
    /// Obtém timeline de uma entidade hierárquica
    /// </summary>
    /// <param name="level">Nível hierárquico</param>
    /// <param name="entityId">ID da entidade</param>
    /// <param name="request">Filtros de busca</param>
    /// <returns>Timeline da entidade</returns>
    Task<ServiceResult<SearchTimelineResultDto>> GetHierarchyTimelineAsync(string level, Guid entityId, SearchTimelineDto request);

    /// <summary>
    /// Obtém timeline global do sistema
    /// </summary>
    /// <param name="request">Filtros de busca</param>
    /// <returns>Timeline global</returns>
    Task<ServiceResult<SearchTimelineResultDto>> GetGlobalTimelineAsync(SearchTimelineDto request);

    /// <summary>
    /// Obtém estatísticas da timeline
    /// </summary>
    /// <param name="request">Filtros de busca</param>
    /// <returns>Estatísticas da timeline</returns>
    Task<ServiceResult<TimelineStatsDto>> GetTimelineStatsAsync(SearchTimelineDto request);

    /// <summary>
    /// Cria uma entrada manual na timeline
    /// </summary>
    /// <param name="request">Dados da entrada</param>
    /// <returns>Entrada criada</returns>
    Task<ServiceResult<TimelineEntryDto>> CreateManualEntryAsync(CreateTimelineEntryDto request);

    /// <summary>
    /// Obtém uma entrada específica da timeline
    /// </summary>
    /// <param name="id">ID da entrada</param>
    /// <returns>Entrada encontrada</returns>
    Task<ServiceResult<TimelineEntryDto>> GetTimelineEntryAsync(Guid id);

    /// <summary>
    /// Cria entrada automática na timeline
    /// </summary>
    /// <param name="memberId">ID do membro</param>
    /// <param name="type">Tipo da entrada</param>
    /// <param name="title">Título</param>
    /// <param name="description">Descrição</param>
    /// <param name="data">Dados adicionais</param>
    /// <param name="eventDateUtc">Data do evento</param>
    /// <param name="membershipId">ID do membership (opcional)</param>
    /// <param name="assignmentId">ID da atribuição (opcional)</param>
    /// <param name="eventId">ID do evento (opcional)</param>
    /// <returns>Entrada criada</returns>
    Task<ServiceResult<TimelineEntryDto>> CreateAutomaticEntryAsync(
        Guid memberId,
        string type,
        string title,
        string description,
        string? data = null,
        DateTime? eventDateUtc = null,
        Guid? membershipId = null,
        Guid? assignmentId = null,
        Guid? eventId = null);
}
