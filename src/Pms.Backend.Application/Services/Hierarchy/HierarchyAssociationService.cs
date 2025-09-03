using AutoMapper;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Hierarchy;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Domain.Entities.Hierarchy;

namespace Pms.Backend.Application.Services.Hierarchy;

/// <summary>
/// Service for Association and Region operations
/// </summary>
public partial class HierarchyService
{
    #region Association Operations

    /// <summary>
    /// Retrieves a specific Association by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the Association.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the AssociationDto if found, or an error.</returns>
    public async Task<BaseResponse<AssociationDto>> GetAssociationAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var association = await _unitOfWork.Repository<Association>().GetByIdAsync(id, cancellationToken);
            if (association == null)
            {
                return BaseResponse<AssociationDto>.ErrorResult("Association not found");
            }

            var dto = _mapper.Map<AssociationDto>(association);
            return BaseResponse<AssociationDto>.SuccessResult(dto);
        }
        catch (Exception ex)
        {
            return BaseResponse<AssociationDto>.ErrorResult($"Error retrieving association: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves a paginated list of Associations belonging to a specific Union.
    /// </summary>
    /// <param name="unionId">The ID of the parent Union.</param>
    /// <param name="pageNumber">The page number for pagination (default is 1).</param>
    /// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing a PaginatedResponse of AssociationDto.</returns>
    public async Task<BaseResponse<PaginatedResponse<IEnumerable<AssociationDto>>>> GetAssociationsAsync(Guid unionId, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            var (items, totalCount) = await _unitOfWork.Repository<Association>().GetPagedAsync(pageNumber, pageSize, a => a.UnionId == unionId, cancellationToken);
            var dtos = _mapper.Map<IEnumerable<AssociationDto>>(items);

            var paginatedResponse = new PaginatedResponse<IEnumerable<AssociationDto>>
            {
                Data = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return BaseResponse<PaginatedResponse<IEnumerable<AssociationDto>>>.SuccessResult(paginatedResponse);
        }
        catch (Exception ex)
        {
            return BaseResponse<PaginatedResponse<IEnumerable<AssociationDto>>>.ErrorResult($"Error retrieving associations: {ex.Message}");
        }
    }

    /// <summary>
    /// Creates a new Association.
    /// </summary>
    /// <param name="dto">The data transfer object containing the Association's details.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the created AssociationDto if successful, or an error.</returns>
    public async Task<BaseResponse<AssociationDto>> CreateAssociationAsync(CreateAssociationDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            // Verify parent union exists
            var union = await _unitOfWork.Repository<Union>().GetByIdAsync(dto.UnionId, cancellationToken);
            if (union == null)
            {
                return BaseResponse<AssociationDto>.ErrorResult("Parent union not found");
            }

            // Check if code already exists within the union
            var existingAssociation = await _unitOfWork.Repository<Association>().ExistsAsync(a => a.Code == dto.Code && a.UnionId == dto.UnionId, cancellationToken);
            if (existingAssociation)
            {
                return BaseResponse<AssociationDto>.ErrorResult("Association code already exists in this union");
            }

            var association = _mapper.Map<Association>(dto);
            association.Id = Guid.NewGuid();
            association.CreatedAtUtc = DateTime.UtcNow;
            association.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.Repository<Association>().AddAsync(association, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var resultDto = _mapper.Map<AssociationDto>(association);
            return BaseResponse<AssociationDto>.SuccessResult(resultDto, "Association created successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<AssociationDto>.ErrorResult($"Error creating association: {ex.Message}");
        }
    }

    /// <summary>
    /// Updates an existing Association.
    /// </summary>
    /// <param name="id">The unique identifier of the Association to update.</param>
    /// <param name="dto">The data transfer object containing the updated Association's details.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the updated AssociationDto if successful, or an error.</returns>
    public async Task<BaseResponse<AssociationDto>> UpdateAssociationAsync(Guid id, UpdateAssociationDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var association = await _unitOfWork.Repository<Association>().GetByIdAsync(id, cancellationToken);
            if (association == null)
            {
                return BaseResponse<AssociationDto>.ErrorResult("Association not found");
            }

            // Check if code already exists within the union (excluding current association)
            var existingAssociation = await _unitOfWork.Repository<Association>().ExistsAsync(a => a.Code == dto.Code && a.UnionId == association.UnionId && a.Id != id, cancellationToken);
            if (existingAssociation)
            {
                return BaseResponse<AssociationDto>.ErrorResult("Association code already exists in this union");
            }

            _mapper.Map(dto, association);
            association.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.Repository<Association>().UpdateAsync(association, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var resultDto = _mapper.Map<AssociationDto>(association);
            return BaseResponse<AssociationDto>.SuccessResult(resultDto, "Association updated successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<AssociationDto>.ErrorResult($"Error updating association: {ex.Message}");
        }
    }

    /// <summary>
    /// Deletes an Association by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the Association to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse indicating success or failure of the deletion.</returns>
    public async Task<BaseResponse<bool>> DeleteAssociationAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if association has child regions
            var hasRegions = await _unitOfWork.Repository<Region>().ExistsAsync(r => r.AssociationId == id, cancellationToken);
            if (hasRegions)
            {
                return BaseResponse<bool>.ErrorResult("Cannot delete association with existing regions");
            }

            var deleted = await _unitOfWork.Repository<Association>().DeleteAsync(id, cancellationToken);
            if (!deleted)
            {
                return BaseResponse<bool>.ErrorResult("Association not found");
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return BaseResponse<bool>.SuccessResult(true, "Association deleted successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.ErrorResult($"Error deleting association: {ex.Message}");
        }
    }

    #endregion

    #region Region Operations

    /// <summary>
    /// Retrieves a specific Region by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the Region.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the RegionDto if found, or an error.</returns>
    public async Task<BaseResponse<RegionDto>> GetRegionAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var region = await _unitOfWork.Repository<Region>().GetByIdAsync(id, cancellationToken);
            if (region == null)
            {
                return BaseResponse<RegionDto>.ErrorResult("Region not found");
            }

            var dto = _mapper.Map<RegionDto>(region);
            return BaseResponse<RegionDto>.SuccessResult(dto);
        }
        catch (Exception ex)
        {
            return BaseResponse<RegionDto>.ErrorResult($"Error retrieving region: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves a paginated list of Regions belonging to a specific Association.
    /// </summary>
    /// <param name="associationId">The ID of the parent Association.</param>
    /// <param name="pageNumber">The page number for pagination (default is 1).</param>
    /// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing a PaginatedResponse of RegionDto.</returns>
    public async Task<BaseResponse<PaginatedResponse<IEnumerable<RegionDto>>>> GetRegionsAsync(Guid associationId, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            var (items, totalCount) = await _unitOfWork.Repository<Region>().GetPagedAsync(pageNumber, pageSize, r => r.AssociationId == associationId, cancellationToken);
            var dtos = _mapper.Map<IEnumerable<RegionDto>>(items);

            var paginatedResponse = new PaginatedResponse<IEnumerable<RegionDto>>
            {
                Data = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return BaseResponse<PaginatedResponse<IEnumerable<RegionDto>>>.SuccessResult(paginatedResponse);
        }
        catch (Exception ex)
        {
            return BaseResponse<PaginatedResponse<IEnumerable<RegionDto>>>.ErrorResult($"Error retrieving regions: {ex.Message}");
        }
    }

    /// <summary>
    /// Creates a new Region.
    /// </summary>
    /// <param name="dto">The data transfer object containing the Region's details.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the created RegionDto if successful, or an error.</returns>
    public async Task<BaseResponse<RegionDto>> CreateRegionAsync(CreateRegionDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            // Verify parent association exists
            var association = await _unitOfWork.Repository<Association>().GetByIdAsync(dto.AssociationId, cancellationToken);
            if (association == null)
            {
                return BaseResponse<RegionDto>.ErrorResult("Parent association not found");
            }

            // Check if code already exists within the association
            var existingRegion = await _unitOfWork.Repository<Region>().ExistsAsync(r => r.Code == dto.Code && r.AssociationId == dto.AssociationId, cancellationToken);
            if (existingRegion)
            {
                return BaseResponse<RegionDto>.ErrorResult("Region code already exists in this association");
            }

            var region = _mapper.Map<Region>(dto);
            region.Id = Guid.NewGuid();
            region.CreatedAtUtc = DateTime.UtcNow;
            region.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.Repository<Region>().AddAsync(region, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var resultDto = _mapper.Map<RegionDto>(region);
            return BaseResponse<RegionDto>.SuccessResult(resultDto, "Region created successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<RegionDto>.ErrorResult($"Error creating region: {ex.Message}");
        }
    }

    /// <summary>
    /// Updates an existing Region.
    /// </summary>
    /// <param name="id">The unique identifier of the Region to update.</param>
    /// <param name="dto">The data transfer object containing the updated Region's details.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the updated RegionDto if successful, or an error.</returns>
    public async Task<BaseResponse<RegionDto>> UpdateRegionAsync(Guid id, UpdateRegionDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var region = await _unitOfWork.Repository<Region>().GetByIdAsync(id, cancellationToken);
            if (region == null)
            {
                return BaseResponse<RegionDto>.ErrorResult("Region not found");
            }

            // Check if code already exists within the association (excluding current region)
            var existingRegion = await _unitOfWork.Repository<Region>().ExistsAsync(r => r.Code == dto.Code && r.AssociationId == region.AssociationId && r.Id != id, cancellationToken);
            if (existingRegion)
            {
                return BaseResponse<RegionDto>.ErrorResult("Region code already exists in this association");
            }

            _mapper.Map(dto, region);
            region.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.Repository<Region>().UpdateAsync(region, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var resultDto = _mapper.Map<RegionDto>(region);
            return BaseResponse<RegionDto>.SuccessResult(resultDto, "Region updated successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<RegionDto>.ErrorResult($"Error updating region: {ex.Message}");
        }
    }

    /// <summary>
    /// Deletes a Region by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the Region to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse indicating success or failure of the deletion.</returns>
    public async Task<BaseResponse<bool>> DeleteRegionAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if region has child districts
            var hasDistricts = await _unitOfWork.Repository<District>().ExistsAsync(d => d.RegionId == id, cancellationToken);
            if (hasDistricts)
            {
                return BaseResponse<bool>.ErrorResult("Cannot delete region with existing districts");
            }

            var deleted = await _unitOfWork.Repository<Region>().DeleteAsync(id, cancellationToken);
            if (!deleted)
            {
                return BaseResponse<bool>.ErrorResult("Region not found");
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return BaseResponse<bool>.SuccessResult(true, "Region deleted successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.ErrorResult($"Error deleting region: {ex.Message}");
        }
    }

    #endregion
}
