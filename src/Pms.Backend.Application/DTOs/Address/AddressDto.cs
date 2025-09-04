using System.ComponentModel.DataAnnotations;

namespace Pms.Backend.Application.DTOs.Address;

/// <summary>
/// Data Transfer Object for Address entity
/// </summary>
public class AddressDto
{
    /// <summary>
    /// Address ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Street address (e.g., "Rua das Flores, 123")
    /// </summary>
    public string Street { get; set; } = string.Empty;

    /// <summary>
    /// Address number
    /// </summary>
    public string? Number { get; set; }

    /// <summary>
    /// Address complement (apartment, floor, etc.)
    /// </summary>
    public string? Complement { get; set; }

    /// <summary>
    /// Neighborhood or district
    /// </summary>
    public string? Neighborhood { get; set; }

    /// <summary>
    /// City where the address is located
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// State where the address is located
    /// </summary>
    public string State { get; set; } = string.Empty;

    /// <summary>
    /// Country where the address is located
    /// </summary>
    public string Country { get; set; } = "Brasil";

    /// <summary>
    /// ZIP code (CEP) - Brazilian postal code
    /// </summary>
    public string? Cep { get; set; }

    /// <summary>
    /// Type of address (Home, Work, Church, etc.)
    /// </summary>
    public AddressTypeDto Type { get; set; } = AddressTypeDto.Home;

    /// <summary>
    /// Indicates if this is the primary address for the entity
    /// </summary>
    public bool IsPrimary { get; set; } = false;

    /// <summary>
    /// Additional notes about the address
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// ID of the entity that owns this address
    /// </summary>
    public Guid EntityId { get; set; }

    /// <summary>
    /// Type of the entity that owns this address
    /// </summary>
    public string EntityType { get; set; } = string.Empty;

    /// <summary>
    /// Full formatted address
    /// </summary>
    public string FullAddress { get; set; } = string.Empty;

    /// <summary>
    /// Creation date
    /// </summary>
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime UpdatedAtUtc { get; set; }
}

/// <summary>
/// Data Transfer Object for creating a new Address
/// </summary>
public class CreateAddressDto
{
    /// <summary>
    /// Street address (e.g., "Rua das Flores, 123")
    /// </summary>
    [Required(ErrorMessage = "Street is required")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "Street must be between 5 and 200 characters")]
    public string Street { get; set; } = string.Empty;

    /// <summary>
    /// Address number
    /// </summary>
    [StringLength(10, ErrorMessage = "Number cannot exceed 10 characters")]
    public string? Number { get; set; }

    /// <summary>
    /// Address complement (apartment, floor, etc.)
    /// </summary>
    [StringLength(100, ErrorMessage = "Complement cannot exceed 100 characters")]
    public string? Complement { get; set; }

    /// <summary>
    /// Neighborhood or district
    /// </summary>
    [StringLength(100, ErrorMessage = "Neighborhood cannot exceed 100 characters")]
    public string? Neighborhood { get; set; }

    /// <summary>
    /// City where the address is located
    /// </summary>
    [Required(ErrorMessage = "City is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "City must be between 2 and 100 characters")]
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// State where the address is located
    /// </summary>
    [Required(ErrorMessage = "State is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "State must be between 2 and 50 characters")]
    public string State { get; set; } = string.Empty;

    /// <summary>
    /// Country where the address is located
    /// </summary>
    [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters")]
    public string Country { get; set; } = "Brasil";

    /// <summary>
    /// ZIP code (CEP) - Brazilian postal code
    /// </summary>
    [RegularExpression(@"^\d{5}-?\d{3}$", ErrorMessage = "CEP must be in format 12345-678 or 12345678")]
    public string? Cep { get; set; }

    /// <summary>
    /// Type of address (Home, Work, Church, etc.)
    /// </summary>
    public AddressTypeDto Type { get; set; } = AddressTypeDto.Home;

    /// <summary>
    /// Indicates if this is the primary address for the entity
    /// </summary>
    public bool IsPrimary { get; set; } = false;

    /// <summary>
    /// Additional notes about the address
    /// </summary>
    [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
    public string? Notes { get; set; }

    /// <summary>
    /// ID of the entity that owns this address
    /// </summary>
    [Required(ErrorMessage = "EntityId is required")]
    public Guid EntityId { get; set; }

    /// <summary>
    /// Type of the entity that owns this address
    /// </summary>
    [Required(ErrorMessage = "EntityType is required")]
    [StringLength(50, ErrorMessage = "EntityType cannot exceed 50 characters")]
    public string EntityType { get; set; } = string.Empty;
}

/// <summary>
/// Data Transfer Object for updating an Address
/// </summary>
public class UpdateAddressDto
{
    /// <summary>
    /// Street address (e.g., "Rua das Flores, 123")
    /// </summary>
    [Required(ErrorMessage = "Street is required")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "Street must be between 5 and 200 characters")]
    public string Street { get; set; } = string.Empty;

    /// <summary>
    /// Address number
    /// </summary>
    [StringLength(10, ErrorMessage = "Number cannot exceed 10 characters")]
    public string? Number { get; set; }

    /// <summary>
    /// Address complement (apartment, floor, etc.)
    /// </summary>
    [StringLength(100, ErrorMessage = "Complement cannot exceed 100 characters")]
    public string? Complement { get; set; }

    /// <summary>
    /// Neighborhood or district
    /// </summary>
    [StringLength(100, ErrorMessage = "Neighborhood cannot exceed 100 characters")]
    public string? Neighborhood { get; set; }

    /// <summary>
    /// City where the address is located
    /// </summary>
    [Required(ErrorMessage = "City is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "City must be between 2 and 100 characters")]
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// State where the address is located
    /// </summary>
    [Required(ErrorMessage = "State is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "State must be between 2 and 50 characters")]
    public string State { get; set; } = string.Empty;

    /// <summary>
    /// Country where the address is located
    /// </summary>
    [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters")]
    public string Country { get; set; } = "Brasil";

    /// <summary>
    /// ZIP code (CEP) - Brazilian postal code
    /// </summary>
    [RegularExpression(@"^\d{5}-?\d{3}$", ErrorMessage = "CEP must be in format 12345-678 or 12345678")]
    public string? Cep { get; set; }

    /// <summary>
    /// Type of address (Home, Work, Church, etc.)
    /// </summary>
    public AddressTypeDto Type { get; set; } = AddressTypeDto.Home;

    /// <summary>
    /// Indicates if this is the primary address for the entity
    /// </summary>
    public bool IsPrimary { get; set; } = false;

    /// <summary>
    /// Additional notes about the address
    /// </summary>
    [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
    public string? Notes { get; set; }
}

/// <summary>
/// Address type enumeration for DTOs
/// </summary>
public enum AddressTypeDto
{
    /// <summary>
    /// Home address
    /// </summary>
    Home = 1,

    /// <summary>
    /// Work address
    /// </summary>
    Work = 2,

    /// <summary>
    /// Church address
    /// </summary>
    Church = 3,

    /// <summary>
    /// School address
    /// </summary>
    School = 4,

    /// <summary>
    /// Other type of address
    /// </summary>
    Other = 5
}
