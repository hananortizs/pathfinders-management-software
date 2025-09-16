using System.Text.Json;
using Pms.Backend.Application.DTOs;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Pms.Backend.Api.Middleware;

/// <summary>
/// Middleware that standardizes all API responses to follow a consistent format
/// and handles exceptions globally
/// </summary>
public class ResponseStandardizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ResponseStandardizationMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the ResponseStandardizationMiddleware
    /// </summary>
    /// <param name="next">The next middleware in the pipeline</param>
    /// <param name="logger">Logger instance</param>
    public ResponseStandardizationMiddleware(RequestDelegate next, ILogger<ResponseStandardizationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invokes the middleware to standardize the response and handle exceptions
    /// </summary>
    /// <param name="context">The HTTP context</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        // Capture the original response stream
        var originalBodyStream = context.Response.Body;

        try
        {
            // Create a new memory stream to capture the response
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            // Continue to the next middleware
            await _next(context);

            // Reset the response body stream
            responseBody.Seek(0, SeekOrigin.Begin);

            // Read the response content
            var responseContent = await new StreamReader(responseBody).ReadToEndAsync();

            // Reset the response body stream again
            responseBody.Seek(0, SeekOrigin.Begin);

            // Standardize the response
            var standardizedResponse = await StandardizeResponseAsync(context, responseContent);

            // Write the standardized response to the original stream
            context.Response.Body = originalBodyStream;
            await context.Response.WriteAsync(standardizedResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception in API: {ExceptionType} - {Message}", ex.GetType().Name, ex.Message);

            // Reset the response body stream
            context.Response.Body = originalBodyStream;

            // Determine if it's a database error and appropriate status code
            var (isDatabaseError, statusCode, message) = DetermineErrorType(ex);

            // Create an error response
            var errorResponse = CreateStandardizedErrorResponse(message, ex.Message, isDatabaseError);
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(errorResponse);
        }
    }

    /// <summary>
    /// Standardizes the response based on the HTTP status code and content
    /// </summary>
    /// <param name="context">The HTTP context</param>
    /// <param name="responseContent">The original response content</param>
    /// <returns>Standardized response as JSON string</returns>
    private Task<string> StandardizeResponseAsync(HttpContext context, string responseContent)
    {
        var statusCode = context.Response.StatusCode;
        var contentType = context.Response.ContentType;

        // Skip standardization for certain content types
        if (ShouldSkipStandardization(contentType))
        {
            return Task.FromResult(responseContent);
        }

        // Skip standardization for Swagger/OpenAPI endpoints
        if (ShouldSkipForSwagger(context.Request.Path))
        {
            return Task.FromResult(responseContent);
        }

        try
        {
            // Try to parse the existing response as BaseResponse
            var existingResponse = JsonSerializer.Deserialize<object>(responseContent);

            // If it's already a BaseResponse, return as is
            if (IsBaseResponse(existingResponse))
            {
                return Task.FromResult(responseContent);
            }

            // For validation errors (400), return the original response as is
            if (statusCode == 400 && responseContent.Contains("ValidationProblemDetails"))
            {
                return Task.FromResult(responseContent);
            }

            // Create standardized response based on status code
            var standardizedResponse = CreateStandardizedResponse(statusCode, existingResponse, responseContent);
            return Task.FromResult(JsonSerializer.Serialize(standardizedResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = null, // Use PascalCase to match API settings
                WriteIndented = true
            }));
        }
        catch (JsonException)
        {
            // If JSON parsing fails, treat as plain text
            return Task.FromResult(CreateStandardizedResponse(statusCode, responseContent, responseContent));
        }
    }

    /// <summary>
    /// Creates a standardized response object
    /// </summary>
    /// <param name="statusCode">HTTP status code</param>
    /// <param name="data">Response data</param>
    /// <param name="originalContent">Original response content</param>
    /// <returns>Standardized response object</returns>
    private string CreateStandardizedResponse(int statusCode, object? data, string originalContent)
    {
        var isSuccess = statusCode >= 200 && statusCode < 300;

        // For 204 No Content, return empty body
        if (statusCode == 204)
        {
            return string.Empty;
        }

        object responseWithData = isSuccess
            ? new
            {
                data,
                isSuccess = true,
                message = GetSuccessMessage(statusCode)
            }
            : new
            {
                data,
                isSuccess = false,
                message = GetErrorMessage(statusCode, originalContent),
                errors = GetErrorDetails(statusCode, originalContent),
                isDatabaseError = false
            };

        return JsonSerializer.Serialize(responseWithData, new JsonSerializerOptions
        {
            PropertyNamingPolicy = null, // Use PascalCase to match API settings
            WriteIndented = true
        });
    }

    /// <summary>
    /// Creates a standardized error response for exceptions
    /// </summary>
    /// <param name="message">Error message</param>
    /// <param name="details">Error details</param>
    /// <param name="isDatabaseError">Indicates if it's a database error</param>
    /// <returns>Error response as JSON string</returns>
    private string CreateStandardizedErrorResponse(string message, string? details = null, bool isDatabaseError = false)
    {
        var errorResponse = new
        {
            data = (object?)null,
            isSuccess = false,
            message = message,
            errors = details != null ? new { details } : null,
            isDatabaseError = isDatabaseError
        };

        return JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = null, // Use PascalCase to match API settings
            WriteIndented = true
        });
    }

    /// <summary>
    /// Determines the error type and appropriate status code based on the exception
    /// </summary>
    /// <param name="ex">The exception</param>
    /// <returns>Tuple containing isDatabaseError, statusCode, and message</returns>
    private (bool isDatabaseError, int statusCode, string message) DetermineErrorType(Exception ex)
    {
                return ex switch
        {
            // Database-related exceptions
            DbUpdateException => (true, 500, "Database operation failed"),
            NpgsqlException => (true, 500, "Database operation failed"),
            TimeoutException when ex.Message.Contains("database", StringComparison.OrdinalIgnoreCase) => (true, 500, "Database operation timed out"),

            // Validation exceptions
            ArgumentNullException => (false, 400, "Required argument is missing"),
            ArgumentException => (false, 400, "Invalid argument provided"),
            InvalidOperationException => (false, 400, "Invalid operation attempted"),

            // Authentication/Authorization exceptions
            UnauthorizedAccessException => (false, 401, "Unauthorized access"),

            // Not found exceptions
            KeyNotFoundException => (false, 404, "Resource not found"),
            FileNotFoundException => (false, 404, "Resource not found"),

            // Timeout exceptions (non-database) - must come after database timeout check
            TimeoutException when !ex.Message.Contains("database", StringComparison.OrdinalIgnoreCase) => (false, 408, "Request timeout"),

            // Default case
            _ => (false, 500, "An unexpected error occurred")
        };
    }

    /// <summary>
    /// Determines if the response should skip standardization
    /// </summary>
    /// <param name="contentType">Response content type</param>
    /// <returns>True if should skip, false otherwise</returns>
    private static bool ShouldSkipStandardization(string? contentType)
    {
        if (string.IsNullOrEmpty(contentType))
            return false;

        return contentType.Contains("text/html") ||
               contentType.Contains("text/css") ||
               contentType.Contains("application/javascript") ||
               contentType.Contains("text/csv") ||
               contentType.Contains("application/octet-stream") ||
               contentType.Contains("image/") ||
               contentType.Contains("font/");
    }

    /// <summary>
    /// Determines if the request should skip standardization for Swagger endpoints
    /// </summary>
    /// <param name="path">Request path</param>
    /// <returns>True if should skip, false otherwise</returns>
    private static bool ShouldSkipForSwagger(PathString path)
    {
        var pathValue = path.Value?.ToLowerInvariant() ?? "";
        return pathValue.Contains("/swagger") ||
               pathValue.Contains("/health") ||
               pathValue.Contains("/favicon.ico");
    }

    /// <summary>
    /// Checks if the response is already a BaseResponse or StandardResponse
    /// </summary>
    /// <param name="response">Response object</param>
    /// <returns>True if is BaseResponse or StandardResponse, false otherwise</returns>
    private static bool IsBaseResponse(object? response)
    {
        if (response == null) return false;

        var responseType = response.GetType();

        // Check for BaseResponse<T>
        if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(BaseResponse<>))
            return true;

        // Check for StandardResponse
        if (responseType == typeof(StandardResponse))
            return true;

        // Check if it's a JsonElement (deserialized JSON) with BaseResponse structure
        if (response is JsonElement jsonElement)
        {
            return HasBaseResponseStructure(jsonElement);
        }

        return false;
    }

    /// <summary>
    /// Checks if a JsonElement has the structure of a BaseResponse
    /// </summary>
    /// <param name="jsonElement">JSON element to check</param>
    /// <returns>True if has BaseResponse structure, false otherwise</returns>
    private static bool HasBaseResponseStructure(JsonElement jsonElement)
    {
        if (jsonElement.ValueKind != JsonValueKind.Object)
            return false;

        // Check for required BaseResponse properties
        var hasIsSuccess = jsonElement.TryGetProperty("isSuccess", out _);
        var hasMessage = jsonElement.TryGetProperty("message", out _);
        var hasData = jsonElement.TryGetProperty("data", out _);

        // Must have at least isSuccess and message to be considered a BaseResponse
        return hasIsSuccess && hasMessage;
    }

    /// <summary>
    /// Gets success message based on status code
    /// </summary>
    /// <param name="statusCode">HTTP status code</param>
    /// <returns>Success message</returns>
    private static string GetSuccessMessage(int statusCode)
    {
        return statusCode switch
        {
            200 => "Request completed successfully",
            201 => "Resource created successfully",
            202 => "Request accepted for processing",
            204 => "Request completed successfully",
            _ => "Request completed successfully"
        };
    }

    /// <summary>
    /// Gets error message based on status code
    /// </summary>
    /// <param name="statusCode">HTTP status code</param>
    /// <param name="originalContent">Original response content</param>
    /// <returns>Error message</returns>
    private static string GetErrorMessage(int statusCode, string originalContent)
    {
        return statusCode switch
        {
            400 => "Bad request - Invalid input provided",
            401 => "Unauthorized - Authentication required",
            403 => "Forbidden - Insufficient permissions",
            404 => "Resource not found",
            409 => "Conflict - Resource already exists",
            422 => "Validation error - Invalid data provided",
            500 => "Internal server error",
            _ => "An error occurred while processing the request"
        };
    }

    /// <summary>
    /// Gets error details based on status code and content
    /// </summary>
    /// <param name="statusCode">HTTP status code</param>
    /// <param name="originalContent">Original response content</param>
    /// <returns>Error details object</returns>
    private static object? GetErrorDetails(int statusCode, string originalContent)
    {
        if (string.IsNullOrEmpty(originalContent))
            return null;

        try
        {
            // Try to parse as JSON to extract validation errors
            var parsed = JsonSerializer.Deserialize<object>(originalContent);

            // If it's already a BaseResponse-like structure, return as is
            if (parsed is JsonElement jsonElement && HasBaseResponseStructure(jsonElement))
            {
                return parsed;
            }

            return parsed;
        }
        catch
        {
            // If not JSON, return as plain text
            return new { details = originalContent };
        }
    }
}
