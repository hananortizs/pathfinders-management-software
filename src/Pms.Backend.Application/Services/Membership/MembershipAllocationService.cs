using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Members;
using Pms.Backend.Application.DTOs.Membership;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Application.Services.Membership;

/// <summary>
/// Partial implementation of MembershipService for unit allocation operations
/// </summary>
public partial class MembershipService
{
    #region Unit Allocation Operations

    /// <summary>
    /// Allocates a member to a unit based on age and gender
    /// </summary>
    /// <param name="membershipId">The membership ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result of the allocation</returns>
    public async Task<BaseResponse<MembershipDto>> AllocateToUnitAsync(Guid membershipId, CancellationToken cancellationToken = default)
    {
        try
        {
            var membership = await _unitOfWork.Repository<Domain.Entities.Membership>()
                .GetFirstOrDefaultAsync(m => m.Id == membershipId, cancellationToken);

            if (membership == null)
            {
                return BaseResponse<MembershipDto>.ErrorResult("Membership not found");
            }

            // Get member details
            var member = await _unitOfWork.Repository<Member>().GetFirstOrDefaultAsync(
                m => m.Id == membership.MemberId, cancellationToken);

            if (member == null)
            {
                return BaseResponse<MembershipDto>.ErrorResult("Member not found");
            }

            // Get available units in the club
            var availableUnits = await _unitOfWork.Repository<Unit>()
                .GetAllAsync(u => u.ClubId == membership.ClubId &&
                                 u.Gender.ToString() == member.Gender.ToString(), cancellationToken);

            if (!availableUnits.Any())
            {
                return BaseResponse<MembershipDto>.ErrorResult("No available units for member's gender in this club");
            }

            // Find unit with available capacity
            Unit? selectedUnit = null;
            foreach (var unit in availableUnits)
            {
                var currentCount = await GetUnitCurrentMemberCount(unit.Id, cancellationToken);
                if (unit.Capacity == null || currentCount < unit.Capacity.Value)
                {
                    selectedUnit = unit;
                    break;
                }
            }

            if (selectedUnit == null)
            {
                return BaseResponse<MembershipDto>.ErrorResult("No units with available capacity for member's gender");
            }

            // Allocate member to unit
            membership.UnitId = selectedUnit.Id;
            membership.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var dto = _mapper.Map<MembershipDto>(membership);
            return BaseResponse<MembershipDto>.SuccessResult(dto, $"Member allocated to unit: {selectedUnit.Name}");
        }
        catch (Exception ex)
        {
            return BaseResponse<MembershipDto>.ErrorResult($"Error allocating member to unit: {ex.Message}");
        }
    }

    /// <summary>
    /// Manually allocates a member to a specific unit
    /// </summary>
    /// <param name="membershipId">The membership ID</param>
    /// <param name="unitId">The unit ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result of the manual allocation</returns>
    public async Task<BaseResponse<MembershipDto>> AllocateToSpecificUnitAsync(Guid membershipId, Guid unitId, CancellationToken cancellationToken = default)
    {
        try
        {
            var membership = await _unitOfWork.Repository<Domain.Entities.Membership>()
                .GetFirstOrDefaultAsync(m => m.Id == membershipId, cancellationToken);

            if (membership == null)
            {
                return BaseResponse<MembershipDto>.ErrorResult("Membership not found");
            }

            // Validate unit exists and belongs to the same club
            var unit = await _unitOfWork.Repository<Unit>().GetFirstOrDefaultAsync(
                u => u.Id == unitId && u.ClubId == membership.ClubId, cancellationToken);

            if (unit == null)
            {
                return BaseResponse<MembershipDto>.ErrorResult("Unit not found or does not belong to the same club");
            }

            // Get member details for gender validation
            var member = await _unitOfWork.Repository<Member>().GetFirstOrDefaultAsync(
                m => m.Id == membership.MemberId, cancellationToken);

            if (member == null)
            {
                return BaseResponse<MembershipDto>.ErrorResult("Member not found");
            }

            // Check gender compatibility
            if (unit.Gender.ToString() != member.Gender.ToString())
            {
                return BaseResponse<MembershipDto>.ErrorResult("Unit gender does not match member gender");
            }

            // Check capacity
            var currentCount = await GetUnitCurrentMemberCount(unitId, cancellationToken);
            if (unit.Capacity != null && currentCount >= unit.Capacity.Value)
            {
                return BaseResponse<MembershipDto>.ErrorResult("Unit is at maximum capacity");
            }

            // Allocate member to unit
            membership.UnitId = unitId;
            membership.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var dto = _mapper.Map<MembershipDto>(membership);
            return BaseResponse<MembershipDto>.SuccessResult(dto, $"Member allocated to unit: {unit.Name}");
        }
        catch (Exception ex)
        {
            return BaseResponse<MembershipDto>.ErrorResult($"Error allocating member to specific unit: {ex.Message}");
        }
    }

