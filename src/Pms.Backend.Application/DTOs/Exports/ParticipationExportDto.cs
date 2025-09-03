namespace Pms.Backend.Application.DTOs.Exports;

/// <summary>
/// DTO for event participation export to CSV
/// </summary>
public class ParticipationExportDto
{
    /// <summary>
    /// Participation ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Member name
    /// </summary>
    public string MemberName { get; set; } = string.Empty;

    /// <summary>
    /// Member email
    /// </summary>
    public string MemberEmail { get; set; } = string.Empty;

    /// <summary>
    /// Member birth date
    /// </summary>
    public DateTime MemberBirthDate { get; set; }

    /// <summary>
    /// Member gender
    /// </summary>
    public string MemberGender { get; set; } = string.Empty;

    /// <summary>
    /// Event name
    /// </summary>
    public string EventName { get; set; } = string.Empty;

    /// <summary>
    /// Event description
    /// </summary>
    public string EventDescription { get; set; } = string.Empty;

    /// <summary>
    /// Event start date
    /// </summary>
    public DateTime EventStartDate { get; set; }

    /// <summary>
    /// Event end date
    /// </summary>
    public DateTime EventEndDate { get; set; }

    /// <summary>
    /// Event location
    /// </summary>
    public string? EventLocation { get; set; }

    /// <summary>
    /// Event fee (in BRL)
    /// </summary>
    public decimal? EventFee { get; set; }

    /// <summary>
    /// Participation registration date
    /// </summary>
    public DateTime RegistrationDate { get; set; }

    /// <summary>
    /// Participation status
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Member's current unit name
    /// </summary>
    public string? CurrentUnitName { get; set; }

    /// <summary>
    /// Member's club name
    /// </summary>
    public string ClubName { get; set; } = string.Empty;

    /// <summary>
    /// Member's club code
    /// </summary>
    public string ClubCode { get; set; } = string.Empty;

    /// <summary>
    /// Member's district name
    /// </summary>
    public string DistrictName { get; set; } = string.Empty;

    /// <summary>
    /// Member's region name
    /// </summary>
    public string RegionName { get; set; } = string.Empty;

    /// <summary>
    /// Member's association name
    /// </summary>
    public string AssociationName { get; set; } = string.Empty;

    /// <summary>
    /// Member's union name
    /// </summary>
    public string UnionName { get; set; } = string.Empty;

    /// <summary>
    /// Member's division name
    /// </summary>
    public string DivisionName { get; set; } = string.Empty;
}
