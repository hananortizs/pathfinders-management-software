using Microsoft.AspNetCore.Mvc;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Hierarchy;
using Pms.Backend.Application.Interfaces;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Controller for Division hierarchy CRUD operations (Create, Read, Update, Delete)
/// Optimized for CRUD operations without nested hierarchies for better performance
/// For rich queries with nested data, use HierarchyQueryController
/// </summary>
[ApiController]
[Route("[controller]")]
public class HierarchyController : ControllerBase
{
    private readonly IHierarchyService _hierarchyService;

    /// <summary>
    /// Initializes a new instance of the HierarchyController
    /// </summary>
    /// <param name="hierarchyService">The hierarchy service</param>
    public HierarchyController(IHierarchyService hierarchyService)
    {
        _hierarchyService = hierarchyService;
    }

    #region Division Operations

    /// <summary>
    /// Gets a division by ID
    /// </summary>
    /// <param name="id">Division ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Division data</returns>
    [HttpGet("divisions/{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<DivisionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDivisionById(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.GetDivisionAsync(id, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets all divisions for CRUD operations (without nested hierarchies)
    /// For rich queries with unions, use /hierarchy-query/divisions
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of divisions without nested hierarchies</returns>
    [HttpGet("divisions")]
    [ProducesResponseType(typeof(BaseResponse<PaginatedResponse<IEnumerable<DivisionSummaryDto>>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllDivisions(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.GetDivisionsAsync(pageNumber, pageSize, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new division
    /// </summary>
    /// <param name="dto">Division creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created division data</returns>
    [HttpPost("divisions")]
    [ProducesResponseType(typeof(BaseResponse<DivisionDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDivision([FromBody] CreateDivisionDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.CreateDivisionAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetDivisionById), new { id = result.Data?.Id }, result);
    }

    /// <summary>
    /// Updates an existing division
    /// </summary>
    /// <param name="id">Division ID</param>
    /// <param name="dto">Division update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated division data</returns>
    [HttpPut("divisions/{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<DivisionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateDivision(Guid id, [FromBody] UpdateDivisionDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.UpdateDivisionAsync(id, dto, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a division
    /// </summary>
    /// <param name="id">Division ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Deletion result</returns>
    [HttpDelete("divisions/{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDivision(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.DeleteDivisionAsync(id, cancellationToken);
        return Ok(result);
    }

    #endregion
}
