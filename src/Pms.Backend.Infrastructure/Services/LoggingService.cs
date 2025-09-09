using Microsoft.Extensions.Logging;
using Pms.Backend.Application.Interfaces;

namespace Pms.Backend.Infrastructure.Services;

/// <summary>
/// Implementação do serviço centralizado de logging
/// </summary>
public class LoggingService : ILoggingService
{
    private readonly ILogger<LoggingService> _logger;

    public LoggingService(ILogger<LoggingService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Log de informação
    /// </summary>
    public void LogInformation(string message, params object?[] args)
    {
        _logger.LogInformation(message, args);
    }

    /// <summary>
    /// Log de aviso
    /// </summary>
    public void LogWarning(string message, params object?[] args)
    {
        _logger.LogWarning(message, args);
    }

    /// <summary>
    /// Log de erro
    /// </summary>
    public void LogError(Exception exception, string message, params object?[] args)
    {
        _logger.LogError(exception, message, args);
    }

    /// <summary>
    /// Log de erro sem exceção
    /// </summary>
    public void LogError(string message, params object?[] args)
    {
        _logger.LogError(message, args);
    }

    /// <summary>
    /// Log de debug
    /// </summary>
    public void LogDebug(string message, params object?[] args)
    {
        _logger.LogDebug(message, args);
    }

    /// <summary>
    /// Log de trace
    /// </summary>
    public void LogTrace(string message, params object?[] args)
    {
        _logger.LogTrace(message, args);
    }
}
