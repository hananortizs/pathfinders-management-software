using System.ComponentModel.DataAnnotations;
using Pms.Backend.Domain.Helpers;

namespace Pms.Backend.Application.Validators;

/// <summary>
/// Validation attribute for CPF (Brazilian tax ID)
/// </summary>
public class ValidCpfAttribute : ValidationAttribute
{
    /// <summary>
    /// Validates CPF format and check digits
    /// </summary>
    /// <param name="value">The value to validate</param>
    /// <param name="validationContext">The validation context</param>
    /// <returns>Validation result</returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success; // CPF is optional
        }

        var cpf = value.ToString();
        if (string.IsNullOrWhiteSpace(cpf))
        {
            return ValidationResult.Success; // CPF is optional
        }

        var errorMessage = CpfHelper.GetValidationError(cpf);
        if (errorMessage != null)
        {
            return new ValidationResult(errorMessage);
        }

        return ValidationResult.Success;
    }
}

