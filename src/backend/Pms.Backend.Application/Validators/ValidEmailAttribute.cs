using System.ComponentModel.DataAnnotations;
using Pms.Backend.Domain.Helpers;

namespace Pms.Backend.Application.Validators;

/// <summary>
/// Validation attribute for email addresses
/// </summary>
public class ValidEmailAttribute : ValidationAttribute
{
    /// <summary>
    /// Validates email format
    /// </summary>
    /// <param name="value">The value to validate</param>
    /// <param name="validationContext">The validation context</param>
    /// <returns>Validation result</returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success; // Email is optional in some contexts
        }

        var email = value.ToString();
        if (string.IsNullOrWhiteSpace(email))
        {
            return ValidationResult.Success; // Email is optional in some contexts
        }

        var errorMessage = EmailHelper.GetValidationError(email);
        if (errorMessage != null)
        {
            return new ValidationResult(errorMessage);
        }

        return ValidationResult.Success;
    }
}

