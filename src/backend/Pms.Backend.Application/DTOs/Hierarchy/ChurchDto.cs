using Pms.Backend.Application.DTOs.Common;

namespace Pms.Backend.Application.DTOs.Hierarchy;

/// <summary>
/// Data Transfer Object for Church entity
/// </summary>
public class ChurchDto : AddressableEntityDtoBase
{
    /// <summary>
    /// Entity type for address relationships
    /// </summary>
    public override string EntityType => "Church";

    /// <summary>
    /// Name of the church
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// CEP (postal code)
    /// </summary>
    public string? Cep { get; set; }


    /// <summary>
    /// Country
    /// </summary>
    public string? Country { get; set; }


    /// <summary>
    /// Parent district ID
    /// </summary>
    public Guid DistrictId { get; set; }


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


    /// <summary>
    /// Country
    /// </summary>
    public string? Country { get; set; }


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


    /// <summary>
    /// Country
    /// </summary>
    public string? Country { get; set; }

}
