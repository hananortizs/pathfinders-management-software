using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Application.DTOs.Auth;

/// <summary>
/// Data Transfer Object for authentication request
/// </summary>
public class LoginRequestDto
{
    /// <summary>
    /// User's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// User's password
    /// </summary>
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// Data Transfer Object for authentication response
/// </summary>
public class LoginResponseDto
{
    /// <summary>
    /// JWT access token
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Token type (Bearer)
    /// </summary>
    public string TokenType { get; set; } = "Bearer";

    /// <summary>
    /// Token expiration time in seconds
    /// </summary>
    public int ExpiresIn { get; set; }

    /// <summary>
    /// User information
    /// </summary>
    public UserInfoDto User { get; set; } = new();
}

/// <summary>
/// Data Transfer Object for user information in authentication response
/// </summary>
public class UserInfoDto
{
    /// <summary>
    /// User's unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// User's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// User's first name
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// User's last name
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// User's full name
    /// </summary>
    public string FullName => $"{FirstName} {LastName}".Trim();

    /// <summary>
    /// User's roles
    /// </summary>
    public List<string> Roles { get; set; } = new();

    /// <summary>
    /// User's scopes
    /// </summary>
    public List<string> Scopes { get; set; } = new();
}

/// <summary>
/// Data Transfer Object for password change request
/// </summary>
public class ChangePasswordRequestDto
{
    /// <summary>
    /// Current password
    /// </summary>
    public string CurrentPassword { get; set; } = string.Empty;

    /// <summary>
    /// New password
    /// </summary>
    public string NewPassword { get; set; } = string.Empty;

    /// <summary>
    /// Confirm new password
    /// </summary>
    public string ConfirmPassword { get; set; } = string.Empty;
}

/// <summary>
/// Data Transfer Object for password reset request
/// </summary>
public class ResetPasswordRequestDto
{
    /// <summary>
    /// User's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;
}

/// <summary>
/// Data Transfer Object for password reset confirmation
/// </summary>
public class ResetPasswordConfirmDto
{
    /// <summary>
    /// Reset token
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// New password
    /// </summary>
    public string NewPassword { get; set; } = string.Empty;

    /// <summary>
    /// Confirm new password
    /// </summary>
    public string ConfirmPassword { get; set; } = string.Empty;
}

/// <summary>
/// Data Transfer Object for member invitation
/// </summary>
public class InviteMemberRequestDto
{
    /// <summary>
    /// Member's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Member's first name
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Member's last name
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Member's date of birth
    /// </summary>
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Member's gender
    /// </summary>
    public MemberGender Gender { get; set; }

    /// <summary>
    /// Member's phone number
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Club ID where the member will be invited
    /// </summary>
    public Guid ClubId { get; set; }
}

/// <summary>
/// Data Transfer Object for member activation
/// </summary>
public class ActivateMemberRequestDto
{
    /// <summary>
    /// Activation token
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// New password
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Confirm password
    /// </summary>
    public string ConfirmPassword { get; set; } = string.Empty;

    /// <summary>
    /// Additional member information
    /// </summary>
    public CompleteMemberInfoDto? MemberInfo { get; set; }
}

/// <summary>
/// Data Transfer Object for completing member information during activation
/// </summary>
public class CompleteMemberInfoDto
{
    /// <summary>
    /// Member's CPF (Brazilian tax ID)
    /// </summary>
    public string? Cpf { get; set; }

    /// <summary>
    /// Member's RG (Brazilian ID)
    /// </summary>
    public string? Rg { get; set; }

    /// <summary>
    /// Member's address
    /// </summary>
    // Address field removed - now using centralized Address entity

    /// <summary>
    /// Member's city
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// Member's state
    /// </summary>
    public string? State { get; set; }

    /// <summary>
    /// Member's ZIP code
    /// </summary>
    public string? ZipCode { get; set; }

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
