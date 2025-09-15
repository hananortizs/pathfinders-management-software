using Pms.Backend.Domain.Exceptions;

namespace Pms.Backend.Application.Helpers;

/// <summary>
/// Helper class for creating and throwing exceptions
/// </summary>
public static class ExceptionHelper
{
    /// <summary>
    /// Throws a validation exception
    /// </summary>
    /// <param name="fieldName">Field name</param>
    /// <param name="errorMessage">Error message</param>
    /// <param name="attemptedValue">Attempted value</param>
    /// <exception cref="ValidationException">Thrown when validation fails</exception>
    public static void ThrowValidationException(string fieldName, string errorMessage, object? attemptedValue = null)
    {
        throw new ValidationException(fieldName, errorMessage, attemptedValue);
    }

    /// <summary>
    /// Throws a validation exception with multiple errors
    /// </summary>
    /// <param name="message">General error message</param>
    /// <param name="validationErrors">List of validation errors</param>
    /// <exception cref="ValidationException">Thrown when validation fails</exception>
    public static void ThrowValidationException(string message, List<ValidationError> validationErrors)
    {
        throw new ValidationException(message, validationErrors);
    }

    /// <summary>
    /// Throws a not found exception
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <param name="id">Entity ID</param>
    /// <param name="message">Custom message</param>
    /// <exception cref="NotFoundException">Thrown when resource is not found</exception>
    public static void ThrowNotFoundException<T>(object id, string? message = null)
    {
        throw NotFoundException.ForEntity<T>(id, message);
    }

    /// <summary>
    /// Throws a not found exception
    /// </summary>
    /// <param name="resourceType">Resource type</param>
    /// <param name="resourceId">Resource ID</param>
    /// <param name="message">Custom message</param>
    /// <exception cref="NotFoundException">Thrown when resource is not found</exception>
    public static void ThrowNotFoundException(string resourceType, object? resourceId = null, string? message = null)
    {
        throw new NotFoundException(resourceType, resourceId, message);
    }

    /// <summary>
    /// Throws a business rule exception
    /// </summary>
    /// <param name="businessRule">Business rule name</param>
    /// <param name="message">Error message</param>
    /// <param name="details">Additional details</param>
    /// <exception cref="BusinessRuleException">Thrown when business rule is violated</exception>
    public static void ThrowBusinessRuleException(string businessRule, string message, Dictionary<string, object>? details = null)
    {
        throw new BusinessRuleException(businessRule, message, details);
    }

    /// <summary>
    /// Throws a duplicate exception
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <param name="fieldName">Field name</param>
    /// <param name="fieldValue">Field value</param>
    /// <param name="message">Custom message</param>
    /// <exception cref="DuplicateException">Thrown when trying to create a duplicate</exception>
    public static void ThrowDuplicateException<T>(string fieldName, object fieldValue, string? message = null)
    {
        throw DuplicateException.ForField<T>(fieldName, fieldValue, message);
    }

    /// <summary>
    /// Throws a duplicate exception
    /// </summary>
    /// <param name="resourceType">Resource type</param>
    /// <param name="fieldName">Field name</param>
    /// <param name="fieldValue">Field value</param>
    /// <param name="message">Custom message</param>
    /// <exception cref="DuplicateException">Thrown when trying to create a duplicate</exception>
    public static void ThrowDuplicateException(string resourceType, string fieldName, object? fieldValue = null, string? message = null)
    {
        throw new DuplicateException(resourceType, fieldName, fieldValue, message);
    }

    /// <summary>
    /// Throws an argument exception
    /// </summary>
    /// <param name="parameterName">Parameter name</param>
    /// <param name="message">Error message</param>
    /// <exception cref="ArgumentException">Thrown when argument is invalid</exception>
    public static void ThrowArgumentException(string parameterName, string message)
    {
        throw new ArgumentException(message, parameterName);
    }

    /// <summary>
    /// Throws an unauthorized access exception
    /// </summary>
    /// <param name="message">Error message</param>
    /// <exception cref="UnauthorizedAccessException">Thrown when access is denied</exception>
    public static void ThrowUnauthorizedException(string message = "Access denied")
    {
        throw new UnauthorizedAccessException(message);
    }

    /// <summary>
    /// Throws a not implemented exception
    /// </summary>
    /// <param name="message">Error message</param>
    /// <exception cref="NotImplementedException">Thrown when feature is not implemented</exception>
    public static void ThrowNotImplementedException(string message = "Feature not implemented")
    {
        throw new NotImplementedException(message);
    }
}
