using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pms.Backend.Application.DTOs.Auth;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.Interfaces;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Controller para operações de autenticação
/// </summary>
[ApiController]
[Route("[controller]")]
public class AuthController : BaseController
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    /// <summary>
    /// Inicializa uma nova instância do AuthController
    /// </summary>
    /// <param name="authService">Serviço de autenticação</param>
    /// <param name="logger">Logger</param>
    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Realiza login do usuário
    /// </summary>
    /// <param name="request">Dados de login</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Token JWT e informações do usuário</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _authService.LoginAsync(request, cancellationToken);
        return ProcessResponse(result);
    }

    /// <summary>
    /// Valida um token JWT
    /// </summary>
    /// <param name="token">Token JWT</param>
    /// <returns>Status da validação</returns>
    [HttpPost("validate")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult ValidateToken([FromBody] string token)
    {
        var result = _authService.ValidateTokenWithUserInfo(token);
        return ProcessResponse(result);
    }

    /// <summary>
    /// Atualiza um token JWT
    /// </summary>
    /// <param name="token">Token atual</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Novo token JWT</returns>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RefreshToken([FromBody] string token, CancellationToken cancellationToken = default)
    {
        var result = await _authService.RefreshTokenAsync(token, cancellationToken);
        return ProcessResponse(result);
    }

    /// <summary>
    /// Obtém informações do usuário a partir do token
    /// </summary>
    /// <param name="token">Token JWT</param>
    /// <returns>Informações do usuário</returns>
    [HttpPost("user-info")]
    [ProducesResponseType(typeof(UserInfoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetUserInfo([FromBody] string token)
    {
        var result = _authService.GetUserInfoFromToken(token);
        return ProcessResponse(result);
    }

}
