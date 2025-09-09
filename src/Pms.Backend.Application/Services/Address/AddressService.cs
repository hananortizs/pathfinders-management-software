using AutoMapper;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Address;
using Pms.Backend.Application.Helpers;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Domain.Enums;
using Pms.Backend.Domain.Exceptions;
using Pms.Backend.Domain.Helpers;
using AddressEntity = Pms.Backend.Domain.Entities.Address;

namespace Pms.Backend.Application.Services.Address;

/// <summary>
/// Service for managing addresses in the system
/// </summary>
public class AddressService : IAddressService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAddressValidationService _validationService;

    /// <summary>
    /// Initializes a new instance of the AddressService
    /// </summary>
    /// <param name="unitOfWork">Unit of work instance</param>
    /// <param name="mapper">AutoMapper instance</param>
    /// <param name="validationService">Address validation service</param>
    public AddressService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IAddressValidationService validationService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validationService = validationService;
    }

    /// <summary>
    /// Gets an address by its ID
    /// </summary>
    /// <param name="id">Address ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Address data or error response</returns>
    public async Task<BaseResponse<AddressDto>> GetAddressAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var address = await _unitOfWork.Repository<AddressEntity>()
            .GetFirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, cancellationToken);

        if (address == null)
        {
            ExceptionHelper.ThrowNotFoundException<AddressEntity>(id);
        }

        var addressDto = _mapper.Map<AddressDto>(address);
        return BaseResponse<AddressDto>.SuccessResult(addressDto);
    }

    /// <summary>
    /// Gets all addresses for a specific entity
    /// </summary>
    /// <param name="entityId">Entity ID</param>
    /// <param name="entityType">Entity type</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of addresses or error response</returns>
    public async Task<BaseResponse<IEnumerable<AddressDto>>> GetAddressesByEntityAsync(Guid entityId, string entityType, CancellationToken cancellationToken = default)
    {
        // Validate entity type
        if (!EntityTypeHelper.IsValidEntityType(entityType))
        {
            ExceptionHelper.ThrowValidationException("EntityType", "Tipo de entidade inválido", entityType);
        }

        // Validate entity exists
        var entityExists = await _validationService.ValidateEntityExistsAsync(entityId, entityType, cancellationToken);
        if (!entityExists)
        {
            ExceptionHelper.ThrowNotFoundException("Entity", entityId, $"Entidade do tipo '{entityType}' não encontrada");
        }

        var addresses = await _unitOfWork.Repository<AddressEntity>()
            .GetAllAsync(a => a.EntityId == entityId && a.EntityType == entityType && !a.IsDeleted, cancellationToken);

        // Order by primary first, then by creation date
        var orderedAddresses = addresses
            .OrderBy(a => a.IsPrimary ? 0 : 1)
            .ThenBy(a => a.CreatedAtUtc);

        var addressDtos = _mapper.Map<IEnumerable<AddressDto>>(orderedAddresses);
        return BaseResponse<IEnumerable<AddressDto>>.SuccessResult(addressDtos);
    }

    /// <summary>
    /// Gets the primary address for a specific entity
    /// </summary>
    /// <param name="entityId">Entity ID</param>
    /// <param name="entityType">Entity type</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Primary address or error response</returns>
    public async Task<BaseResponse<AddressDto?>> GetPrimaryAddressAsync(Guid entityId, string entityType, CancellationToken cancellationToken = default)
    {
        // Validate entity type
        if (!EntityTypeHelper.IsValidEntityType(entityType))
        {
            ExceptionHelper.ThrowValidationException("EntityType", "Tipo de entidade inválido", entityType);
        }

        // Validate entity exists
        var entityExists = await _validationService.ValidateEntityExistsAsync(entityId, entityType, cancellationToken);
        if (!entityExists)
        {
            ExceptionHelper.ThrowNotFoundException("Entity", entityId, $"Entidade do tipo '{entityType}' não encontrada");
        }

        var address = await _unitOfWork.Repository<AddressEntity>()
            .GetFirstOrDefaultAsync(a => a.EntityId == entityId &&
                                       a.EntityType == entityType &&
                                       a.IsPrimary &&
                                       !a.IsDeleted, cancellationToken);

        AddressDto? addressDto = null;
        if (address != null)
        {
            addressDto = _mapper.Map<AddressDto>(address);
        }

        return BaseResponse<AddressDto?>.SuccessResult(addressDto);
    }

    /// <summary>
    /// Creates a new address
    /// </summary>
    /// <param name="dto">Address creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created address or error response</returns>
    public async Task<BaseResponse<AddressDto>> CreateAddressAsync(CreateAddressDto dto, CancellationToken cancellationToken = default)
    {
        // Validate entity type
        if (!EntityTypeHelper.IsValidEntityType(dto.EntityType))
        {
            ExceptionHelper.ThrowValidationException("EntityType", "Tipo de entidade inválido", dto.EntityType);
        }

        // Validate entity exists
        var entityExists = await _validationService.ValidateEntityExistsAsync(dto.EntityId, dto.EntityType, cancellationToken);
        if (!entityExists)
        {
            ExceptionHelper.ThrowNotFoundException("Entity", dto.EntityId, $"Entidade do tipo '{dto.EntityType}' não encontrada");
        }

        // If this is set as primary, unset other primary addresses for this entity
        if (dto.IsPrimary)
        {
            await UnsetPrimaryAddressesAsync(dto.EntityId, dto.EntityType, cancellationToken);
        }

        var address = _mapper.Map<AddressEntity>(dto);
        address.CreatedAtUtc = DateTime.UtcNow;
        address.UpdatedAtUtc = DateTime.UtcNow;

        await _unitOfWork.Repository<AddressEntity>().AddAsync(address, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var addressDto = _mapper.Map<AddressDto>(address);
        return BaseResponse<AddressDto>.SuccessResult(addressDto);
    }

    /// <summary>
    /// Updates an existing address
    /// </summary>
    /// <param name="id">Address ID</param>
    /// <param name="dto">Address update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated address or error response</returns>
    public async Task<BaseResponse<AddressDto>> UpdateAddressAsync(Guid id, UpdateAddressDto dto, CancellationToken cancellationToken = default)
    {
        var address = await _unitOfWork.Repository<AddressEntity>()
            .GetFirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, cancellationToken);

        if (address == null)
        {
            ExceptionHelper.ThrowNotFoundException<AddressEntity>(id);
            return BaseResponse<AddressDto>.ErrorResult("Address not found"); // This line will never be reached
        }

        // If this is set as primary, unset other primary addresses for this entity
        if (dto.IsPrimary && !address.IsPrimary)
        {
            await UnsetPrimaryAddressesAsync(address.EntityId, address.EntityType, cancellationToken);
        }

        _mapper.Map(dto, address);
        address.UpdatedAtUtc = DateTime.UtcNow;

        await _unitOfWork.Repository<AddressEntity>().UpdateAsync(address, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var addressDto = _mapper.Map<AddressDto>(address);
        return BaseResponse<AddressDto>.SuccessResult(addressDto);
    }

    /// <summary>
    /// Deletes an address
    /// </summary>
    /// <param name="id">Address ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success status or error response</returns>
    public async Task<BaseResponse<bool>> DeleteAddressAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var address = await _unitOfWork.Repository<AddressEntity>()
            .GetFirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, cancellationToken);

        if (address == null)
        {
            ExceptionHelper.ThrowNotFoundException<AddressEntity>(id);
            return BaseResponse<bool>.ErrorResult("Address not found"); // This line will never be reached
        }

        // Soft delete
        address.IsDeleted = true;
        address.DeletedAtUtc = DateTime.UtcNow;
        address.UpdatedAtUtc = DateTime.UtcNow;

        await _unitOfWork.Repository<AddressEntity>().UpdateAsync(address, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponse<bool>.SuccessResult(true);
    }

    /// <summary>
    /// Sets an address as primary for an entity
    /// </summary>
    /// <param name="id">Address ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success status or error response</returns>
    public async Task<BaseResponse<bool>> SetAsPrimaryAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var address = await _unitOfWork.Repository<AddressEntity>()
            .GetFirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, cancellationToken);

        if (address == null)
        {
            ExceptionHelper.ThrowNotFoundException<AddressEntity>(id);
            return BaseResponse<bool>.ErrorResult("Address not found"); // This line will never be reached
        }

        // Unset other primary addresses for this entity
        await UnsetPrimaryAddressesAsync(address.EntityId, address.EntityType, cancellationToken);

        // Set this address as primary
        address.IsPrimary = true;
        address.UpdatedAtUtc = DateTime.UtcNow;

        await _unitOfWork.Repository<AddressEntity>().UpdateAsync(address, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return BaseResponse<bool>.SuccessResult(true);
    }

    /// <summary>
    /// Gets addresses by CEP (postal code)
    /// </summary>
    /// <param name="cep">CEP to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of addresses with the specified CEP</returns>
    public async Task<BaseResponse<IEnumerable<AddressDto>>> GetAddressesByCepAsync(string cep, CancellationToken cancellationToken = default)
    {
        var normalizedCep = CepHelper.NormalizeCep(cep);
        if (normalizedCep == null)
        {
            ExceptionHelper.ThrowValidationException("Cep", "CEP inválido", cep);
        }

        var addresses = await _unitOfWork.Repository<AddressEntity>()
            .GetAllAsync(a => a.Cep == normalizedCep && !a.IsDeleted, cancellationToken);

        // Order by primary first, then by creation date
        var orderedAddresses = addresses
            .OrderBy(a => a.IsPrimary ? 0 : 1)
            .ThenBy(a => a.CreatedAtUtc);

        var addressDtos = _mapper.Map<IEnumerable<AddressDto>>(orderedAddresses);
        return BaseResponse<IEnumerable<AddressDto>>.SuccessResult(addressDtos);
    }

    /// <summary>
    /// Gets addresses by city and state
    /// </summary>
    /// <param name="city">City name</param>
    /// <param name="state">State name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of addresses in the specified city and state</returns>
    public async Task<BaseResponse<IEnumerable<AddressDto>>> GetAddressesByLocationAsync(string city, string state, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            ExceptionHelper.ThrowValidationException("City", "Nome da cidade é obrigatório", city);
        }

        if (string.IsNullOrWhiteSpace(state))
        {
            ExceptionHelper.ThrowValidationException("State", "Nome do estado é obrigatório", state);
        }

        var addresses = await _unitOfWork.Repository<AddressEntity>()
            .GetAllAsync(a => a.City.ToLower() == city.ToLower() &&
                            a.State.ToLower() == state.ToLower() &&
                            !a.IsDeleted, cancellationToken);

        // Order by primary first, then by creation date
        var orderedAddresses = addresses
            .OrderBy(a => a.IsPrimary ? 0 : 1)
            .ThenBy(a => a.CreatedAtUtc);

        var addressDtos = _mapper.Map<IEnumerable<AddressDto>>(orderedAddresses);
        return BaseResponse<IEnumerable<AddressDto>>.SuccessResult(addressDtos);
    }

    /// <summary>
    /// Unsets primary status for all addresses of a specific entity
    /// </summary>
    /// <param name="entityId">Entity ID</param>
    /// <param name="entityType">Entity type</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the async operation</returns>
    private async Task UnsetPrimaryAddressesAsync(Guid entityId, string entityType, CancellationToken cancellationToken = default)
    {
        var primaryAddresses = await _unitOfWork.Repository<AddressEntity>()
            .GetAllAsync(a => a.EntityId == entityId &&
                           a.EntityType == entityType &&
                           a.IsPrimary &&
                           !a.IsDeleted, cancellationToken);

        foreach (var address in primaryAddresses)
        {
            address.IsPrimary = false;
            address.UpdatedAtUtc = DateTime.UtcNow;
            await _unitOfWork.Repository<AddressEntity>().UpdateAsync(address, cancellationToken);
        }
    }
}
