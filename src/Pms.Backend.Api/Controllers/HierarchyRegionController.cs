using Microsoft.AspNetCore.Mvc;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Hierarchy;
using Pms.Backend.Application.Interfaces;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Controller for Region and District hierarchy operations
/// </summary>
[ApiController]
[Route("api/hierarchy")]
public class HierarchyRegionController : ControllerBase
{
    private readonly IHierarchyService _hierarchyService;

    /// <summary>
    /// Initializes a new instance of the HierarchyRegionController
    /// </summary>
    /// <param name="hierarchyService">The hierarchy service</param>
    public HierarchyRegionController(IHierarchyService hierarchyService)
    {
        _hierarchyService = hierarchyService;
    }

    #region Region Operations

    /// <summary>
    /// Gets a region by ID
    /// </summary>
    /// <param name="id">Region ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Region data</returns>
    [HttpGet("regions/{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<RegionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRegionById(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.GetRegionAsync(id, cancellationToken);
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Gets all regions
    /// </summary>
    /// <returns>List of regions</returns>
    [HttpGet("regions")]
    [ProducesResponseType(typeof(BaseResponse<IEnumerable<RegionDto>>), StatusCodes.Status200OK)]
    public IActionResult GetAllRegions()
    {
        // Note: This endpoint would need an associationId parameter or a different service method
        return BadRequest("This endpoint requires an associationId parameter. Use GET /associations/{associationId}/regions instead.");
    }

    /// <summary>
    /// Gets regions by association ID
    /// </summary>
    /// <param name="associationId">Association ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of regions</returns>
    [HttpGet("associations/{associationId:guid}/regions")]
    [ProducesResponseType(typeof(BaseResponse<IEnumerable<RegionDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRegionsByAssociationId(Guid associationId, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.GetRegionsAsync(associationId, 1, 100, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new region
    /// </summary>
    /// <param name="dto">Region creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created region data</returns>
    [HttpPost("regions")]
    [ProducesResponseType(typeof(BaseResponse<RegionDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRegion([FromBody] CreateRegionDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.CreateRegionAsync(dto, cancellationToken);
        return result.Success ? CreatedAtAction(nameof(GetRegionById), new { id = result.Data?.Id }, result) : BadRequest(result);
    }

    /// <summary>
    /// Updates an existing region
    /// </summary>
    /// <param name="id">Region ID</param>
    /// <param name="dto">Region update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated region data</returns>
    [HttpPut("regions/{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<RegionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRegion(Guid id, [FromBody] UpdateRegionDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.UpdateRegionAsync(id, dto, cancellationToken);
        return result.Success ? Ok(result) : (result.Message?.Contains("not found") == true ? NotFound(result) : BadRequest(result));
    }

    /// <summary>
    /// Deletes a region
    /// </summary>
    /// <param name="id">Region ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Deletion result</returns>
    [HttpDelete("regions/{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRegion(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.DeleteRegionAsync(id, cancellationToken);
        return result.Success ? Ok(result) : (result.Message?.Contains("not found") == true ? NotFound(result) : BadRequest(result));
    }

    #endregion

    #region District Operations

    /// <summary>
    /// Gets a district by ID
    /// </summary>
    /// <param name="id">District ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>District data</returns>
    [HttpGet("districts/{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<DistrictDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDistrictById(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.GetDistrictAsync(id, cancellationToken);
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Gets all districts
    /// </summary>
    /// <returns>List of districts</returns>
    [HttpGet("districts")]
    [ProducesResponseType(typeof(BaseResponse<IEnumerable<DistrictDto>>), StatusCodes.Status200OK)]
    public IActionResult GetAllDistricts()
    {
        // Note: This endpoint would need a regionId parameter or a different service method
        return BadRequest("This endpoint requires a regionId parameter. Use GET /regions/{regionId}/districts instead.");
    }

    /// <summary>
    /// Gets districts by region ID
    /// </summary>
    /// <param name="regionId">Region ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of districts</returns>
    [HttpGet("regions/{regionId:guid}/districts")]
    [ProducesResponseType(typeof(BaseResponse<IEnumerable<DistrictDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDistrictsByRegionId(Guid regionId, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.GetDistrictsAsync(regionId, 1, 100, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new district
    /// </summary>
    /// <param name="dto">District creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created district data</returns>
    [HttpPost("districts")]
    [ProducesResponseType(typeof(BaseResponse<DistrictDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDistrict([FromBody] CreateDistrictDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.CreateDistrictAsync(dto, cancellationToken);
        return result.Success ? CreatedAtAction(nameof(GetDistrictById), new { id = result.Data?.Id }, result) : BadRequest(result);
    }

    /// <summary>
    /// Updates an existing district
    /// </summary>
    /// <param name="id">District ID</param>
    /// <param name="dto">District update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated district data</returns>
    [HttpPut("districts/{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<DistrictDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateDistrict(Guid id, [FromBody] UpdateDistrictDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.UpdateDistrictAsync(id, dto, cancellationToken);
        return result.Success ? Ok(result) : (result.Message?.Contains("not found") == true ? NotFound(result) : BadRequest(result));
    }

    /// <summary>
    /// Deletes a district
    /// </summary>
    /// <param name="id">District ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Deletion result</returns>
    [HttpDelete("districts/{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDistrict(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.DeleteDistrictAsync(id, cancellationToken);
        return result.Success ? Ok(result) : (result.Message?.Contains("not found") == true ? NotFound(result) : BadRequest(result));
    }

    #endregion
}
