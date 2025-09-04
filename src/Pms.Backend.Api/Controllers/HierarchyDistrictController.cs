using Microsoft.AspNetCore.Mvc;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Hierarchy;
using Pms.Backend.Application.Interfaces;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Controller for District hierarchy operations
/// Manages districts (Distritos) in the organizational hierarchy
/// </summary>
[ApiController]
[Route("[controller]")]
public class HierarchyDistrictController : ControllerBase
{
    private readonly IHierarchyService _hierarchyService;

    /// <summary>
    /// Initializes a new instance of the HierarchyDistrictController
    /// </summary>
    /// <param name="hierarchyService">The hierarchy service</param>
    public HierarchyDistrictController(IHierarchyService hierarchyService)
    {
        _hierarchyService = hierarchyService;
    }

    #region District Operations

    /// <summary>
    /// Gets a district by ID
    /// </summary>
    /// <param name="id">District ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>District data</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<DistrictDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDistrictById(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.GetDistrictAsync(id, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets all districts in the system with pagination
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of all districts</returns>
    [HttpGet]
    [ProducesResponseType(typeof(BaseResponse<PaginatedResponse<IEnumerable<DistrictDto>>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllDistricts(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.GetAllDistrictsAsync(pageNumber, pageSize, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets districts by region ID
    /// </summary>
    /// <param name="regionId">Region ID</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of districts for the region</returns>
    [HttpGet("by-region/{regionId:guid}")]
    [ProducesResponseType(typeof(BaseResponse<PaginatedResponse<IEnumerable<DistrictDto>>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDistrictsByRegionId(Guid regionId, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.GetDistrictsAsync(regionId, pageNumber, pageSize, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new district
    /// </summary>
    /// <param name="dto">District creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created district data</returns>
    [HttpPost]
    [ProducesResponseType(typeof(BaseResponse<DistrictDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDistrict([FromBody] CreateDistrictDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.CreateDistrictAsync(dto, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return CreatedAtAction(
            nameof(GetDistrictById),
            new { id = result.Data!.Id },
            result);
    }

    /// <summary>
    /// Updates an existing district
    /// </summary>
    /// <param name="id">District ID</param>
    /// <param name="dto">District update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated district data</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<DistrictDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateDistrict(Guid id, [FromBody] UpdateDistrictDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.UpdateDistrictAsync(id, dto, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a district
    /// </summary>
    /// <param name="id">District ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Deletion result</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDistrict(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.DeleteDistrictAsync(id, cancellationToken);
        return Ok(result);
    }

    #endregion
}
