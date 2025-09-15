using System.Net;
using System.Text.Json;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Domain.Exceptions;

namespace Pms.Backend.Api.Middleware;

/// <summary>
/// Middleware for handling exceptions globally
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;

    /// <summary>
    /// Initializes a new instance of the ExceptionHandlingMiddleware
    /// </summary>
    /// <param name="next">Next middleware in the pipeline</param>
    /// <param name="logger">Logger instance</param>
    /// <param name="environment">Host environment</param>
    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IWebHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    /// <summary>
    /// Invokes the middleware
    /// </summary>
    /// <param name="context">HTTP context</param>
    /// <returns>Task representing the async operation</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Handles exceptions and returns appropriate responses
    /// </summary>
    /// <param name="context">HTTP context</param>
    /// <param name="exception">Exception to handle</param>
    /// <returns>Task representing the async operation</returns>
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = CreateErrorResponse(exception);
        response.StatusCode = (int)errorResponse.StatusCode;

        var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });

        await response.WriteAsync(jsonResponse);
    }

    /// <summary>
    /// Creates an error response based on the exception type
    /// </summary>
    /// <param name="exception">Exception to process</param>
    /// <returns>Error response</returns>
    private ErrorResponse CreateErrorResponse(Exception exception)
    {
        return exception switch
        {
            ValidationException validationEx => CreateValidationErrorResponse(validationEx),
            NotFoundException notFoundEx => CreateNotFoundErrorResponse(notFoundEx),
            BusinessRuleException businessRuleEx => CreateBusinessRuleErrorResponse(businessRuleEx),
            DuplicateException duplicateEx => CreateDuplicateErrorResponse(duplicateEx),
            ArgumentException argEx => CreateArgumentErrorResponse(argEx),
            UnauthorizedAccessException unauthorizedEx => CreateUnauthorizedErrorResponse(unauthorizedEx),
            NotImplementedException notImplementedEx => CreateNotImplementedErrorResponse(notImplementedEx),
            _ => CreateGenericErrorResponse(exception)
        };
    }

    /// <summary>
    /// Creates a validation error response
    /// </summary>
    /// <param name="exception">Validation exception</param>
    /// <returns>Error response</returns>
    private ErrorResponse CreateValidationErrorResponse(ValidationException exception)
    {
        return new ErrorResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            ErrorCode = exception.ErrorCode,
            Message = exception.Message,
            Details = new Dictionary<string, object>
            {
                ["ValidationErrors"] = exception.ValidationErrors.Select(ve => new
                {
                    FieldName = ve.FieldName,
                    ErrorMessage = ve.ErrorMessage,
                    AttemptedValue = ve.AttemptedValue
                }).ToList()
            },
            TraceId = GetTraceId()
        };
    }

    /// <summary>
    /// Creates a not found error response
    /// </summary>
    /// <param name="exception">Not found exception</param>
    /// <returns>Error response</returns>
    private ErrorResponse CreateNotFoundErrorResponse(NotFoundException exception)
    {
        return new ErrorResponse
        {
            StatusCode = HttpStatusCode.NotFound,
            ErrorCode = exception.ErrorCode,
            Message = exception.Message,
            Details = new Dictionary<string, object>
            {
                ["ResourceType"] = exception.ResourceType,
                ["ResourceId"] = exception.ResourceId ?? "N/A"
            },
            TraceId = GetTraceId()
        };
    }

    /// <summary>
    /// Creates a business rule error response
    /// </summary>
    /// <param name="exception">Business rule exception</param>
    /// <returns>Error response</returns>
    private ErrorResponse CreateBusinessRuleErrorResponse(BusinessRuleException exception)
    {
        return new ErrorResponse
        {
            StatusCode = HttpStatusCode.UnprocessableEntity,
            ErrorCode = exception.ErrorCode,
            Message = exception.Message,
            Details = new Dictionary<string, object>
            {
                ["BusinessRule"] = exception.BusinessRule
            }.Concat(exception.Details).ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
            TraceId = GetTraceId()
        };
    }

    /// <summary>
    /// Creates a duplicate error response
    /// </summary>
    /// <param name="exception">Duplicate exception</param>
    /// <returns>Error response</returns>
    private ErrorResponse CreateDuplicateErrorResponse(DuplicateException exception)
    {
        return new ErrorResponse
        {
            StatusCode = HttpStatusCode.Conflict,
            ErrorCode = exception.ErrorCode,
            Message = exception.Message,
            Details = new Dictionary<string, object>
            {
                ["ResourceType"] = exception.ResourceType,
                ["FieldName"] = exception.FieldName,
                ["FieldValue"] = exception.FieldValue ?? "N/A"
            },
            TraceId = GetTraceId()
        };
    }

    /// <summary>
    /// Creates an argument error response
    /// </summary>
    /// <param name="exception">Argument exception</param>
    /// <returns>Error response</returns>
    private ErrorResponse CreateArgumentErrorResponse(ArgumentException exception)
    {
        return new ErrorResponse
        {
            StatusCode = HttpStatusCode.BadRequest,
            ErrorCode = "INVALID_ARGUMENT",
            Message = exception.Message,
            Details = new Dictionary<string, object>
            {
                ["ParameterName"] = exception.ParamName ?? "Unknown"
            },
            TraceId = GetTraceId()
        };
    }

    /// <summary>
    /// Creates an unauthorized error response
    /// </summary>
    /// <param name="exception">Unauthorized access exception</param>
    /// <returns>Error response</returns>
    private ErrorResponse CreateUnauthorizedErrorResponse(UnauthorizedAccessException exception)
    {
        return new ErrorResponse
        {
            StatusCode = HttpStatusCode.Unauthorized,
            ErrorCode = "UNAUTHORIZED",
            Message = "Access denied. You do not have permission to perform this action.",
            Details = new Dictionary<string, object>
            {
                ["OriginalMessage"] = exception.Message
            },
            TraceId = GetTraceId()
        };
    }

    /// <summary>
    /// Creates a not implemented error response
    /// </summary>
    /// <param name="exception">Not implemented exception</param>
    /// <returns>Error response</returns>
    private ErrorResponse CreateNotImplementedErrorResponse(NotImplementedException exception)
    {
        return new ErrorResponse
        {
            StatusCode = HttpStatusCode.NotImplemented,
            ErrorCode = "NOT_IMPLEMENTED",
            Message = "This feature is not yet implemented.",
            Details = new Dictionary<string, object>
            {
                ["OriginalMessage"] = exception.Message
            },
            TraceId = GetTraceId()
        };
    }

    /// <summary>
    /// Creates a generic error response
    /// </summary>
    /// <param name="exception">Generic exception</param>
    /// <returns>Error response</returns>
    private ErrorResponse CreateGenericErrorResponse(Exception exception)
    {
        return new ErrorResponse
        {
            StatusCode = HttpStatusCode.InternalServerError,
            ErrorCode = "INTERNAL_SERVER_ERROR",
            Message = _environment.IsDevelopment() 
                ? exception.Message 
                : "An internal server error occurred. Please try again later.",
            Details = _environment.IsDevelopment() 
                ? new Dictionary<string, object>
                {
                    ["ExceptionType"] = exception.GetType().Name,
                    ["StackTrace"] = exception.StackTrace ?? "No stack trace available"
                }
                : new Dictionary<string, object>(),
            TraceId = GetTraceId()
        };
    }

    /// <summary>
    /// Gets the trace ID from the current context
    /// </summary>
    /// <returns>Trace ID or null if not available</returns>
    private string? GetTraceId()
    {
        return System.Diagnostics.Activity.Current?.Id ?? 
               System.Diagnostics.Activity.Current?.RootId;
    }
}

/// <summary>
/// Error response model
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// HTTP status code
    /// </summary>
    public HttpStatusCode StatusCode { get; set; }

    /// <summary>
    /// Error code
    /// </summary>
    public string ErrorCode { get; set; } = string.Empty;

    /// <summary>
    /// Error message
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Additional error details
    /// </summary>
    public Dictionary<string, object> Details { get; set; } = new();

    /// <summary>
    /// Trace ID for correlation
    /// </summary>
    public string? TraceId { get; set; }

    /// <summary>
    /// Timestamp when the error occurred
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
