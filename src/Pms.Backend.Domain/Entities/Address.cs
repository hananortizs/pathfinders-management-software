using Pms.Backend.Domain.Entities;
using Pms.Backend.Domain.Helpers;

namespace Pms.Backend.Domain.Entities;

/// <summary>
/// Represents an Address in the system
/// Can be associated with any entity (Member, Church, etc.) through polymorphic relationships
/// </summary>
public class Address : BaseEntity
{
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
    public AddressType Type { get; set; } = AddressType.Home;

    /// <summary>
    /// Indicates if this is the primary address for the entity
    /// </summary>
    public bool IsPrimary { get; set; } = false;

    /// <summary>
    /// Additional notes about the address
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// ID of the entity that owns this address (polymorphic relationship)
    /// </summary>
    public Guid EntityId { get; set; }

    /// <summary>
    /// Type of the entity that owns this address (polymorphic relationship)
    /// </summary>
    public string EntityType { get; set; } = string.Empty;

    /// <summary>
    /// Gets the CEP formatted for display (with dash)
    /// </summary>
    public string? CepFormatted => CepHelper.FormatCepForDisplay(Cep);

    /// <summary>
    /// Gets the CEP normalized for database (digits only)
    /// </summary>
    public string? CepNormalized => CepHelper.NormalizeCep(Cep);

    /// <summary>
    /// Gets the full formatted address
    /// </summary>
    public string FullAddress
    {
        get
        {
            var parts = new List<string>();
            
            if (!string.IsNullOrEmpty(Street))
                parts.Add(Street);
                
            if (!string.IsNullOrEmpty(Number))
                parts.Add($"nº {Number}");
                
            if (!string.IsNullOrEmpty(Complement))
                parts.Add(Complement);
                
            if (!string.IsNullOrEmpty(Neighborhood))
                parts.Add(Neighborhood);
                
            if (!string.IsNullOrEmpty(City))
                parts.Add(City);
                
            if (!string.IsNullOrEmpty(State))
                parts.Add(State);
                
            if (!string.IsNullOrEmpty(CepFormatted))
                parts.Add($"CEP: {CepFormatted}");
                
            if (!string.IsNullOrEmpty(Country) && Country != "Brasil")
                parts.Add(Country);

            return string.Join(", ", parts);
        }
    }
}

/// <summary>
/// Types of addresses in the system
/// </summary>
public enum AddressType
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
