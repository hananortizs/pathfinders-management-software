using AutoMapper;
using Pms.Backend.Application.DTOs.Hierarchy;
using Pms.Backend.Application.DTOs.Members;
using Pms.Backend.Application.DTOs.Membership;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Application.Mappings;

/// <summary>
/// AutoMapper profile for Membership-related mappings
/// </summary>
public class MembershipMappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the MembershipMappingProfile
    /// </summary>
    public MembershipMappingProfile()
    {
        // Membership mappings
        CreateMap<Membership, MembershipDto>()
            .ForMember(dest => dest.AgeOnJuneFirst, opt => opt.MapFrom(src => src.AgeOnJuneFirst))
            .ReverseMap();

        CreateMap<CreateMembershipDto, Membership>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UnitId, opt => opt.Ignore())
            .ForMember(dest => dest.EndDate, opt => opt.Ignore())
            .ForMember(dest => dest.EndReason, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CreatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.Member, opt => opt.Ignore())
            .ForMember(dest => dest.Club, opt => opt.Ignore())
            .ForMember(dest => dest.Unit, opt => opt.Ignore())
            .ForMember(dest => dest.TimelineEntries, opt => opt.Ignore())
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate ?? DateTime.UtcNow));

        CreateMap<UpdateMembershipDto, Membership>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.MemberId, opt => opt.Ignore())
            .ForMember(dest => dest.ClubId, opt => opt.Ignore())
            .ForMember(dest => dest.StartDate, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.Member, opt => opt.Ignore())
            .ForMember(dest => dest.Club, opt => opt.Ignore())
            .ForMember(dest => dest.Unit, opt => opt.Ignore())
            .ForMember(dest => dest.TimelineEntries, opt => opt.Ignore());

        // Unit capacity mappings
        CreateMap<Unit, UnitCapacityDto>()
            .ForMember(dest => dest.UnitId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UnitName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.MaxCapacity, opt => opt.MapFrom(src => src.Capacity ?? 0))
            .ForMember(dest => dest.CurrentCount, opt => opt.Ignore()) // Will be calculated in service
            .ForMember(dest => dest.AvailableCapacity, opt => opt.Ignore()) // Computed property
            .ForMember(dest => dest.HasAvailableCapacity, opt => opt.Ignore()) // Computed property
            .ForMember(dest => dest.CapacityPercentage, opt => opt.Ignore()); // Computed property
    }
}
