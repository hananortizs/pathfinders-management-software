using System.ComponentModel.DataAnnotations;
using Pms.Backend.Domain.Helpers;

namespace Pms.Backend.Application.Validators;

/// <summary>
/// Validation attribute for CEP (Brazilian postal code)
/// </summary>
public class ValidCepAttribute : ValidationAttribute
{
    /// <summary>
    /// Validates CEP format
    /// </summary>
    /// <param name="value">The value to validate</param>
    /// <param name="validationContext">The validation context</param>
    /// <returns>Validation result</returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success; // CEP is optional
        }

        var cep = value.ToString();
        if (string.IsNullOrWhiteSpace(cep))
        {
            return ValidationResult.Success; // CEP is optional
        }

        var errorMessage = CepHelper.GetValidationError(cep);
        if (errorMessage != null)
        {
            return new ValidationResult(errorMessage);
        }

        return ValidationResult.Success;
    }
}
