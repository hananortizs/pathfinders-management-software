using AutoMapper;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Members;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Application.Services.Members;

/// <summary>
/// Service for member management operations (status, search, validation)
/// </summary>
public partial class MemberService : IMemberService
{
    #region Member Status Operations

    /// <summary>
    /// Activates a member account.
    /// </summary>
    /// <param name="id">The unique identifier of the Member to activate.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse indicating success or failure of the activation.</returns>
    public async Task<BaseResponse<bool>> ActivateMemberAccountAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var member = await _unitOfWork.Repository<Member>().GetByIdAsync(id, cancellationToken);
            if (member == null)
            {
                return BaseResponse<bool>.ErrorResult("Member not found");
            }

            if (member.Status == MemberStatus.Active)
            {
                return BaseResponse<bool>.ErrorResult("Member is already active");
            }

            member.Status = MemberStatus.Active;
            member.ActivatedAtUtc = DateTime.UtcNow;
            member.UpdatedAtUtc = DateTime.UtcNow;

            // Also activate user credential if exists
            var userCredential = await _unitOfWork.Repository<UserCredential>().GetFirstOrDefaultAsync(
                uc => uc.MemberId == id,
                cancellationToken);

            if (userCredential != null)
            {
                userCredential.IsActive = true;
                userCredential.UpdatedAtUtc = DateTime.UtcNow;
                await _unitOfWork.Repository<UserCredential>().UpdateAsync(userCredential, cancellationToken);
            }

            await _unitOfWork.Repository<Member>().UpdateAsync(member, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return BaseResponse<bool>.SuccessResult(true, "Member account activated successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.ErrorResult($"Error activating member account: {ex.Message}");
        }
    }

