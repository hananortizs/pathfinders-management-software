using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pms.Backend.Application.DTOs.Timeline;
using Pms.Backend.Application.Interfaces.Timeline;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Infrastructure.Data;
using Pms.Backend.Application.DTOs.BaseResponse;

namespace Pms.Backend.Infrastructure.Services.Timeline;

/// <summary>
/// Serviço para gerenciar timeline de atividades
/// </summary>
public class TimelineService : Pms.Backend.Application.Interfaces.Timeline.ITimelineService
{
    private readonly PmsDbContext _context;
    private readonly ILogger<TimelineService> _logger;

    public TimelineService(
        PmsDbContext context,
        ILogger<TimelineService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Obtém timeline de um membro
    /// </summary>
    public async Task<ServiceResult<SearchTimelineResultDto>> GetMemberTimelineAsync(Guid memberId, SearchTimelineDto request)
    {
        try
        {
            _logger.LogInformation("Obtendo timeline do membro: {MemberId}", memberId);

            var query = _context.TimelineEntries
                .Include(t => t.Member)
                .Include(t => t.Membership)
                    .ThenInclude(m => m.Club)
                .Include(t => t.Assignment)
                    .ThenInclude(a => a.RoleCatalog)
                .Include(t => t.Event)
                .Where(t => t.MemberId == memberId);

            var result = await ExecuteTimelineSearchAsync(query, request);

            return ServiceResult<SearchTimelineResultDto>.Success(result, "Timeline do membro obtida com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter timeline do membro: {MemberId}", memberId);
            return ServiceResult<SearchTimelineResultDto>.Error(
                "Erro interno do servidor",
                500,
                new List<string> { ex.Message }
            );
        }
    }

    /// <summary>
    /// Obtém timeline de um clube
    /// </summary>
    public async Task<ServiceResult<SearchTimelineResultDto>> GetClubTimelineAsync(Guid clubId, SearchTimelineDto request)
    {
        try
        {
            _logger.LogInformation("Obtendo timeline do clube: {ClubId}", clubId);

            var query = _context.TimelineEntries
                .Include(t => t.Member)
                .Include(t => t.Membership)
                    .ThenInclude(m => m.Club)
                .Include(t => t.Assignment)
                    .ThenInclude(a => a.RoleCatalog)
                .Include(t => t.Event)
                .Where(t => t.Membership != null && t.Membership.ClubId == clubId);

            var result = await ExecuteTimelineSearchAsync(query, request);

            return ServiceResult<SearchTimelineResultDto>.Success(result, "Timeline do clube obtida com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter timeline do clube: {ClubId}", clubId);
            return ServiceResult<SearchTimelineResultDto>.Error(
                "Erro interno do servidor",
                500,
                new List<string> { ex.Message }
            );
        }
    }

    /// <summary>
    /// Obtém timeline de uma entidade hierárquica
    /// </summary>
    public async Task<ServiceResult<SearchTimelineResultDto>> GetHierarchyTimelineAsync(string level, Guid entityId, SearchTimelineDto request)
    {
        try
        {
            _logger.LogInformation("Obtendo timeline da entidade {Level}: {EntityId}", level, entityId);

            var query = _context.TimelineEntries
                .Include(t => t.Member)
                .Include(t => t.Membership)
                    .ThenInclude(m => m.Club)
                .Include(t => t.Assignment)
                    .ThenInclude(a => a.RoleCatalog)
                .Include(t => t.Event)
                .AsQueryable();

            // Filtrar por nível hierárquico
            switch (level.ToLower())
            {
                case "division":
                    query = query.Where(t => t.Membership != null &&
                        t.Membership.Club != null &&
                        t.Membership.Club.District != null &&
                        t.Membership.Club.District.Region != null &&
                        t.Membership.Club.District.Region.Association != null &&
                        t.Membership.Club.District.Region.Association.Union != null &&
                        t.Membership.Club.District.Region.Association.Union.DivisionId == entityId);
                    break;
                case "union":
                    query = query.Where(t => t.Membership != null &&
                        t.Membership.Club != null &&
                        t.Membership.Club.District != null &&
                        t.Membership.Club.District.Region != null &&
                        t.Membership.Club.District.Region.Association != null &&
                        t.Membership.Club.District.Region.Association.UnionId == entityId);
                    break;
                case "association":
                    query = query.Where(t => t.Membership != null &&
                        t.Membership.Club != null &&
                        t.Membership.Club.District != null &&
                        t.Membership.Club.District.Region != null &&
                        t.Membership.Club.District.Region.AssociationId == entityId);
                    break;
                case "region":
                    query = query.Where(t => t.Membership != null &&
                        t.Membership.Club != null &&
                        t.Membership.Club.District != null &&
                        t.Membership.Club.District.RegionId == entityId);
                    break;
                case "district":
                    query = query.Where(t => t.Membership != null &&
                        t.Membership.Club != null &&
                        t.Membership.Club.DistrictId == entityId);
                    break;
                default:
                    return ServiceResult<SearchTimelineResultDto>.Error(
                        "Nível hierárquico inválido",
                        400,
                        new List<string> { "Invalid hierarchy level" }
                    );
            }

            var result = await ExecuteTimelineSearchAsync(query, request);

            return ServiceResult<SearchTimelineResultDto>.Success(result, "Timeline da entidade obtida com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter timeline da entidade {Level}: {EntityId}", level, entityId);
            return ServiceResult<SearchTimelineResultDto>.Error(
                "Erro interno do servidor",
                500,
                new List<string> { ex.Message }
            );
        }
    }

    /// <summary>
    /// Obtém timeline global do sistema
    /// </summary>
    public async Task<ServiceResult<SearchTimelineResultDto>> GetGlobalTimelineAsync(SearchTimelineDto request)
    {
        try
        {
            _logger.LogInformation("Obtendo timeline global");

            var query = _context.TimelineEntries
                .Include(t => t.Member)
                .Include(t => t.Membership)
                    .ThenInclude(m => m.Club)
                .Include(t => t.Assignment)
                    .ThenInclude(a => a.RoleCatalog)
                .Include(t => t.Event)
                .AsQueryable();

            var result = await ExecuteTimelineSearchAsync(query, request);

            return ServiceResult<SearchTimelineResultDto>.Success(result, "Timeline global obtida com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter timeline global");
            return ServiceResult<SearchTimelineResultDto>.Error(
                "Erro interno do servidor",
                500,
                new List<string> { ex.Message }
            );
        }
    }

    /// <summary>
    /// Obtém estatísticas da timeline
    /// </summary>
    public async Task<ServiceResult<TimelineStatsDto>> GetTimelineStatsAsync(SearchTimelineDto request)
    {
        try
        {
            _logger.LogInformation("Obtendo estatísticas da timeline");

            var query = _context.TimelineEntries.AsQueryable();

            // Aplicar filtros básicos
            query = ApplyBasicFilters(query, request);

            var totalEntries = await query.CountAsync();

            var entriesByType = await query
                .GroupBy(t => t.Type)
                .Select(g => new { Type = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Type.ToString(), x => x.Count);

            var entriesByPeriod = await query
                .GroupBy(t => new { Year = t.EventDateUtc.Year, Month = t.EventDateUtc.Month })
                .Select(g => new { Period = $"{g.Key.Year}-{g.Key.Month:D2}", Count = g.Count() })
                .ToDictionaryAsync(x => x.Period, x => x.Count);

            var manualEntries = await query.CountAsync(t => t.Data != null && t.Data.Contains("\"IsManual\":true"));
            var automaticEntries = totalEntries - manualEntries;

            var mostActivePeriod = entriesByPeriod
                .OrderByDescending(x => x.Value)
                .FirstOrDefault().Key;

            var mostCommonType = entriesByType
                .OrderByDescending(x => x.Value)
                .FirstOrDefault().Key;

            var lastEntry = await query
                .OrderByDescending(t => t.EventDateUtc)
                .FirstOrDefaultAsync();

            var firstEntry = await query
                .OrderBy(t => t.EventDateUtc)
                .FirstOrDefaultAsync();

            var stats = new TimelineStatsDto
            {
                TotalEntries = totalEntries,
                EntriesByType = entriesByType,
                EntriesByPeriod = entriesByPeriod,
                ManualEntries = manualEntries,
                AutomaticEntries = automaticEntries,
                MostActivePeriod = mostActivePeriod,
                MostCommonType = mostCommonType,
                LastEntryDate = lastEntry?.EventDateUtc,
                FirstEntryDate = firstEntry?.EventDateUtc
            };

            return ServiceResult<TimelineStatsDto>.Success(stats, "Estatísticas obtidas com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter estatísticas da timeline");
            return ServiceResult<TimelineStatsDto>.Error(
                "Erro interno do servidor",
                500,
                new List<string> { ex.Message }
            );
        }
    }

    /// <summary>
    /// Cria uma entrada manual na timeline
    /// </summary>
    public async Task<ServiceResult<TimelineEntryDto>> CreateManualEntryAsync(CreateTimelineEntryDto request)
    {
        try
        {
            _logger.LogInformation("Criando entrada manual na timeline");

            // Verificar se o membro existe
            var member = await _context.Members.FindAsync(request.MemberId);
            if (member == null)
            {
                return ServiceResult<TimelineEntryDto>.Error(
                    "Membro não encontrado",
                    404
                );
            }

            // Criar entrada
            var entry = new TimelineEntry
            {
                MemberId = request.MemberId,
                Type = Enum.Parse<TimelineEntryType>(request.Type),
                Title = request.Title,
                Description = request.Description,
                Data = request.Data,
                EventDateUtc = request.EventDateUtc,
                MembershipId = request.MembershipId,
                AssignmentId = request.AssignmentId,
                EventId = request.EventId
            };

            _context.TimelineEntries.Add(entry);
            await _context.SaveChangesAsync();

            var entryDto = await MapToTimelineEntryDtoAsync(entry);

            _logger.LogInformation("Entrada manual criada com sucesso: {EntryId}", entry.Id);

            return ServiceResult<TimelineEntryDto>.Success(entryDto, "Entrada criada com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar entrada manual na timeline");
            return ServiceResult<TimelineEntryDto>.Error(
                "Erro interno do servidor",
                500,
                new List<string> { ex.Message }
            );
        }
    }

    /// <summary>
    /// Obtém uma entrada específica da timeline
    /// </summary>
    public async Task<ServiceResult<TimelineEntryDto>> GetTimelineEntryAsync(Guid id)
    {
        try
        {
            _logger.LogInformation("Obtendo entrada da timeline: {EntryId}", id);

            var entry = await _context.TimelineEntries
                .Include(t => t.Member)
                .Include(t => t.Membership)
                    .ThenInclude(m => m.Club)
                .Include(t => t.Assignment)
                    .ThenInclude(a => a.RoleCatalog)
                .Include(t => t.Event)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (entry == null)
            {
                return ServiceResult<TimelineEntryDto>.Error(
                    "Entrada não encontrada",
                    404
                );
            }

            var entryDto = await MapToTimelineEntryDtoAsync(entry);

            return ServiceResult<TimelineEntryDto>.Success(entryDto, "Entrada obtida com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter entrada da timeline: {EntryId}", id);
            return ServiceResult<TimelineEntryDto>.Error(
                "Erro interno do servidor",
                500,
                new List<string> { ex.Message }
            );
        }
    }

    /// <summary>
    /// Cria entrada automática na timeline
    /// </summary>
    public async Task<ServiceResult<TimelineEntryDto>> CreateAutomaticEntryAsync(
        Guid memberId,
        string type,
        string title,
        string description,
        string? data = null,
        DateTime? eventDateUtc = null,
        Guid? membershipId = null,
        Guid? assignmentId = null,
        Guid? eventId = null)
    {
        try
        {
            _logger.LogInformation("Criando entrada automática na timeline para membro: {MemberId}", memberId);

            var entry = new TimelineEntry
            {
                MemberId = memberId,
                Type = Enum.Parse<TimelineEntryType>(type),
                Title = title,
                Description = description,
                Data = data,
                EventDateUtc = eventDateUtc ?? DateTime.UtcNow,
                MembershipId = membershipId,
                AssignmentId = assignmentId,
                EventId = eventId
            };

            _context.TimelineEntries.Add(entry);
            await _context.SaveChangesAsync();

            var entryDto = await MapToTimelineEntryDtoAsync(entry);

            _logger.LogInformation("Entrada automática criada com sucesso: {EntryId}", entry.Id);

            return ServiceResult<TimelineEntryDto>.Success(entryDto, "Entrada criada com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar entrada automática na timeline para membro: {MemberId}", memberId);
            return ServiceResult<TimelineEntryDto>.Error(
                "Erro interno do servidor",
                500,
                new List<string> { ex.Message }
            );
        }
    }

    /// <summary>
    /// Executa busca na timeline com filtros
    /// </summary>
    private async Task<SearchTimelineResultDto> ExecuteTimelineSearchAsync(IQueryable<TimelineEntry> query, SearchTimelineDto request)
    {
        // Aplicar filtros
        query = ApplyBasicFilters(query, request);

        // Aplicar ordenação
        if (!string.IsNullOrEmpty(request.SortBy))
        {
            switch (request.SortBy.ToLower())
            {
                case "eventdate":
                    query = request.SortDirection?.ToLower() == "asc"
                        ? query.OrderBy(t => t.EventDateUtc)
                        : query.OrderByDescending(t => t.EventDateUtc);
                    break;
                case "type":
                    query = request.SortDirection?.ToLower() == "asc"
                        ? query.OrderBy(t => t.Type)
                        : query.OrderByDescending(t => t.Type);
                    break;
                case "title":
                    query = request.SortDirection?.ToLower() == "asc"
                        ? query.OrderBy(t => t.Title)
                        : query.OrderByDescending(t => t.Title);
                    break;
                default:
                    query = query.OrderByDescending(t => t.EventDateUtc);
                    break;
            }
        }
        else
        {
            query = query.OrderByDescending(t => t.EventDateUtc);
        }

        // Contar total
        var totalCount = await query.CountAsync();

        // Aplicar paginação
        var entries = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        // Mapear para DTOs
        var entryDtos = new List<TimelineEntryDto>();
        foreach (var entry in entries)
        {
            var entryDto = await MapToTimelineEntryDtoAsync(entry);
            entryDtos.Add(entryDto);
        }

        return new SearchTimelineResultDto
        {
            Entries = entryDtos,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize),
            AppliedFilters = GetAppliedFilters(request)
        };
    }

    /// <summary>
    /// Aplica filtros básicos à query
    /// </summary>
    private IQueryable<TimelineEntry> ApplyBasicFilters(IQueryable<TimelineEntry> query, SearchTimelineDto request)
    {
        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            query = query.Where(t => t.Title.Contains(request.SearchTerm) ||
                                   t.Description.Contains(request.SearchTerm));
        }

        if (!string.IsNullOrEmpty(request.Type))
        {
            var type = Enum.Parse<TimelineEntryType>(request.Type);
            query = query.Where(t => t.Type == type);
        }

        if (request.StartDate.HasValue)
        {
            query = query.Where(t => t.EventDateUtc >= request.StartDate.Value);
        }

        if (request.EndDate.HasValue)
        {
            query = query.Where(t => t.EventDateUtc <= request.EndDate.Value);
        }

        if (request.ClubId.HasValue)
        {
            query = query.Where(t => t.Membership != null && t.Membership.ClubId == request.ClubId.Value);
        }

        if (!string.IsNullOrEmpty(request.RoleName))
        {
            query = query.Where(t => t.Assignment != null &&
                                   t.Assignment.RoleCatalog != null &&
                                   t.Assignment.RoleCatalog.Name.Contains(request.RoleName));
        }

        if (request.EventId.HasValue)
        {
            query = query.Where(t => t.EventId == request.EventId.Value);
        }

        if (request.IsManual.HasValue)
        {
            if (request.IsManual.Value)
            {
                query = query.Where(t => t.Data != null && t.Data.Contains("\"IsManual\":true"));
            }
            else
            {
                query = query.Where(t => t.Data == null || !t.Data.Contains("\"IsManual\":true"));
            }
        }

        return query;
    }

