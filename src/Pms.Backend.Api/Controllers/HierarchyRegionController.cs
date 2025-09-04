using Microsoft.AspNetCore.Mvc;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Hierarchy;
using Pms.Backend.Application.Interfaces;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Controller for Region hierarchy operations
/// Manages regions (Regiões) in the organizational hierarchy
/// </summary>
[ApiController]
[Route("[controller]")]
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
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<RegionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRegionById(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.GetRegionAsync(id, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets regions by association ID
    /// </summary>
    /// <param name="associationId">Association ID</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of regions for the association</returns>
    [HttpGet("by-association/{associationId:guid}")]
    [ProducesResponseType(typeof(BaseResponse<PaginatedResponse<IEnumerable<RegionSummaryDto>>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRegionsByAssociationId(Guid associationId, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.GetRegionsAsync(associationId, pageNumber, pageSize, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new region
    /// </summary>
    /// <param name="dto">Region creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created region data</returns>
    [HttpPost]
    [ProducesResponseType(typeof(BaseResponse<RegionDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRegion([FromBody] CreateRegionDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.CreateRegionAsync(dto, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return CreatedAtAction(
            nameof(GetRegionById),
            new { id = result.Data!.Id },
            result);
    }

    /// <summary>
    /// Updates an existing region
    /// </summary>
    /// <param name="id">Region ID</param>
    /// <param name="dto">Region update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated region data</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<RegionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRegion(Guid id, [FromBody] UpdateRegionDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.UpdateRegionAsync(id, dto, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a region
    /// </summary>
    /// <param name="id">Region ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Deletion result</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteRegion(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.DeleteRegionAsync(id, cancellationToken);
        return Ok(result);
    }

    #endregion
}
