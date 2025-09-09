using Microsoft.Extensions.Logging;

namespace Pms.Backend.Application.Interfaces;

/// <summary>
/// Serviço centralizado de logging para toda a aplicação
/// </summary>
public interface ILoggingService
{
    /// <summary>
    /// Log de informação
    /// </summary>
    /// <param name="message">Mensagem de log</param>
    /// <param name="args">Argumentos para formatação</param>
    void LogInformation(string message, params object?[] args);

    /// <summary>
    /// Log de aviso
    /// </summary>
    /// <param name="message">Mensagem de log</param>
    /// <param name="args">Argumentos para formatação</param>
    void LogWarning(string message, params object?[] args);

    /// <summary>
    /// Log de erro
    /// </summary>
    /// <param name="exception">Exceção</param>
    /// <param name="message">Mensagem de log</param>
    /// <param name="args">Argumentos para formatação</param>
    void LogError(Exception exception, string message, params object?[] args);

    /// <summary>
    /// Log de erro sem exceção
    /// </summary>
    /// <param name="message">Mensagem de log</param>
    /// <param name="args">Argumentos para formatação</param>
    void LogError(string message, params object?[] args);

    /// <summary>
    /// Log de debug
    /// </summary>
    /// <param name="message">Mensagem de log</param>
    /// <param name="args">Argumentos para formatação</param>
    void LogDebug(string message, params object?[] args);

    /// <summary>
    /// Log de trace
    /// </summary>
    /// <param name="message">Mensagem de log</param>
    /// <param name="args">Argumentos para formatação</param>
    void LogTrace(string message, params object?[] args);
}
