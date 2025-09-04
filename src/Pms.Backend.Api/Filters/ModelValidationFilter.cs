using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Pms.Backend.Application.DTOs;

namespace Pms.Backend.Api.Filters;

/// <summary>
/// Action filter that automatically validates ModelState and returns standardized error responses
/// This filter eliminates the need for manual ModelState validation in controllers
/// </summary>
public class ModelValidationFilter : IActionFilter
{
    /// <summary>
    /// Executes before the action method is invoked
    /// Automatically validates ModelState and returns standardized error response if invalid
    /// </summary>
    /// <param name="context">The action executing context</param>
    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Check if ModelState is valid
        if (!context.ModelState.IsValid)
        {
            // Create standardized error response
            var errorResponse = CreateValidationErrorResponse(context.ModelState);

            // Set the result to return the error response
            context.Result = new BadRequestObjectResult(errorResponse);
        }
    }

    /// <summary>
    /// Executes after the action method is invoked
    /// No action needed after execution
    /// </summary>
    /// <param name="context">The action executed context</param>
    public void OnActionExecuted(ActionExecutedContext context)
    {
        // No action needed after execution
    }

    /// <summary>
    /// Creates a standardized validation error response following the BaseResponse pattern
    /// </summary>
    /// <param name="modelState">The model state containing validation errors</param>
    /// <returns>Standardized error response object</returns>
    private static object CreateValidationErrorResponse(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState)
    {
        var errors = new Dictionary<string, string[]>();

        foreach (var key in modelState.Keys)
        {
            var state = modelState[key];
            if (state?.Errors != null && state.Errors.Count > 0)
            {
                errors[key] = state.Errors.Select(e => e.ErrorMessage).ToArray();
            }
        }

        return new
        {
            data = (object?)null,
            isSuccess = false,
            message = "Validation failed. Please check the provided data.",
            errors = new
            {
                validationErrors = errors,
                summary = $"Validation failed for {errors.Count} field(s)"
            },
            isDatabaseError = false
        };
    }
}
