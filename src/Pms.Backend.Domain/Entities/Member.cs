namespace Pms.Backend.Domain.Entities;

/// <summary>
/// Represents a Member in the system
/// Members are the core entities that participate in clubs and activities
/// </summary>
public class Member : BaseEntity
{
    /// <summary>
    /// First name of the member
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Last name of the member
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Full name of the member (computed property)
    /// </summary>
    public string FullName => $"{FirstName} {LastName}".Trim();

    /// <summary>
    /// Email address of the member (must be unique globally)
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Phone number of the member
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Gender of the member
    /// </summary>
    public MemberGender Gender { get; set; }

    /// <summary>
    /// Date of birth of the member
    /// </summary>
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Address of the member
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// City where the member lives
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// State where the member lives
    /// </summary>
    public string State { get; set; } = string.Empty;

    /// <summary>
    /// ZIP code (CEP) of the member
    /// </summary>
    public string ZipCode { get; set; } = string.Empty;

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
    public MemberStatus Status { get; set; } = MemberStatus.Pending;

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
    /// Indicates if the member has been baptized
    /// </summary>
    public bool Baptized { get; set; }

    /// <summary>
    /// Date when the member was baptized (if baptized)
    /// </summary>
    public DateTime? BaptizedAt { get; set; }

    /// <summary>
    /// Place where the member was baptized (if baptized)
    /// </summary>
    public string? BaptizedPlace { get; set; }

    /// <summary>
    /// Indicates if the member has received the scarf (lenço)
    /// </summary>
    public bool ScarfInvested { get; set; }

    /// <summary>
    /// Date when the member received the scarf (if received)
    /// </summary>
    public DateTime? ScarfInvestedAt { get; set; }

    /// <summary>
    /// Navigation property to user credentials (1:1 relationship)
    /// </summary>
    public UserCredential? UserCredential { get; set; }

    /// <summary>
    /// Navigation property to memberships
    /// </summary>
    public ICollection<Membership> Memberships { get; set; } = new List<Membership>();

    /// <summary>
    /// Navigation property to assignments (roles)
    /// </summary>
    public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

    /// <summary>
    /// Navigation property to timeline entries
    /// </summary>
    public ICollection<TimelineEntry> TimelineEntries { get; set; } = new List<TimelineEntry>();

    /// <summary>
    /// Navigation property to event participations
    /// </summary>
    public ICollection<MemberEventParticipation> EventParticipations { get; set; } = new List<MemberEventParticipation>();

    /// <summary>
    /// Navigation property to investitures
    /// </summary>
    public ICollection<Investiture> Investitures { get; set; } = new List<Investiture>();

    /// <summary>
    /// Navigation property to investiture witnesses
    /// </summary>
    public ICollection<InvestitureWitness> InvestitureWitnesses { get; set; } = new List<InvestitureWitness>();

    /// <summary>
    /// Gets the current age of the member
    /// </summary>
    public int CurrentAge => DateTime.UtcNow.Year - DateOfBirth.Year - (DateTime.UtcNow.DayOfYear < DateOfBirth.DayOfYear ? 1 : 0);

    /// <summary>
    /// Gets the age of the member on June 1st of the specified year
    /// This is used for the "1º de junho" rule
    /// </summary>
    /// <param name="year">The year to calculate the age for</param>
    /// <returns>The age on June 1st of the specified year</returns>
    public int GetAgeOnJuneFirst(int year)
    {
        var juneFirst = new DateTime(year, 6, 1);
        return juneFirst.Year - DateOfBirth.Year - (juneFirst.DayOfYear < DateOfBirth.DayOfYear ? 1 : 0);
    }

    /// <summary>
    /// Checks if the member is eligible to be registered (≥10 years old)
    /// </summary>
    public bool IsEligibleForRegistration => CurrentAge >= 10;
}

/// <summary>
/// Enumeration for member gender
/// </summary>
public enum MemberGender
{
    /// <summary>
    /// Masculine
    /// </summary>
    Masculino,

    /// <summary>
    /// Feminine
    /// </summary>
    Feminino
}

/// <summary>
/// Enumeration for member status
/// </summary>
public enum MemberStatus
{
    /// <summary>
    /// Member is pending activation
    /// </summary>
    Pending,

    /// <summary>
    /// Member is active
    /// </summary>
    Active,

    /// <summary>
    /// Member is inactive
    /// </summary>
    Inactive,

    /// <summary>
    /// Member is suspended
    /// </summary>
    Suspended
}
