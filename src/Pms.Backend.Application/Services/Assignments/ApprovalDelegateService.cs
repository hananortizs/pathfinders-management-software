using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Assignments;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Application.Services.Assignments;

/// <summary>
/// Service for managing approval delegations
/// Handles delegation of approval authority when original approvers are unavailable
/// </summary>
public partial class ApprovalDelegateService : IApprovalDelegateService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the ApprovalDelegateService
    /// </summary>
    /// <param name="unitOfWork">The unit of work</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public ApprovalDelegateService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    #region Delegation CRUD Operations

    /// <summary>
    /// Creates a new approval delegation
    /// </summary>
    /// <param name="request">The delegation creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created delegation</returns>
    public async Task<BaseResponse<ApprovalDelegateDto>> CreateDelegationAsync(CreateApprovalDelegateDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate delegation
            var validationResult = await ValidateDelegationAsync(request, cancellationToken);
            if (!validationResult.Success)
            {
                return BaseResponse<ApprovalDelegateDto>.ErrorResult(validationResult.Message ?? "Delegation validation failed");
            }

            // Check for overlapping delegations
            var overlappingDelegation = await _unitOfWork.Repository<ApprovalDelegate>().GetFirstOrDefaultAsync(
                d => d.Role == request.Role &&
                     d.ScopeType == request.ScopeType &&
                     d.ScopeId == request.ScopeId &&
                     d.DelegatedFromAssignmentId == request.DelegatedFromAssignmentId &&
                     d.IsActive &&
                     ((d.StartDate <= request.StartDate && d.EndDate >= request.StartDate) ||
                      (d.StartDate <= request.EndDate && d.EndDate >= request.EndDate) ||
                      (d.StartDate >= request.StartDate && d.EndDate <= request.EndDate)),
                cancellationToken);

            if (overlappingDelegation != null)
            {
                return BaseResponse<ApprovalDelegateDto>.ErrorResult("There is already an active delegation for this role and scope in the specified date range");
            }

            // Create delegation
            var delegation = _mapper.Map<ApprovalDelegate>(request);
            delegation.Id = Guid.NewGuid();
            delegation.CreatedAtUtc = DateTime.UtcNow;
            delegation.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.Repository<ApprovalDelegate>().AddAsync(delegation, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Get the created delegation with navigation properties
            var createdDelegation = await _unitOfWork.Repository<ApprovalDelegate>()
                .GetQueryable(d => d.Id == delegation.Id)
                .Include(d => d.DelegatedToAssignment)
                    .ThenInclude(a => a.Member)
                .Include(d => d.DelegatedToAssignment)
                    .ThenInclude(a => a.RoleCatalog)
                .Include(d => d.DelegatedFromAssignment)
                    .ThenInclude(a => a.Member)
                .Include(d => d.DelegatedFromAssignment)
                    .ThenInclude(a => a.RoleCatalog)
                .FirstOrDefaultAsync(cancellationToken);

            var result = _mapper.Map<ApprovalDelegateDto>(createdDelegation);
            return BaseResponse<ApprovalDelegateDto>.SuccessResult(result);
        }
        catch (Exception ex)
        {
            return BaseResponse<ApprovalDelegateDto>.ErrorResult($"Error creating delegation: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets a delegation by ID
    /// </summary>
    /// <param name="id">The delegation ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The delegation</returns>
    public async Task<BaseResponse<ApprovalDelegateDto>> GetDelegationAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var delegation = await _unitOfWork.Repository<ApprovalDelegate>()
                .GetQueryable(d => d.Id == id)
                .Include(d => d.DelegatedToAssignment)
                    .ThenInclude(a => a.Member)
                .Include(d => d.DelegatedToAssignment)
                    .ThenInclude(a => a.RoleCatalog)
                .Include(d => d.DelegatedFromAssignment)
                    .ThenInclude(a => a.Member)
                .Include(d => d.DelegatedFromAssignment)
                    .ThenInclude(a => a.RoleCatalog)
                .FirstOrDefaultAsync(cancellationToken);

            if (delegation == null)
            {
                return BaseResponse<ApprovalDelegateDto>.ErrorResult("Delegation not found");
            }

            var result = _mapper.Map<ApprovalDelegateDto>(delegation);
            return BaseResponse<ApprovalDelegateDto>.SuccessResult(result);
        }
        catch (Exception ex)
        {
            return BaseResponse<ApprovalDelegateDto>.ErrorResult($"Error retrieving delegation: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets all delegations for a specific scope
    /// </summary>
    /// <param name="scopeType">The scope type</param>
    /// <param name="scopeId">The scope ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of delegations</returns>
    public async Task<BaseResponse<IEnumerable<ApprovalDelegateDto>>> GetDelegationsByScopeAsync(ScopeType scopeType, Guid scopeId, CancellationToken cancellationToken = default)
    {
        try
        {
            var delegations = await _unitOfWork.Repository<ApprovalDelegate>()
                .GetQueryable(d => d.ScopeType == scopeType && d.ScopeId == scopeId)
                .Include(d => d.DelegatedToAssignment)
                    .ThenInclude(a => a.Member)
                .Include(d => d.DelegatedToAssignment)
                    .ThenInclude(a => a.RoleCatalog)
                .Include(d => d.DelegatedFromAssignment)
                    .ThenInclude(a => a.Member)
                .Include(d => d.DelegatedFromAssignment)
                    .ThenInclude(a => a.RoleCatalog)
                .OrderByDescending(d => d.StartDate)
                .ToListAsync(cancellationToken);

            var result = _mapper.Map<IEnumerable<ApprovalDelegateDto>>(delegations);
            return BaseResponse<IEnumerable<ApprovalDelegateDto>>.SuccessResult(result);
        }
        catch (Exception ex)
        {
            return BaseResponse<IEnumerable<ApprovalDelegateDto>>.ErrorResult($"Error retrieving delegations: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets all active delegations for a specific scope
    /// </summary>
    /// <param name="scopeType">The scope type</param>
    /// <param name="scopeId">The scope ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of active delegations</returns>
    public async Task<BaseResponse<IEnumerable<ApprovalDelegateDto>>> GetActiveDelegationsByScopeAsync(ScopeType scopeType, Guid scopeId, CancellationToken cancellationToken = default)
    {
        try
        {
            var now = DateTime.UtcNow;
            var delegations = await _unitOfWork.Repository<ApprovalDelegate>()
                .GetQueryable(d => d.ScopeType == scopeType && 
                                     d.ScopeId == scopeId && 
                                     d.StartDate <= now && 
                                     d.EndDate >= now)
                .Include(d => d.DelegatedToAssignment)
                    .ThenInclude(a => a.Member)
                .Include(d => d.DelegatedToAssignment)
                    .ThenInclude(a => a.RoleCatalog)
                .Include(d => d.DelegatedFromAssignment)
                    .ThenInclude(a => a.Member)
                .Include(d => d.DelegatedFromAssignment)
                    .ThenInclude(a => a.RoleCatalog)
                .OrderByDescending(d => d.StartDate)
                .ToListAsync(cancellationToken);

            var result = _mapper.Map<IEnumerable<ApprovalDelegateDto>>(delegations);
            return BaseResponse<IEnumerable<ApprovalDelegateDto>>.SuccessResult(result);
        }
        catch (Exception ex)
        {
            return BaseResponse<IEnumerable<ApprovalDelegateDto>>.ErrorResult($"Error retrieving active delegations: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets all delegations where a member is the delegate
    /// </summary>
    /// <param name="memberId">The member ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of delegations</returns>
    public async Task<BaseResponse<IEnumerable<ApprovalDelegateDto>>> GetDelegationsByDelegateAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        try
        {
            var delegations = await _unitOfWork.Repository<ApprovalDelegate>()
                .GetQueryable(d => d.DelegatedToAssignment.MemberId == memberId)
                .Include(d => d.DelegatedToAssignment)
                    .ThenInclude(a => a.Member)
                .Include(d => d.DelegatedToAssignment)
                    .ThenInclude(a => a.RoleCatalog)
                .Include(d => d.DelegatedFromAssignment)
                    .ThenInclude(a => a.Member)
                .Include(d => d.DelegatedFromAssignment)
                    .ThenInclude(a => a.RoleCatalog)
                .OrderByDescending(d => d.StartDate)
                .ToListAsync(cancellationToken);

            var result = _mapper.Map<IEnumerable<ApprovalDelegateDto>>(delegations);
            return BaseResponse<IEnumerable<ApprovalDelegateDto>>.SuccessResult(result);
        }
        catch (Exception ex)
        {
            return BaseResponse<IEnumerable<ApprovalDelegateDto>>.ErrorResult($"Error retrieving delegations: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets all delegations where a member is the delegator
    /// </summary>
    /// <param name="memberId">The member ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of delegations</returns>
    public async Task<BaseResponse<IEnumerable<ApprovalDelegateDto>>> GetDelegationsByDelegatorAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        try
        {
            var delegations = await _unitOfWork.Repository<ApprovalDelegate>()
                .GetQueryable(d => d.DelegatedFromAssignment.MemberId == memberId)
                .Include(d => d.DelegatedToAssignment)
                    .ThenInclude(a => a.Member)
                .Include(d => d.DelegatedToAssignment)
                    .ThenInclude(a => a.RoleCatalog)
                .Include(d => d.DelegatedFromAssignment)
                    .ThenInclude(a => a.Member)
                .Include(d => d.DelegatedFromAssignment)
                    .ThenInclude(a => a.RoleCatalog)
                .OrderByDescending(d => d.StartDate)
                .ToListAsync(cancellationToken);

            var result = _mapper.Map<IEnumerable<ApprovalDelegateDto>>(delegations);
            return BaseResponse<IEnumerable<ApprovalDelegateDto>>.SuccessResult(result);
        }
        catch (Exception ex)
        {
            return BaseResponse<IEnumerable<ApprovalDelegateDto>>.ErrorResult($"Error retrieving delegations: {ex.Message}");
        }
    }

    /// <summary>
    /// Updates a delegation
    /// </summary>
    /// <param name="id">The delegation ID</param>
    /// <param name="request">The update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated delegation</returns>
    public async Task<BaseResponse<ApprovalDelegateDto>> UpdateDelegationAsync(Guid id, UpdateApprovalDelegateDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            var delegation = await _unitOfWork.Repository<ApprovalDelegate>().GetFirstOrDefaultAsync(d => d.Id == id, cancellationToken);
            if (delegation == null)
            {
                return BaseResponse<ApprovalDelegateDto>.ErrorResult("Delegation not found");
            }

            _mapper.Map(request, delegation);
            delegation.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.Repository<ApprovalDelegate>().UpdateAsync(delegation, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Get the updated delegation with navigation properties
            var updatedDelegation = await _unitOfWork.Repository<ApprovalDelegate>()
                .GetQueryable(d => d.Id == id)
                .Include(d => d.DelegatedToAssignment)
                    .ThenInclude(a => a.Member)
                .Include(d => d.DelegatedToAssignment)
                    .ThenInclude(a => a.RoleCatalog)
                .Include(d => d.DelegatedFromAssignment)
                    .ThenInclude(a => a.Member)
                .Include(d => d.DelegatedFromAssignment)
                    .ThenInclude(a => a.RoleCatalog)
                .FirstOrDefaultAsync(cancellationToken);

            var result = _mapper.Map<ApprovalDelegateDto>(updatedDelegation);
            return BaseResponse<ApprovalDelegateDto>.SuccessResult(result);
        }
        catch (Exception ex)
        {
            return BaseResponse<ApprovalDelegateDto>.ErrorResult($"Error updating delegation: {ex.Message}");
        }
    }

    /// <summary>
    /// Ends a delegation
    /// </summary>
    /// <param name="id">The delegation ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success result</returns>
    public async Task<BaseResponse<bool>> EndDelegationAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var delegation = await _unitOfWork.Repository<ApprovalDelegate>().GetFirstOrDefaultAsync(d => d.Id == id, cancellationToken);
            if (delegation == null)
            {
                return BaseResponse<bool>.ErrorResult("Delegation not found");
            }

            delegation.EndDate = DateTime.UtcNow;
            delegation.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.Repository<ApprovalDelegate>().UpdateAsync(delegation, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return BaseResponse<bool>.SuccessResult(true);
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.ErrorResult($"Error ending delegation: {ex.Message}");
        }
    }

    /// <summary>
    /// Deletes a delegation
    /// </summary>
    /// <param name="id">The delegation ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success result</returns>
    public async Task<BaseResponse<bool>> DeleteDelegationAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var delegation = await _unitOfWork.Repository<ApprovalDelegate>().GetFirstOrDefaultAsync(d => d.Id == id, cancellationToken);
            if (delegation == null)
            {
                return BaseResponse<bool>.ErrorResult("Delegation not found");
            }

            await _unitOfWork.Repository<ApprovalDelegate>().DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return BaseResponse<bool>.SuccessResult(true);
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.ErrorResult($"Error deleting delegation: {ex.Message}");
        }
    }

    #endregion

    #region Validation and Effective Approver Operations

    /// <summary>
    /// Validates if a delegation can be created
    /// </summary>
    /// <param name="request">The delegation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Validation result</returns>
    public async Task<BaseResponse<bool>> ValidateDelegationAsync(CreateApprovalDelegateDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate date range
            if (request.EndDate <= request.StartDate)
            {
                return BaseResponse<bool>.ErrorResult("End date must be after start date");
            }

            // Validate assignments exist and are active
            var delegatedToAssignment = await _unitOfWork.Repository<Assignment>()
                .GetFirstOrDefaultAsync(a => a.Id == request.DelegatedToAssignmentId && a.IsActive, cancellationToken);
            if (delegatedToAssignment == null)
            {
                return BaseResponse<bool>.ErrorResult("Delegated to assignment not found or inactive");
            }

            var delegatedFromAssignment = await _unitOfWork.Repository<Assignment>()
                .GetFirstOrDefaultAsync(a => a.Id == request.DelegatedFromAssignmentId && a.IsActive, cancellationToken);
            if (delegatedFromAssignment == null)
            {
                return BaseResponse<bool>.ErrorResult("Delegated from assignment not found or inactive");
            }

            // Validate scope consistency
            if (delegatedToAssignment.ScopeType != request.ScopeType || delegatedToAssignment.ScopeId != request.ScopeId)
            {
                return BaseResponse<bool>.ErrorResult("Delegated to assignment scope does not match delegation scope");
            }

            if (delegatedFromAssignment.ScopeType != request.ScopeType || delegatedFromAssignment.ScopeId != request.ScopeId)
            {
                return BaseResponse<bool>.ErrorResult("Delegated from assignment scope does not match delegation scope");
            }

            // Validate role consistency
            if (delegatedFromAssignment.RoleCatalog.Name != request.Role)
            {
                return BaseResponse<bool>.ErrorResult("Delegated from assignment role does not match delegation role");
            }

            return BaseResponse<bool>.SuccessResult(true);
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.ErrorResult($"Error validating delegation: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets the effective approver for a role in a specific scope
    /// </summary>
    /// <param name="role">The role name</param>
    /// <param name="scopeType">The scope type</param>
    /// <param name="scopeId">The scope ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The effective assignment</returns>
    public async Task<BaseResponse<AssignmentDto?>> GetEffectiveApproverAsync(string role, ScopeType scopeType, Guid scopeId, CancellationToken cancellationToken = default)
    {
        try
        {
            var now = DateTime.UtcNow;

            // First, try to find an active delegation
            var activeDelegation = await _unitOfWork.Repository<ApprovalDelegate>()
                .GetQueryable(d => d.Role == role &&
                                     d.ScopeType == scopeType &&
                                     d.ScopeId == scopeId &&
                                     d.StartDate <= now &&
                                     d.EndDate >= now)
                .Include(d => d.DelegatedToAssignment)
                    .ThenInclude(a => a.Member)
                .Include(d => d.DelegatedToAssignment)
                    .ThenInclude(a => a.RoleCatalog)
                .FirstOrDefaultAsync(cancellationToken);

            if (activeDelegation != null)
            {
                var result = _mapper.Map<AssignmentDto>(activeDelegation.DelegatedToAssignment);
                return BaseResponse<AssignmentDto?>.SuccessResult(result);
            }

            // If no delegation, find the original assignment
            var originalAssignment = await _unitOfWork.Repository<Assignment>()
                .GetQueryable(a => a.RoleCatalog.Name == role &&
                                     a.ScopeType == scopeType &&
                                     a.ScopeId == scopeId &&
                                     a.IsActive)
                .Include(a => a.Member)
                .Include(a => a.RoleCatalog)
                .FirstOrDefaultAsync(cancellationToken);

            if (originalAssignment != null)
            {
                var result = _mapper.Map<AssignmentDto>(originalAssignment);
                return BaseResponse<AssignmentDto?>.SuccessResult(result);
            }

            return BaseResponse<AssignmentDto?>.SuccessResult(null);
        }
        catch (Exception ex)
        {
            return BaseResponse<AssignmentDto?>.ErrorResult($"Error getting effective approver: {ex.Message}");
        }
    }

    #endregion
}
