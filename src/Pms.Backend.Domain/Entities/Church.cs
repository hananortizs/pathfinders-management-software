namespace Pms.Backend.Domain.Entities;

/// <summary>
/// Represents a Church in the system
/// Each Club must be linked to exactly one Church
/// </summary>
public class Church : BaseEntity
{
    /// <summary>
    /// Name of the church
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Address of the church
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// City where the church is located
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// State where the church is located
    /// </summary>
    public string State { get; set; } = string.Empty;

    /// <summary>
    /// ZIP code (CEP) of the church - must be unique globally
    /// </summary>
    public string Cep { get; set; } = string.Empty;

    /// <summary>
    /// Phone number of the church
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Email of the church
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Navigation property to the club linked to this church
    /// </summary>
    public Club? Club { get; set; }
}
