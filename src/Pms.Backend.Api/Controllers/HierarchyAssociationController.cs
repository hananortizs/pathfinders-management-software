using Microsoft.AspNetCore.Mvc;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Hierarchy;
using Pms.Backend.Application.Interfaces;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Controller for Association hierarchy operations
/// Manages associations (Associações) in the organizational hierarchy
/// </summary>
[ApiController]
[Route("[controller]")]
public class HierarchyAssociationController : ControllerBase
{
    private readonly IHierarchyService _hierarchyService;

    /// <summary>
    /// Initializes a new instance of the HierarchyAssociationController
    /// </summary>
    /// <param name="hierarchyService">The hierarchy service</param>
    public HierarchyAssociationController(IHierarchyService hierarchyService)
    {
        _hierarchyService = hierarchyService;
    }

    #region Association Operations

    /// <summary>
    /// Gets an association by ID
    /// </summary>
    /// <param name="id">Association ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Association data</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<AssociationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAssociationById(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.GetAssociationAsync(id, cancellationToken);
        return Ok(result);
    }


    /// <summary>
    /// Gets associations by union ID
    /// </summary>
    /// <param name="unionId">Union ID</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of associations</returns>
    [HttpGet("by-union/{unionId:guid}")]
    [ProducesResponseType(typeof(BaseResponse<PaginatedResponse<IEnumerable<AssociationSummaryDto>>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAssociationsByUnionId(Guid unionId, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.GetAssociationsAsync(unionId, pageNumber, pageSize, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new association
    /// </summary>
    /// <param name="dto">Association creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created association data</returns>
    [HttpPost]
    [ProducesResponseType(typeof(BaseResponse<AssociationDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAssociation([FromBody] CreateAssociationDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.CreateAssociationAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetAssociationById), new { id = result.Data?.Id }, result);
    }

    /// <summary>
    /// Updates an existing association
    /// </summary>
    /// <param name="id">Association ID</param>
    /// <param name="dto">Association update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated association data</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<AssociationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAssociation(Guid id, [FromBody] UpdateAssociationDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.UpdateAssociationAsync(id, dto, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Deletes an association
    /// </summary>
    /// <param name="id">Association ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Deletion result</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAssociation(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.DeleteAssociationAsync(id, cancellationToken);
        return Ok(result);
    }

    #endregion
}
