using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pms.Backend.Application.DTOs.Events;
using Pms.Backend.Application.Interfaces.Events;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Infrastructure.Data;

namespace Pms.Backend.Infrastructure.Services.Events;

/// <summary>
/// Serviço para gerenciar participações em eventos
/// </summary>
public class EventParticipationService : Pms.Backend.Application.Interfaces.Events.IEventParticipationService
{
    private readonly PmsDbContext _context;
    private readonly ILogger<EventParticipationService> _logger;
    private readonly ILoggerFactory _loggerFactory;

    public EventParticipationService(
        PmsDbContext context,
        ILogger<EventParticipationService> logger,
        ILoggerFactory loggerFactory)
    {
        _context = context;
        _logger = logger;
        _loggerFactory = loggerFactory;
    }

    /// <summary>
    /// Registra participação de um membro em um evento
    /// </summary>
    public async Task<ServiceResult<EventParticipationDto>> RegisterParticipationAsync(Guid eventId, RegisterParticipationDto request)
    {
        try
        {
            _logger.LogInformation("Registrando participação do membro {MemberId} no evento {EventId}", request.MemberId, eventId);

            // Verificar se o evento existe
            var eventEntity = await _context.Events.FindAsync(eventId);
            if (eventEntity == null)
            {
                return ServiceResult<EventParticipationDto>.Error(
                    "Evento não encontrado",
                    404
                );
            }

            // Verificar se o membro existe
            var member = await _context.Members.FindAsync(request.MemberId);
            if (member == null)
            {
                return ServiceResult<EventParticipationDto>.Error(
                    "Membro não encontrado",
                    404
                );
            }

            // Verificar se já existe participação
            var existingParticipation = await _context.MemberEventParticipations
                .FirstOrDefaultAsync(p => p.EventId == eventId && p.MemberId == request.MemberId);

            if (existingParticipation != null)
            {
                return ServiceResult<EventParticipationDto>.Error(
                    "Membro já está inscrito neste evento",
                    400,
                    new List<string> { "Member already registered for this event" }
                );
            }

            // Verificar elegibilidade
            var eventLogger = _loggerFactory.CreateLogger<EventService>();
            var eligibilityService = new EventService(_context, eventLogger);
            var eligibilityResult = await eligibilityService.ValidateEligibilityAsync(eventId, request.MemberId);

            if (!eligibilityResult.IsSuccess || !eligibilityResult.Data!.IsEligible)
            {
                return ServiceResult<EventParticipationDto>.Error(
                    eligibilityResult.Data?.Reason ?? "Membro não é elegível para este evento",
                    422,
                    new List<string> { eligibilityResult.Data?.ErrorCode ?? "NotEligible" }
                );
            }

            // Verificar capacidade
            if (eventEntity.IsAtCapacity)
            {
                // Adicionar à lista de espera
                var participation = new MemberEventParticipation
                {
                    EventId = eventId,
                    MemberId = request.MemberId,
                    RegisteredAtUtc = DateTime.UtcNow,
                    Status = ParticipationStatus.Waitlisted,
                    Notes = request.Notes
                };

                _context.MemberEventParticipations.Add(participation);
                await _context.SaveChangesAsync();

                // Atualizar contador de lista de espera
                eventEntity.WaitlistedCount++;
                await _context.SaveChangesAsync();

                var participationDto = await MapToParticipationDtoAsync(participation);
                return ServiceResult<EventParticipationDto>.Success(participationDto, "Membro adicionado à lista de espera");
            }
            else
            {
                // Registrar participação normal
                var participation = new MemberEventParticipation
                {
                    EventId = eventId,
                    MemberId = request.MemberId,
                    RegisteredAtUtc = DateTime.UtcNow,
                    Status = ParticipationStatus.Registered,
                    Notes = request.Notes
                };

                _context.MemberEventParticipations.Add(participation);
                await _context.SaveChangesAsync();

                // Atualizar contador de registrados
                eventEntity.RegisteredCount++;
                await _context.SaveChangesAsync();

                var participationDto = await MapToParticipationDtoAsync(participation);
                return ServiceResult<EventParticipationDto>.Success(participationDto, "Participação registrada com sucesso");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar participação do membro {MemberId} no evento {EventId}", request.MemberId, eventId);
            return ServiceResult<EventParticipationDto>.Error(
                "Erro interno do servidor",
                500,
                new List<string> { ex.Message }
            );
        }
    }

    /// <summary>
    /// Obtém uma participação por ID
    /// </summary>
    public async Task<ServiceResult<EventParticipationDto>> GetParticipationAsync(Guid participationId)
    {
        try
        {
            _logger.LogInformation("Obtendo participação: {ParticipationId}", participationId);

            var participation = await _context.MemberEventParticipations
                .Include(p => p.Event)
                .Include(p => p.Member)
                .FirstOrDefaultAsync(p => p.Id == participationId);

            if (participation == null)
            {
                return ServiceResult<EventParticipationDto>.Error(
                    "Participação não encontrada",
                    404
                );
            }

            var participationDto = await MapToParticipationDtoAsync(participation);

            return ServiceResult<EventParticipationDto>.Success(participationDto, "Participação obtida com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter participação: {ParticipationId}", participationId);
            return ServiceResult<EventParticipationDto>.Error(
                "Erro interno do servidor",
                500,
                new List<string> { ex.Message }
            );
        }
    }

    /// <summary>
    /// Busca participações de um evento
    /// </summary>
    public async Task<ServiceResult<SearchParticipationsResultDto>> SearchParticipationsAsync(Guid eventId, SearchParticipationsDto request)
    {
        try
        {
            _logger.LogInformation("Buscando participações do evento: {EventId}", eventId);

            var query = _context.MemberEventParticipations
                .Include(p => p.Event)
                .Include(p => p.Member)
                .Where(p => p.EventId == eventId);

            // Aplicar filtros
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = query.Where(p => p.Member!.FirstName.Contains(request.SearchTerm) ||
                                       p.Member!.LastName.Contains(request.SearchTerm));
            }

            if (!string.IsNullOrEmpty(request.Status))
            {
                var status = Enum.Parse<ParticipationStatus>(request.Status);
                query = query.Where(p => p.Status == status);
            }

            if (request.MemberId.HasValue)
            {
                query = query.Where(p => p.MemberId == request.MemberId.Value);
            }

            if (request.RegisteredFrom.HasValue)
            {
                query = query.Where(p => p.RegisteredAtUtc >= request.RegisteredFrom.Value);
            }

            if (request.RegisteredTo.HasValue)
            {
                query = query.Where(p => p.RegisteredAtUtc <= request.RegisteredTo.Value);
            }

            if (request.HasCheckedIn.HasValue)
            {
                query = query.Where(p => request.HasCheckedIn.Value
                    ? p.CheckedInAtUtc.HasValue
                    : !p.CheckedInAtUtc.HasValue);
            }

            // Aplicar ordenação
            if (!string.IsNullOrEmpty(request.SortBy))
            {
                switch (request.SortBy.ToLower())
                {
                    case "membername":
                        query = request.SortDirection?.ToLower() == "desc"
                            ? query.OrderByDescending(p => p.Member!.FirstName)
                            : query.OrderBy(p => p.Member!.FirstName);
                        break;
                    case "registeredat":
                        query = request.SortDirection?.ToLower() == "desc"
                            ? query.OrderByDescending(p => p.RegisteredAtUtc)
                            : query.OrderBy(p => p.RegisteredAtUtc);
                        break;
                    case "status":
                        query = request.SortDirection?.ToLower() == "desc"
                            ? query.OrderByDescending(p => p.Status)
                            : query.OrderBy(p => p.Status);
                        break;
                    default:
                        query = query.OrderBy(p => p.RegisteredAtUtc);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(p => p.RegisteredAtUtc);
            }

            // Contar total
            var totalCount = await query.CountAsync();

            // Aplicar paginação
            var participations = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            // Mapear para DTOs
            var participationDtos = new List<EventParticipationDto>();
            foreach (var participation in participations)
            {
                var participationDto = await MapToParticipationDtoAsync(participation);
                participationDtos.Add(participationDto);
            }

            var result = new SearchParticipationsResultDto
            {
                Participations = participationDtos,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize)
            };

            return ServiceResult<SearchParticipationsResultDto>.Success(result, "Participações encontradas com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar participações do evento: {EventId}", eventId);
            return ServiceResult<SearchParticipationsResultDto>.Error(
                "Erro interno do servidor",
                500,
                new List<string> { ex.Message }
            );
        }
    }

