using Microsoft.AspNetCore.Mvc;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Address;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Domain.Enums;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Controller for Address operations
/// Manages addresses for any entity in the system (Members, Churches, etc.)
/// </summary>
[ApiController]
[Route("[controller]")]
public class AddressController : ControllerBase
{
    private readonly IAddressService _addressService;

    /// <summary>
    /// Initializes a new instance of the AddressController
    /// </summary>
    /// <param name="addressService">The address service</param>
    public AddressController(IAddressService addressService)
    {
        _addressService = addressService;
    }

    #region Entity Type Operations

    /// <summary>
    /// Gets all valid entity types that can have addresses
    /// </summary>
    /// <returns>List of valid entity types</returns>
    [HttpGet("entity-types")]
    [ProducesResponseType(typeof(BaseResponse<IEnumerable<object>>), StatusCodes.Status200OK)]
    public IActionResult GetValidEntityTypes()
    {
        var entityTypes = EntityTypeHelper.ValidEntityTypes
            .Select(et => new
            {
                Value = et,
                DisplayName = EntityTypeHelper.GetDisplayName(et),
                Description = GetEntityTypeDescription(et)
            })
            .ToList();

        var response = BaseResponse<IEnumerable<object>>.SuccessResult(entityTypes);
        return Ok(response);
    }

    /// <summary>
    /// Gets entity type description
    /// </summary>
    /// <param name="entityType">Entity type name</param>
    /// <returns>Description of the entity type</returns>
    private static string GetEntityTypeDescription(string entityType)
    {
        return entityType switch
        {
            "Member" => "Membro do clube de desbravadores",
            "Church" => "Igreja local",
            "Club" => "Clube de desbravadores",
            "Unit" => "Unidade dentro de um clube",
            "District" => "Distrito da igreja",
            "Association" => "Associação da igreja",
            "Union" => "União da igreja",
            "Division" => "Divisão da igreja",
            "Region" => "Região da igreja",
            _ => "Tipo de entidade do sistema"
        };
    }

    #endregion

    #region Address Operations

    /// <summary>
    /// Gets an address by ID
    /// </summary>
    /// <param name="id">Address ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Address data</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<AddressDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAddressById(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _addressService.GetAddressAsync(id, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets all addresses for a specific entity
    /// </summary>
    /// <param name="entityId">Entity ID</param>
    /// <param name="entityType">Entity type (Member, Church, etc.)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of addresses for the entity</returns>
    [HttpGet("by-entity/{entityId:guid}")]
    [ProducesResponseType(typeof(BaseResponse<IEnumerable<AddressDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAddressesByEntity(Guid entityId, [FromQuery] string entityType, CancellationToken cancellationToken = default)
    {
        var result = await _addressService.GetAddressesByEntityAsync(entityId, entityType, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets the primary address for a specific entity
    /// </summary>
    /// <param name="entityId">Entity ID</param>
    /// <param name="entityType">Entity type (Member, Church, etc.)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Primary address for the entity</returns>
    [HttpGet("primary/{entityId:guid}")]
    [ProducesResponseType(typeof(BaseResponse<AddressDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPrimaryAddress(Guid entityId, [FromQuery] string entityType, CancellationToken cancellationToken = default)
    {
        var result = await _addressService.GetPrimaryAddressAsync(entityId, entityType, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new address
    /// </summary>
    /// <param name="dto">Address creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created address data</returns>
    [HttpPost]
    [ProducesResponseType(typeof(BaseResponse<AddressDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAddress([FromBody] CreateAddressDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _addressService.CreateAddressAsync(dto, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return CreatedAtAction(
            nameof(GetAddressById),
            new { id = result.Data!.Id },
            result);
    }

    /// <summary>
    /// Updates an existing address
    /// </summary>
    /// <param name="id">Address ID</param>
    /// <param name="dto">Address update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated address data</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<AddressDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAddress(Guid id, [FromBody] UpdateAddressDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _addressService.UpdateAddressAsync(id, dto, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Deletes an address
    /// </summary>
    /// <param name="id">Address ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Deletion result</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAddress(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _addressService.DeleteAddressAsync(id, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Sets an address as primary for its entity
    /// </summary>
    /// <param name="id">Address ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success status</returns>
    [HttpPut("{id:guid}/set-primary")]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SetAsPrimary(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _addressService.SetAsPrimaryAsync(id, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets addresses by CEP (postal code)
    /// </summary>
    /// <param name="cep">CEP to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of addresses with the specified CEP</returns>
    [HttpGet("by-cep/{cep}")]
    [ProducesResponseType(typeof(BaseResponse<IEnumerable<AddressDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAddressesByCep(string cep, CancellationToken cancellationToken = default)
    {
        var result = await _addressService.GetAddressesByCepAsync(cep, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets addresses by city and state
    /// </summary>
    /// <param name="city">City name</param>
    /// <param name="state">State name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of addresses in the specified city and state</returns>
    [HttpGet("by-location")]
    [ProducesResponseType(typeof(BaseResponse<IEnumerable<AddressDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAddressesByLocation([FromQuery] string city, [FromQuery] string state, CancellationToken cancellationToken = default)
    {
        var result = await _addressService.GetAddressesByLocationAsync(city, state, cancellationToken);
        return Ok(result);
    }

    #endregion
}
