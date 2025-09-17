using AutoMapper;
using Pms.Backend.Application.DTOs.Events;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Application.Mappings;

/// <summary>
/// AutoMapper profile for Event-related mappings
/// </summary>
public class EventMappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the EventMappingProfile
    /// </summary>
    public EventMappingProfile()
    {
        CreateMap<CreateEventDto, OfficialEvent>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.Participations, opt => opt.Ignore())
            .ForMember(dest => dest.TimelineEntries, opt => opt.Ignore());

        CreateMap<OfficialEvent, EventDto>()
            .ForMember(dest => dest.OrganizerName, opt => opt.Ignore())
            .ForMember(dest => dest.ParticipantCount, opt => opt.Ignore())
            .ForMember(dest => dest.IsCurrentlyHappening, opt => opt.MapFrom(src => src.IsCurrentlyHappening))
            .ForMember(dest => dest.HasEnded, opt => opt.MapFrom(src => src.HasEnded));

        CreateMap<UpdateEventDto, OfficialEvent>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<MemberEventParticipation, EventParticipationDto>()
            .ForMember(dest => dest.MemberName, opt => opt.Ignore())
            .ForMember(dest => dest.EventName, opt => opt.Ignore());

        CreateMap<CreateEventParticipationDto, MemberEventParticipation>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.RegisteredAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.FeePaid, opt => opt.Ignore())
            .ForMember(dest => dest.FeeCurrency, opt => opt.Ignore())
            .ForMember(dest => dest.FeePaidAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.Member, opt => opt.Ignore())
            .ForMember(dest => dest.Event, opt => opt.Ignore())
            .ForMember(dest => dest.TimelineEntries, opt => opt.Ignore());

        CreateMap<UpdateEventParticipationDto, MemberEventParticipation>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}
