namespace Pms.Backend.Application.DTOs.Exports;

/// <summary>
/// DTO for member export to CSV
/// </summary>
public class MemberExportDto
{
    /// <summary>
    /// Member ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Member name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Member email
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Member phone
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Member birth date
    /// </summary>
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// Member gender
    /// </summary>
    public string Gender { get; set; } = string.Empty;

    /// <summary>
    /// Member status
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Baptism date
    /// </summary>
    public DateTime? BaptismDate { get; set; }

    /// <summary>
    /// Scarf investiture date
    /// </summary>
    public DateTime? ScarfInvestitureDate { get; set; }

    /// <summary>
    /// Current unit name
    /// </summary>
    public string? CurrentUnitName { get; set; }

    /// <summary>
    /// Current unit gender
    /// </summary>
    public string? CurrentUnitGender { get; set; }

    /// <summary>
    /// Current unit age range
    /// </summary>
    public string? CurrentUnitAgeRange { get; set; }

    /// <summary>
    /// Membership start date
    /// </summary>
    public DateTime? MembershipStartDate { get; set; }

    /// <summary>
    /// Membership end date
    /// </summary>
    public DateTime? MembershipEndDate { get; set; }

    /// <summary>
    /// Club name
    /// </summary>
    public string ClubName { get; set; } = string.Empty;

    /// <summary>
    /// Club code
    /// </summary>
    public string ClubCode { get; set; } = string.Empty;

    /// <summary>
    /// District name
    /// </summary>
    public string DistrictName { get; set; } = string.Empty;

    /// <summary>
    /// Region name
    /// </summary>
    public string RegionName { get; set; } = string.Empty;

    /// <summary>
    /// Association name
    /// </summary>
    public string AssociationName { get; set; } = string.Empty;

    /// <summary>
    /// Union name
    /// </summary>
    public string UnionName { get; set; } = string.Empty;

    /// <summary>
    /// Division name
    /// </summary>
    public string DivisionName { get; set; } = string.Empty;
}
