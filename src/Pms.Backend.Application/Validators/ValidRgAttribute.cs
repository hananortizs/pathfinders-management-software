using System.ComponentModel.DataAnnotations;
using Pms.Backend.Domain.Helpers;

namespace Pms.Backend.Application.Validators;

/// <summary>
/// Validation attribute for RG (Brazilian ID)
/// </summary>
public class ValidRgAttribute : ValidationAttribute
{
    /// <summary>
    /// Validates RG format
    /// </summary>
    /// <param name="value">The value to validate</param>
    /// <param name="validationContext">The validation context</param>
    /// <returns>Validation result</returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success; // RG is optional
        }

        var rg = value.ToString();
        if (string.IsNullOrWhiteSpace(rg))
        {
            return ValidationResult.Success; // RG is optional
        }

        var errorMessage = RgHelper.GetValidationError(rg);
        if (errorMessage != null)
        {
            return new ValidationResult(errorMessage);
        }

        return ValidationResult.Success;
    }
}

