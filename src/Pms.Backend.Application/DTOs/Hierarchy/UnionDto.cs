using Pms.Backend.Application.DTOs.Common;

namespace Pms.Backend.Application.DTOs.Hierarchy;

/// <summary>
/// Data Transfer Object for Union entity
/// </summary>
public class UnionDto : AddressableEntityDtoBase
{
    /// <summary>
    /// Entity type for address relationships
    /// </summary>
    public override string EntityType => "Union";

    /// <summary>
    /// Unique code for the union (≤5 chars, UPPERCASE A-Z0-9)
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Name of the union
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the union
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Parent division ID
    /// </summary>
    public Guid DivisionId { get; set; }

    // Parent division removed - avoid circular references and performance issues

    /// <summary>
    /// Code path (Division.Code.Union.Code)
    /// </summary>
    public string CodePath { get; set; } = string.Empty;

    /// <summary>
    /// Creation date
    /// </summary>
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime UpdatedAtUtc { get; set; }

    /// <summary>
    /// Child associations
    /// </summary>
    public ICollection<AssociationDto> Associations { get; set; } = new List<AssociationDto>();
}

/// <summary>
/// Data Transfer Object for creating a new Union
/// </summary>
public class CreateUnionDto
{
    /// <summary>
    /// Unique code for the union (≤5 chars, UPPERCASE A-Z0-9)
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Name of the union
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the union
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Parent division ID
    /// </summary>
    public Guid DivisionId { get; set; }
}

/// <summary>
/// Data Transfer Object for updating a Union
/// </summary>
public class UpdateUnionDto
{
    /// <summary>
    /// Unique code for the union (≤5 chars, UPPERCASE A-Z0-9)
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Name of the union
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the union
    /// </summary>
    public string? Description { get; set; }
}
