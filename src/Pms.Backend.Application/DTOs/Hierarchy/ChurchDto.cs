namespace Pms.Backend.Application.DTOs.Hierarchy;

/// <summary>
/// Data Transfer Object for Church entity
/// </summary>
public class ChurchDto
{
    /// <summary>
    /// Church ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Name of the church
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// CEP (postal code)
    /// </summary>
    public string? Cep { get; set; }

    // Address fields removed - now using centralized Address entity

    /// <summary>
    /// Country
    /// </summary>
    public string? Country { get; set; }

    /// <summary>
    /// Phone number
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Parent district ID
    /// </summary>
    public Guid DistrictId { get; set; }

    /// <summary>
    /// Parent district
    /// </summary>
    public DistrictDto? District { get; set; }

    /// <summary>
    /// Creation date
    /// </summary>
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime UpdatedAtUtc { get; set; }

    /// <summary>
    /// Associated club (1:1 relationship)
    /// </summary>
    public ClubDto? Club { get; set; }
}

/// <summary>
/// Data Transfer Object for creating a new Church
/// </summary>
public class CreateChurchDto
{
    /// <summary>
    /// Name of the church
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// CEP (postal code)
    /// </summary>
    public string? Cep { get; set; }

    // Address fields removed - now using centralized Address entity

    /// <summary>
    /// Country
    /// </summary>
    public string? Country { get; set; }

    /// <summary>
    /// Phone number
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Parent district ID
    /// </summary>
    public Guid DistrictId { get; set; }
}

/// <summary>
/// Data Transfer Object for updating a Church
/// </summary>
public class UpdateChurchDto
{
    /// <summary>
    /// Name of the church
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// CEP (postal code)
    /// </summary>
    public string? Cep { get; set; }

    // Address fields removed - now using centralized Address entity

    /// <summary>
    /// Country
    /// </summary>
    public string? Country { get; set; }

    /// <summary>
    /// Phone number
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public string? Email { get; set; }
}
