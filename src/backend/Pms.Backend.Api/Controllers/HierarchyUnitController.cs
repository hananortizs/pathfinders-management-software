using Microsoft.AspNetCore.Mvc;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Hierarchy;
using Pms.Backend.Application.Interfaces;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Controller for Unit hierarchy operations
/// Manages units (Unidades) within clubs in the organizational hierarchy
/// </summary>
[ApiController]
[Route("[controller]")]
public class HierarchyUnitController : BaseController
{
    private readonly IHierarchyService _hierarchyService;

    /// <summary>
    /// Initializes a new instance of the HierarchyUnitController
    /// </summary>
    /// <param name="hierarchyService">The hierarchy service</param>
    public HierarchyUnitController(IHierarchyService hierarchyService)
    {
        _hierarchyService = hierarchyService;
    }

    #region Unit Operations

    /// <summary>
    /// Gets a unit by ID
    /// </summary>
    /// <param name="id">Unit ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Unit data</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<UnitDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUnitById(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.GetUnitAsync(id, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets all units in the system with pagination
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of all units</returns>
    [HttpGet]
    [ProducesResponseType(typeof(BaseResponse<PaginatedResponse<IEnumerable<UnitDto>>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUnits(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.GetAllUnitsAsync(pageNumber, pageSize, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets units by club ID
    /// </summary>
    /// <param name="clubId">Club ID</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of units for the club</returns>
    [HttpGet("by-club/{clubId:guid}")]
    [ProducesResponseType(typeof(BaseResponse<PaginatedResponse<IEnumerable<UnitDto>>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUnitsByClubId(Guid clubId, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.GetUnitsAsync(clubId, pageNumber, pageSize, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new unit
    /// </summary>
    /// <param name="dto">Unit creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created unit data</returns>
    [HttpPost]
    [ProducesResponseType(typeof(BaseResponse<UnitDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUnit([FromBody] CreateUnitDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.CreateUnitAsync(dto, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return CreatedAtAction(
            nameof(GetUnitById),
            new { id = result.Data!.Id },
            result);
    }

    /// <summary>
    /// Updates an existing unit
    /// </summary>
    /// <param name="id">Unit ID</param>
    /// <param name="dto">Unit update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated unit data</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<UnitDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUnit(Guid id, [FromBody] UpdateUnitDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.UpdateUnitAsync(id, dto, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a unit
    /// </summary>
    /// <param name="id">Unit ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Deletion result</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUnit(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.DeleteUnitAsync(id, cancellationToken);
        return Ok(result);
    }

    #endregion
}
