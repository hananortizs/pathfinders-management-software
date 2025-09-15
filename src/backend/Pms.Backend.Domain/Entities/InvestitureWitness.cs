namespace Pms.Backend.Domain.Entities;

/// <summary>
/// Represents a witness to an investiture ceremony
/// Uses hybrid model to support both structured data and legacy text entries
/// </summary>
public class InvestitureWitness : BaseEntity
{
    /// <summary>
    /// Foreign key to the investiture
    /// </summary>
    public Guid InvestitureId { get; set; }

    /// <summary>
    /// Navigation property to the investiture
    /// </summary>
    public Investiture Investiture { get; set; } = null!;

    /// <summary>
    /// Type of witness record (Structured or Text)
    /// </summary>
    public InvestitureWitnessType Type { get; set; }

    /// <summary>
    /// Foreign key to the member (when Type = Structured)
    /// </summary>
    public Guid? MemberId { get; set; }

    /// <summary>
    /// Navigation property to the member
    /// </summary>
    public Member? Member { get; set; }

    /// <summary>
    /// Role snapshot when the witness was active (when Type = Structured)
    /// </summary>
    public RoleSnapshot? RoleSnapshot { get; set; }

    /// <summary>
    /// Name of the witness (when Type = Text)
    /// </summary>
    public string? NameText { get; set; }

    /// <summary>
    /// Role of the witness (when Type = Text)
    /// </summary>
    public string? RoleText { get; set; }

    /// <summary>
    /// Organization of the witness (when Type = Text)
    /// </summary>
    public string? OrgText { get; set; }
}

/// <summary>
/// Enumeration for investiture witness types
/// </summary>
public enum InvestitureWitnessType
{
    /// <summary>
    /// Structured witness (linked to existing member/assignment)
    /// </summary>
    Structured,

    /// <summary>
    /// Text witness (legacy data or external person)
    /// </summary>
    Text
}

/// <summary>
/// Represents a snapshot of a role at a specific point in time
/// Used to capture historical role information for witnesses
/// </summary>
public class RoleSnapshot
{
    /// <summary>
    /// Role name
    /// </summary>
    public string Role { get; set; } = string.Empty;

    /// <summary>
    /// Type of scope
    /// </summary>
    public ScopeType ScopeType { get; set; }

    /// <summary>
    /// Code path of the scope
    /// </summary>
    public string ScopeCodePath { get; set; } = string.Empty;

    /// <summary>
    /// When this role was effective
    /// </summary>
    public DateTime EffectiveDate { get; set; }
}
