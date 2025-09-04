using AutoMapper;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Hierarchy;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Domain.Entities.Hierarchy;

namespace Pms.Backend.Application.Services.Hierarchy;

/// <summary>
/// Main service for hierarchy CRUD operations (Create, Read, Update, Delete)
/// Handles Division, Union, Association, Region, District, Church, Club, and Unit operations
/// Optimized for CRUD operations without nested hierarchies for better performance
/// </summary>
public partial class HierarchyService : IHierarchyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the HierarchyService
    /// </summary>
    /// <param name="unitOfWork">Unit of work for data access</param>
    /// <param name="mapper">AutoMapper instance for object mapping</param>
    public HierarchyService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    #region Division Operations

    /// <summary>
    /// Retrieves a specific Division by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the Division.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the DivisionDto if found, or an error.</returns>
    public async Task<BaseResponse<DivisionDto>> GetDivisionAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var division = await _unitOfWork.Repository<Division>().GetByIdAsync(id, cancellationToken);
            if (division == null)
            {
                return BaseResponse<DivisionDto>.ErrorResult("Division not found");
            }

            var dto = _mapper.Map<DivisionDto>(division);
            return BaseResponse<DivisionDto>.SuccessResult(dto);
        }
        catch (Exception ex)
        {
            return BaseResponse<DivisionDto>.ErrorResult($"Error retrieving division: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves a paginated list of Divisions for CRUD operations (without nested hierarchies).
    /// For rich queries with nested data, use HierarchyQueryService.
    /// </summary>
    /// <param name="pageNumber">The page number for pagination (default is 1).</param>
    /// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing a PaginatedResponse of DivisionSummaryDto.</returns>
    public async Task<BaseResponse<PaginatedResponse<IEnumerable<DivisionSummaryDto>>>> GetDivisionsAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            // Get divisions without related entities for better performance
            var (items, totalCount) = await _unitOfWork.Repository<Division>().GetPagedAsync(pageNumber, pageSize, null, cancellationToken);
            var dtos = _mapper.Map<IEnumerable<DivisionSummaryDto>>(items);

            var paginatedResponse = new PaginatedResponse<IEnumerable<DivisionSummaryDto>>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return BaseResponse<PaginatedResponse<IEnumerable<DivisionSummaryDto>>>.SuccessResult(paginatedResponse);
        }
        catch (Exception ex)
        {
            return BaseResponse<PaginatedResponse<IEnumerable<DivisionSummaryDto>>>.ErrorResult($"Error retrieving divisions: {ex.Message}");
        }
    }

    /// <summary>
    /// Creates a new Division.
    /// </summary>
    /// <param name="dto">The data transfer object containing the Division's details.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the created DivisionDto if successful, or an error.</returns>
    public async Task<BaseResponse<DivisionDto>> CreateDivisionAsync(CreateDivisionDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if code already exists
            var existingDivision = await _unitOfWork.Repository<Division>().ExistsAsync(d => d.Code == dto.Code, cancellationToken);
            if (existingDivision)
            {
                return BaseResponse<DivisionDto>.ErrorResult("Division code already exists");
            }

            var division = _mapper.Map<Division>(dto);
            division.Id = Guid.NewGuid();
            division.CreatedAtUtc = DateTime.UtcNow;
            division.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.Repository<Division>().AddAsync(division, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var resultDto = _mapper.Map<DivisionDto>(division);
            return BaseResponse<DivisionDto>.SuccessResult(resultDto, "Division created successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<DivisionDto>.ErrorResult($"Error creating division: {ex.Message}");
        }
    }

    /// <summary>
    /// Updates an existing Division.
    /// </summary>
    /// <param name="id">The unique identifier of the Division to update.</param>
    /// <param name="dto">The data transfer object containing the updated Division's details.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the updated DivisionDto if successful, or an error.</returns>
    public async Task<BaseResponse<DivisionDto>> UpdateDivisionAsync(Guid id, UpdateDivisionDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var division = await _unitOfWork.Repository<Division>().GetByIdAsync(id, cancellationToken);
            if (division == null)
            {
                return BaseResponse<DivisionDto>.ErrorResult("Division not found");
            }

            // Check if code already exists (excluding current division)
            var existingDivision = await _unitOfWork.Repository<Division>().ExistsAsync(d => d.Code == dto.Code && d.Id != id, cancellationToken);
            if (existingDivision)
            {
                return BaseResponse<DivisionDto>.ErrorResult("Division code already exists");
            }

            _mapper.Map(dto, division);
            division.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.Repository<Division>().UpdateAsync(division, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var resultDto = _mapper.Map<DivisionDto>(division);
            return BaseResponse<DivisionDto>.SuccessResult(resultDto, "Division updated successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<DivisionDto>.ErrorResult($"Error updating division: {ex.Message}");
        }
    }

    /// <summary>
    /// Deletes a Division by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the Division to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse indicating success or failure of the deletion.</returns>
    public async Task<BaseResponse<bool>> DeleteDivisionAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if division has child unions
            var hasUnions = await _unitOfWork.Repository<Union>().ExistsAsync(u => u.DivisionId == id, cancellationToken);
            if (hasUnions)
            {
                return BaseResponse<bool>.ErrorResult("Cannot delete division with existing unions");
            }

            var deleted = await _unitOfWork.Repository<Division>().DeleteAsync(id, cancellationToken);
            if (!deleted)
            {
                return BaseResponse<bool>.ErrorResult("Division not found");
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return BaseResponse<bool>.SuccessResult(true, "Division deleted successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.ErrorResult($"Error deleting division: {ex.Message}");
        }
    }

    #endregion

    #region Union Operations

    /// <summary>
    /// Retrieves a specific Union by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the Union.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the UnionDto if found, or an error.</returns>
    public async Task<BaseResponse<UnionDto>> GetUnionAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var union = await _unitOfWork.Repository<Union>().GetByIdAsync(id, cancellationToken);
            if (union == null)
            {
                return BaseResponse<UnionDto>.ErrorResult("Union not found");
            }

            var dto = _mapper.Map<UnionDto>(union);
            return BaseResponse<UnionDto>.SuccessResult(dto);
        }
        catch (Exception ex)
        {
            return BaseResponse<UnionDto>.ErrorResult($"Error retrieving union: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves a paginated list of all Unions in the system.
    /// </summary>
    /// <param name="pageNumber">The page number for pagination (default is 1).</param>
    /// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing a PaginatedResponse of UnionDto.</returns>
    public async Task<BaseResponse<PaginatedResponse<IEnumerable<UnionDto>>>> GetAllUnionsAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            var (items, totalCount) = await _unitOfWork.Repository<Union>().GetPagedAsync(pageNumber, pageSize, null, cancellationToken);
            var dtos = _mapper.Map<List<UnionDto>>(items);

            var paginatedResponse = new PaginatedResponse<IEnumerable<UnionDto>>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return BaseResponse<PaginatedResponse<IEnumerable<UnionDto>>>.SuccessResult(paginatedResponse);
        }
        catch (Exception ex)
        {
            return BaseResponse<PaginatedResponse<IEnumerable<UnionDto>>>.ErrorResult($"Error retrieving all unions: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves a paginated list of Unions belonging to a specific Division.
    /// </summary>
    /// <param name="divisionId">The ID of the parent Division.</param>
    /// <param name="pageNumber">The page number for pagination (default is 1).</param>
    /// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing a PaginatedResponse of UnionDto.</returns>
    public async Task<BaseResponse<PaginatedResponse<IEnumerable<UnionDto>>>> GetUnionsAsync(Guid divisionId, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            var (items, totalCount) = await _unitOfWork.Repository<Union>().GetPagedAsync(pageNumber, pageSize, u => u.DivisionId == divisionId, cancellationToken);
            var dtos = _mapper.Map<IEnumerable<UnionDto>>(items);

            var paginatedResponse = new PaginatedResponse<IEnumerable<UnionDto>>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return BaseResponse<PaginatedResponse<IEnumerable<UnionDto>>>.SuccessResult(paginatedResponse);
        }
        catch (Exception ex)
        {
            return BaseResponse<PaginatedResponse<IEnumerable<UnionDto>>>.ErrorResult($"Error retrieving unions: {ex.Message}");
        }
    }

    /// <summary>
    /// Creates a new Union.
    /// </summary>
    /// <param name="dto">The data transfer object containing the Union's details.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the created UnionDto if successful, or an error.</returns>
    public async Task<BaseResponse<UnionDto>> CreateUnionAsync(CreateUnionDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            // Verify parent division exists
            var division = await _unitOfWork.Repository<Division>().GetByIdAsync(dto.DivisionId, cancellationToken);
            if (division == null)
            {
                return BaseResponse<UnionDto>.ErrorResult("Parent division not found");
            }

            // Check if code already exists within the division
            var existingUnion = await _unitOfWork.Repository<Union>().ExistsAsync(u => u.Code == dto.Code && u.DivisionId == dto.DivisionId, cancellationToken);
            if (existingUnion)
            {
                return BaseResponse<UnionDto>.ErrorResult("Union code already exists in this division");
            }

            var union = _mapper.Map<Union>(dto);
            union.Id = Guid.NewGuid();
            union.CreatedAtUtc = DateTime.UtcNow;
            union.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.Repository<Union>().AddAsync(union, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var resultDto = _mapper.Map<UnionDto>(union);
            return BaseResponse<UnionDto>.SuccessResult(resultDto, "Union created successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<UnionDto>.ErrorResult($"Error creating union: {ex.Message}");
        }
    }

    /// <summary>
    /// Updates an existing Union.
    /// </summary>
    /// <param name="id">The unique identifier of the Union to update.</param>
    /// <param name="dto">The data transfer object containing the updated Union's details.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the updated UnionDto if successful, or an error.</returns>
    public async Task<BaseResponse<UnionDto>> UpdateUnionAsync(Guid id, UpdateUnionDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var union = await _unitOfWork.Repository<Union>().GetByIdAsync(id, cancellationToken);
            if (union == null)
            {
                return BaseResponse<UnionDto>.ErrorResult("Union not found");
            }

            // Check if code already exists within the division (excluding current union)
            var existingUnion = await _unitOfWork.Repository<Union>().ExistsAsync(u => u.Code == dto.Code && u.DivisionId == union.DivisionId && u.Id != id, cancellationToken);
            if (existingUnion)
            {
                return BaseResponse<UnionDto>.ErrorResult("Union code already exists in this division");
            }

            _mapper.Map(dto, union);
            union.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.Repository<Union>().UpdateAsync(union, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var resultDto = _mapper.Map<UnionDto>(union);
            return BaseResponse<UnionDto>.SuccessResult(resultDto, "Union updated successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<UnionDto>.ErrorResult($"Error updating union: {ex.Message}");
        }
    }

    /// <summary>
    /// Deletes a Union by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the Union to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse indicating success or failure of the deletion.</returns>
    public async Task<BaseResponse<bool>> DeleteUnionAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if union has child associations
            var hasAssociations = await _unitOfWork.Repository<Association>().ExistsAsync(a => a.UnionId == id, cancellationToken);
            if (hasAssociations)
            {
                return BaseResponse<bool>.ErrorResult("Cannot delete union with existing associations");
            }

            var deleted = await _unitOfWork.Repository<Union>().DeleteAsync(id, cancellationToken);
            if (!deleted)
            {
                return BaseResponse<bool>.ErrorResult("Union not found");
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return BaseResponse<bool>.SuccessResult(true, "Union deleted successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.ErrorResult($"Error deleting union: {ex.Message}");
        }
    }

    #endregion
}
