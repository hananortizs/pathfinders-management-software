using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Assignments;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Domain.Entities.Hierarchy;

namespace Pms.Backend.Application.Services.Assignments;

/// <summary>
/// Service for managing role assignments
/// Handles assignment lifecycle, spiritual requirements validation, and role catalog management
/// </summary>
public partial class AssignmentService : IAssignmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the AssignmentService
    /// </summary>
    /// <param name="unitOfWork">The unit of work</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public AssignmentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    #region Assignment CRUD Operations

    /// <summary>
    /// Creates a new assignment for a member
    /// </summary>
    /// <param name="request">The assignment creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created assignment</returns>
    public async Task<BaseResponse<AssignmentDto>> CreateAssignmentAsync(CreateAssignmentDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate assignment
            var validationResult = await ValidateAssignmentAsync(request.MemberId, request.RoleId, request.ScopeType, request.ScopeId, cancellationToken);
            if (!validationResult.Success)
            {
                return BaseResponse<AssignmentDto>.ErrorResult(validationResult.Message ?? "Assignment validation failed");
            }

            // Check if member already has an active assignment for this role in this scope
            var existingAssignment = await _unitOfWork.Repository<Assignment>().GetFirstOrDefaultAsync(
                a => a.MemberId == request.MemberId &&
                     a.RoleId == request.RoleId &&
                     a.ScopeType == request.ScopeType &&
                     a.ScopeId == request.ScopeId &&
                     a.IsActive,
                cancellationToken);

            if (existingAssignment != null)
            {
                return BaseResponse<AssignmentDto>.ErrorResult("Member already has an active assignment for this role in this scope");
            }

            // Create assignment
            var assignment = _mapper.Map<Assignment>(request);
            assignment.Id = Guid.NewGuid();
            assignment.CreatedAtUtc = DateTime.UtcNow;
            assignment.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.Repository<Assignment>().AddAsync(assignment, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Get the created assignment with navigation properties
            var createdAssignment = await _unitOfWork.Repository<Assignment>()
                .GetFirstOrDefaultWithIncludesAsync(a => a.Id == assignment.Id, new[] { "Member", "RoleCatalog" }, cancellationToken);

            var result = _mapper.Map<AssignmentDto>(createdAssignment);
            return BaseResponse<AssignmentDto>.SuccessResult(result);
        }
        catch (Exception ex)
        {
            return BaseResponse<AssignmentDto>.ErrorResult($"Error creating assignment: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets an assignment by ID
    /// </summary>
    /// <param name="id">The assignment ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The assignment</returns>
    public async Task<BaseResponse<AssignmentDto>> GetAssignmentAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var assignment = await _unitOfWork.Repository<Assignment>()
                .GetFirstOrDefaultWithIncludesAsync(a => a.Id == id, new[] { "Member", "RoleCatalog" }, cancellationToken);

            if (assignment == null)
            {
                return BaseResponse<AssignmentDto>.ErrorResult("Assignment not found");
            }

            var result = _mapper.Map<AssignmentDto>(assignment);
            return BaseResponse<AssignmentDto>.SuccessResult(result);
        }
        catch (Exception ex)
        {
            return BaseResponse<AssignmentDto>.ErrorResult($"Error retrieving assignment: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets all assignments for a member
    /// </summary>
    /// <param name="memberId">The member ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of assignments</returns>
    public async Task<BaseResponse<IEnumerable<AssignmentDto>>> GetAssignmentsByMemberAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        try
        {
            var assignments = await _unitOfWork.Repository<Assignment>()
                .GetAllWithIncludesAsync(a => a.MemberId == memberId, new[] { "Member", "RoleCatalog" }, cancellationToken);

            var result = _mapper.Map<IEnumerable<AssignmentDto>>(assignments);
            return BaseResponse<IEnumerable<AssignmentDto>>.SuccessResult(result);
        }
        catch (Exception ex)
        {
            return BaseResponse<IEnumerable<AssignmentDto>>.ErrorResult($"Error retrieving assignments: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets all assignments for a specific scope
    /// </summary>
    /// <param name="scopeType">The scope type</param>
    /// <param name="scopeId">The scope ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of assignments</returns>
    public async Task<BaseResponse<IEnumerable<AssignmentDto>>> GetAssignmentsByScopeAsync(ScopeType scopeType, Guid scopeId, CancellationToken cancellationToken = default)
    {
        try
        {
            var assignments = await _unitOfWork.Repository<Assignment>()
                .GetAllWithIncludesAsync(a => a.ScopeType == scopeType && a.ScopeId == scopeId, new[] { "Member", "RoleCatalog" }, cancellationToken);

            var result = _mapper.Map<IEnumerable<AssignmentDto>>(assignments);
            return BaseResponse<IEnumerable<AssignmentDto>>.SuccessResult(result);
        }
        catch (Exception ex)
        {
            return BaseResponse<IEnumerable<AssignmentDto>>.ErrorResult($"Error retrieving assignments: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets all active assignments for a specific scope
    /// </summary>
    /// <param name="scopeType">The scope type</param>
    /// <param name="scopeId">The scope ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of active assignments</returns>
    public async Task<BaseResponse<IEnumerable<AssignmentDto>>> GetActiveAssignmentsByScopeAsync(ScopeType scopeType, Guid scopeId, CancellationToken cancellationToken = default)
    {
        try
        {
            var assignments = await _unitOfWork.Repository<Assignment>()
                .GetAllWithIncludesAsync(a => a.ScopeType == scopeType && a.ScopeId == scopeId && a.IsActive, new[] { "Member", "RoleCatalog" }, cancellationToken);

            var result = _mapper.Map<IEnumerable<AssignmentDto>>(assignments);
            return BaseResponse<IEnumerable<AssignmentDto>>.SuccessResult(result);
        }
        catch (Exception ex)
        {
            return BaseResponse<IEnumerable<AssignmentDto>>.ErrorResult($"Error retrieving active assignments: {ex.Message}");
        }
    }

    /// <summary>
    /// Updates an assignment
    /// </summary>
    /// <param name="id">The assignment ID</param>
    /// <param name="request">The update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated assignment</returns>
    public async Task<BaseResponse<AssignmentDto>> UpdateAssignmentAsync(Guid id, UpdateAssignmentDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            var assignment = await _unitOfWork.Repository<Assignment>().GetFirstOrDefaultAsync(a => a.Id == id, cancellationToken);
            if (assignment == null)
            {
                return BaseResponse<AssignmentDto>.ErrorResult("Assignment not found");
            }

            _mapper.Map(request, assignment);
            assignment.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.Repository<Assignment>().UpdateAsync(assignment, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Get the updated assignment with navigation properties
            var updatedAssignment = await _unitOfWork.Repository<Assignment>()
                .GetFirstOrDefaultWithIncludesAsync(a => a.Id == id, new[] { "Member", "RoleCatalog" }, cancellationToken);

            var result = _mapper.Map<AssignmentDto>(updatedAssignment);
            return BaseResponse<AssignmentDto>.SuccessResult(result);
        }
        catch (Exception ex)
        {
            return BaseResponse<AssignmentDto>.ErrorResult($"Error updating assignment: {ex.Message}");
        }
    }

    /// <summary>
    /// Ends an assignment
    /// </summary>
    /// <param name="id">The assignment ID</param>
    /// <param name="endReason">Reason for ending the assignment</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success result</returns>
    public async Task<BaseResponse<bool>> EndAssignmentAsync(Guid id, string? endReason = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var assignment = await _unitOfWork.Repository<Assignment>().GetFirstOrDefaultAsync(a => a.Id == id, cancellationToken);
            if (assignment == null)
            {
                return BaseResponse<bool>.ErrorResult("Assignment not found");
            }

            assignment.EndDate = DateTime.UtcNow;
            assignment.EndReason = endReason;
            assignment.IsActive = false;
            assignment.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.Repository<Assignment>().UpdateAsync(assignment, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return BaseResponse<bool>.SuccessResult(true);
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.ErrorResult($"Error ending assignment: {ex.Message}");
        }
    }

    /// <summary>
    /// Deletes an assignment
    /// </summary>
    /// <param name="id">The assignment ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success result</returns>
    public async Task<BaseResponse<bool>> DeleteAssignmentAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var assignment = await _unitOfWork.Repository<Assignment>().GetFirstOrDefaultAsync(a => a.Id == id, cancellationToken);
            if (assignment == null)
            {
                return BaseResponse<bool>.ErrorResult("Assignment not found");
            }

            // Check if assignment has active delegations
            var hasActiveDelegations = await _unitOfWork.Repository<ApprovalDelegate>().ExistsAsync(
                d => (d.DelegatedToAssignmentId == id || d.DelegatedFromAssignmentId == id),
                cancellationToken);

            if (hasActiveDelegations)
            {
                return BaseResponse<bool>.ErrorResult("Cannot delete assignment with active delegations");
            }

            await _unitOfWork.Repository<Assignment>().DeleteAsync(assignment.Id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return BaseResponse<bool>.SuccessResult(true);
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.ErrorResult($"Error deleting assignment: {ex.Message}");
        }
    }

    #endregion

    #region Validation and Role Catalog Operations

    /// <summary>
    /// Validates if a member can be assigned to a role
    /// </summary>
    /// <param name="memberId">The member ID</param>
    /// <param name="roleId">The role ID</param>
    /// <param name="scopeType">The scope type</param>
    /// <param name="scopeId">The scope ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Validation result</returns>
    public async Task<BaseResponse<bool>> ValidateAssignmentAsync(Guid memberId, Guid roleId, ScopeType scopeType, Guid scopeId, CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate member exists
            var member = await _unitOfWork.Repository<Member>().GetFirstOrDefaultAsync(m => m.Id == memberId, cancellationToken);
            if (member == null)
            {
                return BaseResponse<bool>.ErrorResult("Member not found");
            }

            // Validate role exists
            var role = await _unitOfWork.Repository<RoleCatalog>().GetFirstOrDefaultAsync(r => r.Id == roleId, cancellationToken);
            if (role == null)
            {
                return BaseResponse<bool>.ErrorResult("Role not found");
            }

            // Validate scope exists
            var scopeExists = await ValidateScopeExistsAsync(scopeType, scopeId, cancellationToken);
            if (!scopeExists)
            {
                return BaseResponse<bool>.ErrorResult("Scope not found");
            }

            // Check spiritual requirements for leadership roles
            if (role.RequiresBaptism && member.BaptismDate == null)
            {
                return BaseResponse<bool>.ErrorResult("This role requires baptism");
            }

            if (role.RequiresScarf && member.ScarfDate == null)
            {
                return BaseResponse<bool>.ErrorResult("This role requires scarf (lenço)");
            }

            // Check if member already has a conflicting assignment
            var conflictingAssignment = await _unitOfWork.Repository<Assignment>().GetFirstOrDefaultAsync(
                a => a.MemberId == memberId &&
                     a.ScopeType == scopeType &&
                     a.ScopeId == scopeId &&
                     a.IsActive,
                cancellationToken);

            if (conflictingAssignment != null)
            {
                return BaseResponse<bool>.ErrorResult("Member already has an active assignment in this scope");
            }

            return BaseResponse<bool>.SuccessResult(true);
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.ErrorResult($"Error validating assignment: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets all role catalogs
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of role catalogs</returns>
    public async Task<BaseResponse<IEnumerable<RoleCatalogDto>>> GetRoleCatalogsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var roles = await _unitOfWork.Repository<RoleCatalog>()
                .GetAllAsync(r => r.IsActive, cancellationToken);

            var result = _mapper.Map<IEnumerable<RoleCatalogDto>>(roles);
            return BaseResponse<IEnumerable<RoleCatalogDto>>.SuccessResult(result);
        }
        catch (Exception ex)
        {
            return BaseResponse<IEnumerable<RoleCatalogDto>>.ErrorResult($"Error retrieving role catalogs: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets role catalogs by hierarchy level
    /// </summary>
    /// <param name="hierarchyLevel">The hierarchy level</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of role catalogs</returns>
    public async Task<BaseResponse<IEnumerable<RoleCatalogDto>>> GetRoleCatalogsByLevelAsync(string hierarchyLevel, CancellationToken cancellationToken = default)
    {
        try
        {
            // Parse hierarchy level to RoleLevel enum
            if (!Enum.TryParse<RoleLevel>(hierarchyLevel, true, out var roleLevel))
            {
                return BaseResponse<IEnumerable<RoleCatalogDto>>.ErrorResult("Invalid hierarchy level");
            }

            var roles = await _unitOfWork.Repository<RoleCatalog>()
                .GetAllAsync(r => r.Level == roleLevel && r.IsActive, cancellationToken);

            var result = _mapper.Map<IEnumerable<RoleCatalogDto>>(roles);
            return BaseResponse<IEnumerable<RoleCatalogDto>>.SuccessResult(result);
        }
        catch (Exception ex)
        {
            return BaseResponse<IEnumerable<RoleCatalogDto>>.ErrorResult($"Error retrieving role catalogs: {ex.Message}");
        }
    }

    #endregion

    #region Private Helper Methods

    /// <summary>
    /// Validates if a scope exists
    /// </summary>
    /// <param name="scopeType">The scope type</param>
    /// <param name="scopeId">The scope ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if scope exists</returns>
    private async Task<bool> ValidateScopeExistsAsync(ScopeType scopeType, Guid scopeId, CancellationToken cancellationToken)
    {
        return scopeType switch
        {
            ScopeType.Union => await _unitOfWork.Repository<Union>().ExistsAsync(u => u.Id == scopeId, cancellationToken),
            ScopeType.Association => await _unitOfWork.Repository<Association>().ExistsAsync(a => a.Id == scopeId, cancellationToken),
            ScopeType.Region => await _unitOfWork.Repository<Region>().ExistsAsync(r => r.Id == scopeId, cancellationToken),
            ScopeType.District => await _unitOfWork.Repository<District>().ExistsAsync(d => d.Id == scopeId, cancellationToken),
            ScopeType.Club => await _unitOfWork.Repository<Club>().ExistsAsync(c => c.Id == scopeId, cancellationToken),
            ScopeType.Unit => await _unitOfWork.Repository<Unit>().ExistsAsync(u => u.Id == scopeId, cancellationToken),
            _ => false
        };
    }

    #endregion
}
