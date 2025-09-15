namespace Pms.Backend.Application.DTOs.Common;

/// <summary>
/// Base class for DTOs that can have addresses
/// Provides common functionality for address-related operations
/// </summary>
public abstract class AddressableEntityDtoBase : IAddressableEntityDto
{
    /// <summary>
    /// Unique identifier of the entity
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Entity type for address relationships
    /// Must be implemented by derived classes
    /// </summary>
    public abstract string EntityType { get; }

    /// <summary>
    /// Gets the display name for the entity type
    /// </summary>
    public virtual string EntityTypeDisplayName => GetEntityTypeDisplayName(EntityType);

    /// <summary>
    /// Gets the description for the entity type
    /// </summary>
    public virtual string EntityTypeDescription => GetEntityTypeDescription(EntityType);

    /// <summary>
    /// Gets the display name for an entity type
    /// </summary>
    /// <param name="entityType">Entity type name</param>
    /// <returns>Display name for the entity type</returns>
    private static string GetEntityTypeDisplayName(string entityType)
    {
        return entityType switch
        {
            "Member" => "Membro",
            "Church" => "Igreja",
            "Club" => "Clube",
            "Unit" => "Unidade",
            "District" => "Distrito",
            "Association" => "Associação",
            "Union" => "União",
            "Division" => "Divisão",
            "Region" => "Região",
            _ => entityType
        };
    }

    /// <summary>
    /// Gets the description for an entity type
    /// </summary>
    /// <param name="entityType">Entity type name</param>
    /// <returns>Description for the entity type</returns>
    private static string GetEntityTypeDescription(string entityType)
    {
        return entityType switch
        {
            "Member" => "Membro do clube de desbravadores",
            "Church" => "Igreja local",
            "Club" => "Clube de desbravadores",
            "Unit" => "Unidade dentro de um clube",
            "District" => "Distrito da igreja",
            "Association" => "Associação da igreja",
            "Union" => "União da igreja",
            "Division" => "Divisão da igreja",
            "Region" => "Região da igreja",
            _ => "Tipo de entidade do sistema"
        };
    }
}
