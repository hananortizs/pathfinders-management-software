using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Application.DTOs.Members;

/// <summary>
/// Data Transfer Object for Member entity
/// </summary>
public class MemberDto
{
    /// <summary>
    /// Unique identifier of the member
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Member's first name
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Member's last name
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Member's full name (computed property)
    /// </summary>
    public string FullName => $"{FirstName} {LastName}".Trim();

    /// <summary>
    /// Member's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Member's phone number
    /// </summary>
    public string? Phone { get; set; }

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

    // Address fields removed - now using centralized Address entity

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
}

/// <summary>
/// Data Transfer Object for creating a new Member
/// </summary>
public class CreateMemberDto
{
    /// <summary>
    /// Member's first name
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Member's last name
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Member's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Member's phone number
    /// </summary>
    public string? Phone { get; set; }

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

    // Address fields removed - now using centralized Address entity

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
}

/// <summary>
/// Data Transfer Object for updating an existing Member
/// </summary>
public class UpdateMemberDto
{
    /// <summary>
    /// Member's first name
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Member's last name
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Member's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Member's phone number
    /// </summary>
    public string? Phone { get; set; }

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

    // Address fields removed - now using centralized Address entity

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
}
