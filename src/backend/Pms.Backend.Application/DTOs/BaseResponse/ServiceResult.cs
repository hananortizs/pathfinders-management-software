namespace Pms.Backend.Application.DTOs.BaseResponse;

/// <summary>
/// Classe genérica para resultados de serviços
/// </summary>
/// <typeparam name="T">Tipo de dados do resultado</typeparam>
public class ServiceResult<T>
{
    /// <summary>
    /// Indica se a operação foi bem-sucedida
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Dados do resultado
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Mensagem de sucesso ou erro
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Código de status HTTP
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Lista de erros de validação
    /// </summary>
    public List<string> Errors { get; set; } = new List<string>();

    /// <summary>
    /// Cria um resultado de sucesso
    /// </summary>
    public static ServiceResult<T> Success(T data, string message = "Operação realizada com sucesso")
    {
        return new ServiceResult<T>
        {
            IsSuccess = true,
            Data = data,
            Message = message,
            StatusCode = 200
        };
    }

    /// <summary>
    /// Cria um resultado de erro
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
