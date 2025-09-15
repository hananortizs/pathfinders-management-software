using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Assignments;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Application.Services.Assignments;

/// <summary>
/// Service for managing role catalog operations
/// Handles CRUD operations for roles and validation
/// </summary>
public class RoleCatalogService : IRoleCatalogService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILoggingService _loggingService;

    /// <summary>
    /// Initializes a new instance of the RoleCatalogService
    /// </summary>
    /// <param name="unitOfWork">The unit of work</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="loggingService">The logging service</param>
    public RoleCatalogService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILoggingService loggingService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _loggingService = loggingService;
    }

    /// <summary>
    /// Creates a new role in the catalog
    /// </summary>
    public async Task<BaseResponse<RoleCatalogDto>> CreateRoleAsync(CreateRoleCatalogDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            _loggingService.LogInformation("Creating new role: {RoleName} for level {Level}", request.Name, request.Level);

            // Validate role
            var validationResult = ValidateRole(request);
            if (!validationResult.IsSuccess)
            {
                return BaseResponse<RoleCatalogDto>.ErrorResult(validationResult.Message ?? "Role validation failed");
            }

            // Check if role name already exists for this level
            var existingRole = await _unitOfWork.Repository<RoleCatalog>()
                .GetFirstOrDefaultAsync(r => r.Name == request.Name && r.Level == request.Level, cancellationToken);

            if (existingRole != null)
            {
                return BaseResponse<RoleCatalogDto>.ErrorResult($"Role '{request.Name}' already exists for level '{request.Level}'");
            }

            // Create role
            var role = _mapper.Map<RoleCatalog>(request);
            role.Id = Guid.NewGuid();
            role.CreatedAtUtc = DateTime.UtcNow;
            role.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.Repository<RoleCatalog>().AddAsync(role, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _loggingService.LogInformation("Role created successfully: {RoleId} - {RoleName}", role.Id, role.Name);

            var resultDto = _mapper.Map<RoleCatalogDto>(role);
            return BaseResponse<RoleCatalogDto>.SuccessResult(resultDto, "Role created successfully");
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error creating role: {RoleName}", request.Name);
            return BaseResponse<RoleCatalogDto>.ErrorResult($"Error creating role: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets a role by ID
    /// </summary>
    public async Task<BaseResponse<RoleCatalogDto>> GetRoleByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var role = await _unitOfWork.Repository<RoleCatalog>().GetByIdAsync(id, cancellationToken);
            if (role == null)
            {
                return BaseResponse<RoleCatalogDto>.ErrorResult("Role not found");
            }

            var resultDto = _mapper.Map<RoleCatalogDto>(role);
            return BaseResponse<RoleCatalogDto>.SuccessResult(resultDto);
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error retrieving role: {RoleId}", id);
            return BaseResponse<RoleCatalogDto>.ErrorResult($"Error retrieving role: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets all roles with pagination
    /// </summary>
    public async Task<BaseResponse<PaginatedResponse<IEnumerable<RoleCatalogDto>>>> GetAllRolesAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            var roles = await _unitOfWork.Repository<RoleCatalog>()
                .GetAllAsync(null, cancellationToken);

            var totalCount = roles.Count();
            var pagedRoles = roles
                .OrderBy(r => r.Level)
                .ThenBy(r => r.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var resultDtos = _mapper.Map<IEnumerable<RoleCatalogDto>>(pagedRoles);

            var paginatedResponse = new PaginatedResponse<IEnumerable<RoleCatalogDto>>
            {
                Items = resultDtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return BaseResponse<PaginatedResponse<IEnumerable<RoleCatalogDto>>>.SuccessResult(paginatedResponse);
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error retrieving roles");
            return BaseResponse<PaginatedResponse<IEnumerable<RoleCatalogDto>>>.ErrorResult($"Error retrieving roles: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets roles by hierarchy level
    /// </summary>
    public async Task<BaseResponse<IEnumerable<RoleCatalogDto>>> GetRolesByLevelAsync(RoleLevel level, CancellationToken cancellationToken = default)
    {
        try
        {
            var roles = await _unitOfWork.Repository<RoleCatalog>()
                .GetAllAsync(r => r.Level == level, cancellationToken);

            var resultDtos = _mapper.Map<IEnumerable<RoleCatalogDto>>(roles);
            return BaseResponse<IEnumerable<RoleCatalogDto>>.SuccessResult(resultDtos);
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error retrieving roles for level: {Level}", level);
            return BaseResponse<IEnumerable<RoleCatalogDto>>.ErrorResult($"Error retrieving roles: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets active roles only
    /// </summary>
    public async Task<BaseResponse<IEnumerable<RoleCatalogDto>>> GetActiveRolesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var roles = await _unitOfWork.Repository<RoleCatalog>()
                .GetAllAsync(r => r.IsActive, cancellationToken);

            var resultDtos = _mapper.Map<IEnumerable<RoleCatalogDto>>(roles);
            return BaseResponse<IEnumerable<RoleCatalogDto>>.SuccessResult(resultDtos);
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error retrieving active roles");
            return BaseResponse<IEnumerable<RoleCatalogDto>>.ErrorResult($"Error retrieving active roles: {ex.Message}");
        }
    }

    /// <summary>
    /// Updates an existing role
    /// </summary>
    public async Task<BaseResponse<RoleCatalogDto>> UpdateRoleAsync(Guid id, UpdateRoleCatalogDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            _loggingService.LogInformation("Updating role: {RoleId}", id);

            var role = await _unitOfWork.Repository<RoleCatalog>().GetByIdAsync(id, cancellationToken);
            if (role == null)
            {
                return BaseResponse<RoleCatalogDto>.ErrorResult("Role not found");
            }

            // Check if role name already exists for this level (excluding current role)
            var existingRole = await _unitOfWork.Repository<RoleCatalog>()
                .GetFirstOrDefaultAsync(r => r.Name == request.Name && r.Level == role.Level && r.Id != id, cancellationToken);

            if (existingRole != null)
            {
                return BaseResponse<RoleCatalogDto>.ErrorResult($"Role '{request.Name}' already exists for level '{role.Level}'");
            }

            // Update role
            _mapper.Map(request, role);
            role.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _loggingService.LogInformation("Role updated successfully: {RoleId} - {RoleName}", role.Id, role.Name);

            var resultDto = _mapper.Map<RoleCatalogDto>(role);
            return BaseResponse<RoleCatalogDto>.SuccessResult(resultDto, "Role updated successfully");
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error updating role: {RoleId}", id);
            return BaseResponse<RoleCatalogDto>.ErrorResult($"Error updating role: {ex.Message}");
        }
    }

    /// <summary>
    /// Activates a role
    /// </summary>
    public async Task<BaseResponse<bool>> ActivateRoleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            _loggingService.LogInformation("Activating role: {RoleId}", id);

            var role = await _unitOfWork.Repository<RoleCatalog>().GetByIdAsync(id, cancellationToken);
            if (role == null)
            {
                return BaseResponse<bool>.ErrorResult("Role not found");
            }

            role.IsActive = true;
            role.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _loggingService.LogInformation("Role activated successfully: {RoleId}", id);
            return BaseResponse<bool>.SuccessResult(true, "Role activated successfully");
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error activating role: {RoleId}", id);
            return BaseResponse<bool>.ErrorResult($"Error activating role: {ex.Message}");
        }
    }

    /// <summary>
    /// Deactivates a role
    /// </summary>
    public async Task<BaseResponse<bool>> DeactivateRoleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            _loggingService.LogInformation("Deactivating role: {RoleId}", id);

            var role = await _unitOfWork.Repository<RoleCatalog>().GetByIdAsync(id, cancellationToken);
            if (role == null)
            {
                return BaseResponse<bool>.ErrorResult("Role not found");
            }

            // Check if role has active assignments
            var hasActiveAssignments = await _unitOfWork.Repository<Assignment>()
                .GetFirstOrDefaultAsync(a => a.RoleId == id && a.IsActive, cancellationToken) != null;

            if (hasActiveAssignments)
            {
                return BaseResponse<bool>.ErrorResult("Cannot deactivate role with active assignments");
            }

            role.IsActive = false;
            role.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _loggingService.LogInformation("Role deactivated successfully: {RoleId}", id);
            return BaseResponse<bool>.SuccessResult(true, "Role deactivated successfully");
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error deactivating role: {RoleId}", id);
            return BaseResponse<bool>.ErrorResult($"Error deactivating role: {ex.Message}");
        }
    }

    /// <summary>
    /// Soft deletes a role
    /// </summary>
    public async Task<BaseResponse<bool>> DeleteRoleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            _loggingService.LogInformation("Deleting role: {RoleId}", id);

            var role = await _unitOfWork.Repository<RoleCatalog>().GetByIdAsync(id, cancellationToken);
            if (role == null)
            {
                return BaseResponse<bool>.ErrorResult("Role not found");
            }

            // Check if role has any assignments
            var hasAssignments = await _unitOfWork.Repository<Assignment>()
                .GetFirstOrDefaultAsync(a => a.RoleId == id, cancellationToken) != null;

            if (hasAssignments)
            {
                return BaseResponse<bool>.ErrorResult("Cannot delete role with existing assignments");
            }

            await _unitOfWork.Repository<RoleCatalog>().DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _loggingService.LogInformation("Role deleted successfully: {RoleId}", id);
            return BaseResponse<bool>.SuccessResult(true, "Role deleted successfully");
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error deleting role: {RoleId}", id);
            return BaseResponse<bool>.ErrorResult($"Error deleting role: {ex.Message}");
        }
    }

    /// <summary>
    /// Validates if a role can be created with the given parameters
    /// </summary>
    public BaseResponse<bool> ValidateRole(CreateRoleCatalogDto request)
    {
        try
        {
            // Validate age range
            if (request.AgeMin.HasValue && request.AgeMax.HasValue && request.AgeMin > request.AgeMax)
            {
                return BaseResponse<bool>.ErrorResult("AgeMin cannot be greater than AgeMax");
            }

            // Validate max per scope
            if (request.MaxPerScope <= 0)
            {
                return BaseResponse<bool>.ErrorResult("MaxPerScope must be greater than 0");
            }

            return BaseResponse<bool>.SuccessResult(true);
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error validating role");
            return BaseResponse<bool>.ErrorResult($"Error validating role: {ex.Message}");
        }
    }
}
