namespace Pms.Backend.Domain.Entities;

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
