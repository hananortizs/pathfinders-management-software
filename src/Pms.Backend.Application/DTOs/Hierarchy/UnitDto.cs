namespace Pms.Backend.Application.DTOs.Hierarchy;

/// <summary>
/// Data Transfer Object for Unit entity
/// </summary>
public class UnitDto
{
    /// <summary>
    /// Unit ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Name of the unit
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the unit
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gender of the unit (Masculina or Feminina)
    /// </summary>
    public UnitGender Gender { get; set; }

    /// <summary>
    /// Minimum age for the unit (inclusive)
    /// </summary>
    public int AgeMin { get; set; }

    /// <summary>
    /// Maximum age for the unit (inclusive)
    /// </summary>
    public int AgeMax { get; set; }

    /// <summary>
    /// Capacity of the unit (null = unlimited, >0 = limited)
    /// </summary>
    public int? Capacity { get; set; }

    /// <summary>
    /// Parent club ID
    /// </summary>
    public Guid ClubId { get; set; }

    /// <summary>
    /// Parent club
    /// </summary>
    public ClubDto? Club { get; set; }

    /// <summary>
    /// Current number of active members in this unit
    /// </summary>
    public int CurrentMemberCount { get; set; }

    /// <summary>
    /// Indicates if the unit has available capacity
    /// </summary>
    public bool HasAvailableCapacity { get; set; }

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
/// Data Transfer Object for creating a new Unit
/// </summary>
public class CreateUnitDto
{
    /// <summary>
    /// Name of the unit
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the unit
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gender of the unit (Masculina or Feminina)
    /// </summary>
    public UnitGender Gender { get; set; }

    /// <summary>
    /// Minimum age for the unit (inclusive)
    /// </summary>
    public int AgeMin { get; set; }

    /// <summary>
    /// Maximum age for the unit (inclusive)
    /// </summary>
    public int AgeMax { get; set; }

    /// <summary>
    /// Capacity of the unit (null = unlimited, >0 = limited)
    /// </summary>
    public int? Capacity { get; set; }

    /// <summary>
    /// Parent club ID
    /// </summary>
    public Guid ClubId { get; set; }
}

/// <summary>
/// Data Transfer Object for updating a Unit
/// </summary>
public class UpdateUnitDto
{
    /// <summary>
    /// Name of the unit
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the unit
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gender of the unit (Masculina or Feminina)
    /// </summary>
    public UnitGender Gender { get; set; }

    /// <summary>
    /// Minimum age for the unit (inclusive)
    /// </summary>
    public int AgeMin { get; set; }

    /// <summary>
    /// Maximum age for the unit (inclusive)
    /// </summary>
    public int AgeMax { get; set; }

    /// <summary>
    /// Capacity of the unit (null = unlimited, >0 = limited)
    /// </summary>
    public int? Capacity { get; set; }
}

/// <summary>
/// Enumeration for unit gender
/// </summary>
public enum UnitGender
{
    /// <summary>
    /// Masculine unit
    /// </summary>
    Masculina,

    /// <summary>
    /// Feminine unit
    /// </summary>
    Feminina
}
