using Microsoft.AspNetCore.Mvc;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Hierarchy;
using Pms.Backend.Application.Interfaces;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Controller for Club hierarchy operations
/// Manages clubs (Clubes) in the organizational hierarchy
/// </summary>
[ApiController]
[Route("[controller]")]
public class HierarchyClubController : BaseController
{
    private readonly IHierarchyService _hierarchyService;

    /// <summary>
    /// Initializes a new instance of the HierarchyClubController
    /// </summary>
    /// <param name="hierarchyService">The hierarchy service</param>
    public HierarchyClubController(IHierarchyService hierarchyService)
    {
        _hierarchyService = hierarchyService;
    }

    #region Club Operations

    /// <summary>
    /// Gets a club by ID
    /// </summary>
    /// <param name="id">Club ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Club data</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<ClubDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetClubById(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.GetClubAsync(id, cancellationToken);
        return ProcessResponse(result);
    }

    /// <summary>
    /// Gets all clubs in the system with pagination
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of all clubs</returns>
    [HttpGet]
    [ProducesResponseType(typeof(BaseResponse<PaginatedResponse<IEnumerable<ClubDto>>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllClubs(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.GetAllClubsAsync(pageNumber, pageSize, cancellationToken);
        return ProcessResponse(result);
    }

    /// <summary>
    /// Gets clubs by district ID
    /// </summary>
    /// <param name="districtId">District ID</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of clubs for the district</returns>
    [HttpGet("by-district/{districtId:guid}")]
    [ProducesResponseType(typeof(BaseResponse<PaginatedResponse<IEnumerable<ClubDto>>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetClubsByDistrictId(Guid districtId, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.GetClubsAsync(districtId, pageNumber, pageSize, cancellationToken);
        return ProcessResponse(result);
    }

    /// <summary>
    /// Creates a new club
    /// </summary>
    /// <param name="dto">Club creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created club data</returns>
    [HttpPost]
    [ProducesResponseType(typeof(BaseResponse<ClubDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateClub([FromBody] CreateClubDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.CreateClubAsync(dto, cancellationToken);
        return ProcessResponseWithCreatedAtAction(result, nameof(GetClubById), new { id = result.Data?.Id });
    }

    /// <summary>
    /// Updates an existing club
    /// </summary>
    /// <param name="id">Club ID</param>
    /// <param name="dto">Club update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated club data</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<ClubDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateClub(Guid id, [FromBody] UpdateClubDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.UpdateClubAsync(id, dto, cancellationToken);
        return ProcessResponse(result);
    }

    /// <summary>
    /// Deletes a club
    /// </summary>
    /// <param name="id">Club ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Deletion result</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteClub(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.DeleteClubAsync(id, cancellationToken);
        return ProcessResponse(result);
    }

    #endregion
}
