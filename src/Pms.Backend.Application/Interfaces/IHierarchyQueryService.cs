using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Hierarchy;

namespace Pms.Backend.Application.Interfaces;

/// <summary>
/// Service interface for rich hierarchy queries with related entities
/// Optimized for reporting and dashboard operations with complete hierarchy data
/// </summary>
public interface IHierarchyQueryService
{
    #region Division Queries

    /// <summary>
    /// Retrieves a rich list of all divisions with their related entities (unions)
    /// </summary>
    /// <param name="pageNumber">The page number for pagination (default is 1)</param>
    /// <param name="pageSize">The number of items per page (default is 10)</param>
    /// <param name="cancellationToken">A token to cancel the operation</param>
    /// <returns>A BaseResponse containing a PaginatedResponse of DivisionQueryDto with unions</returns>
    Task<BaseResponse<PaginatedResponse<IEnumerable<DivisionQueryDto>>>> GetAllDivisionsAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    #endregion

    #region Union Queries

    /// <summary>
    /// Retrieves a rich list of all unions with their related entities (associations)
    /// </summary>
    /// <param name="pageNumber">The page number for pagination (default is 1)</param>
    /// <param name="pageSize">The number of items per page (default is 10)</param>
    /// <param name="cancellationToken">A token to cancel the operation</param>
    /// <returns>A BaseResponse containing a PaginatedResponse of UnionQueryDto with associations</returns>
    Task<BaseResponse<PaginatedResponse<IEnumerable<UnionQueryDto>>>> GetAllUnionsAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    #endregion

    #region Association Queries

    /// <summary>
    /// Retrieves a rich list of all associations with their related entities (regions)
    /// </summary>
    /// <param name="pageNumber">The page number for pagination (default is 1)</param>
    /// <param name="pageSize">The number of items per page (default is 10)</param>
    /// <param name="cancellationToken">A token to cancel the operation</param>
    /// <returns>A BaseResponse containing a PaginatedResponse of AssociationQueryDto with regions</returns>
    Task<BaseResponse<PaginatedResponse<IEnumerable<AssociationQueryDto>>>> GetAllAssociationsAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    #endregion

    #region Region Queries

    /// <summary>
    /// Retrieves a rich list of all regions with their related entities (districts)
    /// </summary>
    /// <param name="pageNumber">The page number for pagination (default is 1)</param>
    /// <param name="pageSize">The number of items per page (default is 10)</param>
    /// <param name="cancellationToken">A token to cancel the operation</param>
    /// <returns>A BaseResponse containing a PaginatedResponse of RegionQueryDto with districts</returns>
    Task<BaseResponse<PaginatedResponse<IEnumerable<RegionQueryDto>>>> GetAllRegionsAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    #endregion

    #region District Queries

    /// <summary>
    /// Retrieves a rich list of all districts with their related entities (clubs)
    /// </summary>
    /// <param name="pageNumber">The page number for pagination (default is 1)</param>
    /// <param name="pageSize">The number of items per page (default is 10)</param>
    /// <param name="cancellationToken">A token to cancel the operation</param>
    /// <returns>A BaseResponse containing a PaginatedResponse of DistrictQueryDto with clubs</returns>
    Task<BaseResponse<PaginatedResponse<IEnumerable<DistrictQueryDto>>>> GetAllDistrictsAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    #endregion

    #region Club Queries

    /// <summary>
    /// Retrieves a rich list of all clubs with their related entities (units)
    /// </summary>
    /// <param name="pageNumber">The page number for pagination (default is 1)</param>
    /// <param name="pageSize">The number of items per page (default is 10)</param>
    /// <param name="cancellationToken">A token to cancel the operation</param>
    /// <returns>A BaseResponse containing a PaginatedResponse of ClubQueryDto with units</returns>
    Task<BaseResponse<PaginatedResponse<IEnumerable<ClubQueryDto>>>> GetAllClubsAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    #endregion

    #region Unit Queries

    /// <summary>
    /// Retrieves a list of all units (leaf level - no child hierarchy)
    /// </summary>
    /// <param name="pageNumber">The page number for pagination (default is 1)</param>
    /// <param name="pageSize">The number of items per page (default is 10)</param>
    /// <param name="cancellationToken">A token to cancel the operation</param>
    /// <returns>A BaseResponse containing a PaginatedResponse of UnitQueryDto</returns>
    Task<BaseResponse<PaginatedResponse<IEnumerable<UnitQueryDto>>>> GetAllUnitsAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    #endregion
}
