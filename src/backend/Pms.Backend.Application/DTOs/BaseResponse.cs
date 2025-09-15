using System.Text.Json.Serialization;

namespace Pms.Backend.Application.DTOs;

/// <summary>
/// Base response wrapper for all API responses
/// </summary>
/// <typeparam name="T">Data type</typeparam>
public class BaseResponse<T>
{
    /// <summary>
    /// Response data encapsulated in 'data' field
    /// </summary>
    [JsonPropertyName("data")]
    public T? Data { get; set; }

    /// <summary>
    /// Success indicator - indicates if the operation was successful
    /// </summary>
    [JsonPropertyName("isSuccess")]
    public bool IsSuccess { get; set; } = true;

    /// <summary>
    /// Response message
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    /// <summary>
    /// Error details (if any)
    /// </summary>
    [JsonPropertyName("errors")]
    public object? Errors { get; set; }

    /// <summary>
    /// Indicates if the error is related to database operations
    /// </summary>
    [JsonPropertyName("isDatabaseError")]
    public bool IsDatabaseError { get; set; } = false;

    /// <summary>
    /// HTTP status code for the response
    /// </summary>
    [JsonPropertyName("statusCode")]
    public int StatusCode { get; set; } = 200;

    /// <summary>
    /// Creates a successful response
    /// </summary>
    /// <param name="data">Response data</param>
    /// <param name="message">Optional message</param>
    /// <param name="statusCode">HTTP status code (default: 200)</param>
    /// <returns>Successful response</returns>
    public static BaseResponse<T> SuccessResult(T? data = default, string? message = null, int statusCode = 200)
    {
        return new BaseResponse<T>
        {
            Data = data,
            IsSuccess = true,
            Message = message ?? "Operation completed successfully",
            StatusCode = statusCode
        };
    }

    /// <summary>
    /// Creates an error response
    /// </summary>
    /// <param name="message">Error message</param>
    /// <param name="errors">Error details</param>
    /// <param name="isDatabaseError">Indicates if it's a database error</param>
    /// <param name="statusCode">HTTP status code (default: 400)</param>
    /// <returns>Error response</returns>
    public static BaseResponse<T> ErrorResult(string message, object? errors = null, bool isDatabaseError = false, int statusCode = 400)
    {
        return new BaseResponse<T>
        {
            IsSuccess = false,
            Message = message,
            Errors = errors,
            IsDatabaseError = isDatabaseError,
            StatusCode = statusCode
        };
    }

    /// <summary>
    /// Creates a 201 Created response
    /// </summary>
    /// <param name="data">Response data</param>
    /// <param name="message">Optional message</param>
    /// <returns>Created response</returns>
    public static BaseResponse<T> CreatedResult(T? data = default, string? message = null)
    {
        return SuccessResult(data, message ?? "Resource created successfully", 201);
    }

    /// <summary>
    /// Creates a 404 Not Found response
    /// </summary>
    /// <param name="message">Error message</param>
    /// <returns>Not Found response</returns>
    public static BaseResponse<T> NotFoundResult(string message = "Resource not found")
    {
        return ErrorResult(message, statusCode: 404);
    }

    /// <summary>
    /// Creates a 401 Unauthorized response
    /// </summary>
    /// <param name="message">Error message</param>
    /// <returns>Unauthorized response</returns>
    public static BaseResponse<T> UnauthorizedResult(string message = "Unauthorized access")
    {
        return ErrorResult(message, statusCode: 401);
    }

    /// <summary>
    /// Creates a 403 Forbidden response
    /// </summary>
    /// <param name="message">Error message</param>
    /// <returns>Forbidden response</returns>
    public static BaseResponse<T> ForbiddenResult(string message = "Access forbidden")
    {
        return ErrorResult(message, statusCode: 403);
    }

    /// <summary>
    /// Creates a 422 Unprocessable Entity response
    /// </summary>
    /// <param name="message">Error message</param>
    /// <param name="errors">Validation errors</param>
    /// <returns>Unprocessable Entity response</returns>
    public static BaseResponse<T> UnprocessableEntityResult(string message = "Validation failed", object? errors = null)
    {
        return ErrorResult(message, errors, statusCode: 422);
    }

    /// <summary>
    /// Creates a 500 Internal Server Error response
    /// </summary>
    /// <param name="message">Error message</param>
    /// <param name="isDatabaseError">Indicates if it's a database error</param>
    /// <returns>Internal Server Error response</returns>
    public static BaseResponse<T> InternalServerErrorResult(string message = "An internal error occurred", bool isDatabaseError = false)
    {
        return ErrorResult(message, isDatabaseError: isDatabaseError, statusCode: 500);
    }

    /// <summary>
    /// Creates a member pending error response (403)
    /// </summary>
    /// <param name="message">Error message</param>
    /// <returns>Member pending error response</returns>
    public static BaseResponse<T> MemberPendingResult(string message = "Membro com status Pending não pode realizar esta operação")
    {
        return new BaseResponse<T>
        {
            IsSuccess = false,
            Message = message,
            StatusCode = 403,
            Errors = new { ErrorCode = "MemberPending" }
        };
    }

    /// <summary>
    /// Creates a scarf required error response (422)
    /// </summary>
    /// <param name="message">Error message</param>
    /// <returns>Scarf required error response</returns>
    public static BaseResponse<T> ScarfRequiredResult(string message = "Investidura do lenço é obrigatória para esta operação")
    {
        return new BaseResponse<T>
        {
            IsSuccess = false,
            Message = message,
            StatusCode = 422,
            Errors = new { ErrorCode = "ScarfRequired" }
        };
    }

    /// <summary>
    /// Creates a spiritual requirements not met error response (422)
    /// </summary>
    /// <param name="message">Error message</param>
    /// <returns>Spiritual requirements error response</returns>
    public static BaseResponse<T> RequisitosEspirituaisNaoAtendidosResult(string message = "Requisitos espirituais não atendidos para esta operação")
    {
        return new BaseResponse<T>
        {
            IsSuccess = false,
            Message = message,
            StatusCode = 422,
            Errors = new { ErrorCode = "RequisitosEspirituaisNaoAtendidos" }
        };
    }
}

/// <summary>
/// Paginated response wrapper
/// </summary>
/// <typeparam name="T">Data type</typeparam>
public class PaginatedResponse<T>
{
    /// <summary>
    /// Response data (items list)
    /// </summary>
    [JsonPropertyName("items")]
    public T Items { get; set; } = default!;

    /// <summary>
    /// Current page number
    /// </summary>
    [JsonPropertyName("pageNumber")]
    public int PageNumber { get; set; }

    /// <summary>
    /// Page size
    /// </summary>
    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    /// <summary>
    /// Total number of items
    /// </summary>
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    /// <summary>
    /// Total number of pages
    /// </summary>
    [JsonPropertyName("totalPages")]
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    /// <summary>
    /// Indicates if there is a previous page
    /// </summary>
    [JsonPropertyName("hasPreviousPage")]
    public bool HasPreviousPage => PageNumber > 1;

    /// <summary>
    /// Indicates if there is a next page
    /// </summary>
    [JsonPropertyName("hasNextPage")]
    public bool HasNextPage => PageNumber < TotalPages;
}
