using System.ComponentModel.DataAnnotations;

namespace Pms.Backend.Domain.Exceptions;

/// <summary>
/// Exception thrown when validation fails
/// </summary>
public class ValidationException : BaseException
{
    /// <summary>
    /// Validation errors
    /// </summary>
    public List<ValidationError> ValidationErrors { get; }

    /// <summary>
    /// Initializes a new instance of the ValidationException
    /// </summary>
    /// <param name="message">Error message</param>
    /// <param name="validationErrors">Validation errors</param>
    /// <param name="innerException">Inner exception</param>
    public ValidationException(string message, List<ValidationError> validationErrors, Exception? innerException = null)
        : base("VALIDATION_ERROR", message, innerException)
    {
        ValidationErrors = validationErrors ?? new List<ValidationError>();
        AddDetail("ValidationErrors", ValidationErrors);
    }

    /// <summary>
    /// Initializes a new instance of the ValidationException with a single validation error
    /// </summary>
    /// <param name="fieldName">Field name</param>
    /// <param name="errorMessage">Error message</param>
    /// <param name="attemptedValue">Attempted value</param>
    public ValidationException(string fieldName, string errorMessage, object? attemptedValue = null)
        : this($"Validation failed for field '{fieldName}'", 
               new List<ValidationError> { new(fieldName, errorMessage, attemptedValue) })
    {
    }
}

/// <summary>
/// Represents a validation error
/// </summary>
public class ValidationError
{
    /// <summary>
    /// Field name that failed validation
    /// </summary>
    public string FieldName { get; }

    /// <summary>
    /// Error message
    /// </summary>
    public string ErrorMessage { get; }

    /// <summary>
    /// Value that was attempted
    /// </summary>
    public object? AttemptedValue { get; }

    /// <summary>
    /// Initializes a new instance of the ValidationError
    /// </summary>
    /// <param name="fieldName">Field name</param>
    /// <param name="errorMessage">Error message</param>
    /// <param name="attemptedValue">Attempted value</param>
    public ValidationError(string fieldName, string errorMessage, object? attemptedValue = null)
    {
        FieldName = fieldName;
        ErrorMessage = errorMessage;
        AttemptedValue = attemptedValue;
    }
}
