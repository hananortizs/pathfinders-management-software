using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Pms.Backend.Api.Middleware;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Application.Services.Auth;
using System.Text;

namespace Pms.Backend.Api.Extensions;

/// <summary>
/// Extensões para configuração de autenticação
/// </summary>
public static class AuthenticationExtensions
{
    /// <summary>
    /// Adiciona serviços de autenticação JWT
    /// </summary>
    /// <param name="services">Coleção de serviços</param>
    /// <param name="configuration">Configuração da aplicação</param>
    /// <returns>Coleção de serviços</returns>
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        // Configurar JWT
        var jwtSettings = configuration.GetSection("Jwt");
        var secretKey = jwtSettings["SecretKey"]!;
        var issuer = jwtSettings["Issuer"]!;
        var audience = jwtSettings["Audience"]!;

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            
            // Configurar eventos para ignorar endpoints públicos
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var path = context.HttpContext.Request.Path.Value?.ToLowerInvariant();
                    var isPublicEndpoint = path != null && (
                        path.Contains("auth") ||
                        path.Contains("health") ||
                        path.Contains("swagger")
                    );
                    
                    if (isPublicEndpoint)
                    {
                        // Não processar JWT para endpoints públicos
                        return Task.CompletedTask;
                    }
                    
                    return Task.CompletedTask;
                }
            };
        });

        // Registrar serviços
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }

    /// <summary>
    /// Adiciona middleware de autenticação JWT personalizado
    /// </summary>
    /// <param name="app">Builder da aplicação</param>
    /// <returns>Builder da aplicação</returns>
    public static IApplicationBuilder UseJwtAuthentication(this IApplicationBuilder app)
    {
        return app.UseMiddleware<JwtAuthenticationMiddleware>();
    }
}
