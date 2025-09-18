using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pms.Backend.Application.DTOs.Events;
using Pms.Backend.Application.Interfaces.Events;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Infrastructure.Data;

namespace Pms.Backend.Infrastructure.Services.Events;

/// <summary>
/// Serviço para gerenciar eventos oficiais
/// </summary>
public class EventService : Pms.Backend.Application.Interfaces.Events.IEventService
{
    private readonly PmsDbContext _context;
    private readonly ILogger<EventService> _logger;

    public EventService(
        PmsDbContext context,
        ILogger<EventService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Cria um novo evento
    /// </summary>
    public async Task<ServiceResult<EventDto>> CreateEventAsync(CreateEventDto request)
    {
        try
        {
            _logger.LogInformation("Criando evento: {EventName}", request.Name);

            // Validar datas
            if (request.StartDate >= request.EndDate)
            {
                return ServiceResult<EventDto>.Error(
                    "Data de início deve ser anterior à data de fim",
                    400,
                    new List<string> { "StartDate must be before EndDate" }
                );
            }

            // Validar idades
            if (request.MinAge.HasValue && request.MaxAge.HasValue && request.MinAge > request.MaxAge)
            {
                return ServiceResult<EventDto>.Error(
                    "Idade mínima deve ser menor ou igual à idade máxima",
                    400,
                    new List<string> { "MinAge must be less than or equal to MaxAge" }
                );
            }

            // Validar datas de inscrição
            if (request.RegistrationOpenAtUtc.HasValue && request.RegistrationCloseAtUtc.HasValue)
            {
                if (request.RegistrationOpenAtUtc >= request.RegistrationCloseAtUtc)
                {
                    return ServiceResult<EventDto>.Error(
                        "Data de abertura das inscrições deve ser anterior à data de fechamento",
                        400,
                        new List<string> { "RegistrationOpenAtUtc must be before RegistrationCloseAtUtc" }
                    );
                }
            }

            // Criar evento
            var eventEntity = new OfficialEvent
            {
                Name = request.Name,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Location = request.Location,
                OrganizerLevel = Enum.Parse<OrganizerLevel>(request.OrganizerLevel),
                OrganizerId = request.OrganizerId,
                FeeAmount = request.FeeAmount,
                FeeCurrency = request.FeeCurrency,
                MinAge = request.MinAge,
                MaxAge = request.MaxAge,
                RequiresMedicalInfo = request.RequiresMedicalInfo,
                RequiresScarfInvested = request.RequiresScarfInvested,
                Visibility = Enum.Parse<EventVisibility>(request.Visibility),
                AudienceMode = Enum.Parse<EventAudienceMode>(request.AudienceMode),
                AllowLeadersAboveHost = request.AllowLeadersAboveHost,
                AllowList = request.AllowList,
                RegistrationOpenAtUtc = request.RegistrationOpenAtUtc,
                RegistrationCloseAtUtc = request.RegistrationCloseAtUtc,
                Capacity = request.Capacity,
                IsActive = true
            };

            _context.Events.Add(eventEntity);
            await _context.SaveChangesAsync();

            // Adicionar co-organizadores se fornecidos
            if (request.CoHosts != null && request.CoHosts.Any())
            {
                foreach (var coHost in request.CoHosts)
                {
                    var coHostEntity = new EventCoHost
                    {
                        EventId = eventEntity.Id,
                        Level = Enum.Parse<EventLevel>(coHost.Level),
                        EntityId = coHost.EntityId
                    };
                    _context.EventCoHosts.Add(coHostEntity);
                }
                await _context.SaveChangesAsync();
            }

            // Mapear para DTO
            var eventDto = await MapToEventDtoAsync(eventEntity);

            _logger.LogInformation("Evento criado com sucesso: {EventId}", eventEntity.Id);

            return ServiceResult<EventDto>.Success(eventDto, "Evento criado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar evento: {EventName}", request.Name);
            return ServiceResult<EventDto>.Error(
                "Erro interno do servidor",
                500,
                new List<string> { ex.Message }
            );
        }
    }

    /// <summary>
    /// Obtém um evento por ID
    /// </summary>
    public async Task<ServiceResult<EventDto>> GetEventByIdAsync(Guid id)
    {
        try
        {
            _logger.LogInformation("Obtendo evento: {EventId}", id);

            var eventEntity = await _context.Events
                .Include(e => e.CoHosts)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventEntity == null)
            {
                return ServiceResult<EventDto>.Error(
                    "Evento não encontrado",
                    404
                );
            }

            var eventDto = await MapToEventDtoAsync(eventEntity);

            return ServiceResult<EventDto>.Success(eventDto, "Evento obtido com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter evento: {EventId}", id);
            return ServiceResult<EventDto>.Error(
                "Erro interno do servidor",
                500,
                new List<string> { ex.Message }
            );
        }
    }

