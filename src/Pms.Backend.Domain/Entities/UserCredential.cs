namespace Pms.Backend.Domain.Entities;

/// <summary>
/// Represents user credentials for authentication
/// Each Member can have one UserCredential for system access
/// </summary>
public class UserCredential : BaseEntity
{
    /// <summary>
    /// Foreign key to the member
    /// </summary>
    public Guid MemberId { get; set; }

    /// <summary>
    /// Navigation property to the member
    /// </summary>
    public Member Member { get; set; } = null!;

    /// <summary>
    /// Hashed password using BCrypt/Argon2
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Salt used for password hashing
    /// </summary>
    public string Salt { get; set; } = string.Empty;

    /// <summary>
    /// Indicates if the user account is locked out
    /// </summary>
    public bool IsLockedOut { get; set; }

    /// <summary>
    /// When the account was locked out (UTC)
    /// </summary>
    public DateTime? LockedOutUntilUtc { get; set; }

    /// <summary>
    /// Number of failed login attempts
    /// </summary>
    public int FailedLoginAttempts { get; set; }

    /// <summary>
    /// When the last failed login attempt occurred (UTC)
    /// </summary>
    public DateTime? LastFailedLoginAttemptUtc { get; set; }

    /// <summary>
    /// Indicates if the user account is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// When the user was last active (UTC)
    /// </summary>
    public DateTime? LastActiveUtc { get; set; }

    /// <summary>
    /// When the user last logged in (UTC)
    /// </summary>
    public DateTime? LastLoginAtUtc { get; set; }

    /// <summary>
    /// When the account is locked until (UTC)
    /// </summary>
    public DateTime? LockedUntilUtc { get; set; }

    /// <summary>
    /// Email address for the credential
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Indicates if the email has been verified
    /// </summary>
    public bool IsEmailVerified { get; set; }

    /// <summary>
    /// Token for account activation (24h validity)
    /// </summary>
    public string? ActivationToken { get; set; }

    /// <summary>
    /// When the activation token expires (UTC)
    /// </summary>
    public DateTime? ActivationTokenExpiresUtc { get; set; }

    /// <summary>
    /// Token for password reset (24h validity)
    /// </summary>
    public string? PasswordResetToken { get; set; }

    /// <summary>
    /// When the password reset token expires (UTC)
    /// </summary>
    public DateTime? PasswordResetTokenExpiresUtc { get; set; }

    /// <summary>
    /// History of previous password hashes (max 5)
    /// </summary>
    public ICollection<PasswordHistory> PasswordHistory { get; set; } = new List<PasswordHistory>();

    /// <summary>
    /// Checks if the account is currently locked out
    /// </summary>
    public bool IsCurrentlyLockedOut => IsLockedOut && LockedOutUntilUtc.HasValue && LockedOutUntilUtc.Value > DateTime.UtcNow;

    /// <summary>
    /// Checks if the activation token is valid
    /// </summary>
    public bool IsActivationTokenValid => !string.IsNullOrEmpty(ActivationToken) && 
                                         ActivationTokenExpiresUtc.HasValue && 
                                         ActivationTokenExpiresUtc.Value > DateTime.UtcNow;

    /// <summary>
    /// Checks if the password reset token is valid
    /// </summary>
    public bool IsPasswordResetTokenValid => !string.IsNullOrEmpty(PasswordResetToken) && 
                                            PasswordResetTokenExpiresUtc.HasValue && 
                                            PasswordResetTokenExpiresUtc.Value > DateTime.UtcNow;
}

/// <summary>
/// Represents a password in the history
/// Used to prevent reuse of recent passwords
/// </summary>
public class PasswordHistory : BaseEntity
{
    /// <summary>
    /// Foreign key to the user credential
    /// </summary>
    public Guid UserCredentialId { get; set; }

    /// <summary>
    /// Navigation property to the user credential
    /// </summary>
    public UserCredential UserCredential { get; set; } = null!;

    /// <summary>
    /// Hashed password
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Salt used for hashing
    /// </summary>
    public string Salt { get; set; } = string.Empty;

    /// <summary>
    /// When this password was used (UTC)
    /// </summary>
    public DateTime UsedAtUtc { get; set; }
}
