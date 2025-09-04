using AutoMapper;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Hierarchy;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Domain.Entities.Hierarchy;

namespace Pms.Backend.Application.Services.Hierarchy;

/// <summary>
/// Service for District, Church, Club, and Unit operations
/// </summary>
public partial class HierarchyService
{
    #region District Operations

    /// <summary>
    /// Retrieves a specific District by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the District.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the DistrictDto if found, or an error.</returns>
    public async Task<BaseResponse<DistrictDto>> GetDistrictAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var district = await _unitOfWork.Repository<District>().GetByIdAsync(id, cancellationToken);
            if (district == null)
            {
                return BaseResponse<DistrictDto>.ErrorResult("District not found");
            }

            var dto = _mapper.Map<DistrictDto>(district);
            return BaseResponse<DistrictDto>.SuccessResult(dto);
        }
        catch (Exception ex)
        {
            return BaseResponse<DistrictDto>.ErrorResult($"Error retrieving district: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves a paginated list of all Districts in the system.
    /// </summary>
    /// <param name="pageNumber">The page number for pagination (default is 1).</param>
    /// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing a PaginatedResponse of DistrictDto.</returns>
    public async Task<BaseResponse<PaginatedResponse<IEnumerable<DistrictDto>>>> GetAllDistrictsAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            var (items, totalCount) = await _unitOfWork.Repository<District>().GetPagedAsync(pageNumber, pageSize, null, cancellationToken);
            var dtos = _mapper.Map<List<DistrictDto>>(items);

            var paginatedResponse = new PaginatedResponse<IEnumerable<DistrictDto>>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return BaseResponse<PaginatedResponse<IEnumerable<DistrictDto>>>.SuccessResult(paginatedResponse);
        }
        catch (Exception ex)
        {
            return BaseResponse<PaginatedResponse<IEnumerable<DistrictDto>>>.ErrorResult($"Error retrieving all districts: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves a paginated list of Districts belonging to a specific Region.
    /// </summary>
    /// <param name="regionId">The ID of the parent Region.</param>
    /// <param name="pageNumber">The page number for pagination (default is 1).</param>
    /// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing a PaginatedResponse of DistrictDto.</returns>
    public async Task<BaseResponse<PaginatedResponse<IEnumerable<DistrictDto>>>> GetDistrictsAsync(Guid regionId, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            var (items, totalCount) = await _unitOfWork.Repository<District>().GetPagedAsync(pageNumber, pageSize, d => d.RegionId == regionId, cancellationToken);
            var dtos = _mapper.Map<IEnumerable<DistrictDto>>(items);

            var paginatedResponse = new PaginatedResponse<IEnumerable<DistrictDto>>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return BaseResponse<PaginatedResponse<IEnumerable<DistrictDto>>>.SuccessResult(paginatedResponse);
        }
        catch (Exception ex)
        {
            return BaseResponse<PaginatedResponse<IEnumerable<DistrictDto>>>.ErrorResult($"Error retrieving districts: {ex.Message}");
        }
    }

    /// <summary>
    /// Creates a new District.
    /// </summary>
    /// <param name="dto">The data transfer object containing the District's details.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the created DistrictDto if successful, or an error.</returns>
    public async Task<BaseResponse<DistrictDto>> CreateDistrictAsync(CreateDistrictDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            // Verify parent region exists
            var region = await _unitOfWork.Repository<Region>().GetByIdAsync(dto.RegionId, cancellationToken);
            if (region == null)
            {
                return BaseResponse<DistrictDto>.ErrorResult("Parent region not found");
            }

            // Normalize the code (trim and convert to uppercase)
            var normalizedCode = dto.Code.Trim().ToUpperInvariant();

            // Check if code already exists within the region
            var existingDistrict = await _unitOfWork.Repository<District>().ExistsAsync(d => d.Code == normalizedCode && d.RegionId == dto.RegionId, cancellationToken);
            if (existingDistrict)
            {
                return BaseResponse<DistrictDto>.ErrorResult("District code already exists in this region");
            }

            var district = _mapper.Map<District>(dto);
            district.Code = normalizedCode;
            district.Id = Guid.NewGuid();
            district.CreatedAtUtc = DateTime.UtcNow;
            district.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.Repository<District>().AddAsync(district, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Map the created district to DTO (without loading parent Region)
            var resultDto = _mapper.Map<DistrictDto>(district);
            return BaseResponse<DistrictDto>.SuccessResult(resultDto, "District created successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<DistrictDto>.ErrorResult($"Error creating district: {ex.Message}");
        }
    }

    /// <summary>
    /// Updates an existing District.
    /// </summary>
    /// <param name="id">The unique identifier of the District to update.</param>
    /// <param name="dto">The data transfer object containing the updated District's details.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the updated DistrictDto if successful, or an error.</returns>
    public async Task<BaseResponse<DistrictDto>> UpdateDistrictAsync(Guid id, UpdateDistrictDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var district = await _unitOfWork.Repository<District>().GetByIdAsync(id, cancellationToken);
            if (district == null)
            {
                return BaseResponse<DistrictDto>.ErrorResult("District not found");
            }

            // Normalize the code (trim and convert to uppercase)
            var normalizedCode = dto.Code.Trim().ToUpperInvariant();

            // Check if code already exists within the region (excluding current district)
            var existingDistrict = await _unitOfWork.Repository<District>().ExistsAsync(d => d.Code == normalizedCode && d.RegionId == district.RegionId && d.Id != id, cancellationToken);
            if (existingDistrict)
            {
                return BaseResponse<DistrictDto>.ErrorResult("District code already exists in this region");
            }

            _mapper.Map(dto, district);
            district.Code = normalizedCode;
            district.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.Repository<District>().UpdateAsync(district, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var resultDto = _mapper.Map<DistrictDto>(district);
            return BaseResponse<DistrictDto>.SuccessResult(resultDto, "District updated successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<DistrictDto>.ErrorResult($"Error updating district: {ex.Message}");
        }
    }

    /// <summary>
    /// Deletes a District by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the District to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse indicating success or failure of the deletion.</returns>
    public async Task<BaseResponse<bool>> DeleteDistrictAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if district has child clubs
            var hasClubs = await _unitOfWork.Repository<Club>().ExistsAsync(c => c.DistrictId == id, cancellationToken);
            if (hasClubs)
            {
                return BaseResponse<bool>.ErrorResult("Cannot delete district with existing clubs");
            }

            var deleted = await _unitOfWork.Repository<District>().DeleteAsync(id, cancellationToken);
            if (!deleted)
            {
                return BaseResponse<bool>.ErrorResult("District not found");
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return BaseResponse<bool>.SuccessResult(true, "District deleted successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.ErrorResult($"Error deleting district: {ex.Message}");
        }
    }

    #endregion

    #region Church Operations

    /// <summary>
    /// Retrieves a specific Church by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the Church.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the ChurchDto if found, or an error.</returns>
    public async Task<BaseResponse<ChurchDto>> GetChurchAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var church = await _unitOfWork.Repository<Church>().GetByIdAsync(id, cancellationToken);
            if (church == null)
            {
                return BaseResponse<ChurchDto>.ErrorResult("Church not found");
            }

            var dto = _mapper.Map<ChurchDto>(church);
            return BaseResponse<ChurchDto>.SuccessResult(dto);
        }
        catch (Exception ex)
        {
            return BaseResponse<ChurchDto>.ErrorResult($"Error retrieving church: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves a paginated list of Churches.
    /// </summary>
    /// <param name="pageNumber">The page number for pagination (default is 1).</param>
    /// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing a PaginatedResponse of ChurchDto.</returns>
    public async Task<BaseResponse<PaginatedResponse<IEnumerable<ChurchDto>>>> GetChurchesAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            var (items, totalCount) = await _unitOfWork.Repository<Church>().GetPagedAsync(pageNumber, pageSize, null, cancellationToken);
            var dtos = _mapper.Map<IEnumerable<ChurchDto>>(items);

            var paginatedResponse = new PaginatedResponse<IEnumerable<ChurchDto>>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return BaseResponse<PaginatedResponse<IEnumerable<ChurchDto>>>.SuccessResult(paginatedResponse);
        }
        catch (Exception ex)
        {
            return BaseResponse<PaginatedResponse<IEnumerable<ChurchDto>>>.ErrorResult($"Error retrieving churches: {ex.Message}");
        }
    }

    /// <summary>
    /// Creates a new Church.
    /// </summary>
    /// <param name="dto">The data transfer object containing the Church's details.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the created ChurchDto if successful, or an error.</returns>
    public async Task<BaseResponse<ChurchDto>> CreateChurchAsync(CreateChurchDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var church = _mapper.Map<Church>(dto);
            church.Id = Guid.NewGuid();
            church.CreatedAtUtc = DateTime.UtcNow;
            church.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.Repository<Church>().AddAsync(church, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var resultDto = _mapper.Map<ChurchDto>(church);
            return BaseResponse<ChurchDto>.SuccessResult(resultDto, "Church created successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<ChurchDto>.ErrorResult($"Error creating church: {ex.Message}");
        }
    }

    /// <summary>
    /// Updates an existing Church.
    /// </summary>
    /// <param name="id">The unique identifier of the Church to update.</param>
    /// <param name="dto">The data transfer object containing the updated Church's details.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the updated ChurchDto if successful, or an error.</returns>
    public async Task<BaseResponse<ChurchDto>> UpdateChurchAsync(Guid id, UpdateChurchDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var church = await _unitOfWork.Repository<Church>().GetByIdAsync(id, cancellationToken);
            if (church == null)
            {
                return BaseResponse<ChurchDto>.ErrorResult("Church not found");
            }

            _mapper.Map(dto, church);
            church.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.Repository<Church>().UpdateAsync(church, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var resultDto = _mapper.Map<ChurchDto>(church);
            return BaseResponse<ChurchDto>.SuccessResult(resultDto, "Church updated successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<ChurchDto>.ErrorResult($"Error updating church: {ex.Message}");
        }
    }

    /// <summary>
    /// Deletes a Church by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the Church to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse indicating success or failure of the deletion.</returns>
    public async Task<BaseResponse<bool>> DeleteChurchAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if church has associated clubs
            var hasClubs = await _unitOfWork.Repository<Club>().ExistsAsync(c => c.ChurchId == id, cancellationToken);
            if (hasClubs)
            {
                return BaseResponse<bool>.ErrorResult("Cannot delete church with associated clubs");
            }

            var deleted = await _unitOfWork.Repository<Church>().DeleteAsync(id, cancellationToken);
            if (!deleted)
            {
                return BaseResponse<bool>.ErrorResult("Church not found");
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return BaseResponse<bool>.SuccessResult(true, "Church deleted successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.ErrorResult($"Error deleting church: {ex.Message}");
        }
    }

    #endregion

    #region Club Operations

    /// <summary>
    /// Retrieves a specific Club by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the Club.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the ClubDto if found, or an error.</returns>
    public async Task<BaseResponse<ClubDto>> GetClubAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var club = await _unitOfWork.Repository<Club>().GetByIdAsync(id, cancellationToken);
            if (club == null)
            {
                return BaseResponse<ClubDto>.ErrorResult("Club not found");
            }

            var dto = _mapper.Map<ClubDto>(club);
            return BaseResponse<ClubDto>.SuccessResult(dto);
        }
        catch (Exception ex)
        {
            return BaseResponse<ClubDto>.ErrorResult($"Error retrieving club: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves a paginated list of all Clubs in the system.
    /// </summary>
    /// <param name="pageNumber">The page number for pagination (default is 1).</param>
    /// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing a PaginatedResponse of ClubDto.</returns>
    public async Task<BaseResponse<PaginatedResponse<IEnumerable<ClubDto>>>> GetAllClubsAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            var (items, totalCount) = await _unitOfWork.Repository<Club>().GetPagedAsync(pageNumber, pageSize, null, cancellationToken);
            var dtos = _mapper.Map<List<ClubDto>>(items);

            var paginatedResponse = new PaginatedResponse<IEnumerable<ClubDto>>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return BaseResponse<PaginatedResponse<IEnumerable<ClubDto>>>.SuccessResult(paginatedResponse);
        }
        catch (Exception ex)
        {
            return BaseResponse<PaginatedResponse<IEnumerable<ClubDto>>>.ErrorResult($"Error retrieving all clubs: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves a paginated list of Clubs belonging to a specific District.
    /// </summary>
    /// <param name="districtId">The ID of the parent District.</param>
    /// <param name="pageNumber">The page number for pagination (default is 1).</param>
    /// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing a PaginatedResponse of ClubDto.</returns>
    public async Task<BaseResponse<PaginatedResponse<IEnumerable<ClubDto>>>> GetClubsAsync(Guid districtId, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            var (items, totalCount) = await _unitOfWork.Repository<Club>().GetPagedAsync(pageNumber, pageSize, c => c.DistrictId == districtId, cancellationToken);
            var dtos = _mapper.Map<IEnumerable<ClubDto>>(items);

            var paginatedResponse = new PaginatedResponse<IEnumerable<ClubDto>>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return BaseResponse<PaginatedResponse<IEnumerable<ClubDto>>>.SuccessResult(paginatedResponse);
        }
        catch (Exception ex)
        {
            return BaseResponse<PaginatedResponse<IEnumerable<ClubDto>>>.ErrorResult($"Error retrieving clubs: {ex.Message}");
        }
    }

    /// <summary>
    /// Creates a new Club.
    /// </summary>
    /// <param name="dto">The data transfer object containing the Club's details.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the created ClubDto if successful, or an error.</returns>
    public async Task<BaseResponse<ClubDto>> CreateClubAsync(CreateClubDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            // Verify parent district exists
            var district = await _unitOfWork.Repository<District>().GetByIdAsync(dto.DistrictId, cancellationToken);
            if (district == null)
            {
                return BaseResponse<ClubDto>.ErrorResult("Parent district not found");
            }

            // Verify church exists
            var church = await _unitOfWork.Repository<Church>().GetByIdAsync(dto.ChurchId, cancellationToken);
            if (church == null)
            {
                return BaseResponse<ClubDto>.ErrorResult("Church not found");
            }

            // Check if code already exists within the district
            var existingClub = await _unitOfWork.Repository<Club>().ExistsAsync(c => c.Code == dto.Code && c.DistrictId == dto.DistrictId, cancellationToken);
            if (existingClub)
            {
                return BaseResponse<ClubDto>.ErrorResult("Club code already exists in this district");
            }

            var club = _mapper.Map<Club>(dto);
            club.Id = Guid.NewGuid();
            club.IsActive = true; // Default to active
            club.CreatedAtUtc = DateTime.UtcNow;
            club.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.Repository<Club>().AddAsync(club, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var resultDto = _mapper.Map<ClubDto>(club);
            return BaseResponse<ClubDto>.SuccessResult(resultDto, "Club created successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<ClubDto>.ErrorResult($"Error creating club: {ex.Message}");
        }
    }

    /// <summary>
    /// Updates an existing Club.
    /// </summary>
    /// <param name="id">The unique identifier of the Club to update.</param>
    /// <param name="dto">The data transfer object containing the updated Club's details.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the updated ClubDto if successful, or an error.</returns>
    public async Task<BaseResponse<ClubDto>> UpdateClubAsync(Guid id, UpdateClubDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var club = await _unitOfWork.Repository<Club>().GetByIdAsync(id, cancellationToken);
            if (club == null)
            {
                return BaseResponse<ClubDto>.ErrorResult("Club not found");
            }

            // Verify church exists
            var church = await _unitOfWork.Repository<Church>().GetByIdAsync(dto.ChurchId, cancellationToken);
            if (church == null)
            {
                return BaseResponse<ClubDto>.ErrorResult("Church not found");
            }

            // Check if code already exists within the district (excluding current club)
            var existingClub = await _unitOfWork.Repository<Club>().ExistsAsync(c => c.Code == dto.Code && c.DistrictId == club.DistrictId && c.Id != id, cancellationToken);
            if (existingClub)
            {
                return BaseResponse<ClubDto>.ErrorResult("Club code already exists in this district");
            }

            _mapper.Map(dto, club);
            club.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.Repository<Club>().UpdateAsync(club, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var resultDto = _mapper.Map<ClubDto>(club);
            return BaseResponse<ClubDto>.SuccessResult(resultDto, "Club updated successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<ClubDto>.ErrorResult($"Error updating club: {ex.Message}");
        }
    }

    /// <summary>
    /// Deletes a Club by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the Club to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse indicating success or failure of the deletion.</returns>
    public async Task<BaseResponse<bool>> DeleteClubAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if club has child units
            var hasUnits = await _unitOfWork.Repository<Unit>().ExistsAsync(u => u.ClubId == id, cancellationToken);
            if (hasUnits)
            {
                return BaseResponse<bool>.ErrorResult("Cannot delete club with existing units");
            }

            var deleted = await _unitOfWork.Repository<Club>().DeleteAsync(id, cancellationToken);
            if (!deleted)
            {
                return BaseResponse<bool>.ErrorResult("Club not found");
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return BaseResponse<bool>.SuccessResult(true, "Club deleted successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.ErrorResult($"Error deleting club: {ex.Message}");
        }
    }

    #endregion

    #region Unit Operations

    /// <summary>
    /// Retrieves a specific Unit by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the Unit.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the UnitDto if found, or an error.</returns>
    public async Task<BaseResponse<UnitDto>> GetUnitAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var unit = await _unitOfWork.Repository<Unit>().GetByIdAsync(id, cancellationToken);
            if (unit == null)
            {
                return BaseResponse<UnitDto>.ErrorResult("Unit not found");
            }

            var dto = _mapper.Map<UnitDto>(unit);
            return BaseResponse<UnitDto>.SuccessResult(dto);
        }
        catch (Exception ex)
        {
            return BaseResponse<UnitDto>.ErrorResult($"Error retrieving unit: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves a paginated list of all Units in the system.
    /// </summary>
    /// <param name="pageNumber">The page number for pagination (default is 1).</param>
    /// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing a PaginatedResponse of UnitDto.</returns>
    public async Task<BaseResponse<PaginatedResponse<IEnumerable<UnitDto>>>> GetAllUnitsAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            var (items, totalCount) = await _unitOfWork.Repository<Unit>().GetPagedAsync(pageNumber, pageSize, null, cancellationToken);
            var dtos = _mapper.Map<List<UnitDto>>(items);

            var paginatedResponse = new PaginatedResponse<IEnumerable<UnitDto>>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return BaseResponse<PaginatedResponse<IEnumerable<UnitDto>>>.SuccessResult(paginatedResponse);
        }
        catch (Exception ex)
        {
            return BaseResponse<PaginatedResponse<IEnumerable<UnitDto>>>.ErrorResult($"Error retrieving all units: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves a paginated list of Units belonging to a specific Club.
    /// </summary>
    /// <param name="clubId">The ID of the parent Club.</param>
    /// <param name="pageNumber">The page number for pagination (default is 1).</param>
    /// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing a PaginatedResponse of UnitDto.</returns>
    public async Task<BaseResponse<PaginatedResponse<IEnumerable<UnitDto>>>> GetUnitsAsync(Guid clubId, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            var (items, totalCount) = await _unitOfWork.Repository<Unit>().GetPagedAsync(pageNumber, pageSize, u => u.ClubId == clubId, cancellationToken);
            var dtos = _mapper.Map<IEnumerable<UnitDto>>(items);

            var paginatedResponse = new PaginatedResponse<IEnumerable<UnitDto>>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return BaseResponse<PaginatedResponse<IEnumerable<UnitDto>>>.SuccessResult(paginatedResponse);
        }
        catch (Exception ex)
        {
            return BaseResponse<PaginatedResponse<IEnumerable<UnitDto>>>.ErrorResult($"Error retrieving units: {ex.Message}");
        }
    }

    /// <summary>
    /// Creates a new Unit.
    /// </summary>
    /// <param name="dto">The data transfer object containing the Unit's details.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the created UnitDto if successful, or an error.</returns>
    public async Task<BaseResponse<UnitDto>> CreateUnitAsync(CreateUnitDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            // Verify parent club exists
            var club = await _unitOfWork.Repository<Club>().GetByIdAsync(dto.ClubId, cancellationToken);
            if (club == null)
            {
                return BaseResponse<UnitDto>.ErrorResult("Parent club not found");
            }

            // Validate age range
            if (dto.AgeMin >= dto.AgeMax)
            {
                return BaseResponse<UnitDto>.ErrorResult("Minimum age must be less than maximum age");
            }

            var unit = _mapper.Map<Unit>(dto);
            unit.Id = Guid.NewGuid();
            unit.CreatedAtUtc = DateTime.UtcNow;
            unit.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.Repository<Unit>().AddAsync(unit, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var resultDto = _mapper.Map<UnitDto>(unit);
            return BaseResponse<UnitDto>.SuccessResult(resultDto, "Unit created successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<UnitDto>.ErrorResult($"Error creating unit: {ex.Message}");
        }
    }

    /// <summary>
    /// Updates an existing Unit.
    /// </summary>
    /// <param name="id">The unique identifier of the Unit to update.</param>
    /// <param name="dto">The data transfer object containing the updated Unit's details.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the updated UnitDto if successful, or an error.</returns>
    public async Task<BaseResponse<UnitDto>> UpdateUnitAsync(Guid id, UpdateUnitDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var unit = await _unitOfWork.Repository<Unit>().GetByIdAsync(id, cancellationToken);
            if (unit == null)
            {
                return BaseResponse<UnitDto>.ErrorResult("Unit not found");
            }

            // Validate age range
            if (dto.AgeMin >= dto.AgeMax)
            {
                return BaseResponse<UnitDto>.ErrorResult("Minimum age must be less than maximum age");
            }

            _mapper.Map(dto, unit);
            unit.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.Repository<Unit>().UpdateAsync(unit, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var resultDto = _mapper.Map<UnitDto>(unit);
            return BaseResponse<UnitDto>.SuccessResult(resultDto, "Unit updated successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<UnitDto>.ErrorResult($"Error updating unit: {ex.Message}");
        }
    }

    /// <summary>
    /// Deletes a Unit by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the Unit to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse indicating success or failure of the deletion.</returns>
    public async Task<BaseResponse<bool>> DeleteUnitAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if unit has active memberships
            var hasMemberships = await _unitOfWork.Repository<Domain.Entities.Membership>().ExistsAsync(m => m.UnitId == id && m.IsActive, cancellationToken);
            if (hasMemberships)
            {
                return BaseResponse<bool>.ErrorResult("Cannot delete unit with active memberships");
            }

            var deleted = await _unitOfWork.Repository<Unit>().DeleteAsync(id, cancellationToken);
            if (!deleted)
            {
                return BaseResponse<bool>.ErrorResult("Unit not found");
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return BaseResponse<bool>.SuccessResult(true, "Unit deleted successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.ErrorResult($"Error deleting unit: {ex.Message}");
        }
    }

    #endregion
}