    /// <summary>
    /// Busca eventos com filtros
    /// </summary>
    public async Task<ServiceResult<SearchEventsResultDto>> SearchEventsAsync(SearchEventsDto request)
    {
        try
        {
            _logger.LogInformation("Buscando eventos com filtros");

            var query = _context.Events
                .Include(e => e.CoHosts)
                .AsQueryable();

            // Aplicar filtros
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = query.Where(e => e.Name.Contains(request.SearchTerm) ||
                                       (e.Description != null && e.Description.Contains(request.SearchTerm)));
            }

            if (request.OrganizerId.HasValue)
            {
                query = query.Where(e => e.OrganizerId == request.OrganizerId.Value);
            }

            if (!string.IsNullOrEmpty(request.OrganizerLevel))
            {
                var organizerLevel = Enum.Parse<OrganizerLevel>(request.OrganizerLevel);
                query = query.Where(e => e.OrganizerLevel == organizerLevel);
            }

            if (!string.IsNullOrEmpty(request.Visibility))
            {
                var visibility = Enum.Parse<EventVisibility>(request.Visibility);
                query = query.Where(e => e.Visibility == visibility);
            }

            if (request.StartDateFrom.HasValue)
            {
                query = query.Where(e => e.StartDate >= request.StartDateFrom.Value);
            }

            if (request.StartDateTo.HasValue)
            {
                query = query.Where(e => e.StartDate <= request.StartDateTo.Value);
            }

            if (request.HasCapacity.HasValue)
            {
                if (request.HasCapacity.Value)
                {
                    query = query.Where(e => e.Capacity.HasValue);
                }
                else
                {
                    query = query.Where(e => !e.Capacity.HasValue);
                }
            }

            if (request.IsFree.HasValue)
            {
                if (request.IsFree.Value)
                {
                    query = query.Where(e => !e.FeeAmount.HasValue || e.FeeAmount == 0);
                }
                else
                {
                    query = query.Where(e => e.FeeAmount.HasValue && e.FeeAmount > 0);
                }
            }

            // Aplicar ordenação
            if (!string.IsNullOrEmpty(request.SortBy))
            {
                switch (request.SortBy.ToLower())
                {
                    case "name":
                        query = request.SortDirection?.ToLower() == "desc"
                            ? query.OrderByDescending(e => e.Name)
                            : query.OrderBy(e => e.Name);
                        break;
                    case "startdate":
                        query = request.SortDirection?.ToLower() == "desc"
                            ? query.OrderByDescending(e => e.StartDate)
                            : query.OrderBy(e => e.StartDate);
                        break;
                    case "createdat":
                        query = request.SortDirection?.ToLower() == "desc"
                            ? query.OrderByDescending(e => e.CreatedAtUtc)
                            : query.OrderBy(e => e.CreatedAtUtc);
                        break;
                    default:
                        query = query.OrderBy(e => e.StartDate);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(e => e.StartDate);
            }

            // Contar total
            var totalCount = await query.CountAsync();

            // Aplicar paginação
            var events = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            // Mapear para DTOs
            var eventDtos = new List<EventDto>();
            foreach (var eventEntity in events)
            {
                var eventDto = await MapToEventDtoAsync(eventEntity);
                eventDtos.Add(eventDto);
            }

            var result = new SearchEventsResultDto
            {
                Events = eventDtos,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize)
            };

            return ServiceResult<SearchEventsResultDto>.Success(result, "Eventos encontrados com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar eventos");
            return ServiceResult<SearchEventsResultDto>.Error(
                "Erro interno do servidor",
                500,
                new List<string> { ex.Message }
            );
        }
    }

    /// <summary>
    /// Atualiza um evento
    /// </summary>
    public async Task<ServiceResult<EventDto>> UpdateEventAsync(Guid id, UpdateEventDto request)
    {
        try
        {
            _logger.LogInformation("Atualizando evento: {EventId}", id);

            var eventEntity = await _context.Events
                .Include(e => e.CoHosts)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventEntity == null)
            {
                return ServiceResult<EventDto>.Error(
                    "Evento não encontrado",
                    404
                );
            }

            // Aplicar atualizações
            if (!string.IsNullOrEmpty(request.Name))
                eventEntity.Name = request.Name;

            if (request.Description != null)
                eventEntity.Description = request.Description;

            if (request.StartDate.HasValue)
                eventEntity.StartDate = request.StartDate.Value;

            if (request.EndDate.HasValue)
                eventEntity.EndDate = request.EndDate.Value;

            if (request.Location != null)
                eventEntity.Location = request.Location;

            if (request.FeeAmount.HasValue)
                eventEntity.FeeAmount = request.FeeAmount.Value;

            if (!string.IsNullOrEmpty(request.FeeCurrency))
                eventEntity.FeeCurrency = request.FeeCurrency;

            if (request.IsActive.HasValue)
                eventEntity.IsActive = request.IsActive.Value;

            if (request.MinAge.HasValue)
                eventEntity.MinAge = request.MinAge.Value;

            if (request.MaxAge.HasValue)
                eventEntity.MaxAge = request.MaxAge.Value;

            if (request.RequiresMedicalInfo.HasValue)
                eventEntity.RequiresMedicalInfo = request.RequiresMedicalInfo.Value;

            if (request.RequiresScarfInvested.HasValue)
                eventEntity.RequiresScarfInvested = request.RequiresScarfInvested.Value;

            if (!string.IsNullOrEmpty(request.Visibility))
                eventEntity.Visibility = Enum.Parse<EventVisibility>(request.Visibility);

            if (request.Capacity.HasValue)
                eventEntity.Capacity = request.Capacity.Value;

            if (request.RegistrationOpenAtUtc.HasValue)
                eventEntity.RegistrationOpenAtUtc = request.RegistrationOpenAtUtc.Value;

            if (request.RegistrationCloseAtUtc.HasValue)
                eventEntity.RegistrationCloseAtUtc = request.RegistrationCloseAtUtc.Value;

            // Validar datas atualizadas
            if (eventEntity.StartDate >= eventEntity.EndDate)
            {
                return ServiceResult<EventDto>.Error(
                    "Data de início deve ser anterior à data de fim",
                    400,
                    new List<string> { "StartDate must be before EndDate" }
                );
            }

            await _context.SaveChangesAsync();

            var eventDto = await MapToEventDtoAsync(eventEntity);

            _logger.LogInformation("Evento atualizado com sucesso: {EventId}", id);

            return ServiceResult<EventDto>.Success(eventDto, "Evento atualizado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar evento: {EventId}", id);
            return ServiceResult<EventDto>.Error(
                "Erro interno do servidor",
                500,
                new List<string> { ex.Message }
            );
        }
    }

    /// <summary>
    /// Exclui um evento
    /// </summary>
    public async Task<ServiceResult<bool>> DeleteEventAsync(Guid id)
    {
        try
        {
            _logger.LogInformation("Excluindo evento: {EventId}", id);

            var eventEntity = await _context.Events.FindAsync(id);

            if (eventEntity == null)
            {
                return ServiceResult<bool>.Error(
                    "Evento não encontrado",
                    404
                );
            }

            // Verificar se há participações ativas
            var hasActiveParticipations = await _context.MemberEventParticipations
                .AnyAsync(p => p.EventId == id && p.Status == ParticipationStatus.Registered);

            if (hasActiveParticipations)
            {
                return ServiceResult<bool>.Error(
                    "Não é possível excluir evento com participações ativas",
                    400,
                    new List<string> { "Event has active participations" }
                );
            }

            _context.Events.Remove(eventEntity);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Evento excluído com sucesso: {EventId}", id);

            return ServiceResult<bool>.Success(true, "Evento excluído com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir evento: {EventId}", id);
            return ServiceResult<bool>.Error(
                "Erro interno do servidor",
                500,
                new List<string> { ex.Message }
            );
        }
    }

    /// <summary>
    /// Obtém métricas de um evento
    /// </summary>
    public async Task<ServiceResult<EventMetricsDto>> GetEventMetricsAsync(Guid id)
    {
        try
        {
            _logger.LogInformation("Obtendo métricas do evento: {EventId}", id);

            var eventEntity = await _context.Events.FindAsync(id);

            if (eventEntity == null)
            {
                return ServiceResult<EventMetricsDto>.Error(
                    "Evento não encontrado",
                    404
                );
            }

            // Calcular métricas
            var registeredCount = await _context.MemberEventParticipations
                .CountAsync(p => p.EventId == id && p.Status == ParticipationStatus.Registered);

            var waitlistedCount = await _context.MemberEventParticipations
                .CountAsync(p => p.EventId == id && p.Status == ParticipationStatus.Waitlisted);

            var checkedInCount = await _context.MemberEventParticipations
                .CountAsync(p => p.EventId == id && p.Status == ParticipationStatus.CheckedIn);

            var capacityRemaining = eventEntity.Capacity.HasValue
                ? eventEntity.Capacity.Value - registeredCount
                : (int?)null;

            var occupancyRate = eventEntity.Capacity.HasValue && eventEntity.Capacity.Value > 0
                ? (decimal)registeredCount / eventEntity.Capacity.Value * 100
                : (decimal?)null;

            var checkInRate = registeredCount > 0
                ? (decimal)checkedInCount / registeredCount * 100
                : 0;

            var metrics = new EventMetricsDto
            {
                EventId = eventEntity.Id,
                EventName = eventEntity.Name,
                Capacity = eventEntity.Capacity,
                RegisteredCount = registeredCount,
                WaitlistedCount = waitlistedCount,
                CheckedInCount = checkedInCount,
                CapacityRemaining = capacityRemaining,
                OccupancyRate = occupancyRate,
                CheckInRate = checkInRate,
                IsAtCapacity = eventEntity.IsAtCapacity,
                IsRegistrationOpen = eventEntity.IsRegistrationOpen,
                LastUpdated = DateTime.UtcNow
            };

            return ServiceResult<EventMetricsDto>.Success(metrics, "Métricas obtidas com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter métricas do evento: {EventId}", id);
            return ServiceResult<EventMetricsDto>.Error(
                "Erro interno do servidor",
                500,
                new List<string> { ex.Message }
            );
        }
    }

    /// <summary>
    /// Atualiza a audiência de um evento
    /// </summary>
    public async Task<ServiceResult<UpdateEventAudienceResultDto>> UpdateEventAudienceAsync(Guid id, UpdateEventAudienceDto request)
    {
        try
        {
            _logger.LogInformation("Atualizando audiência do evento: {EventId}", id);

            var eventEntity = await _context.Events.FindAsync(id);

            if (eventEntity == null)
            {
                return ServiceResult<UpdateEventAudienceResultDto>.Error(
                    "Evento não encontrado",
                    404
                );
            }

            // Verificar se as inscrições já estão abertas
            var registrationsOpen = eventEntity.IsRegistrationOpen;
            var hasExistingParticipations = await _context.MemberEventParticipations
                .AnyAsync(p => p.EventId == id);

            var result = new UpdateEventAudienceResultDto
            {
                IsSuccess = true,
                Message = "Audiência atualizada com sucesso",
                Warnings = new List<string>(),
                AffectedParticipants = new List<AffectedParticipantDto>()
            };

            // Se não for dry-run, aplicar mudanças
            if (!request.DryRun)
            {
                if (!string.IsNullOrEmpty(request.AudienceMode))
                {
                    eventEntity.AudienceMode = Enum.Parse<EventAudienceMode>(request.AudienceMode);
                }

                if (request.AllowLeadersAboveHost.HasValue)
                {
                    eventEntity.AllowLeadersAboveHost = request.AllowLeadersAboveHost.Value;
                }

                if (request.AllowList != null)
                {
                    eventEntity.AllowList = request.AllowList;
                }

                await _context.SaveChangesAsync();

                // Se há participações existentes e mudanças restritivas, gerar avisos
                if (hasExistingParticipations && registrationsOpen)
                {
                    result.Warnings.Add("Mudança de audiência aplicada com participações existentes");
                    result.Warnings.Add("Participações existentes foram preservadas (grandfathered)");
                }
            }
            else
            {
                result.Message = "Dry-run executado com sucesso";
                result.Warnings.Add("Nenhuma mudança foi aplicada (modo dry-run)");
            }

            // Mapear evento atualizado
            var eventDto = await MapToEventDtoAsync(eventEntity);
            result.Event = eventDto;

            return ServiceResult<UpdateEventAudienceResultDto>.Success(result, result.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar audiência do evento: {EventId}", id);
            return ServiceResult<UpdateEventAudienceResultDto>.Error(
                "Erro interno do servidor",
                500,
                new List<string> { ex.Message }
            );
        }
    }

    /// <summary>
    /// Valida elegibilidade de um membro para um evento
    /// </summary>
    public async Task<ServiceResult<EligibilityValidationDto>> ValidateEligibilityAsync(Guid eventId, Guid memberId)
    {
        try
        {
            _logger.LogInformation("Validando elegibilidade do membro {MemberId} para evento {EventId}", memberId, eventId);

            var eventEntity = await _context.Events.FindAsync(eventId);
            if (eventEntity == null)
            {
                return ServiceResult<EligibilityValidationDto>.Error(
                    "Evento não encontrado",
                    404
                );
            }

            var member = await _context.Members
                .Include(m => m.Memberships)
                .FirstOrDefaultAsync(m => m.Id == memberId);

            if (member == null)
            {
                return ServiceResult<EligibilityValidationDto>.Error(
                    "Membro não encontrado",
                    404
                );
            }

            var validation = new EligibilityValidationDto
            {
                IsEligible = true,
                Details = new Dictionary<string, object>()
            };

            // Verificar se as inscrições estão abertas
            if (!eventEntity.IsRegistrationOpen)
            {
                validation.IsEligible = false;
                validation.Reason = "Inscrições não estão abertas";
                validation.ErrorCode = "RegistrationClosed";
                return ServiceResult<EligibilityValidationDto>.Success(validation);
            }

            // Verificar capacidade
            if (eventEntity.IsAtCapacity)
            {
                validation.IsEligible = false;
                validation.Reason = "Evento está na capacidade máxima";
                validation.ErrorCode = "EventAtCapacity";
                return ServiceResult<EligibilityValidationDto>.Success(validation);
            }

            // Verificar idade
            if (eventEntity.MinAge.HasValue || eventEntity.MaxAge.HasValue)
            {
                var memberAge = CalculateAge(member.DateOfBirth, eventEntity.StartDate);

                if (eventEntity.MinAge.HasValue && memberAge < eventEntity.MinAge.Value)
                {
                    validation.IsEligible = false;
                    validation.Reason = $"Idade mínima requerida: {eventEntity.MinAge} anos";
                    validation.ErrorCode = "AgeNotEligible";
                    return ServiceResult<EligibilityValidationDto>.Success(validation);
                }

                if (eventEntity.MaxAge.HasValue && memberAge > eventEntity.MaxAge.Value)
                {
                    validation.IsEligible = false;
                    validation.Reason = $"Idade máxima permitida: {eventEntity.MaxAge} anos";
                    validation.ErrorCode = "AgeNotEligible";
                    return ServiceResult<EligibilityValidationDto>.Success(validation);
                }

                validation.Details["MemberAge"] = memberAge;
            }

            // Verificar informações médicas
            if (eventEntity.RequiresMedicalInfo)
            {
                // TODO: Implementar validação de informações médicas quando a entidade estiver disponível
                validation.Details["MedicalInfoRequired"] = true;
            }

            // Verificar investidura de lenço
            if (eventEntity.RequiresScarfInvested)
            {
                // TODO: Implementar validação de investidura de lenço quando a entidade estiver disponível
                validation.Details["ScarfRequired"] = true;
            }

            return ServiceResult<EligibilityValidationDto>.Success(validation, "Validação de elegibilidade concluída");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao validar elegibilidade do membro {MemberId} para evento {EventId}", memberId, eventId);
            return ServiceResult<EligibilityValidationDto>.Error(
                "Erro interno do servidor",
                500,
                new List<string> { ex.Message }
            );
        }
    }

    /// <summary>
    /// Mapeia entidade para DTO
    /// </summary>
    private async Task<EventDto> MapToEventDtoAsync(OfficialEvent eventEntity)
    {
        // Buscar nome do organizador
        string? organizerName = null;
        // TODO: Implementar busca do nome do organizador baseado no nível e ID

        return new EventDto
        {
            Id = eventEntity.Id,
            Name = eventEntity.Name,
            Description = eventEntity.Description,
            StartDate = eventEntity.StartDate,
            EndDate = eventEntity.EndDate,
            Location = eventEntity.Location,
            OrganizerLevel = eventEntity.OrganizerLevel.ToString(),
            OrganizerId = eventEntity.OrganizerId,
            OrganizerName = organizerName,
            FeeAmount = eventEntity.FeeAmount,
            FeeCurrency = eventEntity.FeeCurrency,
            IsActive = eventEntity.IsActive,
            MinAge = eventEntity.MinAge,
            MaxAge = eventEntity.MaxAge,
            RequiresMedicalInfo = eventEntity.RequiresMedicalInfo,
            RequiresScarfInvested = eventEntity.RequiresScarfInvested,
            Visibility = eventEntity.Visibility.ToString(),
            AudienceMode = eventEntity.AudienceMode.ToString(),
            AllowLeadersAboveHost = eventEntity.AllowLeadersAboveHost,
            AllowList = eventEntity.AllowList,
            RegistrationOpenAtUtc = eventEntity.RegistrationOpenAtUtc,
            RegistrationCloseAtUtc = eventEntity.RegistrationCloseAtUtc,
            Capacity = eventEntity.Capacity,
            RegisteredCount = eventEntity.RegisteredCount,
            WaitlistedCount = eventEntity.WaitlistedCount,
            CheckedInCount = eventEntity.CheckedInCount,
            CapacityRemaining = eventEntity.CapacityRemaining,
            IsAtCapacity = eventEntity.IsAtCapacity,
            IsRegistrationOpen = eventEntity.IsRegistrationOpen,
            IsCurrentlyHappening = eventEntity.IsCurrentlyHappening,
            HasEnded = eventEntity.HasEnded,
            CoHosts = eventEntity.CoHosts.Select(ch => new EventCoHostDto
            {
                Level = ch.Level.ToString(),
                EntityId = ch.EntityId
            }).ToList(),
                CreatedAt = eventEntity.CreatedAtUtc,
                UpdatedAt = eventEntity.UpdatedAtUtc
        };
    }

    /// <summary>
    /// Calcula idade baseada na data de nascimento e data de referência
    /// </summary>
    private int CalculateAge(DateTime birthDate, DateTime referenceDate)
    {
        var age = referenceDate.Year - birthDate.Year;
        if (referenceDate.Month < birthDate.Month ||
            (referenceDate.Month == birthDate.Month && referenceDate.Day < birthDate.Day))
        {
            age--;
        }
        return age;
    }
}
