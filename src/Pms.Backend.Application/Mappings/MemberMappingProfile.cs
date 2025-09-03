using AutoMapper;
using Pms.Backend.Application.DTOs.Auth;
using Pms.Backend.Application.DTOs.Members;
using Pms.Backend.Domain.Entities;

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
            .ReverseMap();

        CreateMap<CreateMemberDto, Member>()
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
            .ForMember(dest => dest.TimelineEntries, opt => opt.Ignore());

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
            .ForMember(dest => dest.Member, opt => opt.Ignore());

        // Auth mappings
        CreateMap<Member, UserInfoDto>()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Assignments
                .Where(a => a.IsActive)
                .Select(a => a.RoleCatalog.Name)
                .ToList()))
            .ForMember(dest => dest.Scopes, opt => opt.Ignore());

        // Invitation mappings
        CreateMap<InviteMemberRequestDto, CreateMemberDto>()
            .ForMember(dest => dest.Cpf, opt => opt.Ignore())
            .ForMember(dest => dest.Rg, opt => opt.Ignore())
            .ForMember(dest => dest.Address, opt => opt.Ignore())
            .ForMember(dest => dest.City, opt => opt.Ignore())
            .ForMember(dest => dest.State, opt => opt.Ignore())
            .ForMember(dest => dest.ZipCode, opt => opt.Ignore())
            .ForMember(dest => dest.EmergencyContactName, opt => opt.Ignore())
            .ForMember(dest => dest.EmergencyContactPhone, opt => opt.Ignore())
            .ForMember(dest => dest.EmergencyContactRelationship, opt => opt.Ignore())
            .ForMember(dest => dest.MedicalInfo, opt => opt.Ignore())
            .ForMember(dest => dest.Allergies, opt => opt.Ignore())
            .ForMember(dest => dest.Medications, opt => opt.Ignore())
            .ForMember(dest => dest.BaptismDate, opt => opt.Ignore())
            .ForMember(dest => dest.BaptismChurch, opt => opt.Ignore())
            .ForMember(dest => dest.BaptismPastor, opt => opt.Ignore())
            .ForMember(dest => dest.ScarfDate, opt => opt.Ignore())
            .ForMember(dest => dest.ScarfChurch, opt => opt.Ignore())
            .ForMember(dest => dest.ScarfPastor, opt => opt.Ignore());
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
    /// Whether the email is verified
    /// </summary>
    public bool IsEmailVerified { get; set; }

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

    /// <summary>
    /// Whether the email is verified
    /// </summary>
    public bool IsEmailVerified { get; set; } = false;
}
