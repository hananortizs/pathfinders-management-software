using Microsoft.AspNetCore.Mvc;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Hierarchy;
using Pms.Backend.Application.Interfaces;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Controller for Church hierarchy operations
/// Manages churches (Igrejas) in the organizational hierarchy
/// </summary>
[ApiController]
[Route("[controller]")]
public class HierarchyChurchController : ControllerBase
{
    private readonly IHierarchyService _hierarchyService;

    /// <summary>
    /// Initializes a new instance of the HierarchyChurchController
    /// </summary>
    /// <param name="hierarchyService">The hierarchy service</param>
    public HierarchyChurchController(IHierarchyService hierarchyService)
    {
        _hierarchyService = hierarchyService;
    }

    #region Church Operations

    /// <summary>
    /// Gets a church by ID
    /// </summary>
    /// <param name="id">Church ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Church data</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<ChurchDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetChurchById(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.GetChurchAsync(id, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets all churches in the system with pagination
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of all churches</returns>
    [HttpGet]
    [ProducesResponseType(typeof(BaseResponse<PaginatedResponse<IEnumerable<ChurchDto>>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllChurches(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.GetChurchesAsync(pageNumber, pageSize, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets churches by district ID
    /// </summary>
    /// <param name="districtId">District ID</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of churches for the district</returns>
    [HttpGet("by-district/{districtId:guid}")]
    [ProducesResponseType(typeof(BaseResponse<PaginatedResponse<IEnumerable<ChurchDto>>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetChurchesByDistrictId(Guid districtId, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        // Note: This method would need to be implemented in the service to filter by district
        // For now, we'll return all churches
        var result = await _hierarchyService.GetChurchesAsync(pageNumber, pageSize, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new church
    /// </summary>
    /// <param name="dto">Church creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created church data</returns>
    [HttpPost]
    [ProducesResponseType(typeof(BaseResponse<ChurchDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateChurch([FromBody] CreateChurchDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.CreateChurchAsync(dto, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return CreatedAtAction(
            nameof(GetChurchById),
            new { id = result.Data!.Id },
            result);
    }

    /// <summary>
    /// Updates an existing church
    /// </summary>
    /// <param name="id">Church ID</param>
    /// <param name="dto">Church update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated church data</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<ChurchDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateChurch(Guid id, [FromBody] UpdateChurchDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.UpdateChurchAsync(id, dto, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a church
    /// </summary>
    /// <param name="id">Church ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Deletion result</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteChurch(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.DeleteChurchAsync(id, cancellationToken);
        return Ok(result);
    }

    #endregion
}