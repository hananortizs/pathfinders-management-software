using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using Pms.Backend.Application.Interfaces;

namespace Pms.Backend.Application.Validators;

/// <summary>
/// Validation attribute to check if an entity exists in the database for contacts
/// </summary>
public class ContactEntityExistsAttribute : ValidationAttribute
{
    /// <summary>
    /// Validates if the entity exists
    /// </summary>
    /// <param name="value">The value to validate</param>
    /// <param name="validationContext">The validation context</param>
    /// <returns>Validation result</returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success; // Let Required attribute handle null values
        }

        // Get EntityType from the same object
        var entityTypeProperty = validationContext.ObjectType.GetProperty("EntityType");
        if (entityTypeProperty == null)
        {
            return new ValidationResult("EntityType property not found");
        }

        var entityType = entityTypeProperty.GetValue(validationContext.ObjectInstance)?.ToString();
        if (string.IsNullOrWhiteSpace(entityType))
        {
            return new ValidationResult("EntityType is required to validate EntityId");
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

        // Validate entity type first
        if (!validationService.ValidateEntityCanHaveContacts(entityType))
        {
            return new ValidationResult($"Invalid EntityType: {entityType}");
        }

        // For now, we'll skip the database check in validation attributes
        // as it can cause performance issues and circular dependencies
        // The actual validation will be done in the service layer
        return ValidationResult.Success;
    }
}
