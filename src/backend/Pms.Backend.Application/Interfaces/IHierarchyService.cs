using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Hierarchy;

namespace Pms.Backend.Application.Interfaces;

/// <summary>
/// Service interface for hierarchy management operations
/// </summary>
public interface IHierarchyService
{
    // Division operations
    /// <summary>
    /// Gets a division by its ID
    /// </summary>
    /// <param name="id">Division ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Division data or error response</returns>
    Task<BaseResponse<DivisionDto>> GetDivisionAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all divisions with pagination
    /// </summary>
    /// <param name="pageNumber">Page number (1-based)</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated divisions or error response</returns>
    Task<BaseResponse<PaginatedResponse<IEnumerable<DivisionSummaryDto>>>> GetDivisionsAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new division
    /// </summary>
    /// <param name="dto">Division creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created division or error response</returns>
    Task<BaseResponse<DivisionDto>> CreateDivisionAsync(CreateDivisionDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing division
    /// </summary>
    /// <param name="id">Division ID</param>
    /// <param name="dto">Division update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated division or error response</returns>
    Task<BaseResponse<DivisionDto>> UpdateDivisionAsync(Guid id, UpdateDivisionDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a division
    /// </summary>
    /// <param name="id">Division ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success status or error response</returns>
    Task<BaseResponse<bool>> DeleteDivisionAsync(Guid id, CancellationToken cancellationToken = default);

    // Union operations
    /// <summary>
    /// Gets a union by its ID
    /// </summary>
    /// <param name="id">Union ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Union data or error response</returns>
    Task<BaseResponse<UnionDto>> GetUnionAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all unions in the system with pagination
    /// </summary>
    /// <param name="pageNumber">Page number (1-based)</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated unions or error response</returns>
    Task<BaseResponse<PaginatedResponse<IEnumerable<UnionDto>>>> GetAllUnionsAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all unions for a specific division with pagination
    /// </summary>
    /// <param name="divisionId">Parent division ID</param>
    /// <param name="pageNumber">Page number (1-based)</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated unions or error response</returns>
    Task<BaseResponse<PaginatedResponse<IEnumerable<UnionDto>>>> GetUnionsAsync(Guid divisionId, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new union
    /// </summary>
    /// <param name="dto">Union creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created union or error response</returns>
    Task<BaseResponse<UnionDto>> CreateUnionAsync(CreateUnionDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing union
    /// </summary>
    /// <param name="id">Union ID</param>
    /// <param name="dto">Union update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated union or error response</returns>
    Task<BaseResponse<UnionDto>> UpdateUnionAsync(Guid id, UpdateUnionDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a union
    /// </summary>
    /// <param name="id">Union ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success status or error response</returns>
    Task<BaseResponse<bool>> DeleteUnionAsync(Guid id, CancellationToken cancellationToken = default);

    // Association operations
    /// <summary>
    /// Gets an association by its ID
    /// </summary>
    /// <param name="id">Association ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Association data or error response</returns>
    Task<BaseResponse<AssociationDto>> GetAssociationAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all associations for a specific union with pagination
    /// </summary>
    /// <param name="unionId">Parent union ID</param>
    /// <param name="pageNumber">Page number (1-based)</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated associations or error response</returns>
    Task<BaseResponse<PaginatedResponse<IEnumerable<AssociationDto>>>> GetAssociationsAsync(Guid unionId, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new association
    /// </summary>
    /// <param name="dto">Association creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created association or error response</returns>
    Task<BaseResponse<AssociationDto>> CreateAssociationAsync(CreateAssociationDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing association
    /// </summary>
    /// <param name="id">Association ID</param>
    /// <param name="dto">Association update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated association or error response</returns>
    Task<BaseResponse<AssociationDto>> UpdateAssociationAsync(Guid id, UpdateAssociationDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an association
    /// </summary>
    /// <param name="id">Association ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success status or error response</returns>
    Task<BaseResponse<bool>> DeleteAssociationAsync(Guid id, CancellationToken cancellationToken = default);

    // Region operations
    /// <summary>
    /// Gets a region by its ID
    /// </summary>
    /// <param name="id">Region ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Region data or error response</returns>
    Task<BaseResponse<RegionDto>> GetRegionAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all regions for a specific association with pagination
    /// </summary>
    /// <param name="associationId">Parent association ID</param>
    /// <param name="pageNumber">Page number (1-based)</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated regions or error response</returns>
    Task<BaseResponse<PaginatedResponse<IEnumerable<RegionDto>>>> GetRegionsAsync(Guid associationId, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new region
    /// </summary>
    /// <param name="dto">Region creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created region or error response</returns>
    Task<BaseResponse<RegionDto>> CreateRegionAsync(CreateRegionDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing region
    /// </summary>
    /// <param name="id">Region ID</param>
    /// <param name="dto">Region update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated region or error response</returns>
    Task<BaseResponse<RegionDto>> UpdateRegionAsync(Guid id, UpdateRegionDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a region
    /// </summary>
    /// <param name="id">Region ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success status or error response</returns>
    Task<BaseResponse<bool>> DeleteRegionAsync(Guid id, CancellationToken cancellationToken = default);

    // District operations
    /// <summary>
    /// Gets a district by its ID
    /// </summary>
    /// <param name="id">District ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>District data or error response</returns>
    Task<BaseResponse<DistrictDto>> GetDistrictAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all districts in the system with pagination
    /// </summary>
    /// <param name="pageNumber">Page number (1-based)</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated districts or error response</returns>
    Task<BaseResponse<PaginatedResponse<IEnumerable<DistrictDto>>>> GetAllDistrictsAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all districts for a specific region with pagination
    /// </summary>
    /// <param name="regionId">Parent region ID</param>
    /// <param name="pageNumber">Page number (1-based)</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated districts or error response</returns>
    Task<BaseResponse<PaginatedResponse<IEnumerable<DistrictDto>>>> GetDistrictsAsync(Guid regionId, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new district
    /// </summary>
    /// <param name="dto">District creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created district or error response</returns>
    Task<BaseResponse<DistrictDto>> CreateDistrictAsync(CreateDistrictDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing district
    /// </summary>
    /// <param name="id">District ID</param>
    /// <param name="dto">District update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated district or error response</returns>
    Task<BaseResponse<DistrictDto>> UpdateDistrictAsync(Guid id, UpdateDistrictDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a district
    /// </summary>
    /// <param name="id">District ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success status or error response</returns>
    Task<BaseResponse<bool>> DeleteDistrictAsync(Guid id, CancellationToken cancellationToken = default);

    // Church operations
    /// <summary>
    /// Gets a church by its ID
    /// </summary>
    /// <param name="id">Church ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Church data or error response</returns>
    Task<BaseResponse<ChurchDto>> GetChurchAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all churches with pagination
    /// </summary>
    /// <param name="pageNumber">Page number (1-based)</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated churches or error response</returns>
    Task<BaseResponse<PaginatedResponse<IEnumerable<ChurchDto>>>> GetChurchesAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new church
    /// </summary>
    /// <param name="dto">Church creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created church or error response</returns>
    Task<BaseResponse<ChurchDto>> CreateChurchAsync(CreateChurchDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing church
    /// </summary>
    /// <param name="id">Church ID</param>
    /// <param name="dto">Church update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated church or error response</returns>
    Task<BaseResponse<ChurchDto>> UpdateChurchAsync(Guid id, UpdateChurchDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a church
    /// </summary>
    /// <param name="id">Church ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success status or error response</returns>
    Task<BaseResponse<bool>> DeleteChurchAsync(Guid id, CancellationToken cancellationToken = default);

    // Club operations
    /// <summary>
    /// Gets a club by its ID
    /// </summary>
    /// <param name="id">Club ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Club data or error response</returns>
    Task<BaseResponse<ClubDto>> GetClubAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all clubs in the system with pagination
    /// </summary>
    /// <param name="pageNumber">Page number (1-based)</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated clubs or error response</returns>
    Task<BaseResponse<PaginatedResponse<IEnumerable<ClubDto>>>> GetAllClubsAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all clubs for a specific district with pagination
    /// </summary>
    /// <param name="districtId">Parent district ID</param>
    /// <param name="pageNumber">Page number (1-based)</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated clubs or error response</returns>
    Task<BaseResponse<PaginatedResponse<IEnumerable<ClubDto>>>> GetClubsAsync(Guid districtId, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new club
    /// </summary>
    /// <param name="dto">Club creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created club or error response</returns>
    Task<BaseResponse<ClubDto>> CreateClubAsync(CreateClubDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing club
    /// </summary>
    /// <param name="id">Club ID</param>
    /// <param name="dto">Club update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated club or error response</returns>
    Task<BaseResponse<ClubDto>> UpdateClubAsync(Guid id, UpdateClubDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a club
    /// </summary>
    /// <param name="id">Club ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success status or error response</returns>
    Task<BaseResponse<bool>> DeleteClubAsync(Guid id, CancellationToken cancellationToken = default);

    // Unit operations
    /// <summary>
    /// Gets a unit by its ID
    /// </summary>
    /// <param name="id">Unit ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Unit data or error response</returns>
    Task<BaseResponse<UnitDto>> GetUnitAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all units in the system with pagination
    /// </summary>
    /// <param name="pageNumber">Page number (1-based)</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated units or error response</returns>
    Task<BaseResponse<PaginatedResponse<IEnumerable<UnitDto>>>> GetAllUnitsAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all units for a specific club with pagination
    /// </summary>
    /// <param name="clubId">Parent club ID</param>
    /// <param name="pageNumber">Page number (1-based)</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated units or error response</returns>
    Task<BaseResponse<PaginatedResponse<IEnumerable<UnitDto>>>> GetUnitsAsync(Guid clubId, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new unit
    /// </summary>
    /// <param name="dto">Unit creation data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created unit or error response</returns>
    Task<BaseResponse<UnitDto>> CreateUnitAsync(CreateUnitDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing unit
    /// </summary>
    /// <param name="id">Unit ID</param>
    /// <param name="dto">Unit update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated unit or error response</returns>
    Task<BaseResponse<UnitDto>> UpdateUnitAsync(Guid id, UpdateUnitDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a unit
    /// </summary>
    /// <param name="id">Unit ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success status or error response</returns>
    Task<BaseResponse<bool>> DeleteUnitAsync(Guid id, CancellationToken cancellationToken = default);
}
