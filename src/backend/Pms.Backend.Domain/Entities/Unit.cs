using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Domain.Entities;

/// <summary>
/// Represents a Unit within a Club
/// Units are organized by gender and age ranges
/// </summary>
public class Unit : BaseEntity
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
    /// Foreign key to the parent club
    /// </summary>
    public Guid ClubId { get; set; }

    /// <summary>
    /// Navigation property to parent club
    /// </summary>
    public Club Club { get; set; } = null!;

    /// <summary>
    /// Navigation property to memberships in this unit
    /// </summary>
    public ICollection<Membership> Memberships { get; set; } = new List<Membership>();

    /// <summary>
    /// Gets the current number of active members in this unit
    /// </summary>
    public int CurrentMemberCount => Memberships.Count(m => m.IsActive);

    /// <summary>
    /// Checks if the unit has available capacity
    /// </summary>
    public bool HasAvailableCapacity => Capacity == null || CurrentMemberCount < Capacity.Value;
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
