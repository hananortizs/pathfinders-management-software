using System.ComponentModel.DataAnnotations;
using Pms.Backend.Domain.Helpers;

namespace Pms.Backend.Application.Validators;

/// <summary>
/// Validation attribute for date of birth with minimum age requirement
/// </summary>
public class ValidDateOfBirthAttribute : ValidationAttribute
{
    /// <summary>
    /// Validates date of birth format and age requirements
    /// </summary>
    /// <param name="value">The value to validate</param>
    /// <param name="validationContext">The validation context</param>
    /// <returns>Validation result</returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success; // Date of birth is optional in some contexts
        }

        if (value is not DateTime dateOfBirth)
        {
            return new ValidationResult("Data de nascimento deve ser uma data válida");
        }

        var errorMessage = DateOfBirthHelper.GetValidationError(dateOfBirth);
        if (errorMessage != null)
        {
            return new ValidationResult(errorMessage);
        }

        return ValidationResult.Success;
    }
}
