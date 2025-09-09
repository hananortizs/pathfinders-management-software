using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Auth;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Domain.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Pms.Backend.Application.Services.Members;

/// <summary>
/// Service for member authentication operations
/// </summary>
public partial class MemberService : IMemberService
{
    #region Authentication Operations

    /// <summary>
    /// Authenticates a user with email and password.
    /// </summary>
    /// <param name="request">The login request containing email and password.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the login response with JWT token if successful, or an error.</returns>
    public async Task<BaseResponse<LoginResponseDto>> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            // Find member by email through contacts
            var member = await _unitOfWork.Repository<Member>().GetFirstOrDefaultAsync(
                m => m.Contacts.Any(c => c.Type == ContactType.Email &&
                                        c.Value == request.Email &&
                                        !c.IsDeleted),
                cancellationToken);

            if (member == null)
            {
                return BaseResponse<LoginResponseDto>.ErrorResult("Invalid email or password");
            }

            // Check if member has user credentials
            var userCredential = await _unitOfWork.Repository<UserCredential>().GetFirstOrDefaultAsync(
                uc => uc.MemberId == member.Id,
                cancellationToken);

            if (userCredential == null)
            {
                return BaseResponse<LoginResponseDto>.ErrorResult("Account not activated");
            }

            // Check if account is active
            if (!userCredential.IsActive)
            {
                return BaseResponse<LoginResponseDto>.ErrorResult("Account is deactivated");
            }

            // Check if account is locked
            if (userCredential.LockedUntilUtc.HasValue && userCredential.LockedUntilUtc > DateTime.UtcNow)
            {
                return BaseResponse<LoginResponseDto>.ErrorResult("Account is temporarily locked");
            }

            // Verify password
            if (!VerifyPassword(request.Password, userCredential.PasswordHash))
            {
                // Increment failed login attempts
                userCredential.FailedLoginAttempts++;

                // Lock account after 5 failed attempts
                if (userCredential.FailedLoginAttempts >= 5)
                {
                    userCredential.LockedUntilUtc = DateTime.UtcNow.AddMinutes(30);
                }

                userCredential.UpdatedAtUtc = DateTime.UtcNow;
                await _unitOfWork.Repository<UserCredential>().UpdateAsync(userCredential, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return BaseResponse<LoginResponseDto>.ErrorResult("Invalid email or password");
            }

            // Reset failed login attempts on successful login
            userCredential.FailedLoginAttempts = 0;
            userCredential.LockedUntilUtc = null;
            userCredential.LastLoginAtUtc = DateTime.UtcNow;
            userCredential.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.Repository<UserCredential>().UpdateAsync(userCredential, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Generate JWT token
            var token = GenerateJwtToken(member);
            var userInfo = _mapper.Map<UserInfoDto>(member);

            var response = new LoginResponseDto
            {
                AccessToken = token,
                TokenType = "Bearer",
                ExpiresIn = int.Parse(_configuration["JwtSettings:ExpirationMinutes"] ?? "60") * 60,
                User = userInfo
            };

            return BaseResponse<LoginResponseDto>.SuccessResult(response, "Login successful");
        }
        catch (Exception ex)
        {
            return BaseResponse<LoginResponseDto>.ErrorResult($"Error during login: {ex.Message}");
        }
    }

