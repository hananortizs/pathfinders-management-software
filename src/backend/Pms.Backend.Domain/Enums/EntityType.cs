namespace Pms.Backend.Domain.Enums;

/// <summary>
/// Valid entity types that can have addresses
/// </summary>
public enum EntityType
{
    /// <summary>
    /// Member entity
    /// </summary>
    Member = 1,

    /// <summary>
    /// Church entity
    /// </summary>
    Church = 2,

    /// <summary>
    /// Club entity
    /// </summary>
    Club = 3,

    /// <summary>
    /// Unit entity
    /// </summary>
    Unit = 4,

    /// <summary>
    /// District entity
    /// </summary>
    District = 5,

    /// <summary>
    /// Association entity
    /// </summary>
    Association = 6,

    /// <summary>
    /// Union entity
    /// </summary>
    Union = 7,

    /// <summary>
    /// Division entity
    /// </summary>
    Division = 8,

    /// <summary>
    /// Region entity
    /// </summary>
    Region = 9
}

/// <summary>
/// Helper class for EntityType operations
/// </summary>
public static class EntityTypeHelper
{
    /// <summary>
    /// Gets all valid entity type names
    /// </summary>
    public static readonly string[] ValidEntityTypes = Enum.GetNames<EntityType>();

    /// <summary>
    /// Gets all valid entity type values
    /// </summary>
    public static readonly EntityType[] ValidEntityTypeValues = Enum.GetValues<EntityType>();

    /// <summary>
    /// Checks if a string represents a valid entity type
    /// </summary>
    /// <param name="entityType">The entity type string to validate</param>
    /// <returns>True if valid, false otherwise</returns>
    public static bool IsValidEntityType(string entityType)
    {
        if (string.IsNullOrWhiteSpace(entityType))
            return false;

        return Enum.TryParse<EntityType>(entityType, true, out _);
    }

    /// <summary>
    /// Gets the display name for an entity type
    /// </summary>
    /// <param name="entityType">The entity type</param>
    /// <returns>Display name for the entity type</returns>
    public static string GetDisplayName(EntityType entityType)
    {
        return entityType switch
        {
            EntityType.Member => "Membro",
            EntityType.Church => "Igreja",
            EntityType.Club => "Clube",
            EntityType.Unit => "Unidade",
            EntityType.District => "Distrito",
            EntityType.Association => "Associação",
            EntityType.Union => "União",
            EntityType.Division => "Divisão",
            EntityType.Region => "Região",
            _ => entityType.ToString()
        };
    }

    /// <summary>
    /// Gets the display name for an entity type string
    /// </summary>
    /// <param name="entityType">The entity type string</param>
    /// <returns>Display name for the entity type</returns>
    public static string GetDisplayName(string entityType)
    {
        if (Enum.TryParse<EntityType>(entityType, true, out var parsedType))
        {
            return GetDisplayName(parsedType);
        }
        return entityType;
    }

    /// <summary>
    /// Gets the entity type from a string
    /// </summary>
    /// <param name="entityType">The entity type string</param>
    /// <returns>Parsed entity type or null if invalid</returns>
    public static EntityType? ParseEntityType(string entityType)
    {
        if (Enum.TryParse<EntityType>(entityType, true, out var parsedType))
        {
            return parsedType;
        }
        return null;
    }
}