    /// <summary>
    /// Mapeia entidade para DTO
    /// </summary>
    private async Task<TimelineEntryDto> MapToTimelineEntryDtoAsync(TimelineEntry entry)
    {
        // Converter UTC para BRT (UTC-3)
        var eventDateBrt = entry.EventDateUtc.AddHours(-3);

        return new TimelineEntryDto
        {
            Id = entry.Id,
            MemberId = entry.MemberId,
            MemberName = entry.Member != null
                ? $"{entry.Member.FirstName} {entry.Member.LastName}"
                : null,
            Type = entry.Type.ToString(),
            Title = entry.Title,
            Description = entry.Description,
            Data = entry.Data,
            EventDateUtc = entry.EventDateUtc,
            EventDateBrt = eventDateBrt,
            MembershipId = entry.MembershipId,
            ClubName = entry.Membership?.Club?.Name,
            AssignmentId = entry.AssignmentId,
            RoleName = entry.Assignment?.RoleCatalog?.Name,
            EventId = entry.EventId,
            EventName = entry.Event?.Name,
            IsManual = entry.Data != null && entry.Data.Contains("\"IsManual\":true"),
                CreatedAt = entry.CreatedAtUtc,
                UpdatedAt = entry.UpdatedAtUtc
        };
    }

    /// <summary>
    /// Obtém filtros aplicados
    /// </summary>
    private Dictionary<string, object> GetAppliedFilters(SearchTimelineDto request)
    {
        var filters = new Dictionary<string, object>();

        if (!string.IsNullOrEmpty(request.SearchTerm))
            filters["SearchTerm"] = request.SearchTerm;

        if (!string.IsNullOrEmpty(request.Type))
            filters["Type"] = request.Type;

        if (request.StartDate.HasValue)
            filters["StartDate"] = request.StartDate.Value;

        if (request.EndDate.HasValue)
            filters["EndDate"] = request.EndDate.Value;

        if (request.ClubId.HasValue)
            filters["ClubId"] = request.ClubId.Value;

        if (!string.IsNullOrEmpty(request.RoleName))
            filters["RoleName"] = request.RoleName;

        if (request.EventId.HasValue)
            filters["EventId"] = request.EventId.Value;

        if (request.IsManual.HasValue)
            filters["IsManual"] = request.IsManual.Value;

        return filters;
    }
}
