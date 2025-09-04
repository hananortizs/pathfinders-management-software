using Microsoft.AspNetCore.Mvc;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Hierarchy;
using Pms.Backend.Application.Interfaces;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Controller for rich hierarchy queries with related entities
/// Optimized for reporting and dashboard operations with complete hierarchy data
/// </summary>
[ApiController]
[Route("hierarchy-query")]
public class HierarchyQueryController : ControllerBase
{
    private readonly IHierarchyQueryService _hierarchyQueryService;

    /// <summary>
    /// Initializes a new instance of the HierarchyQueryController
    /// </summary>
    /// <param name="hierarchyQueryService">The hierarchy query service</param>
    public HierarchyQueryController(IHierarchyQueryService hierarchyQueryService)
    {
        _hierarchyQueryService = hierarchyQueryService;
    }

    #region Division Queries

    /// <summary>
    /// Gets all divisions in the system (rich - with related entities like unions)
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of divisions with their unions for reporting and dashboard operations</returns>
    [HttpGet("divisions")]
    [ProducesResponseType(typeof(BaseResponse<PaginatedResponse<IEnumerable<DivisionQueryDto>>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllDivisions(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyQueryService.GetAllDivisionsAsync(pageNumber, pageSize, cancellationToken);
        return Ok(result);
    }

    #endregion

    #region Union Queries

    /// <summary>
    /// Gets all unions in the system (rich - with related entities like associations)
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of unions with their associations for reporting and dashboard operations</returns>
    [HttpGet("unions")]
    [ProducesResponseType(typeof(BaseResponse<PaginatedResponse<IEnumerable<UnionQueryDto>>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUnions(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyQueryService.GetAllUnionsAsync(pageNumber, pageSize, cancellationToken);
        return Ok(result);
    }

    #endregion

    #region Association Queries

    /// <summary>
    /// Gets all associations in the system (rich - with related entities like regions)
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of associations with their regions for reporting and dashboard operations</returns>
    [HttpGet("associations")]
    [ProducesResponseType(typeof(BaseResponse<PaginatedResponse<IEnumerable<AssociationQueryDto>>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAssociations(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyQueryService.GetAllAssociationsAsync(pageNumber, pageSize, cancellationToken);
        return Ok(result);
    }

    #endregion

    #region Region Queries

    /// <summary>
    /// Gets all regions in the system (rich - with related entities like districts)
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of regions with their districts for reporting and dashboard operations</returns>
    [HttpGet("regions")]
    [ProducesResponseType(typeof(BaseResponse<PaginatedResponse<IEnumerable<RegionQueryDto>>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllRegions(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyQueryService.GetAllRegionsAsync(pageNumber, pageSize, cancellationToken);
        return Ok(result);
    }

    #endregion

    #region District Queries

    /// <summary>
    /// Gets all districts in the system (rich - with related entities like clubs)
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of districts with their clubs for reporting and dashboard operations</returns>
    [HttpGet("districts")]
    [ProducesResponseType(typeof(BaseResponse<PaginatedResponse<IEnumerable<DistrictQueryDto>>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllDistricts(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyQueryService.GetAllDistrictsAsync(pageNumber, pageSize, cancellationToken);
        return Ok(result);
    }

    #endregion

    #region Club Queries

    /// <summary>
    /// Gets all clubs in the system (rich - with related entities like units)
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of clubs with their units for reporting and dashboard operations</returns>
    [HttpGet("clubs")]
    [ProducesResponseType(typeof(BaseResponse<PaginatedResponse<IEnumerable<ClubQueryDto>>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllClubs(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyQueryService.GetAllClubsAsync(pageNumber, pageSize, cancellationToken);
        return Ok(result);
    }

    #endregion

    #region Unit Queries

    /// <summary>
    /// Gets all units in the system (leaf level - no child hierarchy)
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of units (leaf level entities)</returns>
    [HttpGet("units")]
    [ProducesResponseType(typeof(BaseResponse<PaginatedResponse<IEnumerable<UnitQueryDto>>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUnits(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyQueryService.GetAllUnitsAsync(pageNumber, pageSize, cancellationToken);
        return Ok(result);
    }

    #endregion
}
