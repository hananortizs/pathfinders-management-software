using Pms.Backend.Domain.Entities;
using Pms.Backend.Application.DTOs.Common;
using Pms.Backend.Application.Validators;
using System.Text.Json.Serialization;

namespace Pms.Backend.Application.DTOs.Members;

/// <summary>
/// Data Transfer Object for Member entity
/// </summary>
public class MemberDto : AddressableEntityDtoBase
{
    /// <summary>
    /// Entity type for address relationships
    /// </summary>
    public override string EntityType => "Member";

    /// <summary>
    /// Member's first name
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Member's middle names (optional) - can contain multiple middle names
    /// </summary>
    [JsonPropertyName("middleNames")]
    public string? MiddleNames { get; set; }

    /// <summary>
    /// Member's last name
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Member's social name (preferred name for identification)
    /// </summary>
    public string? SocialName { get; set; }

    /// <summary>
    /// Member's full name (computed property)
    /// </summary>
    public string FullName => $"{FirstName} {MiddleNames} {LastName}".Trim().Replace("  ", " ");

    /// <summary>
    /// Member's display name (prefers social name if available, otherwise full name)
    /// </summary>
    public string DisplayName => !string.IsNullOrWhiteSpace(SocialName) ? SocialName : FullName;

    /// <summary>
    /// Member's date of birth
    /// </summary>
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Member's gender
    /// </summary>
    public MemberGender Gender { get; set; }

    /// <summary>
    /// Member's CPF (Brazilian tax ID)
    /// </summary>
    public string? Cpf { get; set; }

    /// <summary>
    /// Member's RG (Brazilian ID)
    /// </summary>
    public string? Rg { get; set; }


    /// <summary>
    /// Member's emergency contact name
    /// </summary>
    public string? EmergencyContactName { get; set; }

    /// <summary>
    /// Member's emergency contact phone
    /// </summary>
    public string? EmergencyContactPhone { get; set; }

    /// <summary>
    /// Member's emergency contact relationship
    /// </summary>
    public string? EmergencyContactRelationship { get; set; }

    /// <summary>
    /// Member's medical information
    /// </summary>
    public string? MedicalInfo { get; set; }

    /// <summary>
    /// Member's allergies
    /// </summary>
    public string? Allergies { get; set; }

    /// <summary>
    /// Member's medications
    /// </summary>
    public string? Medications { get; set; }

    /// <summary>
    /// Member's baptism date
    /// </summary>
    public DateTime? BaptismDate { get; set; }

    /// <summary>
    /// Member's baptism church
    /// </summary>
    public string? BaptismChurch { get; set; }

    /// <summary>
    /// Member's baptism pastor
    /// </summary>
    public string? BaptismPastor { get; set; }

    /// <summary>
    /// Member's scarf date
    /// </summary>
    public DateTime? ScarfDate { get; set; }

    /// <summary>
    /// Member's scarf church
    /// </summary>
    public string? ScarfChurch { get; set; }

    /// <summary>
    /// Member's scarf pastor
    /// </summary>
    public string? ScarfPastor { get; set; }

    /// <summary>
    /// Member's status
    /// </summary>
    public MemberStatus Status { get; set; }

    /// <summary>
    /// Member's creation date
    /// </summary>
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Member's last update date
    /// </summary>
    public DateTime UpdatedAtUtc { get; set; }

    /// <summary>
    /// Member's activation date
    /// </summary>
    public DateTime? ActivatedAtUtc { get; set; }

    /// <summary>
    /// Member's deactivation date
    /// </summary>
    public DateTime? DeactivatedAtUtc { get; set; }

    /// <summary>
    /// Member's deactivation reason
    /// </summary>
    public string? DeactivationReason { get; set; }

    /// <summary>
    /// Member's primary email address (computed from contacts)
    /// </summary>
    public string? PrimaryEmail { get; set; }

    /// <summary>
    /// Member's primary phone number (computed from contacts)
    /// </summary>
    public string? PrimaryPhone { get; set; }
}
