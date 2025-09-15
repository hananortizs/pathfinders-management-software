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
            .ForMember(dest => dest.PrimaryEmail, opt => opt.MapFrom(src => src.PrimaryEmail))
            .ForMember(dest => dest.PrimaryPhone, opt => opt.MapFrom(src => src.PrimaryPhone))
            .ForMember(dest => dest.MiddleNames, opt => opt.MapFrom(src => src.MiddleNames))
            .ReverseMap();

        // Optimized member list mapping
        CreateMap<Member, MemberListDto>()
            .ForMember(dest => dest.PrimaryEmail, opt => opt.MapFrom(src => src.PrimaryEmail))
            .ForMember(dest => dest.PrimaryPhone, opt => opt.MapFrom(src => src.PrimaryPhone))
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