    /// <summary>
    /// Cancela uma participação
    /// </summary>
    public async Task<ServiceResult<bool>> CancelParticipationAsync(Guid participationId)
    {
        try
        {
            _logger.LogInformation("Cancelando participação: {ParticipationId}", participationId);

            var participation = await _context.MemberEventParticipations
                .Include(p => p.Event)
                .FirstOrDefaultAsync(p => p.Id == participationId);

            if (participation == null)
            {
                return ServiceResult<bool>.Error(
                    "Participação não encontrada",
                    404
                );
            }

            // Verificar se pode cancelar
            if (participation.Status == ParticipationStatus.Cancelled)
            {
                return ServiceResult<bool>.Error(
                    "Participação já foi cancelada",
                    400,
                    new List<string> { "Participation already cancelled" }
                );
            }

            // Atualizar status
            participation.Status = ParticipationStatus.Cancelled;

            // Atualizar contadores do evento
            var eventEntity = participation.Event;
            if (participation.Status == ParticipationStatus.Registered)
            {
                eventEntity.RegisteredCount--;
            }
            else if (participation.Status == ParticipationStatus.Waitlisted)
            {
                eventEntity.WaitlistedCount--;
            }

            await _context.SaveChangesAsync();

            // Se havia lista de espera, promover próximo da lista
            if (eventEntity.WaitlistedCount > 0)
            {
                await PromoteNextFromWaitlistAsync(eventEntity.Id);
            }

            _logger.LogInformation("Participação cancelada com sucesso: {ParticipationId}", participationId);

            return ServiceResult<bool>.Success(true, "Participação cancelada com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao cancelar participação: {ParticipationId}", participationId);
            return ServiceResult<bool>.Error(
                "Erro interno do servidor",
                500,
                new List<string> { ex.Message }
            );
        }
    }

