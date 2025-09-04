using Microsoft.AspNetCore.Mvc;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Hierarchy;
using Pms.Backend.Application.Interfaces;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Controller for Union hierarchy operations
/// </summary>
[ApiController]
[Route("[controller]")]
public class HierarchyUnionController : ControllerBase
{
    private readonly IHierarchyService _hierarchyService;

    /// <summary>
    /// Initializes a new instance of the HierarchyUnionController
    /// </summary>
    /// <param name="hierarchyService">The hierarchy service</param>
    public HierarchyUnionController(IHierarchyService hierarchyService)
    {
        _hierarchyService = hierarchyService;
    }

    #region Union Operations

    /// <summary>
    /// Gets a union by ID
    /// </summary>
    /// <param name="id">Union ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Union data</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<UnionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUnionById(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.GetUnionAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Gets all unions in the system
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of unions</returns>
    [HttpGet]
    [ProducesResponseType(typeof(BaseResponse<PaginatedResponse<IEnumerable<UnionSummaryDto>>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUnions(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.GetAllUnionsAsync(pageNumber, pageSize, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets unions by division ID
    /// </summary>
    /// <param name="divisionId">Division ID</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of unions</returns>
    [HttpGet("by-division/{divisionId:guid}")]
    [ProducesResponseType(typeof(BaseResponse<PaginatedResponse<IEnumerable<UnionSummaryDto>>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUnionsByDivisionId(Guid divisionId, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.GetUnionsAsync(divisionId, pageNumber, pageSize, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new union
    /// </summary>
    /// <param name="dto">Union creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created union data</returns>
    [HttpPost]
    [ProducesResponseType(typeof(BaseResponse<UnionDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUnion([FromBody] CreateUnionDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.CreateUnionAsync(dto, cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(GetUnionById), new { id = result.Data?.Id }, result) : BadRequest(result);
    }

    /// <summary>
    /// Updates an existing union
    /// </summary>
    /// <param name="id">Union ID</param>
    /// <param name="dto">Union update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated union data</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<UnionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUnion(Guid id, [FromBody] UpdateUnionDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.UpdateUnionAsync(id, dto, cancellationToken);
        return result.IsSuccess ? Ok(result) : (result.Message?.Contains("not found") == true ? NotFound(result) : BadRequest(result));
    }

    /// <summary>
    /// Deletes a union
    /// </summary>
    /// <param name="id">Union ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Deletion result</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUnion(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.DeleteUnionAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : (result.Message?.Contains("not found") == true ? NotFound(result) : BadRequest(result));
    }

    #endregion
}
