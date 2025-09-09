using System.ComponentModel.DataAnnotations;
using Pms.Backend.Domain.Helpers;

namespace Pms.Backend.Application.Validators;

/// <summary>
/// Validation attribute for name fields
/// </summary>
public class ValidNameAttribute : ValidationAttribute
{
    /// <summary>
    /// Validates name format and content
    /// </summary>
    /// <param name="value">The value to validate</param>
    /// <param name="validationContext">The validation context</param>
    /// <returns>Validation result</returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success; // Name is optional in some contexts
        }

        var name = value.ToString();
        if (string.IsNullOrWhiteSpace(name))
        {
            return ValidationResult.Success; // Name is optional in some contexts
        }

        var errorMessage = NameHelper.GetValidationError(name);
        if (errorMessage != null)
        {
            return new ValidationResult(errorMessage);
        }

        return ValidationResult.Success;
    }
}
