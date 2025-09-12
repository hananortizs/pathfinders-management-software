using Microsoft.AspNetCore.Authorization;
using Pms.Backend.Application.Interfaces;
using System.Security.Claims;

namespace Pms.Backend.Api.Middleware;

/// <summary>
/// Middleware para autenticação JWT
/// </summary>
public class JwtAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<JwtAuthenticationMiddleware> _logger;

    /// <summary>
    /// Inicializa uma nova instância do JwtAuthenticationMiddleware
    /// </summary>
    /// <param name="next">Próximo middleware na pipeline</param>
    /// <param name="logger">Logger</param>
    public JwtAuthenticationMiddleware(RequestDelegate next, ILogger<JwtAuthenticationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Processa a requisição HTTP
    /// </summary>
    /// <param name="context">Contexto HTTP</param>
    /// <param name="authenticationService">Serviço de autenticação</param>
    public async Task InvokeAsync(HttpContext context, IAuthenticationService authenticationService)
    {
        try
        {
            // Verificar se o endpoint requer autenticação
            var endpoint = context.GetEndpoint();
            var allowAnonymous = endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null;

            if (allowAnonymous)
            {
                await _next(context);
                return;
            }

            // Extrair token do header Authorization
            var token = ExtractTokenFromHeader(context);

            if (string.IsNullOrWhiteSpace(token))
            {
                _logger.LogWarning("Token JWT não fornecido para endpoint: {Path}", context.Request.Path);
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Token de autenticação é obrigatório");
                return;
            }

            // Validar token
            if (!authenticationService.ValidateToken(token))
            {
                _logger.LogWarning("Token JWT inválido para endpoint: {Path}", context.Request.Path);
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Token de autenticação inválido");
                return;
            }

            // Extrair informações do usuário do token
            var userInfo = authenticationService.GetUserFromToken(token);
            if (userInfo == null)
            {
                _logger.LogWarning("Não foi possível extrair informações do usuário do token para endpoint: {Path}", context.Request.Path);
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Token de autenticação inválido");
                return;
            }

            // Criar claims principal
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userInfo.Id.ToString()),
                new(ClaimTypes.Email, userInfo.Email),
                new(ClaimTypes.GivenName, userInfo.FirstName),
                new(ClaimTypes.Surname, userInfo.LastName),
                new(ClaimTypes.Name, userInfo.FullName),
                new("is_active", userInfo.IsActive.ToString()),
                new("created_at", userInfo.CreatedAtUtc.ToString("O")),
                new("updated_at", userInfo.UpdatedAtUtc.ToString("O"))
            };

            // Adicionar papéis
            foreach (var role in userInfo.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Adicionar escopos
            foreach (var scope in userInfo.Scopes)
            {
                claims.Add(new Claim("scope", scope));
            }

            var identity = new ClaimsIdentity(claims, "JWT");
            var principal = new ClaimsPrincipal(identity);

            context.User = principal;

            _logger.LogDebug("Usuário autenticado: {UserId} para endpoint: {Path}", userInfo.Id, context.Request.Path);

            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro no middleware de autenticação JWT para endpoint: {Path}", context.Request.Path);
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Erro interno do servidor");
        }
    }

    /// <summary>
    /// Extrai o token JWT do header Authorization
    /// </summary>
    /// <param name="context">Contexto HTTP</param>
    /// <returns>Token JWT ou null</returns>
    private static string? ExtractTokenFromHeader(HttpContext context)
    {
        var authHeader = context.Request.Headers.Authorization.FirstOrDefault();

        if (string.IsNullOrWhiteSpace(authHeader))
            return null;

        if (!authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            return null;

        return authHeader.Substring("Bearer ".Length).Trim();
    }
}
