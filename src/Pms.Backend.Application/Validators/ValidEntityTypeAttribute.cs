using System.ComponentModel.DataAnnotations;
using Pms.Backend.Domain.Enums;

namespace Pms.Backend.Application.Validators;

/// <summary>
/// Validation attribute for EntityType field
/// Ensures that the provided entity type is valid
/// </summary>
public class ValidEntityTypeAttribute : ValidationAttribute
{
    /// <summary>
    /// Validates the entity type value
    /// </summary>
    /// <param name="value">The value to validate</param>
    /// <param name="validationContext">The validation context</param>
    /// <returns>Validation result</returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("EntityType is required");
        }

        var entityType = value.ToString();
        if (string.IsNullOrWhiteSpace(entityType))
        {
            return new ValidationResult("EntityType cannot be empty");
        }

        if (!EntityTypeHelper.IsValidEntityType(entityType))
        {
            var validTypes = string.Join(", ", EntityTypeHelper.ValidEntityTypes);
            return new ValidationResult($"Invalid EntityType '{entityType}'. Valid types are: {validTypes}");
        }

        return ValidationResult.Success;
    }
}
