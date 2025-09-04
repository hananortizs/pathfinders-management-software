using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Address;

namespace Pms.Backend.Application.Interfaces;

/// <summary>
/// Service interface for Address operations
/// </summary>
public interface IAddressService
{
    /// <summary>
    /// Gets an address by its ID
    /// </summary>
    /// <param name="id">Address ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Address data or error response</returns>
    Task<BaseResponse<AddressDto>> GetAddressAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all addresses for a specific entity
    /// </summary>
    /// <param name="entityId">Entity ID</param>
    /// <param name="entityType">Entity type</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of addresses or error response</returns>
    Task<BaseResponse<IEnumerable<AddressDto>>> GetAddressesByEntityAsync(Guid entityId, string entityType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the primary address for a specific entity
    /// </summary>
    /// <param name="entityId">Entity ID</param>
    /// <param name="entityType">Entity type</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Primary address or error response</returns>
    Task<BaseResponse<AddressDto?>> GetPrimaryAddressAsync(Guid entityId, string entityType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new address
    /// </summary>
    /// <param name="dto">Address creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created address or error response</returns>
    Task<BaseResponse<AddressDto>> CreateAddressAsync(CreateAddressDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing address
    /// </summary>
    /// <param name="id">Address ID</param>
    /// <param name="dto">Address update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated address or error response</returns>
    Task<BaseResponse<AddressDto>> UpdateAddressAsync(Guid id, UpdateAddressDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an address
    /// </summary>
    /// <param name="id">Address ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success status or error response</returns>
    Task<BaseResponse<bool>> DeleteAddressAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets an address as primary for an entity
    /// </summary>
    /// <param name="id">Address ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success status or error response</returns>
    Task<BaseResponse<bool>> SetAsPrimaryAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets addresses by CEP (postal code)
    /// </summary>
    /// <param name="cep">CEP to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of addresses with the specified CEP</returns>
    Task<BaseResponse<IEnumerable<AddressDto>>> GetAddressesByCepAsync(string cep, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets addresses by city and state
    /// </summary>
    /// <param name="city">City name</param>
    /// <param name="state">State name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of addresses in the specified city and state</returns>
    Task<BaseResponse<IEnumerable<AddressDto>>> GetAddressesByLocationAsync(string city, string state, CancellationToken cancellationToken = default);
}
