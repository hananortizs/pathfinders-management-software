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
    /// Creates a successful response
    /// </summary>
    /// <param name="data">Response data</param>
    /// <param name="message">Optional message</param>
    /// <returns>Successful response</returns>
    public static BaseResponse<T> SuccessResult(T? data = default, string? message = null)
    {
        return new BaseResponse<T>
        {
            Data = data,
            IsSuccess = true,
            Message = message ?? "Operation completed successfully"
        };
    }

    /// <summary>
    /// Creates an error response
    /// </summary>
    /// <param name="message">Error message</param>
    /// <param name="errors">Error details</param>
    /// <param name="isDatabaseError">Indicates if it's a database error</param>
    /// <returns>Error response</returns>
    public static BaseResponse<T> ErrorResult(string message, object? errors = null, bool isDatabaseError = false)
    {
        return new BaseResponse<T>
        {
            IsSuccess = false,
            Message = message,
            Errors = errors,
            IsDatabaseError = isDatabaseError
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
