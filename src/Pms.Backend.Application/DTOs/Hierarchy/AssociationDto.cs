using Pms.Backend.Application.DTOs.Common;

namespace Pms.Backend.Application.DTOs.Hierarchy;

/// <summary>
/// Data Transfer Object for Association entity
/// </summary>
public class AssociationDto : AddressableEntityDtoBase
{
    /// <summary>
    /// Entity type for address relationships
    /// </summary>
    public override string EntityType => "Association";

    /// <summary>
    /// Unique code for the association (≤5 chars, UPPERCASE A-Z0-9)
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Name of the association
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the association
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Parent union ID
    /// </summary>
    public Guid UnionId { get; set; }

    // Parent union removed - avoid circular references and performance issues

    /// <summary>
    /// Code path (Division.Code.Union.Code.Association.Code)
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
    /// Child regions
    /// </summary>
    public ICollection<RegionDto> Regions { get; set; } = new List<RegionDto>();
}

/// <summary>
/// Data Transfer Object for creating a new Association
/// </summary>
public class CreateAssociationDto
{
    /// <summary>
    /// Unique code for the association (≤5 chars, UPPERCASE A-Z0-9)
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Name of the association
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the association
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Parent union ID
    /// </summary>
    public Guid UnionId { get; set; }
}

/// <summary>
/// Data Transfer Object for updating an Association
/// </summary>
public class UpdateAssociationDto
{
    /// <summary>
    /// Unique code for the association (≤5 chars, UPPERCASE A-Z0-9)
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Name of the association
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the association
    /// </summary>
    public string? Description { get; set; }
}
