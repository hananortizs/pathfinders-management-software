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
        CreateMap<Division, DivisionSummaryDto>();
        CreateMap<CreateDivisionDto, Division>();
        CreateMap<UpdateDivisionDto, Division>();

        // Union mappings
        CreateMap<Union, UnionDto>()
            .ForMember(dest => dest.Associations, opt => opt.MapFrom(src => src.Associations));
        CreateMap<Union, UnionSummaryDto>();
        CreateMap<CreateUnionDto, Union>();
        CreateMap<UpdateUnionDto, Union>();

        // Association mappings
        CreateMap<Association, AssociationDto>()
            .ForMember(dest => dest.Regions, opt => opt.MapFrom(src => src.Regions));
        CreateMap<Association, AssociationSummaryDto>();
        CreateMap<CreateAssociationDto, Association>();
        CreateMap<UpdateAssociationDto, Association>();

        // Region mappings
        CreateMap<Region, RegionDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.AssociationId, opt => opt.MapFrom(src => src.AssociationId))
            // Parent Association removed - avoid circular references
            .ForMember(dest => dest.CodePath, opt => opt.MapFrom(src => src.CodePath))
            .ForMember(dest => dest.CreatedAtUtc, opt => opt.MapFrom(src => src.CreatedAtUtc))
            .ForMember(dest => dest.UpdatedAtUtc, opt => opt.MapFrom(src => src.UpdatedAtUtc))
            .ForMember(dest => dest.Districts, opt => opt.MapFrom(src => src.Districts));
        CreateMap<Region, RegionSummaryDto>();
        CreateMap<CreateRegionDto, Region>();
        CreateMap<UpdateRegionDto, Region>();

        // District mappings
        CreateMap<District, DistrictDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.RegionId, opt => opt.MapFrom(src => src.RegionId))
            // Parent Region removed - avoid circular references
            .ForMember(dest => dest.CodePath, opt => opt.MapFrom(src => src.CodePath))
            .ForMember(dest => dest.CreatedAtUtc, opt => opt.MapFrom(src => src.CreatedAtUtc))
            .ForMember(dest => dest.UpdatedAtUtc, opt => opt.MapFrom(src => src.UpdatedAtUtc))
            .ForMember(dest => dest.Clubs, opt => opt.MapFrom(src => src.Clubs));
        CreateMap<District, DistrictSummaryDto>();
        CreateMap<CreateDistrictDto, District>();
        CreateMap<UpdateDistrictDto, District>();

        // Church mappings
        CreateMap<Church, ChurchDto>();
        CreateMap<CreateChurchDto, Church>();
        CreateMap<UpdateChurchDto, Church>();

        // Club mappings
        CreateMap<Club, ClubDto>()
            .ForMember(dest => dest.Units, opt => opt.MapFrom(src => src.Units))
            .ForMember(dest => dest.CodePath, opt => opt.MapFrom(src => src.CodePath));
        CreateMap<Club, ClubSummaryDto>()
            .ForMember(dest => dest.CodePath, opt => opt.MapFrom(src => src.CodePath));
        CreateMap<CreateClubDto, Club>();
        CreateMap<UpdateClubDto, Club>();

        // Unit mappings
        CreateMap<Unit, UnitDto>()
            .ForMember(dest => dest.CurrentMemberCount, opt => opt.MapFrom(src => src.CurrentMemberCount))
            .ForMember(dest => dest.HasAvailableCapacity, opt => opt.MapFrom(src => src.HasAvailableCapacity));
        CreateMap<Unit, UnitSummaryDto>();
        CreateMap<CreateUnitDto, Unit>();
        CreateMap<UpdateUnitDto, Unit>();

        // UnitGender enum mapping
        CreateMap<Domain.Entities.UnitGender, DTOs.Hierarchy.UnitGender>().ReverseMap();

        // Hierarchy Query DTOs (without parent objects)
        CreateMap<Division, DivisionQueryDto>()
            .ForMember(dest => dest.Unions, opt => opt.MapFrom(src => src.Unions));
        CreateMap<Union, UnionQueryDto>()
            .ForMember(dest => dest.Associations, opt => opt.MapFrom(src => src.Associations))
            .ForMember(dest => dest.CodePath, opt => opt.MapFrom(src => src.CodePath));
        CreateMap<Association, AssociationQueryDto>()
            .ForMember(dest => dest.Regions, opt => opt.MapFrom(src => src.Regions))
            .ForMember(dest => dest.CodePath, opt => opt.MapFrom(src => src.CodePath));
        CreateMap<Region, RegionQueryDto>()
            .ForMember(dest => dest.Districts, opt => opt.MapFrom(src => src.Districts))
            .ForMember(dest => dest.CodePath, opt => opt.MapFrom(src => src.CodePath));
        CreateMap<District, DistrictQueryDto>()
            .ForMember(dest => dest.Clubs, opt => opt.MapFrom(src => src.Clubs))
            .ForMember(dest => dest.CodePath, opt => opt.MapFrom(src => src.CodePath));
        CreateMap<Club, ClubQueryDto>()
            .ForMember(dest => dest.Units, opt => opt.MapFrom(src => src.Units))
            .ForMember(dest => dest.CodePath, opt => opt.MapFrom(src => src.CodePath));
        CreateMap<Unit, UnitQueryDto>();
    }
}
