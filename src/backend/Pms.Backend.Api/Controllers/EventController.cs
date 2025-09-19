using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pms.Backend.Application.DTOs.Events;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Application.Interfaces.Events;
using Pms.Backend.Application.Services.Auth;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Controller para gerenciar eventos oficiais
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    private readonly Pms.Backend.Application.Interfaces.Events.IEventService _eventService;
    private readonly IEventParticipationService _participationService;
    private readonly IAuthService _authService;
    private readonly ILogger<EventController> _logger;

    /// <summary>
    /// Inicializa uma nova instância da classe EventController
    /// </summary>
    /// <param name="eventService">Serviço de eventos</param>
    /// <param name="participationService">Serviço de participações em eventos</param>
    /// <param name="authService">Serviço de autenticação</param>
    /// <param name="logger">Logger para registro de eventos</param>
    public EventController(
        Pms.Backend.Application.Interfaces.Events.IEventService eventService,
        IEventParticipationService participationService,
        IAuthService authService,
        ILogger<EventController> logger)
    {
        _eventService = eventService;
        _participationService = participationService;
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Cria um novo evento
    /// </summary>
    /// <param name="request">Dados do evento</param>
    /// <returns>Evento criado</returns>
    [HttpPost]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto request)
    {
        try
        {
            _logger.LogInformation("Criando novo evento: {EventName}", request.Name);

            var result = await _eventService.CreateEventAsync(request);

            if (!result.IsSuccess)
            {
                return BadRequest(new
                {
                    isSuccess = false,
                    message = result.Message,
                    statusCode = 400,
                    errors = result.Errors
                });
            }

            return CreatedAtAction(nameof(GetEvent), new { id = result.Data?.Id }, new
            {
                isSuccess = true,
                message = "Evento criado com sucesso",
                statusCode = 201,
                data = result.Data
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar evento");
            return StatusCode(500, new
            {
                isSuccess = false,
                message = "Erro interno do servidor",
                statusCode = 500
            });
        }
    }

    /// <summary>
    /// Obtém um evento por ID
    /// </summary>
    /// <param name="id">ID do evento</param>
    /// <returns>Evento encontrado</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEvent(Guid id)
    {
        try
        {
            _logger.LogInformation("Obtendo evento: {EventId}", id);

            var result = await _eventService.GetEventByIdAsync(id);

            if (!result.IsSuccess)
            {
                return NotFound(new
                {
                    isSuccess = false,
                    message = result.Message,
                    statusCode = 404
                });
            }

            return Ok(new
            {
                isSuccess = true,
                message = "Evento obtido com sucesso",
                statusCode = 200,
                data = result.Data
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter evento {EventId}", id);
            return StatusCode(500, new
            {
                isSuccess = false,
                message = "Erro interno do servidor",
                statusCode = 500
            });
        }
    }

    /// <summary>
    /// Lista eventos com filtros
    /// </summary>
    /// <param name="request">Filtros de busca</param>
    /// <returns>Lista de eventos</returns>
    [HttpPost("search")]
    public async Task<IActionResult> SearchEvents([FromBody] SearchEventsDto request)
    {
        try
        {
            _logger.LogInformation("Buscando eventos com filtros");

            var result = await _eventService.SearchEventsAsync(request);

            if (!result.IsSuccess)
            {
                return BadRequest(new
                {
                    isSuccess = false,
                    message = result.Message,
                    statusCode = 400,
                    errors = result.Errors
                });
            }

            return Ok(new
            {
                isSuccess = true,
                message = "Eventos encontrados com sucesso",
                statusCode = 200,
                data = result.Data
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar eventos");
            return StatusCode(500, new
            {
                isSuccess = false,
                message = "Erro interno do servidor",
                statusCode = 500
            });
        }
    }

    /// <summary>
    /// Atualiza um evento
    /// </summary>
    /// <param name="id">ID do evento</param>
    /// <param name="request">Dados atualizados</param>
    /// <returns>Evento atualizado</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(Guid id, [FromBody] UpdateEventDto request)
    {
        try
        {
            _logger.LogInformation("Atualizando evento: {EventId}", id);

            var result = await _eventService.UpdateEventAsync(id, request);

            if (!result.IsSuccess)
            {
                return BadRequest(new
                {
                    isSuccess = false,
                    message = result.Message,
                    statusCode = 400,
                    errors = result.Errors
                });
            }

            return Ok(new
            {
                isSuccess = true,
                message = "Evento atualizado com sucesso",
                statusCode = 200,
                data = result.Data
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar evento {EventId}", id);
            return StatusCode(500, new
            {
                isSuccess = false,
                message = "Erro interno do servidor",
                statusCode = 500
            });
        }
    }

    /// <summary>
    /// Exclui um evento
    /// </summary>
    /// <param name="id">ID do evento</param>
    /// <returns>Confirmação de exclusão</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(Guid id)
    {
        try
        {
            _logger.LogInformation("Excluindo evento: {EventId}", id);

            var result = await _eventService.DeleteEventAsync(id);

            if (!result.IsSuccess)
            {
                return BadRequest(new
                {
                    isSuccess = false,
                    message = result.Message,
                    statusCode = 400
                });
            }

            return Ok(new
            {
                isSuccess = true,
                message = "Evento excluído com sucesso",
                statusCode = 200
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir evento {EventId}", id);
            return StatusCode(500, new
            {
                isSuccess = false,
                message = "Erro interno do servidor",
                statusCode = 500
            });
        }
    }

    /// <summary>
    /// Obtém métricas de um evento
    /// </summary>
    /// <param name="id">ID do evento</param>
    /// <returns>Métricas do evento</returns>
    [HttpGet("{id}/metrics")]
    public async Task<IActionResult> GetEventMetrics(Guid id)
    {
        try
        {
            _logger.LogInformation("Obtendo métricas do evento: {EventId}", id);

            var result = await _eventService.GetEventMetricsAsync(id);

            if (!result.IsSuccess)
            {
                return NotFound(new
                {
                    isSuccess = false,
                    message = result.Message,
                    statusCode = 404
                });
            }

            return Ok(new
            {
                isSuccess = true,
                message = "Métricas obtidas com sucesso",
                statusCode = 200,
                data = result.Data
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter métricas do evento {EventId}", id);
            return StatusCode(500, new
            {
                isSuccess = false,
                message = "Erro interno do servidor",
                statusCode = 500
            });
        }
    }

    /// <summary>
    /// Atualiza a audiência de um evento
    /// </summary>
    /// <param name="id">ID do evento</param>
    /// <param name="request">Nova configuração de audiência</param>
    /// <returns>Resultado da atualização</returns>
    [HttpPatch("{id}/audience")]
    public async Task<IActionResult> UpdateEventAudience(Guid id, [FromBody] UpdateEventAudienceDto request)
    {
        try
        {
            _logger.LogInformation("Atualizando audiência do evento: {EventId}", id);

            var result = await _eventService.UpdateEventAudienceAsync(id, request);

            if (!result.IsSuccess)
            {
                return BadRequest(new
                {
                    isSuccess = false,
                    message = result.Message,
                    statusCode = 400,
                    errors = result.Errors
                });
            }

            return Ok(new
            {
                isSuccess = true,
                message = "Audiência atualizada com sucesso",
                statusCode = 200,
                data = result.Data
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar audiência do evento {EventId}", id);
            return StatusCode(500, new
            {
                isSuccess = false,
                message = "Erro interno do servidor",
                statusCode = 500
            });
        }
    }

    /// <summary>
    /// Inscreve um membro em um evento
    /// </summary>
    /// <param name="eventId">ID do evento</param>
    /// <param name="request">Dados da inscrição</param>
    /// <returns>Inscrição criada</returns>
    [HttpPost("{eventId}/participations")]
    public async Task<IActionResult> RegisterParticipation(Guid eventId, [FromBody] RegisterParticipationDto request)
    {
        try
        {
            _logger.LogInformation("Registrando participação no evento: {EventId}", eventId);

            var result = await _participationService.RegisterParticipationAsync(eventId, request);

            if (!result.IsSuccess)
            {
                return BadRequest(new
                {
                    isSuccess = false,
                    message = result.Message,
                    statusCode = 400,
                    errors = result.Errors
                });
            }

            return CreatedAtAction(nameof(GetParticipation), new { eventId, participationId = result.Data?.Id }, new
            {
                isSuccess = true,
                message = "Participação registrada com sucesso",
                statusCode = 201,
                data = result.Data
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao registrar participação no evento {EventId}", eventId);
            return StatusCode(500, new
            {
                isSuccess = false,
                message = "Erro interno do servidor",
                statusCode = 500
            });
        }
    }

    /// <summary>
    /// Obtém uma participação específica
    /// </summary>
    /// <param name="eventId">ID do evento</param>
    /// <param name="participationId">ID da participação</param>
    /// <returns>Participação encontrada</returns>
    [HttpGet("{eventId}/participations/{participationId}")]
    public async Task<IActionResult> GetParticipation(Guid eventId, Guid participationId)
    {
        try
        {
            _logger.LogInformation("Obtendo participação: {ParticipationId}", participationId);

            var result = await _participationService.GetParticipationAsync(participationId);

            if (!result.IsSuccess)
            {
                return NotFound(new
                {
                    isSuccess = false,
                    message = result.Message,
                    statusCode = 404
                });
            }

            return Ok(new
            {
                isSuccess = true,
                message = "Participação obtida com sucesso",
                statusCode = 200,
                data = result.Data
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter participação {ParticipationId}", participationId);
            return StatusCode(500, new
            {
                isSuccess = false,
                message = "Erro interno do servidor",
                statusCode = 500
            });
        }
    }

    /// <summary>
    /// Lista participações de um evento
    /// </summary>
    /// <param name="eventId">ID do evento</param>
    /// <param name="request">Filtros de busca</param>
    /// <returns>Lista de participações</returns>
    [HttpPost("{eventId}/participations/search")]
    public async Task<IActionResult> SearchParticipations(Guid eventId, [FromBody] SearchParticipationsDto request)
    {
        try
        {
            _logger.LogInformation("Buscando participações do evento: {EventId}", eventId);

            var result = await _participationService.SearchParticipationsAsync(eventId, request);

            if (!result.IsSuccess)
            {
                return BadRequest(new
                {
                    isSuccess = false,
                    message = result.Message,
                    statusCode = 400,
                    errors = result.Errors
                });
            }

            return Ok(new
            {
                isSuccess = true,
                message = "Participações encontradas com sucesso",
                statusCode = 200,
                data = result.Data
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar participações do evento {EventId}", eventId);
            return StatusCode(500, new
            {
                isSuccess = false,
                message = "Erro interno do servidor",
                statusCode = 500
            });
        }
    }

    /// <summary>
    /// Cancela uma participação
    /// </summary>
    /// <param name="eventId">ID do evento</param>
    /// <param name="participationId">ID da participação</param>
    /// <returns>Confirmação de cancelamento</returns>
    [HttpDelete("{eventId}/participations/{participationId}")]
    public async Task<IActionResult> CancelParticipation(Guid eventId, Guid participationId)
    {
        try
        {
            _logger.LogInformation("Cancelando participação: {ParticipationId}", participationId);

            var result = await _participationService.CancelParticipationAsync(participationId);

            if (!result.IsSuccess)
            {
                return BadRequest(new
                {
                    isSuccess = false,
                    message = result.Message,
                    statusCode = 400
                });
            }

            return Ok(new
            {
                isSuccess = true,
                message = "Participação cancelada com sucesso",
                statusCode = 200
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao cancelar participação {ParticipationId}", participationId);
            return StatusCode(500, new
            {
                isSuccess = false,
                message = "Erro interno do servidor",
                statusCode = 500
            });
        }
    }

    /// <summary>
    /// Faz check-in de uma participação
    /// </summary>
    /// <param name="eventId">ID do evento</param>
    /// <param name="participationId">ID da participação</param>
    /// <returns>Confirmação de check-in</returns>
    [HttpPost("{eventId}/participations/{participationId}/checkin")]
    public async Task<IActionResult> CheckInParticipation(Guid eventId, Guid participationId)
    {
        try
        {
            _logger.LogInformation("Fazendo check-in da participação: {ParticipationId}", participationId);

            var result = await _participationService.CheckInParticipationAsync(participationId);

            if (!result.IsSuccess)
            {
                return BadRequest(new
                {
                    isSuccess = false,
                    message = result.Message,
                    statusCode = 400
                });
            }

            return Ok(new
            {
                isSuccess = true,
                message = "Check-in realizado com sucesso",
                statusCode = 200
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao fazer check-in da participação {ParticipationId}", participationId);
            return StatusCode(500, new
            {
                isSuccess = false,
                message = "Erro interno do servidor",
                statusCode = 500
            });
        }
    }
}
