using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.Interfaces;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Controller para operações de seeds e dados iniciais
/// </summary>
[ApiController]
[Route("api/seeds")]
public class SeedController : ControllerBase
{
    private readonly ISeedService _seedService;
    private readonly ILogger<SeedController> _logger;

    /// <summary>
    /// Inicializa uma nova instância do SeedController
    /// </summary>
    /// <param name="seedService">Serviço de seeds</param>
    /// <param name="logger">Logger</param>
    public SeedController(ISeedService seedService, ILogger<SeedController> logger)
    {
        _seedService = seedService;
        _logger = logger;
    }

    /// <summary>
    /// Executa todos os seeds necessários para o MVP0
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da operação</returns>
    [HttpPost("all")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SeedAll(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Executando todos os seeds...");

            var result = await _seedService.SeedAllAsync(cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Falha ao executar seeds: {Error}", result.Message);
                return BadRequest(result);
            }

            _logger.LogInformation("Todos os seeds executados com sucesso");
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao executar seeds");
            return StatusCode(500, BaseResponse<bool>.ErrorResult("Erro interno do servidor"));
        }
    }

    /// <summary>
    /// Cria apenas o SystemAdmin inicial
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da operação</returns>
    [HttpPost("system-admin")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateSystemAdmin(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Criando SystemAdmin...");

            var result = await _seedService.CreateSystemAdminAsync(cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Falha ao criar SystemAdmin: {Error}", result.Message);
                return BadRequest(result);
            }

            _logger.LogInformation("SystemAdmin criado com sucesso");
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao criar SystemAdmin");
            return StatusCode(500, BaseResponse<bool>.ErrorResult("Erro interno do servidor"));
        }
    }

    /// <summary>
    /// Cria apenas a hierarquia inicial
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da operação</returns>
    [HttpPost("hierarchy")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateHierarchy(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Criando hierarquia inicial...");

            var result = await _seedService.CreateInitialHierarchyAsync(cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Falha ao criar hierarquia: {Error}", result.Message);
                return BadRequest(result);
            }

            _logger.LogInformation("Hierarquia inicial criada com sucesso");
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao criar hierarquia");
            return StatusCode(500, BaseResponse<bool>.ErrorResult("Erro interno do servidor"));
        }
    }

    /// <summary>
    /// Cria apenas o usuário Hanan Del Chiaro
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da operação</returns>
    [HttpPost("hanan-user")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateHananUser(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Criando usuário Hanan Del Chiaro...");

            var result = await _seedService.CreateHananUserAsync(cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Falha ao criar usuário Hanan: {Error}", result.Message);
                return BadRequest(result);
            }

            _logger.LogInformation("Usuário Hanan Del Chiaro criado com sucesso");
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao criar usuário Hanan");
            return StatusCode(500, BaseResponse<bool>.ErrorResult("Erro interno do servidor"));
        }
    }
}
