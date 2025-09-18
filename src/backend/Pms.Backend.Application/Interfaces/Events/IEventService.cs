using Pms.Backend.Application.DTOs.Events;

namespace Pms.Backend.Application.Interfaces.Events;

/// <summary>
/// Interface para serviços de eventos
/// </summary>
public interface IEventService
{
    /// <summary>
    /// Cria um novo evento
    /// </summary>
    /// <param name="request">Dados do evento</param>
    /// <returns>Evento criado</returns>
    Task<ServiceResult<EventDto>> CreateEventAsync(CreateEventDto request);

    /// <summary>
    /// Obtém um evento por ID
    /// </summary>
    /// <param name="id">ID do evento</param>
    /// <returns>Evento encontrado</returns>
    Task<ServiceResult<EventDto>> GetEventByIdAsync(Guid id);

    /// <summary>
    /// Busca eventos com filtros
    /// </summary>
    /// <param name="request">Filtros de busca</param>
    /// <returns>Lista de eventos</returns>
    Task<ServiceResult<SearchEventsResultDto>> SearchEventsAsync(SearchEventsDto request);

    /// <summary>
    /// Atualiza um evento
    /// </summary>
    /// <param name="id">ID do evento</param>
    /// <param name="request">Dados atualizados</param>
    /// <returns>Evento atualizado</returns>
    Task<ServiceResult<EventDto>> UpdateEventAsync(Guid id, UpdateEventDto request);

    /// <summary>
    /// Exclui um evento
    /// </summary>
    /// <param name="id">ID do evento</param>
    /// <returns>Resultado da operação</returns>
    Task<ServiceResult<bool>> DeleteEventAsync(Guid id);

    /// <summary>
    /// Obtém métricas de um evento
    /// </summary>
    /// <param name="id">ID do evento</param>
    /// <returns>Métricas do evento</returns>
    Task<ServiceResult<EventMetricsDto>> GetEventMetricsAsync(Guid id);

    /// <summary>
    /// Atualiza a audiência de um evento
    /// </summary>
    /// <param name="id">ID do evento</param>
    /// <param name="request">Nova configuração de audiência</param>
    /// <returns>Resultado da atualização</returns>
    Task<ServiceResult<UpdateEventAudienceResultDto>> UpdateEventAudienceAsync(Guid id, UpdateEventAudienceDto request);

    /// <summary>
    /// Valida elegibilidade de um membro para um evento
    /// </summary>
    /// <param name="eventId">ID do evento</param>
    /// <param name="memberId">ID do membro</param>
    /// <returns>Resultado da validação</returns>
    Task<ServiceResult<EligibilityValidationDto>> ValidateEligibilityAsync(Guid eventId, Guid memberId);
}

/// <summary>
/// Interface para serviços de participação em eventos
/// </summary>
public interface IEventParticipationService
{
    /// <summary>
    /// Registra participação de um membro em um evento
    /// </summary>
    /// <param name="eventId">ID do evento</param>
    /// <param name="request">Dados da participação</param>
    /// <returns>Participação criada</returns>
    Task<ServiceResult<EventParticipationDto>> RegisterParticipationAsync(Guid eventId, RegisterParticipationDto request);

    /// <summary>
    /// Obtém uma participação por ID
    /// </summary>
    /// <param name="participationId">ID da participação</param>
    /// <returns>Participação encontrada</returns>
    Task<ServiceResult<EventParticipationDto>> GetParticipationAsync(Guid participationId);

    /// <summary>
    /// Busca participações de um evento
    /// </summary>
    /// <param name="eventId">ID do evento</param>
    /// <param name="request">Filtros de busca</param>
    /// <returns>Lista de participações</returns>
    Task<ServiceResult<SearchParticipationsResultDto>> SearchParticipationsAsync(Guid eventId, SearchParticipationsDto request);

    /// <summary>
    /// Cancela uma participação
    /// </summary>
    /// <param name="participationId">ID da participação</param>
    /// <returns>Resultado da operação</returns>
    Task<ServiceResult<bool>> CancelParticipationAsync(Guid participationId);

    /// <summary>
    /// Faz check-in de uma participação
    /// </summary>
    /// <param name="participationId">ID da participação</param>
    /// <returns>Resultado da operação</returns>
    Task<ServiceResult<bool>> CheckInParticipationAsync(Guid participationId);

    /// <summary>
    /// Promove participação da lista de espera
    /// </summary>
    /// <param name="participationId">ID da participação</param>
    /// <returns>Resultado da operação</returns>
    Task<ServiceResult<bool>> PromoteFromWaitlistAsync(Guid participationId);
}

/// <summary>
/// DTO para validação de elegibilidade
/// </summary>
public class EligibilityValidationDto
{
    /// <summary>
    /// Indica se o membro é elegível
    /// </summary>
    public bool IsEligible { get; set; }

    /// <summary>
    /// Motivo da não elegibilidade
    /// </summary>
    public string? Reason { get; set; }

    /// <summary>
    /// Código do erro (se houver)
    /// </summary>
    public string? ErrorCode { get; set; }

    /// <summary>
    /// Detalhes adicionais
    /// </summary>
    public Dictionary<string, object> Details { get; set; } = new Dictionary<string, object>();
}

/// <summary>
/// Classe base para resultados de serviço
/// </summary>
public class ServiceResult<T>
{
    /// <summary>
    /// Indica se a operação foi bem-sucedida
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Dados retornados
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Mensagem de resultado
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Lista de erros
    /// </summary>
    public List<string> Errors { get; set; } = new List<string>();

    /// <summary>
    /// Código de status HTTP
    /// </summary>
    public int StatusCode { get; set; } = 200;

    /// <summary>
    /// Construtor para sucesso
    /// </summary>
    public static ServiceResult<T> Success(T data, string message = "Operação realizada com sucesso")
    {
        return new ServiceResult<T>
        {
            IsSuccess = true,
            Data = data,
            Message = message
        };
    }

    /// <summary>
    /// Construtor para erro
    /// </summary>
    public static ServiceResult<T> Error(string message, int statusCode = 400, List<string>? errors = null)
    {
        return new ServiceResult<T>
        {
            IsSuccess = false,
            Message = message,
            StatusCode = statusCode,
            Errors = errors ?? new List<string>()
        };
    }
}