    /// <summary>
    /// Reallocates a member to a different unit
    /// </summary>
    /// <param name="membershipId">The membership ID</param>
    /// <param name="newUnitId">The new unit ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result of the reallocation</returns>
    public async Task<BaseResponse<MembershipDto>> ReallocateToUnitAsync(Guid membershipId, Guid newUnitId, CancellationToken cancellationToken = default)
    {
        try
        {
            var membership = await _unitOfWork.Repository<Domain.Entities.Membership>()
                .GetFirstOrDefaultAsync(m => m.Id == membershipId, cancellationToken);

            if (membership == null)
            {
                return BaseResponse<MembershipDto>.ErrorResult("Membership not found");
            }

            // Validate new unit exists and belongs to the same club
            var newUnit = await _unitOfWork.Repository<Unit>().GetFirstOrDefaultAsync(
                u => u.Id == newUnitId && u.ClubId == membership.ClubId, cancellationToken);

            if (newUnit == null)
            {
                return BaseResponse<MembershipDto>.ErrorResult("New unit not found or does not belong to the same club");
            }

            // Get member details for gender validation
            var member = await _unitOfWork.Repository<Member>().GetFirstOrDefaultAsync(
                m => m.Id == membership.MemberId, cancellationToken);

            if (member == null)
            {
                return BaseResponse<MembershipDto>.ErrorResult("Member not found");
            }

            // Check gender compatibility
            if (newUnit.Gender.ToString() != member.Gender.ToString())
            {
                return BaseResponse<MembershipDto>.ErrorResult("New unit gender does not match member gender");
            }

            // Check capacity
            var currentCount = await GetUnitCurrentMemberCount(newUnitId, cancellationToken);
            if (newUnit.Capacity != null && currentCount >= newUnit.Capacity.Value)
            {
                return BaseResponse<MembershipDto>.ErrorResult("New unit is at maximum capacity");
            }

            // Reallocate member to new unit
            membership.UnitId = newUnitId;
            membership.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var dto = _mapper.Map<MembershipDto>(membership);
            return BaseResponse<MembershipDto>.SuccessResult(dto, $"Member reallocated to unit: {newUnit.Name}");
        }
        catch (Exception ex)
        {
            return BaseResponse<MembershipDto>.ErrorResult($"Error reallocating member: {ex.Message}");
        }
    }

    /// <summary>
    /// Removes a member from their current unit
    /// </summary>
    /// <param name="membershipId">The membership ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result of the removal</returns>
    public async Task<BaseResponse<MembershipDto>> RemoveFromUnitAsync(Guid membershipId, CancellationToken cancellationToken = default)
    {
        try
        {
            var membership = await _unitOfWork.Repository<Domain.Entities.Membership>()
                .GetFirstOrDefaultAsync(m => m.Id == membershipId, cancellationToken);

            if (membership == null)
            {
                return BaseResponse<MembershipDto>.ErrorResult("Membership not found");
            }

            if (membership.UnitId == null)
            {
                return BaseResponse<MembershipDto>.ErrorResult("Member is not currently allocated to any unit");
            }

            // Remove member from unit
            membership.UnitId = null;
            membership.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var dto = _mapper.Map<MembershipDto>(membership);
            return BaseResponse<MembershipDto>.SuccessResult(dto, "Member removed from unit");
        }
        catch (Exception ex)
        {
            return BaseResponse<MembershipDto>.ErrorResult($"Error removing member from unit: {ex.Message}");
        }
    }

    #endregion

    #region Club Capacity Operations

