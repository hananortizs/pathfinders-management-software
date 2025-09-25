using AutoMapper;
using Pms.Backend.Application.DTOs.Auth;
using Pms.Backend.Application.DTOs.Members;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Domain.Enums;

namespace Pms.Backend.Application.Mappings;

/// <summary>
/// AutoMapper profile for Member-related mappings
/// </summary>
public class MemberMappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the MemberMappingProfile
    /// </summary>
    public MemberMappingProfile()
    {
        // Member mappings
        CreateMap<Member, MemberDto>()
            .ForMember(dest => dest.PrimaryEmail, opt => opt.MapFrom(src => MemberMappingHelpers.GetPrimaryEmail(src)))
            .ForMember(dest => dest.PrimaryPhone, opt => opt.MapFrom(src => MemberMappingHelpers.GetPrimaryPhone(src)))
            .ForMember(dest => dest.MiddleNames, opt => opt.MapFrom(src => src.MiddleNames))
            .ForMember(dest => dest.ClubName, opt => opt.MapFrom(src => MemberMappingHelpers.GetClubName(src)))
            .ForMember(dest => dest.UnitName, opt => opt.MapFrom(src => MemberMappingHelpers.GetUnitName(src)))
            .ReverseMap();

        // Optimized member list mapping
        CreateMap<Member, MemberListDto>()
            .ForMember(dest => dest.PrimaryEmail, opt => opt.MapFrom(src => GetPrimaryEmail(src)))
            .ForMember(dest => dest.PrimaryPhone, opt => opt.MapFrom(src => GetPrimaryPhone(src)))
            .ForMember(dest => dest.MiddleNames, opt => opt.MapFrom(src => src.MiddleNames))
            .ForMember(dest => dest.SocialName, opt => opt.MapFrom(src => src.SocialName))
            .ForMember(dest => dest.Rg, opt => opt.MapFrom(src => src.Rg))
            .ForMember(dest => dest.Cpf, opt => opt.MapFrom(src => src.Cpf))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.CreatedAtUtc, opt => opt.MapFrom(src => src.CreatedAtUtc))
            .ForMember(dest => dest.UpdatedAtUtc, opt => opt.MapFrom(src => src.UpdatedAtUtc))
            .ForMember(dest => dest.ActivatedAtUtc, opt => opt.MapFrom(src => src.ActivatedAtUtc))
            .ForMember(dest => dest.DeactivatedAtUtc, opt => opt.MapFrom(src => src.DeactivatedAtUtc))
            .ForMember(dest => dest.DeactivationReason, opt => opt.MapFrom(src => src.DeactivationReason))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.Status == MemberStatus.Active));

            // Member summary mapping for list views - using real data
            CreateMap<Member, MemberSummaryDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}".Trim()))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}".Trim()))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => CalculateAge(src.DateOfBirth)))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.ClubName, opt => opt.MapFrom(src => GetClubName(src)))
                .ForMember(dest => dest.UnitName, opt => opt.MapFrom(src => GetUnitName(src)))
                .ForMember(dest => dest.CurrentRole, opt => opt.MapFrom(src => GetCurrentRole(src)))
                .ForMember(dest => dest.AllRoles, opt => opt.MapFrom(src => GetAllActiveRoles(src)))
                .ForMember(dest => dest.PrimaryEmail, opt => opt.MapFrom(src => GetPrimaryEmail(src)))
                .ForMember(dest => dest.PrimaryPhone, opt => opt.MapFrom(src => GetPrimaryPhone(src)))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAtUtc))
                .ForMember(dest => dest.HasScarfInvestiture, opt => opt.MapFrom(src => src.ScarfInvested))
                .ForMember(dest => dest.HasValidBaptism, opt => opt.MapFrom(src => src.Baptized));

        // CreateMemberDto mapping removed - using CreateMemberCompleteDto only

        CreateMap<CreateMemberCompleteDto, Member>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.MiddleNames, opt => opt.MapFrom(src => src.MiddleNames))
            .ForMember(dest => dest.SocialName, opt => opt.MapFrom(src => src.SocialName))
            .ForMember(dest => dest.Cpf, opt => opt.MapFrom(src => src.Cpf))
            .ForMember(dest => dest.Rg, opt => opt.MapFrom(src => src.Rg))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src =>
                src.DateOfBirth.Kind == DateTimeKind.Utc ? src.DateOfBirth : DateTime.SpecifyKind(src.DateOfBirth, DateTimeKind.Utc)))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => MemberStatus.Pending))
            .ForMember(dest => dest.CreatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.ActivatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.DeactivatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.DeactivationReason, opt => opt.Ignore())
            .ForMember(dest => dest.UserCredential, opt => opt.Ignore())
            .ForMember(dest => dest.Memberships, opt => opt.Ignore())
            .ForMember(dest => dest.Assignments, opt => opt.Ignore())
            .ForMember(dest => dest.EventParticipations, opt => opt.Ignore())
            .ForMember(dest => dest.Investitures, opt => opt.Ignore())
            .ForMember(dest => dest.InvestitureWitnesses, opt => opt.Ignore())
            .ForMember(dest => dest.TimelineEntries, opt => opt.Ignore())
            .ForMember(dest => dest.Contacts, opt => opt.Ignore())
            .ForMember(dest => dest.BaptismRecords, opt => opt.Ignore())
            .ForMember(dest => dest.DisciplineRecords, opt => opt.Ignore());

        // Mapping for CreateMemberWithContactsDto
        CreateMap<CreateMemberWithContactsDto, Member>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => MemberStatus.Pending))
            .ForMember(dest => dest.CreatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.ActivatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.DeactivatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.DeactivationReason, opt => opt.Ignore())
            .ForMember(dest => dest.UserCredential, opt => opt.Ignore())
            .ForMember(dest => dest.Memberships, opt => opt.Ignore())
            .ForMember(dest => dest.Assignments, opt => opt.Ignore())
            .ForMember(dest => dest.EventParticipations, opt => opt.Ignore())
            .ForMember(dest => dest.Investitures, opt => opt.Ignore())
            .ForMember(dest => dest.InvestitureWitnesses, opt => opt.Ignore())
            .ForMember(dest => dest.TimelineEntries, opt => opt.Ignore())
            .ForMember(dest => dest.Contacts, opt => opt.Ignore())
            .ForMember(dest => dest.BaptismRecords, opt => opt.Ignore())
            .ForMember(dest => dest.DisciplineRecords, opt => opt.Ignore());

        // Mapping for MemberContactDto to Contact
        CreateMap<MemberContactDto, Contact>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.EntityId, opt => opt.Ignore())
            .ForMember(dest => dest.EntityType, opt => opt.MapFrom(src => "Member"))
            .ForMember(dest => dest.CreatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

        // Mapping for CreateMemberContactDto to Contact
        CreateMap<CreateMemberContactDto, Contact>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.EntityId, opt => opt.Ignore())
            .ForMember(dest => dest.EntityType, opt => opt.MapFrom(src => "Member"))
            .ForMember(dest => dest.CreatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

        // Mapping for CreateMemberAddressDto to Address
        CreateMap<CreateMemberAddressDto, Address>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.EntityId, opt => opt.Ignore())
            .ForMember(dest => dest.EntityType, opt => opt.MapFrom(src => "Member"))
            .ForMember(dest => dest.Cep, opt => opt.MapFrom(src => src.PostalCode))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => AddressType.Home))
            .ForMember(dest => dest.CreatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

        CreateMap<UpdateMemberDto, Member>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.ActivatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.DeactivatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.DeactivationReason, opt => opt.Ignore())
            .ForMember(dest => dest.UserCredential, opt => opt.Ignore())
            .ForMember(dest => dest.Memberships, opt => opt.Ignore())
            .ForMember(dest => dest.Assignments, opt => opt.Ignore())
            .ForMember(dest => dest.EventParticipations, opt => opt.Ignore())
            .ForMember(dest => dest.Investitures, opt => opt.Ignore())
            .ForMember(dest => dest.InvestitureWitnesses, opt => opt.Ignore())
            .ForMember(dest => dest.TimelineEntries, opt => opt.Ignore());

        // UserCredential mappings
        CreateMap<UserCredential, UserCredentialDto>()
            .ReverseMap();

        CreateMap<CreateUserCredentialDto, UserCredential>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.MemberId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.LastLoginAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.FailedLoginAttempts, opt => opt.Ignore())
            .ForMember(dest => dest.LockedUntilUtc, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHistory, opt => opt.Ignore())
            .ForMember(dest => dest.Member, opt => opt.Ignore())
            .ForMember(dest => dest.IsEmailVerified, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.IsLockedOut, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.LockedOutUntilUtc, opt => opt.Ignore())
            .ForMember(dest => dest.LastFailedLoginAttemptUtc, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
            .ForMember(dest => dest.LastActiveUtc, opt => opt.Ignore())
            .ForMember(dest => dest.ActivationToken, opt => opt.Ignore())
            .ForMember(dest => dest.ActivationTokenExpiresUtc, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordResetToken, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordResetTokenExpiresUtc, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.Salt, opt => opt.Ignore());

        // Auth mappings
        CreateMap<Member, UserInfoDto>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => GetPrimaryEmail(src)))
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Assignments
                .Where(a => a.IsActive)
                .Select(a => a.RoleCatalog.Name)
                .ToList()))
            .ForMember(dest => dest.Scopes, opt => opt.Ignore());

        // Invitation mappings
        CreateMap<InviteMemberRequestDto, CreateMemberCompleteDto>()
            .ForMember(dest => dest.Cpf, opt => opt.Ignore())
            .ForMember(dest => dest.Rg, opt => opt.Ignore())
            .ForMember(dest => dest.AddressInfo, opt => opt.Ignore())
            .ForMember(dest => dest.BaptismInfo, opt => opt.Ignore())
            .ForMember(dest => dest.Contacts, opt => opt.Ignore())
            .ForMember(dest => dest.MedicalInfo, opt => opt.Ignore())
            .ForMember(dest => dest.LoginInfo, opt => opt.Ignore());
    }

    /// <summary>
    /// Gets the primary email from member contacts
    /// </summary>
    /// <param name="member">Member entity</param>
    /// <returns>Primary email or null</returns>
    private static string? GetPrimaryEmail(Member member)
    {
        if (member.Contacts == null)
            return null;

        return member.Contacts
            .Where(c => !c.IsDeleted && c.Type == ContactType.Email && c.IsPrimary)
            .OrderBy(c => c.CreatedAtUtc)
            .FirstOrDefault()?.Value;
    }

    /// <summary>
    /// Gets the primary phone from member contacts
    /// </summary>
    /// <param name="member">Member entity</param>
    /// <returns>Primary phone or null</returns>
    private static string? GetPrimaryPhone(Member member)
    {
        if (member.Contacts == null)
            return null;

        // Primeiro tenta encontrar um contato marcado como primário
        var primaryPhone = member.Contacts
            .Where(c => !c.IsDeleted && (c.Type == ContactType.Mobile || c.Type == ContactType.Landline) && c.IsPrimary)
            .OrderBy(c => c.CreatedAtUtc)
            .FirstOrDefault()?.Value;

        // Se não encontrar, pega o primeiro telefone disponível
        if (string.IsNullOrEmpty(primaryPhone))
        {
            primaryPhone = member.Contacts
                .Where(c => !c.IsDeleted && (c.Type == ContactType.Mobile || c.Type == ContactType.Landline))
                .OrderBy(c => c.CreatedAtUtc)
                .FirstOrDefault()?.Value;
        }

        return primaryPhone;
    }

    /// <summary>
    /// Calculates the age based on date of birth
    /// </summary>
    /// <param name="dateOfBirth">Date of birth</param>
    /// <returns>Age in years</returns>
    private static int CalculateAge(DateTime dateOfBirth)
    {
        var today = DateTime.Today;
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > today.AddYears(-age)) age--;
        return age;
    }

    /// <summary>
    /// Gets the club name from member's active membership
    /// </summary>
    /// <param name="member">Member entity</param>
    /// <returns>Club name or empty string</returns>
    private static string GetClubName(Member member)
    {
        if (member.Memberships == null)
            return string.Empty;

        var activeMembership = member.Memberships
            .FirstOrDefault(m => m.IsActive && !m.IsDeleted);

        return activeMembership?.Club?.Name ?? string.Empty;
    }

    /// <summary>
    /// Gets the unit name from member's active membership
    /// </summary>
    /// <param name="member">Member entity</param>
    /// <returns>Unit name or empty string</returns>
    private static string GetUnitName(Member member)
    {
        if (member.Memberships == null)
            return string.Empty;

        var activeMembership = member.Memberships
            .FirstOrDefault(m => m.IsActive && !m.IsDeleted);

        return activeMembership?.Unit?.Name ?? string.Empty;
    }

    /// <summary>
    /// Gets the current role from member's active assignments
    /// </summary>
    /// <param name="member">Member entity</param>
    /// <returns>Current role or empty string</returns>
    private static string GetCurrentRole(Member member)
    {
        if (member.Assignments == null)
            return string.Empty;

        var activeAssignment = member.Assignments
            .FirstOrDefault(a => a.IsActive && !a.IsDeleted);

        return activeAssignment?.RoleCatalog?.Name ?? string.Empty;
    }

    /// <summary>
    /// Gets all active roles from member's assignments, ordered by hierarchy level (highest to lowest)
    /// </summary>
    /// <param name="member">Member entity</param>
    /// <returns>All active roles separated by " | " or empty string</returns>
    private static string GetAllActiveRoles(Member member)
    {
        if (member.Assignments == null)
            return string.Empty;

        var activeRoles = member.Assignments
            .Where(a => a.IsActive && !a.IsDeleted && a.RoleCatalog != null)
            .Select(a => a.RoleCatalog!.Name)
            .Distinct()
            .ToList();

        if (!activeRoles.Any())
            return string.Empty;

        // Ordenar por nível hierárquico (maior para menor)
        var roleHierarchy = new Dictionary<string, int>
        {
            { "SystemAdmin", 1 },
            { "Division", 2 },
            { "Union", 3 },
            { "Association", 4 },
            { "Region", 5 },
            { "District", 6 },
            { "Club", 7 },
            { "Unit", 8 }
        };

        var sortedRoles = activeRoles
            .OrderBy(role => roleHierarchy.GetValueOrDefault(role, 999))
            .ToList();

        return string.Join(" | ", sortedRoles);
    }
}

