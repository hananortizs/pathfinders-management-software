using AutoMapper;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Hierarchy;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Domain.Entities.Hierarchy;

namespace Pms.Backend.Application.Services.Hierarchy;

/// <summary>
/// Service for lightweight hierarchy queries without related entities
/// Optimized for listing operations to avoid performance issues
/// </summary>
public class HierarchyQueryService : IHierarchyQueryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the HierarchyQueryService
    /// </summary>
    /// <param name="unitOfWork">The unit of work</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public HierarchyQueryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    #region Division Queries

    /// <summary>
    /// Retrieves a lightweight list of all divisions without related entities
    /// </summary>
    /// <param name="pageNumber">The page number for pagination (default is 1)</param>
    /// <param name="pageSize">The number of items per page (default is 10)</param>
    /// <param name="cancellationToken">A token to cancel the operation</param>
    /// <returns>A BaseResponse containing a PaginatedResponse of DivisionDto</returns>
    public async Task<BaseResponse<PaginatedResponse<IEnumerable<DivisionQueryDto>>>> GetAllDivisionsAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            // Get divisions with their unions included for rich query data
            var divisions = await _unitOfWork.Repository<Division>().GetWithIncludesAsync(null, cancellationToken, d => d.Unions);
            var totalCount = divisions.Count();

            var pagedDivisions = divisions
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var dtos = _mapper.Map<List<DivisionQueryDto>>(pagedDivisions);

            var paginatedResponse = new PaginatedResponse<IEnumerable<DivisionQueryDto>>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return BaseResponse<PaginatedResponse<IEnumerable<DivisionQueryDto>>>.SuccessResult(paginatedResponse);
        }
        catch (Exception ex)
        {
            return BaseResponse<PaginatedResponse<IEnumerable<DivisionQueryDto>>>.ErrorResult($"Error retrieving all divisions: {ex.Message}");
        }
    }

    #endregion

    #region Union Queries

    /// <summary>
    /// Retrieves a rich list of all unions with their related entities (associations)
    /// </summary>
    /// <param name="pageNumber">The page number for pagination (default is 1)</param>
    /// <param name="pageSize">The number of items per page (default is 10)</param>
    /// <param name="cancellationToken">A token to cancel the operation</param>
    /// <returns>A BaseResponse containing a PaginatedResponse of UnionDto with associations</returns>
    public async Task<BaseResponse<PaginatedResponse<IEnumerable<UnionQueryDto>>>> GetAllUnionsAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            // Get unions with their associations and division included for rich query data (division needed for CodePath calculation)
            var unions = await _unitOfWork.Repository<Union>().GetWithIncludesAsync(null, cancellationToken, u => u.Associations, u => u.Division);
            var totalCount = unions.Count();

            var pagedUnions = unions
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var dtos = _mapper.Map<List<UnionQueryDto>>(pagedUnions);

            var paginatedResponse = new PaginatedResponse<IEnumerable<UnionQueryDto>>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return BaseResponse<PaginatedResponse<IEnumerable<UnionQueryDto>>>.SuccessResult(paginatedResponse);
        }
        catch (Exception ex)
        {
            return BaseResponse<PaginatedResponse<IEnumerable<UnionQueryDto>>>.ErrorResult($"Error retrieving all unions: {ex.Message}");
        }
    }

    #endregion

    #region Association Queries

    /// <summary>
    /// Retrieves a lightweight list of all associations without related entities
    /// </summary>
    /// <param name="pageNumber">The page number for pagination (default is 1)</param>
    /// <param name="pageSize">The number of items per page (default is 10)</param>
    /// <param name="cancellationToken">A token to cancel the operation</param>
    /// <returns>A BaseResponse containing a PaginatedResponse of AssociationQueryDto</returns>
    public async Task<BaseResponse<PaginatedResponse<IEnumerable<AssociationQueryDto>>>> GetAllAssociationsAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            // Get associations with their regions and union included for rich query data (union needed for CodePath calculation)
            var associations = await _unitOfWork.Repository<Association>().GetWithIncludesAsync(null, cancellationToken, a => a.Regions, a => a.Union);
            var totalCount = associations.Count();

            var pagedAssociations = associations
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var dtos = _mapper.Map<List<AssociationQueryDto>>(pagedAssociations);

            var paginatedResponse = new PaginatedResponse<IEnumerable<AssociationQueryDto>>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return BaseResponse<PaginatedResponse<IEnumerable<AssociationQueryDto>>>.SuccessResult(paginatedResponse);
        }
        catch (Exception ex)
        {
            return BaseResponse<PaginatedResponse<IEnumerable<AssociationQueryDto>>>.ErrorResult($"Error retrieving all associations: {ex.Message}");
        }
    }

    #endregion

    #region Region Queries

    /// <summary>
    /// Retrieves a lightweight list of all regions without related entities
    /// </summary>
    /// <param name="pageNumber">The page number for pagination (default is 1)</param>
    /// <param name="pageSize">The number of items per page (default is 10)</param>
    /// <param name="cancellationToken">A token to cancel the operation</param>
    /// <returns>A BaseResponse containing a PaginatedResponse of RegionQueryDto</returns>
    public async Task<BaseResponse<PaginatedResponse<IEnumerable<RegionQueryDto>>>> GetAllRegionsAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            // Get regions with their districts and association included for rich query data (association needed for CodePath calculation)
            var regions = await _unitOfWork.Repository<Region>().GetWithIncludesAsync(null, cancellationToken, r => r.Districts, r => r.Association);
            var totalCount = regions.Count();

            var pagedRegions = regions
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var dtos = _mapper.Map<List<RegionQueryDto>>(pagedRegions);

            var paginatedResponse = new PaginatedResponse<IEnumerable<RegionQueryDto>>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return BaseResponse<PaginatedResponse<IEnumerable<RegionQueryDto>>>.SuccessResult(paginatedResponse);
        }
        catch (Exception ex)
        {
            return BaseResponse<PaginatedResponse<IEnumerable<RegionQueryDto>>>.ErrorResult($"Error retrieving all regions: {ex.Message}");
        }
    }

    #endregion

    #region District Queries

    /// <summary>
    /// Retrieves a lightweight list of all districts without related entities
    /// </summary>
    /// <param name="pageNumber">The page number for pagination (default is 1)</param>
    /// <param name="pageSize">The number of items per page (default is 10)</param>
    /// <param name="cancellationToken">A token to cancel the operation</param>
    /// <returns>A BaseResponse containing a PaginatedResponse of DistrictQueryDto</returns>
    public async Task<BaseResponse<PaginatedResponse<IEnumerable<DistrictQueryDto>>>> GetAllDistrictsAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            // Get districts with their clubs and region included for rich query data (region needed for CodePath calculation)
            var districts = await _unitOfWork.Repository<District>().GetWithIncludesAsync(null, cancellationToken, d => d.Clubs, d => d.Region);
            var totalCount = districts.Count();

            var pagedDistricts = districts
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var dtos = _mapper.Map<List<DistrictQueryDto>>(pagedDistricts);

            var paginatedResponse = new PaginatedResponse<IEnumerable<DistrictQueryDto>>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return BaseResponse<PaginatedResponse<IEnumerable<DistrictQueryDto>>>.SuccessResult(paginatedResponse);
        }
        catch (Exception ex)
        {
            return BaseResponse<PaginatedResponse<IEnumerable<DistrictQueryDto>>>.ErrorResult($"Error retrieving all districts: {ex.Message}");
        }
    }

    #endregion

    #region Club Queries

    /// <summary>
    /// Retrieves a lightweight list of all clubs without related entities
    /// </summary>
    /// <param name="pageNumber">The page number for pagination (default is 1)</param>
    /// <param name="pageSize">The number of items per page (default is 10)</param>
    /// <param name="cancellationToken">A token to cancel the operation</param>
    /// <returns>A BaseResponse containing a PaginatedResponse of ClubQueryDto</returns>
    public async Task<BaseResponse<PaginatedResponse<IEnumerable<ClubQueryDto>>>> GetAllClubsAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            // Get clubs with their units included for rich query data (no parent objects)
            var clubs = await _unitOfWork.Repository<Club>().GetWithIncludesAsync(null, cancellationToken, c => c.Units);
            var totalCount = clubs.Count();

            var pagedClubs = clubs
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var dtos = _mapper.Map<List<ClubQueryDto>>(pagedClubs);

            var paginatedResponse = new PaginatedResponse<IEnumerable<ClubQueryDto>>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return BaseResponse<PaginatedResponse<IEnumerable<ClubQueryDto>>>.SuccessResult(paginatedResponse);
        }
        catch (Exception ex)
        {
            return BaseResponse<PaginatedResponse<IEnumerable<ClubQueryDto>>>.ErrorResult($"Error retrieving all clubs: {ex.Message}");
        }
    }

    #endregion

    #region Unit Queries

    /// <summary>
    /// Retrieves a lightweight list of all units without related entities
    /// </summary>
    /// <param name="pageNumber">The page number for pagination (default is 1)</param>
    /// <param name="pageSize">The number of items per page (default is 10)</param>
    /// <param name="cancellationToken">A token to cancel the operation</param>
    /// <returns>A BaseResponse containing a PaginatedResponse of UnitQueryDto</returns>
    public async Task<BaseResponse<PaginatedResponse<IEnumerable<UnitQueryDto>>>> GetAllUnitsAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            // Get units (no child hierarchy - units are the leaf level, no parent objects)
            var (items, totalCount) = await _unitOfWork.Repository<Unit>().GetPagedAsync(pageNumber, pageSize, null, cancellationToken);
            var dtos = _mapper.Map<List<UnitQueryDto>>(items);

            var paginatedResponse = new PaginatedResponse<IEnumerable<UnitQueryDto>>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return BaseResponse<PaginatedResponse<IEnumerable<UnitQueryDto>>>.SuccessResult(paginatedResponse);
        }
        catch (Exception ex)
        {
            return BaseResponse<PaginatedResponse<IEnumerable<UnitQueryDto>>>.ErrorResult($"Error retrieving all units: {ex.Message}");
        }
    }

    #endregion
}