    /// <summary>
    /// Gets the current capacity status of all units in a club
    /// </summary>
    /// <param name="clubId">The club ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Capacity status for each unit</returns>
    public async Task<BaseResponse<IEnumerable<UnitCapacityDto>>> GetClubCapacityStatusAsync(Guid clubId, CancellationToken cancellationToken = default)
    {
        try
        {
            var units = await _unitOfWork.Repository<Unit>()
                .GetAllAsync(u => u.ClubId == clubId, cancellationToken);

            var capacityStatus = new List<UnitCapacityDto>();

            foreach (var unit in units)
            {
                var currentCount = await GetUnitCurrentMemberCount(unit.Id, cancellationToken);

                var capacityDto = _mapper.Map<UnitCapacityDto>(unit);
                capacityDto.CurrentCount = currentCount;

                capacityStatus.Add(capacityDto);
            }

            return BaseResponse<IEnumerable<UnitCapacityDto>>.SuccessResult(capacityStatus);
        }
        catch (Exception ex)
        {
            return BaseResponse<IEnumerable<UnitCapacityDto>>.ErrorResult($"Error retrieving club capacity status: {ex.Message}");
        }
    }

    /// <summary>
    /// Checks if a unit has available capacity
    /// </summary>
    /// <param name="unitId">The unit ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Capacity availability status</returns>
    public async Task<BaseResponse<bool>> IsUnitCapacityAvailableAsync(Guid unitId, CancellationToken cancellationToken = default)
    {
        try
        {
            var unit = await _unitOfWork.Repository<Unit>().GetFirstOrDefaultAsync(
                u => u.Id == unitId, cancellationToken);

            if (unit == null)
            {
                return BaseResponse<bool>.ErrorResult("Unit not found");
            }

            var currentCount = await GetUnitCurrentMemberCount(unitId, cancellationToken);
            var hasCapacity = unit.Capacity == null || currentCount < unit.Capacity.Value;

            return BaseResponse<bool>.SuccessResult(hasCapacity);
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.ErrorResult($"Error checking unit capacity: {ex.Message}");
        }
    }

    #endregion

    #region Gender Change Operations

    /// <summary>
    /// Handles gender change for a member and reallocates to appropriate unit
    /// </summary>
    /// <param name="memberId">The member ID</param>
    /// <param name="newGender">The new gender</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Result of the gender change and reallocation</returns>
    public async Task<BaseResponse<MembershipDto>> HandleGenderChangeAsync(Guid memberId, MemberGender newGender, CancellationToken cancellationToken = default)
    {
        try
        {
            var member = await _unitOfWork.Repository<Member>().GetFirstOrDefaultAsync(
                m => m.Id == memberId, cancellationToken);

            if (member == null)
            {
                return BaseResponse<MembershipDto>.ErrorResult("Member not found");
            }

            // Get active membership
            var membership = await _unitOfWork.Repository<Domain.Entities.Membership>()
                .GetFirstOrDefaultAsync(m => m.MemberId == memberId && m.IsActive, cancellationToken);

            if (membership == null)
            {
                return BaseResponse<MembershipDto>.ErrorResult("No active membership found for member");
            }

            // Update member gender
            member.Gender = newGender;
            member.UpdatedAtUtc = DateTime.UtcNow;

            // Remove from current unit if allocated
            if (membership.UnitId.HasValue)
            {
                membership.UnitId = null;
                membership.UpdatedAtUtc = DateTime.UtcNow;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Try to reallocate to appropriate unit
            var allocationResult = await AllocateToUnitAsync(membership.Id, cancellationToken);
            if (allocationResult.IsSuccess)
            {
                // Refresh membership data
                membership = await _unitOfWork.Repository<Domain.Entities.Membership>()
                    .GetFirstOrDefaultAsync(m => m.Id == membership.Id, cancellationToken);
            }

            var dto = _mapper.Map<MembershipDto>(membership);
            return BaseResponse<MembershipDto>.SuccessResult(dto, "Gender changed and member reallocated successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<MembershipDto>.ErrorResult($"Error handling gender change: {ex.Message}");
        }
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Gets the current number of active members in a unit
    /// </summary>
    /// <param name="unitId">The unit ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Current member count</returns>
    private async Task<int> GetUnitCurrentMemberCount(Guid unitId, CancellationToken cancellationToken = default)
    {
        var count = await _unitOfWork.Repository<Domain.Entities.Membership>()
            .GetAllAsync(m => m.UnitId == unitId && m.IsActive, cancellationToken);

        return count.Count();
    }

    #endregion
}
