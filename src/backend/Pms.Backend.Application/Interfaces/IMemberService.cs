using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Auth;
using Pms.Backend.Application.DTOs.Members;

namespace Pms.Backend.Application.Interfaces;

/// <summary>
/// Interface for member management operations
/// </summary>
public interface IMemberService
{
    #region Member CRUD Operations

    /// <summary>
    /// Retrieves a specific Member by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the Member.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the MemberDto if found, or an error.</returns>
    Task<BaseResponse<MemberDto>> GetMemberAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paginated list of Members.
    /// </summary>
    /// <param name="pageNumber">The page number for pagination (default is 1).</param>
    /// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing a PaginatedResponse of MemberDto.</returns>
    Task<BaseResponse<PaginatedResponse<IEnumerable<MemberDto>>>> GetMembersAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paginated list of Members by Club ID.
    /// </summary>
    /// <param name="clubId">The ID of the Club.</param>
    /// <param name="pageNumber">The page number for pagination (default is 1).</param>
    /// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing a PaginatedResponse of MemberDto.</returns>
    Task<BaseResponse<PaginatedResponse<IEnumerable<MemberDto>>>> GetMembersByClubAsync(Guid clubId, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paginated list of Members with optimized response structure.
    /// </summary>
    /// <param name="pageNumber">The page number for pagination (default is 1).</param>
    /// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing an OptimizedMemberListResponse.</returns>
    Task<BaseResponse<OptimizedMemberListResponse>> GetMembersOptimizedAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new Member with complete information including address, medical info, contacts, etc.
    /// Valida sub-objetos se preenchidos e faz inserção sequencial com rollback em caso de erro.
    /// </summary>
    /// <param name="dto">The data transfer object containing the Member's complete details.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the created MemberDto if successful, or an error.</returns>
    Task<BaseResponse<MemberDto>> CreateMemberCompleteAsync(CreateMemberCompleteDto dto, CancellationToken cancellationToken = default);


    /// <summary>
    /// Updates an existing Member.
    /// </summary>
    /// <param name="id">The unique identifier of the Member to update.</param>
    /// <param name="dto">The data transfer object containing the updated Member's details.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the updated MemberDto if successful, or an error.</returns>
    Task<BaseResponse<MemberDto>> UpdateMemberAsync(Guid id, UpdateMemberDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Completes a member's pending information (for /me/complete endpoint)
    /// </summary>
    /// <param name="id">The unique identifier of the Member to complete.</param>
    /// <param name="dto">The complete member information data.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the updated MemberDto if successful, or an error.</returns>
    Task<BaseResponse<MemberDto>> CompleteMemberAsync(Guid id, CreateMemberCompleteDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a Member by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the Member to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse indicating success or failure of the deletion.</returns>
    Task<BaseResponse<bool>> DeleteMemberAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Hard deletes a Member and all related data permanently.
    /// </summary>
    /// <param name="id">The unique identifier of the Member to hard delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse indicating success or failure of the hard deletion.</returns>
    Task<BaseResponse<bool>> HardDeleteMemberAsync(Guid id, CancellationToken cancellationToken = default);

    #endregion

    #region Authentication Operations

    /// <summary>
    /// Authenticates a user with email and password.
    /// </summary>
    /// <param name="request">The login request containing email and password.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the login response with JWT token if successful, or an error.</returns>
    Task<BaseResponse<DTOs.Auth.LoginResponseDto>> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Changes the password for a user.
    /// </summary>
    /// <param name="memberId">The ID of the member.</param>
    /// <param name="request">The password change request.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse indicating success or failure of the password change.</returns>
    Task<BaseResponse<bool>> ChangePasswordAsync(Guid memberId, ChangePasswordRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Initiates a password reset process.
    /// </summary>
    /// <param name="request">The password reset request containing email.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse indicating success or failure of the password reset initiation.</returns>
    Task<BaseResponse<bool>> ResetPasswordAsync(ResetPasswordRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Confirms a password reset with a token.
    /// </summary>
    /// <param name="request">The password reset confirmation request.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse indicating success or failure of the password reset confirmation.</returns>
    Task<BaseResponse<bool>> ResetPasswordConfirmAsync(ResetPasswordConfirmDto request, CancellationToken cancellationToken = default);

    #endregion

    #region Member Invitation and Activation

    /// <summary>
    /// Invites a new member to join a club.
    /// </summary>
    /// <param name="request">The member invitation request.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse indicating success or failure of the invitation.</returns>
    Task<BaseResponse<bool>> InviteMemberAsync(InviteMemberRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Activates a member account using an activation token.
    /// </summary>
    /// <param name="request">The member activation request.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the login response with JWT token if successful, or an error.</returns>
    Task<BaseResponse<DTOs.Auth.LoginResponseDto>> ActivateMemberAsync(ActivateMemberRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Resends an activation email to a member.
    /// </summary>
    /// <param name="memberId">The ID of the member.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse indicating success or failure of the resend operation.</returns>
    Task<BaseResponse<bool>> ResendActivationEmailAsync(Guid memberId, CancellationToken cancellationToken = default);

    #endregion

    #region Member Status Operations

    /// <summary>
    /// Activates a member account.
    /// </summary>
    /// <param name="id">The unique identifier of the Member to activate.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse indicating success or failure of the activation.</returns>
    Task<BaseResponse<bool>> ActivateMemberAccountAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deactivates a member account.
    /// </summary>
    /// <param name="id">The unique identifier of the Member to deactivate.</param>
    /// <param name="reason">The reason for deactivation.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse indicating success or failure of the deactivation.</returns>
    Task<BaseResponse<bool>> DeactivateMemberAccountAsync(Guid id, string reason, CancellationToken cancellationToken = default);

    /// <summary>
    /// Locks a member account.
    /// </summary>
    /// <param name="id">The unique identifier of the Member to lock.</param>
    /// <param name="reason">The reason for locking.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse indicating success or failure of the locking.</returns>
    Task<BaseResponse<bool>> LockMemberAccountAsync(Guid id, string reason, CancellationToken cancellationToken = default);

    /// <summary>
    /// Unlocks a member account.
    /// </summary>
    /// <param name="id">The unique identifier of the Member to unlock.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse indicating success or failure of the unlocking.</returns>
    Task<BaseResponse<bool>> UnlockMemberAccountAsync(Guid id, CancellationToken cancellationToken = default);

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
    Task<BaseResponse<PaginatedResponse<IEnumerable<MemberDto>>>> SearchMembersAsync(string searchTerm, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates if an email is available for use.
    /// </summary>
    /// <param name="email">The email address to validate.</param>
    /// <param name="excludeMemberId">Optional member ID to exclude from validation (for updates).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing a boolean indicating if the email is available.</returns>
    Task<BaseResponse<bool>> IsEmailAvailableAsync(string email, Guid? excludeMemberId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates if a CPF is available for use.
    /// </summary>
    /// <param name="cpf">The CPF to validate.</param>
    /// <param name="excludeMemberId">Optional member ID to exclude from validation (for updates).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing a boolean indicating if the CPF is available.</returns>
    Task<BaseResponse<bool>> IsCpfAvailableAsync(string cpf, Guid? excludeMemberId = null, CancellationToken cancellationToken = default);

    #endregion
}
