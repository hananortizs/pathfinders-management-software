using System.ComponentModel.DataAnnotations;
using Pms.Backend.Domain.Helpers;

namespace Pms.Backend.Application.Validators;

/// <summary>
/// Validation attribute for phone numbers
/// </summary>
public class ValidPhoneAttribute : ValidationAttribute
{
    /// <summary>
    /// Validates phone format
    /// </summary>
    /// <param name="value">The value to validate</param>
    /// <param name="validationContext">The validation context</param>
    /// <returns>Validation result</returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success; // Phone is optional
        }

        var phone = value.ToString();
        if (string.IsNullOrWhiteSpace(phone))
        {
            return ValidationResult.Success; // Phone is optional
        }

        var errorMessage = PhoneHelper.GetValidationError(phone);
        if (errorMessage != null)
        {
            return new ValidationResult(errorMessage);
        }

        return ValidationResult.Success;
    }
}

