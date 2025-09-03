using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Members;
using Pms.Backend.Application.DTOs.Membership;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Application.Services.Membership;

/// <summary>
/// Service for managing memberships and unit allocations
/// Implements the "1º de junho" rule and automatic unit allocation
/// </summary>
public partial class MembershipService : IMembershipService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the MembershipService
    /// </summary>
    /// <param name="unitOfWork">The unit of work</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public MembershipService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    #region Membership CRUD Operations

    /// <summary>
    /// Creates a new membership for a member in a club
    /// </summary>
    /// <param name="memberId">The member ID</param>
    /// <param name="clubId">The club ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result of the membership creation</returns>
    public async Task<BaseResponse<MembershipDto>> CreateMembershipAsync(Guid memberId, Guid clubId, CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate member exists
            var member = await _unitOfWork.Repository<Member>().GetFirstOrDefaultAsync(
                m => m.Id == memberId, cancellationToken);

            if (member == null)
            {
                return BaseResponse<MembershipDto>.ErrorResult("Member not found");
            }

            // Validate club exists
            var club = await _unitOfWork.Repository<Club>().GetFirstOrDefaultAsync(
                c => c.Id == clubId, cancellationToken);

            if (club == null)
            {
                return BaseResponse<MembershipDto>.ErrorResult("Club not found");
            }

            // Check if member already has an active membership
            var existingMembership = await _unitOfWork.Repository<Domain.Entities.Membership>().GetFirstOrDefaultAsync(
                m => m.MemberId == memberId && m.IsActive, cancellationToken);

            if (existingMembership != null)
            {
                return BaseResponse<MembershipDto>.ErrorResult("Member already has an active membership");
            }

            // Check if member is eligible (≥10 years old)
            if (!member.IsEligibleForRegistration)
            {
                return BaseResponse<MembershipDto>.ErrorResult("Member must be at least 10 years old to join a club");
            }

            // Create new membership
            var membership = new Domain.Entities.Membership
            {
                MemberId = memberId,
                ClubId = clubId,
                StartDate = DateTime.UtcNow,
                IsActive = true
            };

            await _unitOfWork.Repository<Domain.Entities.Membership>().AddAsync(membership);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Auto-allocate to unit if possible
            var allocationResult = await AllocateToUnitAsync(membership.Id, cancellationToken);
            if (allocationResult.Success)
            {
                membership = await _unitOfWork.Repository<Domain.Entities.Membership>().GetFirstOrDefaultAsync(
                    m => m.Id == membership.Id, cancellationToken);
            }

            var dto = _mapper.Map<MembershipDto>(membership);
            return BaseResponse<MembershipDto>.SuccessResult(dto, "Membership created successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<MembershipDto>.ErrorResult($"Error creating membership: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets a membership by ID
    /// </summary>
    /// <param name="id">The membership ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The membership data</returns>
    public async Task<BaseResponse<MembershipDto>> GetMembershipAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var membership = await _unitOfWork.Repository<Domain.Entities.Membership>()
                .GetFirstOrDefaultAsync(m => m.Id == id, cancellationToken);

            if (membership == null)
            {
                return BaseResponse<MembershipDto>.ErrorResult("Membership not found");
            }

            var dto = _mapper.Map<MembershipDto>(membership);
            return BaseResponse<MembershipDto>.SuccessResult(dto);
        }
        catch (Exception ex)
        {
            return BaseResponse<MembershipDto>.ErrorResult($"Error retrieving membership: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets all memberships for a specific member
    /// </summary>
    /// <param name="memberId">The member ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of memberships</returns>
    public async Task<BaseResponse<IEnumerable<MembershipDto>>> GetMembershipsByMemberAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        try
        {
            var memberships = await _unitOfWork.Repository<Domain.Entities.Membership>()
                .GetAllAsync(m => m.MemberId == memberId, cancellationToken);

            var dtos = _mapper.Map<IEnumerable<MembershipDto>>(memberships);
            return BaseResponse<IEnumerable<MembershipDto>>.SuccessResult(dtos);
        }
        catch (Exception ex)
        {
            return BaseResponse<IEnumerable<MembershipDto>>.ErrorResult($"Error retrieving memberships: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets all memberships for a specific club
    /// </summary>
    /// <param name="clubId">The club ID</param>
    /// <param name="pageNumber">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of memberships</returns>
    public async Task<BaseResponse<PaginatedResponse<IEnumerable<MembershipDto>>>> GetMembershipsByClubAsync(
        Guid clubId, 
        int pageNumber = 1, 
        int pageSize = 10, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var (items, totalCount) = await _unitOfWork.Repository<Domain.Entities.Membership>().GetPagedAsync(
                pageNumber,
                pageSize,
                m => m.ClubId == clubId,
                cancellationToken);

            var dtos = _mapper.Map<IEnumerable<MembershipDto>>(items);

            var paginatedResponse = new PaginatedResponse<IEnumerable<MembershipDto>>
            {
                Data = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return BaseResponse<PaginatedResponse<IEnumerable<MembershipDto>>>.SuccessResult(paginatedResponse);
        }
        catch (Exception ex)
        {
            return BaseResponse<PaginatedResponse<IEnumerable<MembershipDto>>>.ErrorResult($"Error retrieving memberships: {ex.Message}");
        }
    }

    /// <summary>
    /// Updates a membership
    /// </summary>
    /// <param name="id">The membership ID</param>
    /// <param name="dto">The updated membership data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result of the update</returns>
    public async Task<BaseResponse<MembershipDto>> UpdateMembershipAsync(Guid id, UpdateMembershipDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var membership = await _unitOfWork.Repository<Domain.Entities.Membership>()
                .GetFirstOrDefaultAsync(m => m.Id == id, cancellationToken);

            if (membership == null)
            {
                return BaseResponse<MembershipDto>.ErrorResult("Membership not found");
            }

            _mapper.Map(dto, membership);
            membership.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var updatedDto = _mapper.Map<MembershipDto>(membership);
            return BaseResponse<MembershipDto>.SuccessResult(updatedDto, "Membership updated successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<MembershipDto>.ErrorResult($"Error updating membership: {ex.Message}");
        }
    }

    /// <summary>
    /// Deactivates a membership
    /// </summary>
    /// <param name="id">The membership ID</param>
    /// <param name="reason">Reason for deactivation</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result of the deactivation</returns>
    public async Task<BaseResponse<bool>> DeactivateMembershipAsync(Guid id, string reason, CancellationToken cancellationToken = default)
    {
        try
        {
            var membership = await _unitOfWork.Repository<Domain.Entities.Membership>()
                .GetFirstOrDefaultAsync(m => m.Id == id, cancellationToken);

            if (membership == null)
            {
                return BaseResponse<bool>.ErrorResult("Membership not found");
            }

            membership.IsActive = false;
            membership.EndDate = DateTime.UtcNow;
            membership.EndReason = reason;
            membership.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return BaseResponse<bool>.SuccessResult(true, "Membership deactivated successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.ErrorResult($"Error deactivating membership: {ex.Message}");
        }
    }

    #endregion

    #region Age and Eligibility Operations

    /// <summary>
    /// Gets the age of a member on June 1st of the specified year
    /// </summary>
    /// <param name="memberId">The member ID</param>
    /// <param name="year">The year to calculate age for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The age on June 1st</returns>
    public async Task<BaseResponse<int>> GetAgeOnJuneFirstAsync(Guid memberId, int year, CancellationToken cancellationToken = default)
    {
        try
        {
            var member = await _unitOfWork.Repository<Member>().GetFirstOrDefaultAsync(
                m => m.Id == memberId, cancellationToken);

            if (member == null)
            {
                return BaseResponse<int>.ErrorResult("Member not found");
            }

            var age = member.GetAgeOnJuneFirst(year);
            return BaseResponse<int>.SuccessResult(age);
        }
        catch (Exception ex)
        {
            return BaseResponse<int>.ErrorResult($"Error calculating age: {ex.Message}");
        }
    }

    /// <summary>
    /// Checks if a member is eligible for membership (≥10 years old)
    /// </summary>
    /// <param name="memberId">The member ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Eligibility status</returns>
    public async Task<BaseResponse<bool>> IsEligibleForMembershipAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        try
        {
            var member = await _unitOfWork.Repository<Member>().GetFirstOrDefaultAsync(
                m => m.Id == memberId, cancellationToken);

            if (member == null)
            {
                return BaseResponse<bool>.ErrorResult("Member not found");
            }

            var isEligible = member.IsEligibleForRegistration;
            return BaseResponse<bool>.SuccessResult(isEligible);
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.ErrorResult($"Error checking eligibility: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets all members who need unit allocation for a specific club
    /// </summary>
    /// <param name="clubId">The club ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of members needing allocation</returns>
    public async Task<BaseResponse<IEnumerable<MemberDto>>> GetMembersNeedingAllocationAsync(Guid clubId, CancellationToken cancellationToken = default)
    {
        try
        {
            var memberships = await _unitOfWork.Repository<Domain.Entities.Membership>()
                .GetAllAsync(m => m.ClubId == clubId && m.IsActive && m.UnitId == null, cancellationToken);

            var memberIds = memberships.Select(m => m.MemberId).ToList();
            var members = await _unitOfWork.Repository<Member>()
                .GetAllAsync(m => memberIds.Contains(m.Id), cancellationToken);

            var dtos = _mapper.Map<IEnumerable<MemberDto>>(members);
            return BaseResponse<IEnumerable<MemberDto>>.SuccessResult(dtos);
        }
        catch (Exception ex)
        {
            return BaseResponse<IEnumerable<MemberDto>>.ErrorResult($"Error retrieving members needing allocation: {ex.Message}");
        }
    }

    #endregion
}
