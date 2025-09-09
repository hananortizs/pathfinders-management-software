using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using Pms.Backend.Application.Interfaces;

namespace Pms.Backend.Application.Validators;

/// <summary>
/// Validation attribute for Contact EntityType field
/// Ensures that the provided entity type is valid and can have contacts
/// </summary>
public class ValidContactEntityTypeAttribute : ValidationAttribute
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

        // Get the validation service
        var serviceProvider = validationContext.GetService<IServiceProvider>();
        if (serviceProvider == null)
        {
            return new ValidationResult("Service provider not available for validation");
        }

        var validationService = serviceProvider.GetService<IContactValidationService>();
        if (validationService == null)
        {
            return new ValidationResult("Contact validation service not available");
        }

        // Validate entity type
        if (!validationService.ValidateEntityCanHaveContacts(entityType))
        {
            var validTypes = string.Join(", ", Domain.Enums.EntityTypeHelper.ValidEntityTypes);
            return new ValidationResult($"Invalid EntityType '{entityType}'. Valid types are: {validTypes}");
        }

        return ValidationResult.Success;
    }
}