    /// <summary>
    /// Deactivates a member account.
    /// </summary>
    /// <param name="id">The unique identifier of the Member to deactivate.</param>
    /// <param name="reason">The reason for deactivation.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse indicating success or failure of the deactivation.</returns>
    public async Task<BaseResponse<bool>> DeactivateMemberAccountAsync(Guid id, string reason, CancellationToken cancellationToken = default)
    {
        try
        {
            var member = await _unitOfWork.Repository<Member>().GetByIdAsync(id, cancellationToken);
            if (member == null)
            {
                return BaseResponse<bool>.ErrorResult("Member not found");
            }

            if (member.Status == MemberStatus.Inactive)
            {
                return BaseResponse<bool>.ErrorResult("Member is already inactive");
            }

            member.Status = MemberStatus.Inactive;
            member.DeactivatedAtUtc = DateTime.UtcNow;
            member.DeactivationReason = reason;
            member.UpdatedAtUtc = DateTime.UtcNow;

            // Also deactivate user credential if exists
            var userCredential = await _unitOfWork.Repository<UserCredential>().GetFirstOrDefaultAsync(
                uc => uc.MemberId == id,
                cancellationToken);

            if (userCredential != null)
            {
                userCredential.IsActive = false;
                userCredential.UpdatedAtUtc = DateTime.UtcNow;
                await _unitOfWork.Repository<UserCredential>().UpdateAsync(userCredential, cancellationToken);
            }

            // Deactivate all active memberships
            var activeMemberships = await _unitOfWork.Repository<Domain.Entities.Membership>().GetAllAsync(
                m => m.MemberId == id && m.IsActive,
                cancellationToken);

            foreach (var membership in activeMemberships)
            {
                membership.IsActive = false;
                membership.EndDate = DateTime.UtcNow;
                membership.UpdatedAtUtc = DateTime.UtcNow;
                await _unitOfWork.Repository<Domain.Entities.Membership>().UpdateAsync(membership, cancellationToken);
            }

            // Deactivate all active assignments
            var activeAssignments = await _unitOfWork.Repository<Assignment>().GetAllAsync(
                a => a.MemberId == id && a.IsActive,
                cancellationToken);

            foreach (var assignment in activeAssignments)
            {
                assignment.IsActive = false;
                assignment.EndDate = DateTime.UtcNow;
                assignment.UpdatedAtUtc = DateTime.UtcNow;
                await _unitOfWork.Repository<Assignment>().UpdateAsync(assignment, cancellationToken);
            }

            await _unitOfWork.Repository<Member>().UpdateAsync(member, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return BaseResponse<bool>.SuccessResult(true, "Member account deactivated successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.ErrorResult($"Error deactivating member account: {ex.Message}");
        }
    }

    /// <summary>
    /// Locks a member account.
    /// </summary>
    /// <param name="id">The unique identifier of the Member to lock.</param>
    /// <param name="reason">The reason for locking.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse indicating success or failure of the locking.</returns>
    public async Task<BaseResponse<bool>> LockMemberAccountAsync(Guid id, string reason, CancellationToken cancellationToken = default)
    {
        try
        {
            var member = await _unitOfWork.Repository<Member>().GetByIdAsync(id, cancellationToken);
            if (member == null)
            {
                return BaseResponse<bool>.ErrorResult("Member not found");
            }

            var userCredential = await _unitOfWork.Repository<UserCredential>().GetFirstOrDefaultAsync(
                uc => uc.MemberId == id,
                cancellationToken);

            if (userCredential == null)
            {
                return BaseResponse<bool>.ErrorResult("User credential not found");
            }

            userCredential.IsActive = false;
            userCredential.LockedUntilUtc = DateTime.UtcNow.AddDays(30); // Lock for 30 days
            userCredential.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.Repository<UserCredential>().UpdateAsync(userCredential, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return BaseResponse<bool>.SuccessResult(true, "Member account locked successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.ErrorResult($"Error locking member account: {ex.Message}");
        }
    }

    /// <summary>
    /// Unlocks a member account.
    /// </summary>
    /// <param name="id">The unique identifier of the Member to unlock.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse indicating success or failure of the unlocking.</returns>
    public async Task<BaseResponse<bool>> UnlockMemberAccountAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var member = await _unitOfWork.Repository<Member>().GetByIdAsync(id, cancellationToken);
            if (member == null)
            {
                return BaseResponse<bool>.ErrorResult("Member not found");
            }

            var userCredential = await _unitOfWork.Repository<UserCredential>().GetFirstOrDefaultAsync(
                uc => uc.MemberId == id,
                cancellationToken);

            if (userCredential == null)
            {
                return BaseResponse<bool>.ErrorResult("User credential not found");
            }

            userCredential.IsActive = true;
            userCredential.LockedUntilUtc = null;
            userCredential.FailedLoginAttempts = 0;
            userCredential.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.Repository<UserCredential>().UpdateAsync(userCredential, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return BaseResponse<bool>.SuccessResult(true, "Member account unlocked successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.ErrorResult($"Error unlocking member account: {ex.Message}");
        }
    }

    #endregion

    #region Member Search and Validation

    /// <summary>
    /// Searches for members by name or email.
    /// </summary>
    /// <param name="searchTerm">The search term.</param>
    /// <param name="pageNumber">The page number for pagination (default is 1).</param>
    /// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing a PaginatedResponse of MemberDto.</returns>
    public async Task<BaseResponse<PaginatedResponse<IEnumerable<MemberDto>>>> SearchMembersAsync(string searchTerm, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetMembersAsync(pageNumber, pageSize, cancellationToken);
            }

            var searchTermLower = searchTerm.ToLower();
            var (items, totalCount) = await _unitOfWork.Repository<Member>().GetPagedAsync(
                pageNumber,
                pageSize,
                m => m.FirstName.ToLower().Contains(searchTermLower) ||
                     m.LastName.ToLower().Contains(searchTermLower) ||
                     m.Email.ToLower().Contains(searchTermLower) ||
                     (m.FirstName + " " + m.LastName).ToLower().Contains(searchTermLower),
                cancellationToken);

            var dtos = _mapper.Map<IEnumerable<MemberDto>>(items);

            var paginatedResponse = new PaginatedResponse<IEnumerable<MemberDto>>
            {
                Data = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return BaseResponse<PaginatedResponse<IEnumerable<MemberDto>>>.SuccessResult(paginatedResponse);
        }
        catch (Exception ex)
        {
            return BaseResponse<PaginatedResponse<IEnumerable<MemberDto>>>.ErrorResult($"Error searching members: {ex.Message}");
        }
    }

    /// <summary>
    /// Validates if an email is available for use.
    /// </summary>
    /// <param name="email">The email address to validate.</param>
    /// <param name="excludeMemberId">Optional member ID to exclude from validation (for updates).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing a boolean indicating if the email is available.</returns>
    public async Task<BaseResponse<bool>> IsEmailAvailableAsync(string email, Guid? excludeMemberId = null, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BaseResponse<bool>.ErrorResult("Email cannot be empty");
            }

            var exists = await _unitOfWork.Repository<Member>().ExistsAsync(
                m => m.Email.ToLower() == email.ToLower() && (excludeMemberId == null || m.Id != excludeMemberId),
                cancellationToken);

            return BaseResponse<bool>.SuccessResult(!exists);
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.ErrorResult($"Error validating email: {ex.Message}");
        }
    }

    /// <summary>
    /// Validates if a CPF is available for use.
    /// </summary>
    /// <param name="cpf">The CPF to validate.</param>
    /// <param name="excludeMemberId">Optional member ID to exclude from validation (for updates).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing a boolean indicating if the CPF is available.</returns>
    public async Task<BaseResponse<bool>> IsCpfAvailableAsync(string cpf, Guid? excludeMemberId = null, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(cpf))
            {
                return BaseResponse<bool>.SuccessResult(true); // Empty CPF is considered available
            }

            // Basic CPF validation (11 digits)
            var cleanCpf = cpf.Replace(".", "").Replace("-", "").Replace(" ", "");
            if (cleanCpf.Length != 11 || !cleanCpf.All(char.IsDigit))
            {
                return BaseResponse<bool>.ErrorResult("Invalid CPF format");
            }

            var exists = await _unitOfWork.Repository<Member>().ExistsAsync(
                m => m.Cpf == cleanCpf && (excludeMemberId == null || m.Id != excludeMemberId),
                cancellationToken);

            return BaseResponse<bool>.SuccessResult(!exists);
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.ErrorResult($"Error validating CPF: {ex.Message}");
        }
    }

    #endregion
}
