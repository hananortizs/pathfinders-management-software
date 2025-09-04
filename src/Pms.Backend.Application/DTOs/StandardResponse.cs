using System.Text.Json.Serialization;

namespace Pms.Backend.Application.DTOs;

/// <summary>
/// Standardized response for operations that don't return data (like 204 No Content)
/// </summary>
public class StandardResponse
{
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
    /// Creates a successful response without data
    /// </summary>
    /// <param name="message">Optional message</param>
    /// <returns>Successful response</returns>
    public static StandardResponse SuccessResult(string? message = null)
    {
        return new StandardResponse
        {
            IsSuccess = true,
            Message = message ?? "Operation completed successfully"
        };
    }

    /// <summary>
    /// Creates an error response without data
    /// </summary>
    /// <param name="message">Error message</param>
    /// <param name="errors">Error details</param>
    /// <param name="isDatabaseError">Indicates if it's a database error</param>
    /// <returns>Error response</returns>
    public static StandardResponse ErrorResult(string message, object? errors = null, bool isDatabaseError = false)
    {
        return new StandardResponse
        {
            IsSuccess = false,
            Message = message,
            Errors = errors,
            IsDatabaseError = isDatabaseError
        };
    }
}
