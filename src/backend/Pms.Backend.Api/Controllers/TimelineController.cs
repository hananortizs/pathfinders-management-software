using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pms.Backend.Application.DTOs.Timeline;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Application.Interfaces.Timeline;
using Pms.Backend.Application.Services.Auth;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Controller para gerenciar timeline de atividades
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TimelineController : ControllerBase
{
    private readonly ITimelineService _timelineService;
    private readonly IAuthService _authService;
    private readonly ILogger<TimelineController> _logger;

    /// <summary>
    /// Inicializa uma nova instância da classe TimelineController
    /// </summary>
    /// <param name="timelineService">Serviço de timeline</param>
    /// <param name="authService">Serviço de autenticação</param>
    /// <param name="logger">Logger para registro de eventos</param>
    public TimelineController(
        ITimelineService timelineService,
        IAuthService authService,
        ILogger<TimelineController> logger)
    {
        _timelineService = timelineService;
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Obtém timeline de um membro
    /// </summary>
    /// <param name="memberId">ID do membro</param>
    /// <param name="request">Filtros de busca</param>
    /// <returns>Timeline do membro</returns>
    [HttpPost("member/{memberId}")]
    public async Task<IActionResult> GetMemberTimeline(Guid memberId, [FromBody] SearchTimelineDto request)
    {
        try
        {
            _logger.LogInformation("Obtendo timeline do membro: {MemberId}", memberId);

            var result = await _timelineService.GetMemberTimelineAsync(memberId, request);

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
                message = "Timeline obtida com sucesso",
                statusCode = 200,
                data = result.Data
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter timeline do membro: {MemberId}", memberId);
            return StatusCode(500, new
            {
                isSuccess = false,
                message = "Erro interno do servidor",
                statusCode = 500
            });
        }
    }

    /// <summary>
    /// Obtém timeline de um clube
    /// </summary>
    /// <param name="clubId">ID do clube</param>
    /// <param name="request">Filtros de busca</param>
    /// <returns>Timeline do clube</returns>
    [HttpPost("club/{clubId}")]
    public async Task<IActionResult> GetClubTimeline(Guid clubId, [FromBody] SearchTimelineDto request)
    {
        try
        {
            _logger.LogInformation("Obtendo timeline do clube: {ClubId}", clubId);

            var result = await _timelineService.GetClubTimelineAsync(clubId, request);

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
                message = "Timeline obtida com sucesso",
                statusCode = 200,
                data = result.Data
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter timeline do clube: {ClubId}", clubId);
            return StatusCode(500, new
            {
                isSuccess = false,
                message = "Erro interno do servidor",
                statusCode = 500
            });
        }
    }

    /// <summary>
    /// Obtém timeline de uma entidade hierárquica
    /// </summary>
    /// <param name="level">Nível hierárquico</param>
    /// <param name="entityId">ID da entidade</param>
    /// <param name="request">Filtros de busca</param>
    /// <returns>Timeline da entidade</returns>
    [HttpPost("hierarchy/{level}/{entityId}")]
    public async Task<IActionResult> GetHierarchyTimeline(string level, Guid entityId, [FromBody] SearchTimelineDto request)
    {
        try
        {
            _logger.LogInformation("Obtendo timeline da entidade {Level}: {EntityId}", level, entityId);

            var result = await _timelineService.GetHierarchyTimelineAsync(level, entityId, request);

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
                message = "Timeline obtida com sucesso",
                statusCode = 200,
                data = result.Data
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter timeline da entidade {Level}: {EntityId}", level, entityId);
            return StatusCode(500, new
            {
                isSuccess = false,
                message = "Erro interno do servidor",
                statusCode = 500
            });
        }
    }

    /// <summary>
    /// Obtém timeline global do sistema
    /// </summary>
    /// <param name="request">Filtros de busca</param>
    /// <returns>Timeline global</returns>
    [HttpPost("global")]
    public async Task<IActionResult> GetGlobalTimeline([FromBody] SearchTimelineDto request)
    {
        try
        {
            _logger.LogInformation("Obtendo timeline global");

            var result = await _timelineService.GetGlobalTimelineAsync(request);

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
                message = "Timeline obtida com sucesso",
                statusCode = 200,
                data = result.Data
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter timeline global");
            return StatusCode(500, new
            {
                isSuccess = false,
                message = "Erro interno do servidor",
                statusCode = 500
            });
        }
    }

    /// <summary>
    /// Obtém estatísticas da timeline
    /// </summary>
    /// <param name="request">Filtros de busca</param>
    /// <returns>Estatísticas da timeline</returns>
    [HttpPost("stats")]
    public async Task<IActionResult> GetTimelineStats([FromBody] SearchTimelineDto request)
    {
        try
        {
            _logger.LogInformation("Obtendo estatísticas da timeline");

            var result = await _timelineService.GetTimelineStatsAsync(request);

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
                message = "Estatísticas obtidas com sucesso",
                statusCode = 200,
                data = result.Data
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter estatísticas da timeline");
            return StatusCode(500, new
            {
                isSuccess = false,
                message = "Erro interno do servidor",
                statusCode = 500
            });
        }
    }

    /// <summary>
    /// Cria uma entrada manual na timeline
    /// </summary>
    /// <param name="request">Dados da entrada</param>
    /// <returns>Entrada criada</returns>
    [HttpPost("manual")]
    public async Task<IActionResult> CreateManualEntry([FromBody] CreateTimelineEntryDto request)
    {
        try
        {
            _logger.LogInformation("Criando entrada manual na timeline");

            var result = await _timelineService.CreateManualEntryAsync(request);

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

            return CreatedAtAction(nameof(GetTimelineEntry), new { id = result.Data?.Id }, new
            {
                isSuccess = true,
                message = "Entrada criada com sucesso",
                statusCode = 201,
                data = result.Data
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar entrada manual na timeline");
            return StatusCode(500, new
            {
                isSuccess = false,
                message = "Erro interno do servidor",
                statusCode = 500
            });
        }
    }

    /// <summary>
    /// Obtém uma entrada específica da timeline
    /// </summary>
    /// <param name="id">ID da entrada</param>
    /// <returns>Entrada encontrada</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTimelineEntry(Guid id)
    {
        try
        {
            _logger.LogInformation("Obtendo entrada da timeline: {EntryId}", id);

            var result = await _timelineService.GetTimelineEntryAsync(id);

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
                message = "Entrada obtida com sucesso",
                statusCode = 200,
                data = result.Data
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter entrada da timeline: {EntryId}", id);
            return StatusCode(500, new
            {
                isSuccess = false,
                message = "Erro interno do servidor",
                statusCode = 500
            });
        }
    }
}
