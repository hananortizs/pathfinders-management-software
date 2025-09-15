using AutoMapper;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Auth;
using Pms.Backend.Application.DTOs.Members;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Application.Services.Members;

/// <summary>
/// Service for member invitation and activation operations
/// </summary>
public partial class MemberService : IMemberService
{
    #region Member Invitation and Activation

    /// <summary>
    /// Invites a new member to join a club.
    /// </summary>
    /// <param name="request">The member invitation request.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse indicating success or failure of the invitation.</returns>
    public async Task<BaseResponse<bool>> InviteMemberAsync(InviteMemberRequestDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate email availability
            var emailAvailable = await IsEmailAvailableAsync(request.Email, null, cancellationToken);
            if (!emailAvailable.Data)
            {
                return BaseResponse<bool>.ErrorResult("Email already exists");
            }

            // Verify club exists
            var club = await _unitOfWork.Repository<Club>().GetByIdAsync(request.ClubId, cancellationToken);
            if (club == null)
            {
                return BaseResponse<bool>.ErrorResult("Club not found");
            }

            // Validate age (minimum 10 years old)
            var age = CalculateAge(request.DateOfBirth);
            if (age < 10)
            {
                return BaseResponse<bool>.ErrorResult("Member must be at least 10 years old");
            }

            // Create member from invitation
            var createMemberDto = _mapper.Map<CreateMemberCompleteDto>(request);
            var member = _mapper.Map<Member>(createMemberDto);
            member.Id = Guid.NewGuid();
            member.Status = MemberStatus.Pending;
            member.CreatedAtUtc = DateTime.UtcNow;
            member.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.Repository<Member>().AddAsync(member, cancellationToken);

            // Create user credential with activation token
            var activationToken = GenerateSecureToken();
            var userCredential = new UserCredential
            {
                Id = Guid.NewGuid(),
                MemberId = member.Id,
                PasswordHash = string.Empty, // Will be set during activation
                IsActive = false,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow
            };

            await _unitOfWork.Repository<UserCredential>().AddAsync(userCredential, cancellationToken);

            // Create membership
            var membership = new Domain.Entities.Membership
            {
                Id = Guid.NewGuid(),
                MemberId = member.Id,
                ClubId = request.ClubId,
                UnitId = null, // Will be assigned during activation
                IsActive = false,
                StartDate = DateTime.UtcNow,
                EndDate = null,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow
            };

            await _unitOfWork.Repository<Domain.Entities.Membership>().AddAsync(membership, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // TODO: Implement email sending service
            // await _emailService.SendMemberInvitationEmailAsync(member.PrimaryEmail, activationToken, member.DisplayName);

            return BaseResponse<bool>.SuccessResult(true, "Member invitation sent successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.ErrorResult($"Error inviting member: {ex.Message}");
        }
    }

    /// <summary>
    /// Activates a member account using an activation token.
    /// </summary>
    /// <param name="request">The member activation request.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the login response with JWT token if successful, or an error.</returns>
    public async Task<BaseResponse<DTOs.Auth.LoginResponseDto>> ActivateMemberAsync(ActivateMemberRequestDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate passwords match
            if (request.Password != request.ConfirmPassword)
            {
                return BaseResponse<DTOs.Auth.LoginResponseDto>.ErrorResult("Password and confirmation do not match");
            }

            // Validate password strength
            if (request.Password.Length < 8)
            {
                return BaseResponse<DTOs.Auth.LoginResponseDto>.ErrorResult("Password must be at least 8 characters long");
            }

            // TODO: Implement token validation logic
            // In a real implementation, you would:
            // 1. Validate the activation token
            // 2. Check if it's not expired
            // 3. Find the associated member
            // 4. Complete the activation process

            // For now, we'll simulate finding a member by token
            // This is a placeholder implementation
            var member = await _unitOfWork.Repository<Member>().GetFirstOrDefaultAsync(
                m => m.Status == MemberStatus.Pending,
                cancellationToken);

            if (member == null)
            {
                return BaseResponse<DTOs.Auth.LoginResponseDto>.ErrorResult("Invalid or expired activation token");
            }

            // Find user credential
            var userCredential = await _unitOfWork.Repository<UserCredential>().GetFirstOrDefaultAsync(
                uc => uc.MemberId == member.Id,
                cancellationToken);

            if (userCredential == null)
            {
                return BaseResponse<DTOs.Auth.LoginResponseDto>.ErrorResult("User credential not found");
            }

            // Update member information if provided
            if (request.MemberInfo != null)
            {
                if (!string.IsNullOrEmpty(request.MemberInfo.Cpf))
                {
                    var cpfAvailable = await IsCpfAvailableAsync(request.MemberInfo.Cpf, member.Id, cancellationToken);
                    if (!cpfAvailable.Data)
                    {
                        return BaseResponse<DTOs.Auth.LoginResponseDto>.ErrorResult("CPF already exists");
                    }
                    member.Cpf = request.MemberInfo.Cpf;
                }

                member.Rg = request.MemberInfo.Rg ?? string.Empty;
                // Address fields removed - now using centralized Address entity
                member.EmergencyContactName = request.MemberInfo.EmergencyContactName ?? string.Empty;
                member.EmergencyContactPhone = request.MemberInfo.EmergencyContactPhone ?? string.Empty;
                member.EmergencyContactRelationship = request.MemberInfo.EmergencyContactRelationship ?? string.Empty;
                member.MedicalInfo = request.MemberInfo.MedicalInfo ?? string.Empty;
                member.Allergies = request.MemberInfo.Allergies;
                member.Medications = request.MemberInfo.Medications;
                member.BaptismDate = request.MemberInfo.BaptismDate;
                member.BaptismChurch = request.MemberInfo.BaptismChurch;
                member.BaptismPastor = request.MemberInfo.BaptismPastor;
                member.ScarfDate = request.MemberInfo.ScarfDate;
                member.ScarfChurch = request.MemberInfo.ScarfChurch;
                member.ScarfPastor = request.MemberInfo.ScarfPastor;
            }

            // Activate member
            member.Status = MemberStatus.Active;
            member.ActivatedAtUtc = DateTime.UtcNow;
            member.UpdatedAtUtc = DateTime.UtcNow;

            // Update user credential
            userCredential.PasswordHash = HashPassword(request.Password);
            userCredential.IsActive = true;
            userCredential.UpdatedAtUtc = DateTime.UtcNow;

            // Add to password history
            var passwordHistory = new PasswordHistory
            {
                Id = Guid.NewGuid(),
                UserCredentialId = userCredential.Id,
                PasswordHash = userCredential.PasswordHash,
                CreatedAtUtc = DateTime.UtcNow
            };

            await _unitOfWork.Repository<PasswordHistory>().AddAsync(passwordHistory, cancellationToken);

            // Activate membership
            var membership = await _unitOfWork.Repository<Domain.Entities.Membership>().GetFirstOrDefaultAsync(
                m => m.MemberId == member.Id && !m.IsActive,
                cancellationToken);

            if (membership != null)
            {
                membership.IsActive = true;
                membership.UpdatedAtUtc = DateTime.UtcNow;
                await _unitOfWork.Repository<Domain.Entities.Membership>().UpdateAsync(membership, cancellationToken);
            }

            await _unitOfWork.Repository<Member>().UpdateAsync(member, cancellationToken);
            await _unitOfWork.Repository<UserCredential>().UpdateAsync(userCredential, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Generate JWT token
            var token = GenerateJwtToken(member);
            var userInfo = _mapper.Map<UserInfoDto>(member);

            var response = new DTOs.Auth.LoginResponseDto
            {
                AccessToken = token,
                TokenType = "Bearer",
                ExpiresIn = int.Parse(_configuration["JwtSettings:ExpirationMinutes"] ?? "60") * 60,
            };

            return BaseResponse<DTOs.Auth.LoginResponseDto>.SuccessResult(response, "Member activated successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<DTOs.Auth.LoginResponseDto>.ErrorResult($"Error activating member: {ex.Message}");
        }
    }

    /// <summary>
    /// Resends an activation email to a member.
    /// </summary>
    /// <param name="memberId">The ID of the member.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse indicating success or failure of the resend operation.</returns>
    public async Task<BaseResponse<bool>> ResendActivationEmailAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        try
        {
            var member = await _unitOfWork.Repository<Member>().GetByIdAsync(memberId, cancellationToken);
            if (member == null)
            {
                return BaseResponse<bool>.ErrorResult("Member not found");
            }

            if (member.Status != MemberStatus.Pending)
            {
                return BaseResponse<bool>.ErrorResult("Member is not in pending status");
            }

            // Generate new activation token
            var activationToken = GenerateSecureToken();

            // TODO: Implement email sending service
            // await _emailService.SendMemberInvitationEmailAsync(member.PrimaryEmail, activationToken, member.DisplayName);

            return BaseResponse<bool>.SuccessResult(true, "Activation email sent successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.ErrorResult($"Error resending activation email: {ex.Message}");
        }
    }

    #endregion
}
