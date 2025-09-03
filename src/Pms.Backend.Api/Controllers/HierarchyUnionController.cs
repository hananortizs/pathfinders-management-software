using Microsoft.AspNetCore.Mvc;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Hierarchy;
using Pms.Backend.Application.Interfaces;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Controller for Union and Association hierarchy operations
/// </summary>
[ApiController]
[Route("api/hierarchy")]
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
    [HttpGet("unions/{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<UnionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUnionById(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.GetUnionAsync(id, cancellationToken);
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Gets all unions
    /// </summary>
    /// <returns>List of unions</returns>
    [HttpGet("unions")]
    [ProducesResponseType(typeof(BaseResponse<IEnumerable<UnionDto>>), StatusCodes.Status200OK)]
    public IActionResult GetAllUnions()
    {
        // Note: This endpoint would need a divisionId parameter or a different service method
        return BadRequest("This endpoint requires a divisionId parameter. Use GET /divisions/{divisionId}/unions instead.");
    }

    /// <summary>
    /// Gets unions by division ID
    /// </summary>
    /// <param name="divisionId">Division ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of unions</returns>
    [HttpGet("divisions/{divisionId:guid}/unions")]
    [ProducesResponseType(typeof(BaseResponse<IEnumerable<UnionDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUnionsByDivisionId(Guid divisionId, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.GetUnionsAsync(divisionId, 1, 100, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new union
    /// </summary>
    /// <param name="dto">Union creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created union data</returns>
    [HttpPost("unions")]
    [ProducesResponseType(typeof(BaseResponse<UnionDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUnion([FromBody] CreateUnionDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.CreateUnionAsync(dto, cancellationToken);
        return result.Success ? CreatedAtAction(nameof(GetUnionById), new { id = result.Data?.Id }, result) : BadRequest(result);
    }

    /// <summary>
    /// Updates an existing union
    /// </summary>
    /// <param name="id">Union ID</param>
    /// <param name="dto">Union update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated union data</returns>
    [HttpPut("unions/{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<UnionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUnion(Guid id, [FromBody] UpdateUnionDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.UpdateUnionAsync(id, dto, cancellationToken);
        return result.Success ? Ok(result) : (result.Message?.Contains("not found") == true ? NotFound(result) : BadRequest(result));
    }

    /// <summary>
    /// Deletes a union
    /// </summary>
    /// <param name="id">Union ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Deletion result</returns>
    [HttpDelete("unions/{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUnion(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.DeleteUnionAsync(id, cancellationToken);
        return result.Success ? Ok(result) : (result.Message?.Contains("not found") == true ? NotFound(result) : BadRequest(result));
    }

    #endregion

    #region Association Operations

    /// <summary>
    /// Gets an association by ID
    /// </summary>
    /// <param name="id">Association ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Association data</returns>
    [HttpGet("associations/{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<AssociationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAssociationById(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.GetAssociationAsync(id, cancellationToken);
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Gets all associations
    /// </summary>
    /// <returns>List of associations</returns>
    [HttpGet("associations")]
    [ProducesResponseType(typeof(BaseResponse<IEnumerable<AssociationDto>>), StatusCodes.Status200OK)]
    public IActionResult GetAllAssociations()
    {
        // Note: This endpoint would need a unionId parameter or a different service method
        return BadRequest("This endpoint requires a unionId parameter. Use GET /unions/{unionId}/associations instead.");
    }

    /// <summary>
    /// Gets associations by union ID
    /// </summary>
    /// <param name="unionId">Union ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of associations</returns>
    [HttpGet("unions/{unionId:guid}/associations")]
    [ProducesResponseType(typeof(BaseResponse<IEnumerable<AssociationDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAssociationsByUnionId(Guid unionId, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.GetAssociationsAsync(unionId, 1, 100, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new association
    /// </summary>
    /// <param name="dto">Association creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created association data</returns>
    [HttpPost("associations")]
    [ProducesResponseType(typeof(BaseResponse<AssociationDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAssociation([FromBody] CreateAssociationDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.CreateAssociationAsync(dto, cancellationToken);
        return result.Success ? CreatedAtAction(nameof(GetAssociationById), new { id = result.Data?.Id }, result) : BadRequest(result);
    }

    /// <summary>
    /// Updates an existing association
    /// </summary>
    /// <param name="id">Association ID</param>
    /// <param name="dto">Association update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated association data</returns>
    [HttpPut("associations/{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<AssociationDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAssociation(Guid id, [FromBody] UpdateAssociationDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.UpdateAssociationAsync(id, dto, cancellationToken);
        return result.Success ? Ok(result) : (result.Message?.Contains("not found") == true ? NotFound(result) : BadRequest(result));
    }

    /// <summary>
    /// Deletes an association
    /// </summary>
    /// <param name="id">Association ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Deletion result</returns>
    [HttpDelete("associations/{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAssociation(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.DeleteAssociationAsync(id, cancellationToken);
        return result.Success ? Ok(result) : (result.Message?.Contains("not found") == true ? NotFound(result) : BadRequest(result));
    }

    #endregion
}
