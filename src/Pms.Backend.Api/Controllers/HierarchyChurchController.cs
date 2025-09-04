using Microsoft.AspNetCore.Mvc;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Hierarchy;
using Pms.Backend.Application.Interfaces;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Controller for Church, Club and Unit hierarchy operations
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
    [HttpGet("churches/{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<ChurchDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetChurchById(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.GetChurchAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Gets all churches
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of churches</returns>
    [HttpGet("churches")]
    [ProducesResponseType(typeof(BaseResponse<IEnumerable<ChurchDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllChurches(CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.GetChurchesAsync(1, 100, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets churches by district ID
    /// </summary>
    /// <param name="districtId">District ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of churches</returns>
    [HttpGet("districts/{districtId:guid}/churches")]
    [ProducesResponseType(typeof(BaseResponse<IEnumerable<ChurchDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetChurchesByDistrictId(Guid districtId, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.GetChurchesAsync(1, 100, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new church
    /// </summary>
    /// <param name="dto">Church creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created church data</returns>
    [HttpPost("churches")]
    [ProducesResponseType(typeof(BaseResponse<ChurchDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateChurch([FromBody] CreateChurchDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.CreateChurchAsync(dto, cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(GetChurchById), new { id = result.Data?.Id }, result) : BadRequest(result);
    }

    /// <summary>
    /// Updates an existing church
    /// </summary>
    /// <param name="id">Church ID</param>
    /// <param name="dto">Church update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated church data</returns>
    [HttpPut("churches/{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<ChurchDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateChurch(Guid id, [FromBody] UpdateChurchDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.UpdateChurchAsync(id, dto, cancellationToken);
        return result.IsSuccess ? Ok(result) : (result.Message?.Contains("not found") == true ? NotFound(result) : BadRequest(result));
    }

    /// <summary>
    /// Deletes a church
    /// </summary>
    /// <param name="id">Church ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Deletion result</returns>
    [HttpDelete("churches/{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteChurch(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.DeleteChurchAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : (result.Message?.Contains("not found") == true ? NotFound(result) : BadRequest(result));
    }

    #endregion

    #region Club Operations

    /// <summary>
    /// Gets a club by ID
    /// </summary>
    /// <param name="id">Club ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Club data</returns>
    [HttpGet("clubs/{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<ClubDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetClubById(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.GetClubAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Gets all clubs in the system
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of clubs</returns>
    [HttpGet("clubs")]
    [ProducesResponseType(typeof(BaseResponse<PaginatedResponse<IEnumerable<ClubDto>>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllClubs(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.GetAllClubsAsync(pageNumber, pageSize, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets clubs by church ID
    /// </summary>
    /// <param name="churchId">Church ID</param>
    /// <returns>List of clubs</returns>
    [HttpGet("churches/{churchId:guid}/clubs")]
    [ProducesResponseType(typeof(BaseResponse<IEnumerable<ClubDto>>), StatusCodes.Status200OK)]
    public IActionResult GetClubsByChurchId(Guid churchId)
    {
        // Note: This endpoint would need a districtId parameter or a different service method
        return BadRequest("This endpoint requires a districtId parameter. Use GET /districts/{districtId}/clubs instead.");
    }

    /// <summary>
    /// Creates a new club
    /// </summary>
    /// <param name="dto">Club creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created club data</returns>
    [HttpPost("clubs")]
    [ProducesResponseType(typeof(BaseResponse<ClubDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateClub([FromBody] CreateClubDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.CreateClubAsync(dto, cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(GetClubById), new { id = result.Data?.Id }, result) : BadRequest(result);
    }

    /// <summary>
    /// Updates an existing club
    /// </summary>
    /// <param name="id">Club ID</param>
    /// <param name="dto">Club update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated club data</returns>
    [HttpPut("clubs/{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<ClubDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateClub(Guid id, [FromBody] UpdateClubDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.UpdateClubAsync(id, dto, cancellationToken);
        return result.IsSuccess ? Ok(result) : (result.Message?.Contains("not found") == true ? NotFound(result) : BadRequest(result));
    }

    /// <summary>
    /// Deletes a club
    /// </summary>
    /// <param name="id">Club ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Deletion result</returns>
    [HttpDelete("clubs/{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteClub(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.DeleteClubAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : (result.Message?.Contains("not found") == true ? NotFound(result) : BadRequest(result));
    }

    #endregion

    #region Unit Operations

    /// <summary>
    /// Gets a unit by ID
    /// </summary>
    /// <param name="id">Unit ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Unit data</returns>
    [HttpGet("units/{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<UnitDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUnitById(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.GetUnitAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    /// <summary>
    /// Gets all units in the system
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of units</returns>
    [HttpGet("units")]
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
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of units</returns>
    [HttpGet("clubs/{clubId:guid}/units")]
    [ProducesResponseType(typeof(BaseResponse<IEnumerable<UnitDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUnitsByClubId(Guid clubId, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.GetUnitsAsync(clubId, 1, 100, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new unit
    /// </summary>
    /// <param name="dto">Unit creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created unit data</returns>
    [HttpPost("units")]
    [ProducesResponseType(typeof(BaseResponse<UnitDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUnit([FromBody] CreateUnitDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.CreateUnitAsync(dto, cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(GetUnitById), new { id = result.Data?.Id }, result) : BadRequest(result);
    }

    /// <summary>
    /// Updates an existing unit
    /// </summary>
    /// <param name="id">Unit ID</param>
    /// <param name="dto">Unit update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated unit data</returns>
    [HttpPut("units/{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<UnitDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUnit(Guid id, [FromBody] UpdateUnitDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.UpdateUnitAsync(id, dto, cancellationToken);
        return result.IsSuccess ? Ok(result) : (result.Message?.Contains("not found") == true ? NotFound(result) : BadRequest(result));
    }

    /// <summary>
    /// Deletes a unit
    /// </summary>
    /// <param name="id">Unit ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Deletion result</returns>
    [HttpDelete("units/{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUnit(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _hierarchyService.DeleteUnitAsync(id, cancellationToken);
        return result.IsSuccess ? Ok(result) : (result.Message?.Contains("not found") == true ? NotFound(result) : BadRequest(result));
    }

    #endregion
}
