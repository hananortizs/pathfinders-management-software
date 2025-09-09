using Pms.Backend.Application.DTOs.Common;

namespace Pms.Backend.Application.DTOs.Hierarchy;

/// <summary>
/// Data Transfer Object for Division entity
/// </summary>
public class DivisionDto : AddressableEntityDtoBase
{
    /// <summary>
    /// Entity type for address relationships
    /// </summary>
    public override string EntityType => "Division";

    /// <summary>
    /// Unique code for the division (≤5 chars, UPPERCASE A-Z0-9)
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Name of the division
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the division
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Creation date
    /// </summary>
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime UpdatedAtUtc { get; set; }

    /// <summary>
    /// Child unions
    /// </summary>
    public ICollection<UnionDto> Unions { get; set; } = new List<UnionDto>();
}

/// <summary>
/// Data Transfer Object for creating a new Division
/// </summary>
public class CreateDivisionDto
{
    /// <summary>
    /// Unique code for the division (≤5 chars, UPPERCASE A-Z0-9)
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Name of the division
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the division
    /// </summary>
    public string? Description { get; set; }
}

/// <summary>
/// Data Transfer Object for updating a Division
/// </summary>
public class UpdateDivisionDto
{
    /// <summary>
    /// Unique code for the division (≤5 chars, UPPERCASE A-Z0-9)
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Name of the division
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the division
    /// </summary>
    public string? Description { get; set; }
}
