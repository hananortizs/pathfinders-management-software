namespace Pms.Backend.Application.DTOs;

/// <summary>
/// Base response wrapper for all API responses
/// </summary>
/// <typeparam name="T">Data type</typeparam>
public class BaseResponse<T>
{
    /// <summary>
    /// Response data
    /// </summary>
    public T Data { get; set; } = default!;

    /// <summary>
    /// Success indicator
    /// </summary>
    public bool Success { get; set; } = true;

    /// <summary>
    /// Error message (if any)
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Error details (if any)
    /// </summary>
    public object? Errors { get; set; }

    /// <summary>
    /// Creates a successful response
    /// </summary>
    /// <param name="data">Response data</param>
    /// <param name="message">Optional message</param>
    /// <returns>Successful response</returns>
    public static BaseResponse<T> SuccessResult(T data, string? message = null)
    {
        return new BaseResponse<T>
        {
            Data = data,
            Success = true,
            Message = message
        };
    }

    /// <summary>
    /// Creates an error response
    /// </summary>
    /// <param name="message">Error message</param>
    /// <param name="errors">Error details</param>
    /// <returns>Error response</returns>
    public static BaseResponse<T> ErrorResult(string message, object? errors = null)
    {
        return new BaseResponse<T>
        {
            Success = false,
            Message = message,
            Errors = errors
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
    /// Response data
    /// </summary>
    public T Data { get; set; } = default!;

    /// <summary>
    /// Current page number
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Page size
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total number of items
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Total number of pages
    /// </summary>
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    /// <summary>
    /// Indicates if there is a previous page
    /// </summary>
    public bool HasPreviousPage => PageNumber > 1;

    /// <summary>
    /// Indicates if there is a next page
    /// </summary>
    public bool HasNextPage => PageNumber < TotalPages;
}
