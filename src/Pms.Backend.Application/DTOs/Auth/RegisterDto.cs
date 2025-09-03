using Pms.Backend.Domain.Entities;
using Pms.Backend.Application.DTOs.Members;

namespace Pms.Backend.Application.DTOs.Auth;

/// <summary>
/// Data Transfer Object for member registration
/// </summary>
public class RegisterDto
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
    /// Member's full name (computed from FirstName and LastName)
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
    /// Club ID where the member will be registered
    /// </summary>
    public Guid ClubId { get; set; }

    /// <summary>
    /// Password for the account
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Confirm password
    /// </summary>
    public string ConfirmPassword { get; set; } = string.Empty;
}

/// <summary>
/// Data Transfer Object for member login
/// </summary>
public class LoginDto
{
    /// <summary>
    /// Member's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Member's password
    /// </summary>
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// Data Transfer Object for authentication result
/// </summary>
public class AuthResultDto
{
    /// <summary>
    /// JWT access token
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Token type (Bearer)
    /// </summary>
    public string TokenType { get; set; } = "Bearer";

    /// <summary>
    /// Token expiration time in seconds
    /// </summary>
    public int ExpiresIn { get; set; }

    /// <summary>
    /// Member information
    /// </summary>
    public MemberDto Member { get; set; } = new();
}


