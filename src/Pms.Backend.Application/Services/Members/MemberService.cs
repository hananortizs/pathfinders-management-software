using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Auth;
using Pms.Backend.Application.DTOs.Members;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Pms.Backend.Application.Services.Members;

/// <summary>
/// Service implementation for member management operations
/// </summary>
public partial class MemberService : IMemberService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the MemberService
    /// </summary>
    /// <param name="unitOfWork">Unit of work for data access</param>
    /// <param name="mapper">AutoMapper instance for object mapping</param>
    /// <param name="configuration">Configuration instance for JWT settings</param>
    public MemberService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _configuration = configuration;
    }

    #region Member CRUD Operations

    /// <summary>
    /// Retrieves a specific Member by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the Member.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the MemberDto if found, or an error.</returns>
    public async Task<BaseResponse<MemberDto>> GetMemberAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var member = await _unitOfWork.Repository<Member>().GetByIdAsync(id, cancellationToken);
            if (member == null)
            {
                return BaseResponse<MemberDto>.ErrorResult("Member not found");
            }

            var dto = _mapper.Map<MemberDto>(member);
            return BaseResponse<MemberDto>.SuccessResult(dto);
        }
        catch (Exception ex)
        {
            return BaseResponse<MemberDto>.ErrorResult($"Error retrieving member: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves a paginated list of Members.
    /// </summary>
    /// <param name="pageNumber">The page number for pagination (default is 1).</param>
    /// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing a PaginatedResponse of MemberDto.</returns>
    public async Task<BaseResponse<PaginatedResponse<IEnumerable<MemberDto>>>> GetMembersAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            var (items, totalCount) = await _unitOfWork.Repository<Member>().GetPagedAsync(pageNumber, pageSize, null, cancellationToken);
            var dtos = _mapper.Map<IEnumerable<MemberDto>>(items);

            var paginatedResponse = new PaginatedResponse<IEnumerable<MemberDto>>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return BaseResponse<PaginatedResponse<IEnumerable<MemberDto>>>.SuccessResult(paginatedResponse);
        }
        catch (Exception ex)
        {
            return BaseResponse<PaginatedResponse<IEnumerable<MemberDto>>>.ErrorResult($"Error retrieving members: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves a paginated list of Members by Club ID.
    /// </summary>
    /// <param name="clubId">The ID of the Club.</param>
    /// <param name="pageNumber">The page number for pagination (default is 1).</param>
    /// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing a PaginatedResponse of MemberDto.</returns>
    public async Task<BaseResponse<PaginatedResponse<IEnumerable<MemberDto>>>> GetMembersByClubAsync(Guid clubId, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            // Verify club exists
            var club = await _unitOfWork.Repository<Club>().GetByIdAsync(clubId, cancellationToken);
            if (club == null)
            {
                return BaseResponse<PaginatedResponse<IEnumerable<MemberDto>>>.ErrorResult("Club not found");
            }

            var (items, totalCount) = await _unitOfWork.Repository<Member>().GetPagedAsync(
                pageNumber,
                pageSize,
                m => m.Memberships.Any(mem => mem.ClubId == clubId && mem.IsActive),
                cancellationToken);

            var dtos = _mapper.Map<IEnumerable<MemberDto>>(items);

            var paginatedResponse = new PaginatedResponse<IEnumerable<MemberDto>>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return BaseResponse<PaginatedResponse<IEnumerable<MemberDto>>>.SuccessResult(paginatedResponse);
        }
        catch (Exception ex)
        {
            return BaseResponse<PaginatedResponse<IEnumerable<MemberDto>>>.ErrorResult($"Error retrieving members by club: {ex.Message}");
        }
    }

    /// <summary>
    /// Creates a new Member.
    /// </summary>
    /// <param name="dto">The data transfer object containing the Member's details.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the created MemberDto if successful, or an error.</returns>
    public async Task<BaseResponse<MemberDto>> CreateMemberAsync(CreateMemberDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate email availability
            var emailAvailable = await IsEmailAvailableAsync(dto.Email, null, cancellationToken);
            if (!emailAvailable.Data)
            {
                return BaseResponse<MemberDto>.ErrorResult("Email already exists");
            }

            // Validate CPF if provided
            if (!string.IsNullOrEmpty(dto.Cpf))
            {
                var cpfAvailable = await IsCpfAvailableAsync(dto.Cpf, null, cancellationToken);
                if (!cpfAvailable.Data)
                {
                    return BaseResponse<MemberDto>.ErrorResult("CPF already exists");
                }
            }

            // Validate age (minimum 10 years old)
            var age = CalculateAge(dto.DateOfBirth);
            if (age < 10)
            {
                return BaseResponse<MemberDto>.ErrorResult("Member must be at least 10 years old");
            }

            var member = _mapper.Map<Member>(dto);
            member.Id = Guid.NewGuid();
            member.Status = MemberStatus.Pending;
            member.CreatedAtUtc = DateTime.UtcNow;
            member.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.Repository<Member>().AddAsync(member, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var resultDto = _mapper.Map<MemberDto>(member);
            return BaseResponse<MemberDto>.SuccessResult(resultDto, "Member created successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<MemberDto>.ErrorResult($"Error creating member: {ex.Message}");
        }
    }

    /// <summary>
    /// Updates an existing Member.
    /// </summary>
    /// <param name="id">The unique identifier of the Member to update.</param>
    /// <param name="dto">The data transfer object containing the updated Member's details.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the updated MemberDto if successful, or an error.</returns>
    public async Task<BaseResponse<MemberDto>> UpdateMemberAsync(Guid id, UpdateMemberDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var member = await _unitOfWork.Repository<Member>().GetByIdAsync(id, cancellationToken);
            if (member == null)
            {
                return BaseResponse<MemberDto>.ErrorResult("Member not found");
            }

            // Validate email availability (excluding current member)
            var emailAvailable = await IsEmailAvailableAsync(dto.Email, id, cancellationToken);
            if (!emailAvailable.Data)
            {
                return BaseResponse<MemberDto>.ErrorResult("Email already exists");
            }

            // Validate CPF if provided (excluding current member)
            if (!string.IsNullOrEmpty(dto.Cpf))
            {
                var cpfAvailable = await IsCpfAvailableAsync(dto.Cpf, id, cancellationToken);
                if (!cpfAvailable.Data)
                {
                    return BaseResponse<MemberDto>.ErrorResult("CPF already exists");
                }
            }

            // Validate age (minimum 10 years old)
            var age = CalculateAge(dto.DateOfBirth);
            if (age < 10)
            {
                return BaseResponse<MemberDto>.ErrorResult("Member must be at least 10 years old");
            }

            _mapper.Map(dto, member);
            member.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.Repository<Member>().UpdateAsync(member, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var resultDto = _mapper.Map<MemberDto>(member);
            return BaseResponse<MemberDto>.SuccessResult(resultDto, "Member updated successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<MemberDto>.ErrorResult($"Error updating member: {ex.Message}");
        }
    }

    /// <summary>
    /// Deletes a Member by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the Member to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse indicating success or failure of the deletion.</returns>
    public async Task<BaseResponse<bool>> DeleteMemberAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if member has active memberships
            var hasActiveMemberships = await _unitOfWork.Repository<Domain.Entities.Membership>().ExistsAsync(m => m.MemberId == id && m.IsActive, cancellationToken);
            if (hasActiveMemberships)
            {
                return BaseResponse<bool>.ErrorResult("Cannot delete member with active memberships");
            }

            // Check if member has active assignments
            var hasActiveAssignments = await _unitOfWork.Repository<Assignment>().ExistsAsync(a => a.MemberId == id && a.IsActive, cancellationToken);
            if (hasActiveAssignments)
            {
                return BaseResponse<bool>.ErrorResult("Cannot delete member with active assignments");
            }

            var deleted = await _unitOfWork.Repository<Member>().DeleteAsync(id, cancellationToken);
            if (!deleted)
            {
                return BaseResponse<bool>.ErrorResult("Member not found");
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return BaseResponse<bool>.SuccessResult(true, "Member deleted successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.ErrorResult($"Error deleting member: {ex.Message}");
        }
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Calculates the age based on date of birth
    /// </summary>
    /// <param name="dateOfBirth">Date of birth</param>
    /// <returns>Age in years</returns>
    private static int CalculateAge(DateTime dateOfBirth)
    {
        var today = DateTime.Today;
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > today.AddYears(-age)) age--;
        return age;
    }

    /// <summary>
    /// Generates a JWT token for authentication
    /// </summary>
    /// <param name="member">Member entity</param>
    /// <returns>JWT token string</returns>
    private string GenerateJwtToken(Member member)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
        var issuer = jwtSettings["Issuer"] ?? "PmsBackend";
        var audience = jwtSettings["Audience"] ?? "PmsBackend";
        var expirationMinutes = int.Parse(jwtSettings["ExpirationMinutes"] ?? "60");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, member.Id.ToString()),
            new(ClaimTypes.Email, member.Email),
            new(ClaimTypes.Name, member.FullName),
            new("sub", member.Id.ToString())
        };

        // Add roles from assignments
        var activeAssignments = member.Assignments?.Where(a => a.IsActive).ToList() ?? new List<Assignment>();
        foreach (var assignment in activeAssignments)
        {
            claims.Add(new Claim(ClaimTypes.Role, assignment.RoleCatalog.Name));
            // Add scope based on assignment scope type
            claims.Add(new Claim("scope", assignment.ScopeType.ToString()));
        }

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Hashes a password using BCrypt
    /// </summary>
    /// <param name="password">Plain text password</param>
    /// <returns>Hashed password</returns>
    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    /// <summary>
    /// Verifies a password against a hash
    /// </summary>
    /// <param name="password">Plain text password</param>
    /// <param name="hash">Hashed password</param>
    /// <returns>True if password matches hash</returns>
    private static bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }

    /// <summary>
    /// Generates a secure random token
    /// </summary>
    /// <param name="length">Token length in bytes</param>
    /// <returns>Base64 encoded token</returns>
    private static string GenerateSecureToken(int length = 32)
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[length];
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }

    #endregion
}
