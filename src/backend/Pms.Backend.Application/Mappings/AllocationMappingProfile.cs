using AutoMapper;
using Pms.Backend.Application.DTOs.Membership;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Application.Mappings;

/// <summary>
/// Perfil de mapeamento AutoMapper para DTOs de alocação
/// </summary>
public class AllocationMappingProfile : Profile
{
    /// <summary>
    /// Inicializa o perfil de mapeamento
    /// </summary>
    public AllocationMappingProfile()
    {
        CreateMap<Unit, CompatibleUnitDto>()
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
            .ForMember(dest => dest.CurrentMemberCount, opt => opt.Ignore()) // Será preenchido pelo serviço
            .ForMember(dest => dest.HasAvailableCapacity, opt => opt.Ignore()) // Será preenchido pelo serviço
            .ForMember(dest => dest.OccupancyPercentage, opt => opt.Ignore()) // Será preenchido pelo serviço
            .ForMember(dest => dest.Priority, opt => opt.Ignore()); // Será preenchido pelo serviço

        CreateMap<Domain.Entities.Membership, MembershipDto>()
            .ForMember(dest => dest.AgeOnJuneFirst, opt => opt.MapFrom(src => src.Member != null ? src.Member.GetAgeOnJuneFirst(src.CreatedAtUtc.Year) : 0));
    }
}
