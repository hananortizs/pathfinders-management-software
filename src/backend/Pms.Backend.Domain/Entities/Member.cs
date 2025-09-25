using System.Text.Json.Serialization;
using Pms.Backend.Domain.Enums;
using Pms.Backend.Domain.Helpers;

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
    /// Middle names of the member (optional) - can contain multiple middle names
    /// </summary>
    public string? MiddleNames { get; set; }

    /// <summary>
    /// Last name of the member
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Social name of the member (preferred name for identification)
    /// </summary>
    public string? SocialName { get; set; }

    /// <summary>
    /// Full name of the member (computed property)
    /// </summary>
    public string FullName => NameHelper.CombineFullName(FirstName, MiddleNames, LastName);

    /// <summary>
    /// Display name of the member (prefers social name if available, otherwise full name)
    /// </summary>
    public string DisplayName => !string.IsNullOrWhiteSpace(SocialName) ? SocialName : FullName;

    /// <summary>
    /// Gender of the member
    /// </summary>
    public MemberGender Gender { get; set; }

    /// <summary>
    /// Date of birth of the member
    /// </summary>
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Alias for DateOfBirth for compatibility
    /// </summary>
    public DateTime BirthDate => DateOfBirth;

    // Address fields removed - now using centralized Address entity

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
    /// Gets the emergency contact phone formatted for display
    /// </summary>
    public string? EmergencyContactPhoneFormatted => PhoneHelper.FormatPhoneForDisplay(EmergencyContactPhone);

    /// <summary>
    /// Gets the CPF formatted for display
    /// </summary>
    public string? CpfFormatted => CpfHelper.FormatCpfForDisplay(Cpf);

    /// <summary>
    /// Gets the RG formatted for display
    /// </summary>
    public string? RgFormatted => RgHelper.FormatRgForDisplay(Rg);

    /// <summary>
    /// Gets the first name normalized for display
    /// </summary>
    public string? FirstNameFormatted => NameHelper.NormalizeName(FirstName);

    /// <summary>
    /// Gets the middle names normalized for display
    /// </summary>
    public string? MiddleNamesFormatted => NameHelper.NormalizeName(MiddleNames);

    /// <summary>
    /// Gets the last name normalized for display
    /// </summary>
    public string? LastNameFormatted => NameHelper.NormalizeName(LastName);

    /// <summary>
    /// Gets the social name normalized for display
    /// </summary>
    public string? SocialNameFormatted => NameHelper.NormalizeName(SocialName);

    /// <summary>
    /// Gets the full name normalized for display
    /// </summary>
    public string? FullNameFormatted => NameHelper.NormalizeName(FullName);

    /// <summary>
    /// Gets the display name normalized for display
    /// </summary>
    public string? DisplayNameFormatted => NameHelper.NormalizeName(DisplayName);

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
    /// Navigation property to contacts
    /// </summary>
    public ICollection<Contact> Contacts { get; set; } = new List<Contact>();

    /// <summary>
    /// Navigation property to addresses
    /// </summary>
    public ICollection<Address> Addresses { get; set; } = new List<Address>();

    /// <summary>
    /// Navigation property to baptism records
    /// </summary>
    public ICollection<BaptismRecord> BaptismRecords { get; set; } = new List<BaptismRecord>();

    /// <summary>
    /// Navigation property to discipline records
    /// </summary>
    public ICollection<DisciplineRecord> DisciplineRecords { get; set; } = new List<DisciplineRecord>();

    /// <summary>
    /// Medical record for this member (1:1 relationship)
    /// </summary>
    public virtual MedicalRecord? MedicalRecord { get; set; }

    // Propriedades computadas para batismo e disciplina

    /// <summary>
    /// Verifica se o membro foi batizado (possui registros de batismo)
    /// </summary>
    [JsonIgnore]
    public bool BaptizedComputed =>
        BaptismRecords.Any(b => !b.IsDeleted);

    /// <summary>
    /// Data do último batismo
    /// </summary>
    [JsonIgnore]
    public DateTime? LastBaptismAtComputed =>
        BaptismRecords.Where(b => !b.IsDeleted)
                      .OrderByDescending(b => b.Date)
                      .FirstOrDefault()?.Date;

    /// <summary>
    /// Data da última remoção da igreja
    /// </summary>
    [JsonIgnore]
    public DateTime? LastRemovalAtComputed =>
        DisciplineRecords.Where(d => !d.IsDeleted && d.Type == DisciplineType.Removal)
                         .OrderByDescending(d => d.StartDate)
                         .FirstOrDefault()?.StartDate;

    /// <summary>
    /// Verifica se o membro possui censura ativa em uma data específica
    /// </summary>
    /// <param name="date">Data para verificação</param>
    /// <returns>True se possui censura ativa, false caso contrário</returns>
    public bool HasActiveCensureComputed(DateTime date) =>
        DisciplineRecords.Any(d => !d.IsDeleted &&
                                  d.Type == DisciplineType.Censure &&
                                  d.IsActiveAt(date));

    /// <summary>
    /// Verifica se o membro possui censura ativa atualmente
    /// </summary>
    [JsonIgnore]
    public bool HasCurrentActiveCensure => HasActiveCensureComputed(DateTime.UtcNow);

    /// <summary>
    /// Verifica se o batismo do membro é válido (batizado e sem remoção posterior)
    /// </summary>
    [JsonIgnore]
    public bool BaptismValidComputed =>
        BaptizedComputed &&
        (LastRemovalAtComputed == null ||
         (LastBaptismAtComputed.HasValue && LastBaptismAtComputed > LastRemovalAtComputed));

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

    /// <summary>
    /// Gets the primary email address from contacts
    /// </summary>
    [JsonIgnore]
    public string? PrimaryEmail => Contacts
        .Where(c => !c.IsDeleted && c.Type == ContactType.Email && c.IsPrimary)
        .OrderBy(c => c.CreatedAtUtc)
        .FirstOrDefault()?.Value;

    /// <summary>
    /// Gets the primary phone number from contacts
    /// </summary>
    [JsonIgnore]
    public string? PrimaryPhone => Contacts
        .Where(c => !c.IsDeleted && (c.Type == ContactType.Mobile || c.Type == ContactType.Landline) && c.IsPrimary)
        .OrderBy(c => c.CreatedAtUtc)
        .FirstOrDefault()?.Value;

    /// <summary>
    /// Gets all email addresses from contacts
    /// </summary>
    [JsonIgnore]
    public IEnumerable<string> EmailAddresses => Contacts
        .Where(c => !c.IsDeleted && c.Type == ContactType.Email)
        .Select(c => c.Value);

    /// <summary>
    /// Gets all phone numbers from contacts
    /// </summary>
    [JsonIgnore]
    public IEnumerable<string> PhoneNumbers => Contacts
        .Where(c => !c.IsDeleted && (c.Type == ContactType.Mobile || c.Type == ContactType.Landline))
        .Select(c => c.Value);

    /// <summary>
    /// Gets the primary email formatted for display
    /// </summary>
    [JsonIgnore]
    public string? PrimaryEmailFormatted => EmailHelper.NormalizeEmail(PrimaryEmail);

    /// <summary>
    /// Gets the primary phone formatted for display
    /// </summary>
    [JsonIgnore]
    public string? PrimaryPhoneFormatted => PhoneHelper.FormatPhoneForDisplay(PrimaryPhone);
}

/// <summary>
/// Enumeration for member gender
/// </summary>
public enum MemberGender
{
    /// <summary>
    /// Masculine
    /// </summary>
    Male = 0,

    /// <summary>
    /// Feminine
    /// </summary>
    Female = 1
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
    Suspended,

    /// <summary>
    /// Member is archived
    /// </summary>
    Archived
}
