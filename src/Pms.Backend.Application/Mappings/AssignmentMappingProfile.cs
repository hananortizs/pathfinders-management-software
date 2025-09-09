using AutoMapper;
using Pms.Backend.Application.DTOs.Assignments;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Application.Mappings;

/// <summary>
/// AutoMapper profile for Assignment and related entities
/// </summary>
public class AssignmentMappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the AssignmentMappingProfile
    /// </summary>
    public AssignmentMappingProfile()
    {
        // Assignment mappings
        CreateMap<Assignment, AssignmentDto>()
            .ForMember(dest => dest.Member, opt => opt.MapFrom(src => src.Member))
            .ForMember(dest => dest.RoleCatalog, opt => opt.MapFrom(src => src.RoleCatalog));

        CreateMap<CreateAssignmentDto, Assignment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Member, opt => opt.Ignore())
            .ForMember(dest => dest.RoleCatalog, opt => opt.Ignore())
            .ForMember(dest => dest.TimelineEntries, opt => opt.Ignore())
            .ForMember(dest => dest.DelegatedToAssignments, opt => opt.Ignore())
            .ForMember(dest => dest.DelegatedFromAssignments, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

        CreateMap<UpdateAssignmentDto, Assignment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.MemberId, opt => opt.Ignore())
            .ForMember(dest => dest.Member, opt => opt.Ignore())
            .ForMember(dest => dest.RoleId, opt => opt.Ignore())
            .ForMember(dest => dest.RoleCatalog, opt => opt.Ignore())
            .ForMember(dest => dest.ScopeType, opt => opt.Ignore())
            .ForMember(dest => dest.ScopeId, opt => opt.Ignore())
            .ForMember(dest => dest.StartDate, opt => opt.Ignore())
            .ForMember(dest => dest.TimelineEntries, opt => opt.Ignore())
            .ForMember(dest => dest.DelegatedToAssignments, opt => opt.Ignore())
            .ForMember(dest => dest.DelegatedFromAssignments, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

        // RoleCatalog mappings
        CreateMap<RoleCatalog, RoleCatalogDto>();

        CreateMap<CreateRoleCatalogDto, RoleCatalog>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Assignments, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

        CreateMap<UpdateRoleCatalogDto, RoleCatalog>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Level, opt => opt.Ignore())
            .ForMember(dest => dest.Assignments, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

        // ApprovalDelegate mappings
        CreateMap<ApprovalDelegate, ApprovalDelegateDto>()
            .ForMember(dest => dest.DelegatedToAssignment, opt => opt.MapFrom(src => src.DelegatedToAssignment))
            .ForMember(dest => dest.DelegatedFromAssignment, opt => opt.MapFrom(src => src.DelegatedFromAssignment))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

        CreateMap<CreateApprovalDelegateDto, ApprovalDelegate>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DelegatedToAssignment, opt => opt.Ignore())
            .ForMember(dest => dest.DelegatedFromAssignment, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

        CreateMap<UpdateApprovalDelegateDto, ApprovalDelegate>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ScopeType, opt => opt.Ignore())
            .ForMember(dest => dest.ScopeId, opt => opt.Ignore())
            .ForMember(dest => dest.DelegatedToAssignmentId, opt => opt.Ignore())
            .ForMember(dest => dest.DelegatedToAssignment, opt => opt.Ignore())
            .ForMember(dest => dest.DelegatedFromAssignmentId, opt => opt.Ignore())
            .ForMember(dest => dest.DelegatedFromAssignment, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAtUtc, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.EndedAtUtc, opt => opt.Ignore());
    }
}