    /// <summary>
    /// Faz check-in de uma participação
    /// </summary>
    public async Task<ServiceResult<bool>> CheckInParticipationAsync(Guid participationId)
    {
        try
        {
            _logger.LogInformation("Fazendo check-in da participação: {ParticipationId}", participationId);

            var participation = await _context.MemberEventParticipations
                .Include(p => p.Event)
                .FirstOrDefaultAsync(p => p.Id == participationId);

            if (participation == null)
            {
                return ServiceResult<bool>.Error(
                    "Participação não encontrada",
                    404
                );
            }

            // Verificar se pode fazer check-in
            if (participation.Status != ParticipationStatus.Registered)
            {
                return ServiceResult<bool>.Error(
                    "Apenas participantes registrados podem fazer check-in",
                    400,
                    new List<string> { "Only registered participants can check in" }
                );
            }

            if (participation.CheckedInAtUtc.HasValue)
            {
                return ServiceResult<bool>.Error(
                    "Check-in já foi realizado",
                    400,
                    new List<string> { "Check-in already completed" }
                );
            }

            // Fazer check-in
            participation.Status = ParticipationStatus.CheckedIn;
            participation.CheckedInAtUtc = DateTime.UtcNow;

            // Atualizar contador de check-ins
            var eventEntity = participation.Event;
            eventEntity.CheckedInCount++;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Check-in realizado com sucesso: {ParticipationId}", participationId);

            return ServiceResult<bool>.Success(true, "Check-in realizado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao fazer check-in da participação: {ParticipationId}", participationId);
            return ServiceResult<bool>.Error(
                "Erro interno do servidor",
                500,
                new List<string> { ex.Message }
            );
        }
    }

