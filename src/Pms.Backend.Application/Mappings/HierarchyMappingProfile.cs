using AutoMapper;
using Pms.Backend.Application.DTOs.Hierarchy;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Domain.Entities.Hierarchy;

namespace Pms.Backend.Application.Mappings;

/// <summary>
/// AutoMapper profile for hierarchy entities
/// </summary>
public class HierarchyMappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the HierarchyMappingProfile class
    /// </summary>
    public HierarchyMappingProfile()
    {
        // Division mappings
        CreateMap<Division, DivisionDto>()
            .ForMember(dest => dest.Unions, opt => opt.MapFrom(src => src.Unions));
        CreateMap<CreateDivisionDto, Division>();
        CreateMap<UpdateDivisionDto, Division>();

        // Union mappings
        CreateMap<Union, UnionDto>()
            .ForMember(dest => dest.Division, opt => opt.MapFrom(src => src.Division))
            .ForMember(dest => dest.Associations, opt => opt.MapFrom(src => src.Associations));
        CreateMap<CreateUnionDto, Union>();
        CreateMap<UpdateUnionDto, Union>();

        // Association mappings
        CreateMap<Association, AssociationDto>()
            .ForMember(dest => dest.Union, opt => opt.MapFrom(src => src.Union))
            .ForMember(dest => dest.Regions, opt => opt.MapFrom(src => src.Regions));
        CreateMap<CreateAssociationDto, Association>();
        CreateMap<UpdateAssociationDto, Association>();

        // Region mappings
        CreateMap<Region, RegionDto>()
            .ForMember(dest => dest.Association, opt => opt.MapFrom(src => src.Association))
            .ForMember(dest => dest.Districts, opt => opt.MapFrom(src => src.Districts));
        CreateMap<CreateRegionDto, Region>();
        CreateMap<UpdateRegionDto, Region>();

        // District mappings
        CreateMap<District, DistrictDto>()
            .ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.Region))
            .ForMember(dest => dest.Clubs, opt => opt.MapFrom(src => src.Clubs));
        CreateMap<CreateDistrictDto, District>();
        CreateMap<UpdateDistrictDto, District>();

        // Church mappings
        CreateMap<Church, ChurchDto>();
        CreateMap<CreateChurchDto, Church>();
        CreateMap<UpdateChurchDto, Church>();

        // Club mappings
        CreateMap<Club, ClubDto>()
            .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.District))
            .ForMember(dest => dest.Church, opt => opt.MapFrom(src => src.Church))
            .ForMember(dest => dest.Units, opt => opt.MapFrom(src => src.Units));
        CreateMap<CreateClubDto, Club>();
        CreateMap<UpdateClubDto, Club>();

        // Unit mappings
        CreateMap<Unit, UnitDto>()
            .ForMember(dest => dest.Club, opt => opt.MapFrom(src => src.Club))
            .ForMember(dest => dest.CurrentMemberCount, opt => opt.MapFrom(src => src.CurrentMemberCount))
            .ForMember(dest => dest.HasAvailableCapacity, opt => opt.MapFrom(src => src.HasAvailableCapacity));
        CreateMap<CreateUnitDto, Unit>();
        CreateMap<UpdateUnitDto, Unit>();

        // UnitGender enum mapping
        CreateMap<Domain.Entities.UnitGender, DTOs.Hierarchy.UnitGender>().ReverseMap();
    }
}
