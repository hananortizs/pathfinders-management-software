using AutoMapper;
using Pms.Backend.Application.DTOs.Address;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Application.Mappings;

/// <summary>
/// AutoMapper profile for Address entities
/// </summary>
public class AddressMappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the AddressMappingProfile
    /// </summary>
    public AddressMappingProfile()
    {
        CreateMap<Address, AddressDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Street))
            .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Number))
            .ForMember(dest => dest.Complement, opt => opt.MapFrom(src => src.Complement))
            .ForMember(dest => dest.Neighborhood, opt => opt.MapFrom(src => src.Neighborhood))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
            .ForMember(dest => dest.Cep, opt => opt.MapFrom(src => src.Cep))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (AddressTypeDto)src.Type))
            .ForMember(dest => dest.IsPrimary, opt => opt.MapFrom(src => src.IsPrimary))
            .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
            .ForMember(dest => dest.EntityId, opt => opt.MapFrom(src => src.EntityId))
            .ForMember(dest => dest.EntityType, opt => opt.MapFrom(src => src.EntityType))
            .ForMember(dest => dest.CepFormatted, opt => opt.MapFrom(src => src.CepFormatted))
            .ForMember(dest => dest.CepNormalized, opt => opt.MapFrom(src => src.CepNormalized))
            .ForMember(dest => dest.FullAddress, opt => opt.MapFrom(src => src.FullAddress))
            .ForMember(dest => dest.CreatedAtUtc, opt => opt.MapFrom(src => src.CreatedAtUtc))
            .ForMember(dest => dest.UpdatedAtUtc, opt => opt.MapFrom(src => src.UpdatedAtUtc));

        CreateMap<CreateAddressDto, Address>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Street))
            .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Number))
            .ForMember(dest => dest.Complement, opt => opt.MapFrom(src => src.Complement))
            .ForMember(dest => dest.Neighborhood, opt => opt.MapFrom(src => src.Neighborhood))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
            .ForMember(dest => dest.Cep, opt => opt.MapFrom(src => src.Cep))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (AddressType)src.Type))
            .ForMember(dest => dest.IsPrimary, opt => opt.MapFrom(src => src.IsPrimary))
            .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
            .ForMember(dest => dest.EntityId, opt => opt.MapFrom(src => src.EntityId))
            .ForMember(dest => dest.EntityType, opt => opt.MapFrom(src => src.EntityType))
            .ForMember(dest => dest.CreatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAtUtc, opt => opt.Ignore());

        CreateMap<UpdateAddressDto, Address>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Street))
            .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Number))
            .ForMember(dest => dest.Complement, opt => opt.MapFrom(src => src.Complement))
            .ForMember(dest => dest.Neighborhood, opt => opt.MapFrom(src => src.Neighborhood))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
            .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
            .ForMember(dest => dest.Cep, opt => opt.MapFrom(src => src.Cep))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => (AddressType)src.Type))
            .ForMember(dest => dest.IsPrimary, opt => opt.MapFrom(src => src.IsPrimary))
            .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
            .ForMember(dest => dest.EntityId, opt => opt.Ignore())
            .ForMember(dest => dest.EntityType, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAtUtc, opt => opt.Ignore());
    }
}