    /// <summary>
    /// Promove participação da lista de espera
    /// </summary>
    public async Task<ServiceResult<bool>> PromoteFromWaitlistAsync(Guid participationId)
    {
        try
        {
            _logger.LogInformation("Promovendo participação da lista de espera: {ParticipationId}", participationId);

            var participation = await _context.MemberEventParticipations
                .Include(p => p.Event)
                .FirstOrDefaultAsync(p => p.Id == participationId);

            if (participation == null)
            {
                return ServiceResult<bool>.Error(
                    "Participação não encontrada",
                    404
                );
            }

            if (participation.Status != ParticipationStatus.Waitlisted)
            {
                return ServiceResult<bool>.Error(
                    "Apenas participantes na lista de espera podem ser promovidos",
                    400,
                    new List<string> { "Only waitlisted participants can be promoted" }
                );
            }

            // Verificar se há capacidade
            var eventEntity = participation.Event;
            if (eventEntity.IsAtCapacity)
            {
                return ServiceResult<bool>.Error(
                    "Evento está na capacidade máxima",
                    400,
                    new List<string> { "Event is at capacity" }
                );
            }

            // Promover da lista de espera
            participation.Status = ParticipationStatus.Registered;

            // Atualizar contadores
            eventEntity.WaitlistedCount--;
            eventEntity.RegisteredCount++;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Participação promovida com sucesso: {ParticipationId}", participationId);

            return ServiceResult<bool>.Success(true, "Participação promovida com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao promover participação da lista de espera: {ParticipationId}", participationId);
            return ServiceResult<bool>.Error(
                "Erro interno do servidor",
                500,
                new List<string> { ex.Message }
            );
        }
    }

    /// <summary>
    /// Promove próximo da lista de espera automaticamente
    /// </summary>
    private async Task PromoteNextFromWaitlistAsync(Guid eventId)
    {
        try
        {
            var nextWaitlisted = await _context.MemberEventParticipations
                .Where(p => p.EventId == eventId && p.Status == ParticipationStatus.Waitlisted)
                .OrderBy(p => p.RegisteredAtUtc)
                .FirstOrDefaultAsync();

            if (nextWaitlisted != null)
            {
                await PromoteFromWaitlistAsync(nextWaitlisted.Id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao promover próximo da lista de espera para evento: {EventId}", eventId);
        }
    }

    /// <summary>
    /// Mapeia entidade para DTO
    /// </summary>
    private Task<EventParticipationDto> MapToParticipationDtoAsync(MemberEventParticipation participation)
    {
        return Task.FromResult(new EventParticipationDto
        {
            Id = participation.Id,
            EventId = participation.EventId,
            EventName = participation.Event?.Name,
            MemberId = participation.MemberId,
            MemberName = participation.Member != null
                ? $"{participation.Member.FirstName} {participation.Member.LastName}"
                : null,
            RegisteredAtUtc = participation.RegisteredAtUtc,
            Status = participation.Status.ToString(),
            FeePaid = participation.FeePaid,
            FeeCurrency = participation.FeeCurrency,
            FeePaidAtUtc = participation.FeePaidAtUtc,
            Notes = participation.Notes,
            CheckedInAtUtc = participation.CheckedInAtUtc,
            HasCheckedIn = participation.CheckedInAtUtc.HasValue,
            CreatedAt = participation.CreatedAtUtc,
            UpdatedAt = participation.UpdatedAtUtc
        });
    }
}
