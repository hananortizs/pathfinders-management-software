using AutoMapper;
using Pms.Backend.Application.DTOs.Contacts;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Domain.Helpers;

namespace Pms.Backend.Application.Mappings;

/// <summary>
/// Mapeamento AutoMapper para entidade Contact
/// </summary>
public class ContactMappingProfile : Profile
{
    /// <summary>
    /// Inicializa o mapeamento
    /// </summary>
    public ContactMappingProfile()
    {
        // Mapeamento de Contact para ContactDto
        CreateMap<Contact, ContactDto>()
            .ForMember(dest => dest.FormattedValue, opt => opt.MapFrom(src => src.FormattedValue))
            .ForMember(dest => dest.TypeDisplayName, opt => opt.MapFrom(src => src.TypeDisplayName))
            .ForMember(dest => dest.CategoryDisplayName, opt => opt.MapFrom(src => src.CategoryDisplayName))
            .ForMember(dest => dest.FullDisplayName, opt => opt.MapFrom(src => src.FullDisplayName))
            .ForMember(dest => dest.IsSocialMedia, opt => opt.MapFrom(src => src.IsSocialMedia))
            .ForMember(dest => dest.IsPhone, opt => opt.MapFrom(src => src.IsPhone))
            .ForMember(dest => dest.IsDigital, opt => opt.MapFrom(src => src.IsDigital))
            .ForMember(dest => dest.IsEmergency, opt => opt.MapFrom(src => src.IsEmergency))
            .ForMember(dest => dest.IsPersonal, opt => opt.MapFrom(src => src.IsPersonal))
            .ForMember(dest => dest.IsChurch, opt => opt.MapFrom(src => src.IsChurch))
            .ForMember(dest => dest.IsLegalGuardian, opt => opt.MapFrom(src => src.IsLegalGuardian));

        // Mapeamento de CreateContactDto para Contact
        CreateMap<CreateContactDto, Contact>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.LastVerifiedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsVerified, opt => opt.MapFrom(src => false));

        // Mapeamento de UpdateContactDto para Contact
        CreateMap<UpdateContactDto, Contact>()
            .ForMember(dest => dest.CreatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.LastVerifiedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsVerified, opt => opt.Ignore());

        // Mapeamento de Contact para CreateContactDto (para edição)
        CreateMap<Contact, CreateContactDto>();

        // Mapeamento de Contact para UpdateContactDto (para edição)
        CreateMap<Contact, UpdateContactDto>();
    }
}
