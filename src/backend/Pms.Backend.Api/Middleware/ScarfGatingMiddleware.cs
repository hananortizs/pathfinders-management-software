using Microsoft.AspNetCore.Authorization;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.Interfaces.Validation;
using Pms.Backend.Domain.Entities;
using System.Text.Json;

namespace Pms.Backend.Api.Middleware;

/// <summary>
/// Middleware para aplicar gating por lenço em endpoints específicos
/// </summary>
public class ScarfGatingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ScarfGatingMiddleware> _logger;

    /// <summary>
    /// Inicializa uma nova instância do ScarfGatingMiddleware
    /// </summary>
    /// <param name="next">Próximo middleware na pipeline</param>
    /// <param name="logger">Logger</param>
    public ScarfGatingMiddleware(RequestDelegate next, ILogger<ScarfGatingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Executa o middleware
    /// </summary>
    /// <param name="context">Contexto HTTP</param>
    /// <param name="scarfGatingService">Serviço de validação de lenço</param>
    /// <returns>Task</returns>
    public async Task InvokeAsync(HttpContext context, IScarfGatingService scarfGatingService)
    {
        // Verificar se o endpoint requer validação de lenço
        if (RequiresScarfValidation(context))
        {
            try
            {
                // Extrair memberId da rota ou do corpo da requisição
                var memberId = await ExtractMemberIdAsync(context);
                
                if (memberId.HasValue)
                {
                    // TODO: Buscar membro do banco de dados
                    // Por enquanto, vamos simular um membro para teste
                    var member = new Member
                    {
                        Id = memberId.Value,
                        ScarfInvested = false // Simular membro sem lenço para teste
                    };

                    // Validar se o membro possui lenço
                    var validationResult = scarfGatingService.ValidateScarfRequirement(member, GetOperationType(context));
                    
                    if (!validationResult.IsSuccess)
                    {
                        // Retornar erro 422 ScarfRequired
                        context.Response.StatusCode = 422;
                        context.Response.ContentType = "application/json";
                        
                        var errorResponse = new BaseResponse<bool>
                        {
                            IsSuccess = false,
                            Message = validationResult.Message,
                            Errors = new { ErrorCode = "ScarfRequired" },
                            Data = false
                        };

                        var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = null, // Use PascalCase
                            WriteIndented = true
                        });

                        await context.Response.WriteAsync(jsonResponse);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao validar gating por lenço");
                // Em caso de erro, continuar com a requisição (fail-open)
            }
        }

        await _next(context);
    }

    /// <summary>
    /// Verifica se o endpoint requer validação de lenço
    /// </summary>
    /// <param name="context">Contexto HTTP</param>
    /// <returns>True se requer validação</returns>
    private static bool RequiresScarfValidation(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLowerInvariant();
        var method = context.Request.Method.ToUpperInvariant();

        // Endpoints que requerem validação de lenço
        var protectedEndpoints = new[]
        {
            "/progress/classes/start",
            "/progress/classes/patch",
            "/progress/classes/submit",
            "/progress/classes/approve",
            "/progress/specialties/start",
            "/progress/specialties/patch",
            "/progress/specialties/submit",
            "/progress/specialties/approve",
            "/progress/masteries/start",
            "/progress/masteries/patch",
            "/progress/masteries/submit",
            "/progress/masteries/approve"
        };

        return protectedEndpoints.Any(endpoint => path?.Contains(endpoint) == true) &&
               (method == "POST" || method == "PATCH" || method == "PUT");
    }

    /// <summary>
    /// Extrai o ID do membro da requisição
    /// </summary>
    /// <param name="context">Contexto HTTP</param>
    /// <returns>ID do membro ou null</returns>
    private static async Task<Guid?> ExtractMemberIdAsync(HttpContext context)
    {
        // Tentar extrair da rota primeiro
        var routeValues = context.Request.RouteValues;
        if (routeValues.TryGetValue("memberId", out var memberIdValue) && 
            Guid.TryParse(memberIdValue?.ToString(), out var memberId))
        {
            return memberId;
        }

        // Tentar extrair do corpo da requisição para POST/PATCH
        if (context.Request.Method is "POST" or "PATCH" or "PUT")
        {
            try
            {
                context.Request.EnableBuffering();
                context.Request.Body.Position = 0;
                
                using var reader = new StreamReader(context.Request.Body);
                var body = await reader.ReadToEndAsync();
                
                if (!string.IsNullOrEmpty(body))
                {
                    var jsonDoc = JsonDocument.Parse(body);
                    if (jsonDoc.RootElement.TryGetProperty("memberId", out var memberIdElement) &&
                        Guid.TryParse(memberIdElement.GetString(), out var memberIdFromBody))
                    {
                        return memberIdFromBody;
                    }
                }
                
                context.Request.Body.Position = 0;
            }
            catch
            {
                // Ignorar erros de parsing
            }
        }

        return null;
    }

    /// <summary>
    /// Obtém o tipo de operação baseado no endpoint
    /// </summary>
    /// <param name="context">Contexto HTTP</param>
    /// <returns>Tipo de operação</returns>
    private static string GetOperationType(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLowerInvariant();
        var method = context.Request.Method.ToUpperInvariant();

        if (path?.Contains("/classes/") == true)
        {
            return method switch
            {
                "POST" when path.Contains("/start") => "iniciar classe",
                "PATCH" or "PUT" when path.Contains("/patch") => "atualizar classe",
                "POST" when path.Contains("/submit") => "submeter classe",
                "POST" when path.Contains("/approve") => "aprovar classe",
                _ => "operar classe"
            };
        }

        if (path?.Contains("/specialties/") == true)
        {
            return method switch
            {
                "POST" when path.Contains("/start") => "iniciar especialidade",
                "PATCH" or "PUT" when path.Contains("/patch") => "atualizar especialidade",
                "POST" when path.Contains("/submit") => "submeter especialidade",
                "POST" when path.Contains("/approve") => "aprovar especialidade",
                _ => "operar especialidade"
            };
        }

        if (path?.Contains("/masteries/") == true)
        {
            return method switch
            {
                "POST" when path.Contains("/start") => "iniciar mestrado",
                "PATCH" or "PUT" when path.Contains("/patch") => "atualizar mestrado",
                "POST" when path.Contains("/submit") => "submeter mestrado",
                "POST" when path.Contains("/approve") => "aprovar mestrado",
                _ => "operar mestrado"
            };
        }

        return "progresso";
    }
}