    /// <summary>
    /// Changes the password for a user.
    /// </summary>
    /// <param name="memberId">The ID of the member.</param>
    /// <param name="request">The password change request.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse indicating success or failure of the password change.</returns>
    public async Task<BaseResponse<bool>> ChangePasswordAsync(Guid memberId, ChangePasswordRequestDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate passwords match
            if (request.NewPassword != request.ConfirmPassword)
            {
                return BaseResponse<bool>.ErrorResult("New password and confirmation do not match");
            }

            // Validate password strength
            if (request.NewPassword.Length < 8)
            {
                return BaseResponse<bool>.ErrorResult("Password must be at least 8 characters long");
            }

            // Find user credential
            var userCredential = await _unitOfWork.Repository<UserCredential>().GetFirstOrDefaultAsync(
                uc => uc.MemberId == memberId,
                cancellationToken);

            if (userCredential == null)
            {
                return BaseResponse<bool>.ErrorResult("User credential not found");
            }

            // Verify current password
            if (!VerifyPassword(request.CurrentPassword, userCredential.PasswordHash))
            {
                return BaseResponse<bool>.ErrorResult("Current password is incorrect");
            }

            // Check password history (prevent reusing last 5 passwords)
            var recentPasswords = await _unitOfWork.Repository<PasswordHistory>().GetAllAsync(
                ph => ph.UserCredentialId == userCredential.Id,
                cancellationToken);

            foreach (var passwordHistory in recentPasswords.OrderByDescending(ph => ph.CreatedAtUtc).Take(5))
            {
                if (VerifyPassword(request.NewPassword, passwordHistory.PasswordHash))
                {
                    return BaseResponse<bool>.ErrorResult("Cannot reuse a recent password");
                }
            }

            // Hash new password
            var newPasswordHash = HashPassword(request.NewPassword);

            // Add to password history
            var passwordHistoryEntry = new PasswordHistory
            {
                Id = Guid.NewGuid(),
                UserCredentialId = userCredential.Id,
                PasswordHash = newPasswordHash,
                CreatedAtUtc = DateTime.UtcNow
            };

            await _unitOfWork.Repository<PasswordHistory>().AddAsync(passwordHistoryEntry, cancellationToken);

            // Update user credential
            userCredential.PasswordHash = newPasswordHash;
            userCredential.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.Repository<UserCredential>().UpdateAsync(userCredential, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return BaseResponse<bool>.SuccessResult(true, "Password changed successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.ErrorResult($"Error changing password: {ex.Message}");
        }
    }

    /// <summary>
    /// Initiates a password reset process.
    /// </summary>
    /// <param name="request">The password reset request containing email.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse indicating success or failure of the password reset initiation.</returns>
    public async Task<BaseResponse<bool>> ResetPasswordAsync(ResetPasswordRequestDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            // Find member by email through contacts
            var member = await _unitOfWork.Repository<Member>().GetFirstOrDefaultAsync(
                m => m.Contacts.Any(c => c.Type == ContactType.Email &&
                                        c.Value == request.Email &&
                                        !c.IsDeleted),
                cancellationToken);

            if (member == null)
            {
                // Return success even if email doesn't exist (security best practice)
                return BaseResponse<bool>.SuccessResult(true, "If the email exists, a reset link has been sent");
            }

            // Find user credential
            var userCredential = await _unitOfWork.Repository<UserCredential>().GetFirstOrDefaultAsync(
                uc => uc.MemberId == member.Id,
                cancellationToken);

            if (userCredential == null)
            {
                return BaseResponse<bool>.SuccessResult(true, "If the email exists, a reset link has been sent");
            }

            // Generate reset token
            var resetToken = GenerateSecureToken();
            var resetTokenExpiry = DateTime.UtcNow.AddHours(1); // Token expires in 1 hour

            // Store reset token (in a real implementation, you might want to store this in a separate table)
            // For now, we'll use a simple approach with the UserCredential entity
            // In a production system, you'd want a dedicated PasswordResetToken table

            // TODO: Implement email sending service
            // await _emailService.SendPasswordResetEmailAsync(member.PrimaryEmail, resetToken);

            return BaseResponse<bool>.SuccessResult(true, "If the email exists, a reset link has been sent");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.ErrorResult($"Error initiating password reset: {ex.Message}");
        }
    }

    /// <summary>
    /// Confirms a password reset with a token.
    /// </summary>
    /// <param name="request">The password reset confirmation request.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse indicating success or failure of the password reset confirmation.</returns>
    public Task<BaseResponse<bool>> ResetPasswordConfirmAsync(ResetPasswordConfirmDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate passwords match
            if (request.NewPassword != request.ConfirmPassword)
            {
                return Task.FromResult(BaseResponse<bool>.ErrorResult("New password and confirmation do not match"));
            }

            // Validate password strength
            if (request.NewPassword.Length < 8)
            {
                return Task.FromResult(BaseResponse<bool>.ErrorResult("Password must be at least 8 characters long"));
            }

            // TODO: Implement token validation logic
            // In a real implementation, you would:
            // 1. Validate the reset token
            // 2. Check if it's not expired
            // 3. Find the associated user
            // 4. Update the password
            // 5. Invalidate the token

            return Task.FromResult(BaseResponse<bool>.ErrorResult("Password reset confirmation not implemented yet"));
        }
        catch (Exception ex)
        {
            return Task.FromResult(BaseResponse<bool>.ErrorResult($"Error confirming password reset: {ex.Message}"));
        }
    }

    #endregion
}