/// <summary>
/// Data Transfer Object for UserCredential entity
/// </summary>
public class UserCredentialDto
{
    /// <summary>
    /// Unique identifier of the user credential
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Associated member ID
    /// </summary>
    public Guid MemberId { get; set; }

    /// <summary>
    /// Email address (username)
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Whether the account is active
    /// </summary>
    public bool IsActive { get; set; }


    /// <summary>
    /// Last login date
    /// </summary>
    public DateTime? LastLoginAtUtc { get; set; }

    /// <summary>
    /// Number of failed login attempts
    /// </summary>
    public int FailedLoginAttempts { get; set; }

    /// <summary>
    /// Account locked until date
    /// </summary>
    public DateTime? LockedUntilUtc { get; set; }

    /// <summary>
    /// Creation date
    /// </summary>
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime UpdatedAtUtc { get; set; }
}

/// <summary>
/// Data Transfer Object for creating UserCredential
/// </summary>
public class CreateUserCredentialDto
{
    /// <summary>
    /// Email address (username)
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Password
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Whether the account is active
    /// </summary>
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// Helper methods for MemberMappingProfile
/// </summary>
public static class MemberMappingHelpers
{
    /// <summary>
    /// Helper method to get primary email from member contacts
    /// </summary>
    public static string GetPrimaryEmail(Member member)
    {
        return member.Contacts?
            .FirstOrDefault(c => c.Type == ContactType.Email && c.IsPrimary && c.IsActive)?
            .Value ?? string.Empty;
    }

    /// <summary>
    /// Helper method to get primary phone from member contacts
    /// </summary>
    public static string GetPrimaryPhone(Member member)
    {
        return member.Contacts?
            .FirstOrDefault(c => c.Type == ContactType.Mobile && c.IsPrimary && c.IsActive)?
            .Value ?? string.Empty;
    }

    /// <summary>
    /// Helper method to get club name from active membership
    /// </summary>
    public static string GetClubName(Member member)
    {
        var activeMembership = member.Memberships?
            .FirstOrDefault(m => m.IsActive && !m.IsDeleted);
        return activeMembership?.Club?.Name ?? string.Empty;
    }

    /// <summary>
    /// Helper method to get unit name from active membership
    /// </summary>
    public static string GetUnitName(Member member)
    {
        var activeMembership = member.Memberships?
            .FirstOrDefault(m => m.IsActive && !m.IsDeleted);
        return activeMembership?.Unit?.Name ?? string.Empty;
    }
}
