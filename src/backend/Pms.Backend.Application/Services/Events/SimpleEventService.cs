using AutoMapper;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Events;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Application.Interfaces.Validation;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Domain.Enums;

namespace Pms.Backend.Application.Services.Events;

/// <summary>
/// Simplified Event Service for MVP0
/// Focuses on core event functionality without complex pagination
/// </summary>
public partial class SimpleEventService : IEventService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IClubStatusService _clubStatusService;

    /// <summary>
    /// Initializes a new instance of the SimpleEventService
    /// </summary>
    /// <param name="unitOfWork">Unit of work for data access</param>
    /// <param name="mapper">AutoMapper instance</param>
    /// <param name="clubStatusService">Service for club status validation</param>
    public SimpleEventService(IUnitOfWork unitOfWork, IMapper mapper, IClubStatusService clubStatusService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _clubStatusService = clubStatusService;
    }

    #region Event CRUD Operations

    /// <summary>
    /// Creates a new official event
    /// </summary>
    /// <param name="request">The event creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created event</returns>
    public async Task<BaseResponse<EventDto>> CreateEventAsync(CreateEventDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            // Validate event dates
            if (request.StartDate >= request.EndDate)
            {
                return BaseResponse<EventDto>.ErrorResult("Start date must be before end date");
            }

            // Create event
            var eventEntity = _mapper.Map<OfficialEvent>(request);
            eventEntity.Id = Guid.NewGuid();
            eventEntity.CreatedAtUtc = DateTime.UtcNow;
            eventEntity.UpdatedAtUtc = DateTime.UtcNow;
            eventEntity.IsActive = true;

            await _unitOfWork.Repository<OfficialEvent>().AddAsync(eventEntity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var result = _mapper.Map<EventDto>(eventEntity);
            result.OrganizerName = "Organizer"; // Simplified for MVP0
            result.RegisteredCount = 0;

            return BaseResponse<EventDto>.SuccessResult(result);
        }
        catch (Exception ex)
        {
            return BaseResponse<EventDto>.InternalServerErrorResult($"Error creating event: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets an event by ID
    /// </summary>
    /// <param name="id">The event ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The event</returns>
    public async Task<BaseResponse<EventDto>> GetEventAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var eventEntity = await _unitOfWork.Repository<OfficialEvent>().GetByIdAsync(id, cancellationToken);
            if (eventEntity == null)
            {
                return BaseResponse<EventDto>.ErrorResult("Event not found");
            }

            var result = _mapper.Map<EventDto>(eventEntity);
            result.OrganizerName = "Organizer"; // Simplified for MVP0
            result.RegisteredCount = await GetParticipantCountAsync(id, cancellationToken);

            return BaseResponse<EventDto>.SuccessResult(result);
        }
        catch (Exception ex)
        {
            return BaseResponse<EventDto>.InternalServerErrorResult($"Error retrieving event: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets all events with pagination (simplified for MVP0)
    /// </summary>
    public async Task<BaseResponse<PaginatedResponse<IEnumerable<EventDto>>>> GetEventsAsync(
        int pageNumber = 1, 
        int pageSize = 10, 
        OrganizerLevel? organizerLevel = null, 
        Guid? organizerId = null, 
        bool? isActive = null, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Simplified implementation for MVP0
            var events = await _unitOfWork.Repository<OfficialEvent>().GetAsync(e => true, cancellationToken);
            
            var eventDtos = new List<EventDto>();
            foreach (var eventEntity in events)
            {
                var dto = _mapper.Map<EventDto>(eventEntity);
                dto.OrganizerName = "Organizer"; // Simplified for MVP0
                dto.RegisteredCount = await GetParticipantCountAsync(eventEntity.Id, cancellationToken);
                eventDtos.Add(dto);
            }

            var paginatedResponse = new PaginatedResponse<IEnumerable<EventDto>>
            {
                Items = eventDtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = eventDtos.Count
            };

            return BaseResponse<PaginatedResponse<IEnumerable<EventDto>>>.SuccessResult(paginatedResponse);
        }
        catch (Exception ex)
        {
            return BaseResponse<PaginatedResponse<IEnumerable<EventDto>>>.InternalServerErrorResult($"Error retrieving events: {ex.Message}");
        }
    }

    /// <summary>
    /// Updates an existing event (simplified for MVP0)
    /// </summary>
    public async Task<BaseResponse<EventDto>> UpdateEventAsync(Guid id, UpdateEventDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            var eventEntity = await _unitOfWork.Repository<OfficialEvent>().GetByIdAsync(id, cancellationToken);
            if (eventEntity == null)
            {
                return BaseResponse<EventDto>.ErrorResult("Event not found");
            }

            // Update fields
            if (request.Name != null) eventEntity.Name = request.Name;
            if (request.Description != null) eventEntity.Description = request.Description;
            if (request.StartDate.HasValue) eventEntity.StartDate = request.StartDate.Value;
            if (request.EndDate.HasValue) eventEntity.EndDate = request.EndDate.Value;
            if (request.Location != null) eventEntity.Location = request.Location;
            if (request.FeeAmount.HasValue) eventEntity.FeeAmount = request.FeeAmount.Value;
            if (request.FeeCurrency != null) eventEntity.FeeCurrency = request.FeeCurrency;
            if (request.IsActive.HasValue) eventEntity.IsActive = request.IsActive.Value;

            eventEntity.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var result = _mapper.Map<EventDto>(eventEntity);
            result.OrganizerName = "Organizer"; // Simplified for MVP0
            result.RegisteredCount = await GetParticipantCountAsync(id, cancellationToken);

            return BaseResponse<EventDto>.SuccessResult(result);
        }
        catch (Exception ex)
        {
            return BaseResponse<EventDto>.InternalServerErrorResult($"Error updating event: {ex.Message}");
        }
    }

    /// <summary>
    /// Deactivates an event (soft delete)
    /// </summary>
    public async Task<BaseResponse<bool>> DeactivateEventAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var eventEntity = await _unitOfWork.Repository<OfficialEvent>().GetByIdAsync(id, cancellationToken);
            if (eventEntity == null)
            {
                return BaseResponse<bool>.ErrorResult("Event not found");
            }

            eventEntity.IsActive = false;
            eventEntity.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return BaseResponse<bool>.SuccessResult(true, "Event deactivated successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.InternalServerErrorResult($"Error deactivating event: {ex.Message}");
        }
    }

    #endregion

    #region Event Participation Operations (Simplified for MVP0)

    /// <summary>
    /// Registers a member for an event
    /// </summary>
    public async Task<BaseResponse<EventParticipationDto>> RegisterForEventAsync(CreateEventParticipationDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if event exists and is active
            var eventEntity = await _unitOfWork.Repository<OfficialEvent>().GetByIdAsync(request.EventId, cancellationToken);
            if (eventEntity == null)
            {
                return BaseResponse<EventParticipationDto>.ErrorResult("Event not found");
            }

            if (!eventEntity.IsActive)
            {
                return BaseResponse<EventParticipationDto>.ErrorResult("Event is not active");
            }

            // Check if member exists
            var member = await _unitOfWork.Repository<Member>().GetByIdAsync(request.MemberId, cancellationToken);
            if (member == null)
            {
                return BaseResponse<EventParticipationDto>.ErrorResult("Member not found");
            }

            // Check eligibility (simplified for MVP0)
            var eligibility = await CheckEligibilityAsync(request.MemberId, request.EventId, cancellationToken);
            if (!eligibility.IsSuccess || !eligibility.Data?.IsEligible == true)
            {
                return BaseResponse<EventParticipationDto>.ErrorResult(
                    eligibility.Data?.Reason ?? "Member is not eligible for this event");
            }

            // Create participation
            var participation = new MemberEventParticipation
            {
                Id = Guid.NewGuid(),
                MemberId = request.MemberId,
                EventId = request.EventId,
                RegisteredAtUtc = DateTime.UtcNow,
                Status = ParticipationStatus.Registered,
                FeePaid = eventEntity.FeeAmount,
                FeeCurrency = eventEntity.FeeCurrency,
                Notes = request.Notes,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow
            };

            await _unitOfWork.Repository<MemberEventParticipation>().AddAsync(participation, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var result = _mapper.Map<EventParticipationDto>(participation);
            result.MemberName = $"{member.FirstName} {member.LastName}";
            result.EventName = eventEntity.Name;

            return BaseResponse<EventParticipationDto>.SuccessResult(result);
        }
        catch (Exception ex)
        {
            return BaseResponse<EventParticipationDto>.InternalServerErrorResult($"Error registering for event: {ex.Message}");
        }
    }

    /// <summary>
    /// Updates event participation (simplified for MVP0)
    /// </summary>
    public async Task<BaseResponse<EventParticipationDto>> UpdateParticipationAsync(Guid id, UpdateEventParticipationDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            var participation = await _unitOfWork.Repository<MemberEventParticipation>().GetByIdAsync(id, cancellationToken);
            if (participation == null)
            {
                return BaseResponse<EventParticipationDto>.ErrorResult("Participation not found");
            }

            // Update fields
            if (!string.IsNullOrEmpty(request.Status)) participation.Status = Enum.Parse<ParticipationStatus>(request.Status);
            if (request.FeePaid.HasValue) participation.FeePaid = request.FeePaid.Value;
            if (!string.IsNullOrEmpty(request.FeeCurrency)) participation.FeeCurrency = request.FeeCurrency;
            if (request.Notes != null) participation.Notes = request.Notes;

            participation.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var result = _mapper.Map<EventParticipationDto>(participation);
            
            // Load related data
            var member = await _unitOfWork.Repository<Member>().GetByIdAsync(participation.MemberId, cancellationToken);
            var eventEntity = await _unitOfWork.Repository<OfficialEvent>().GetByIdAsync(participation.EventId, cancellationToken);
            
            result.MemberName = member != null ? $"{member.FirstName} {member.LastName}" : "Unknown";
            result.EventName = eventEntity?.Name ?? "Unknown";

            return BaseResponse<EventParticipationDto>.SuccessResult(result);
        }
        catch (Exception ex)
        {
            return BaseResponse<EventParticipationDto>.InternalServerErrorResult($"Error updating participation: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets all participations for an event (simplified for MVP0)
    /// </summary>
    public async Task<BaseResponse<PaginatedResponse<IEnumerable<EventParticipationDto>>>> GetEventParticipationsAsync(
        Guid eventId, 
        int pageNumber = 1, 
        int pageSize = 10, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var participations = await _unitOfWork.Repository<MemberEventParticipation>()
                .GetAsync(p => p.EventId == eventId, cancellationToken);

            var participationDtos = new List<EventParticipationDto>();
            foreach (var participation in participations)
            {
                var dto = _mapper.Map<EventParticipationDto>(participation);
                
                // Load related data
                var member = await _unitOfWork.Repository<Member>().GetByIdAsync(participation.MemberId, cancellationToken);
                var eventEntity = await _unitOfWork.Repository<OfficialEvent>().GetByIdAsync(participation.EventId, cancellationToken);
                
                dto.MemberName = member != null ? $"{member.FirstName} {member.LastName}" : "Unknown";
                dto.EventName = eventEntity?.Name ?? "Unknown";
                
                participationDtos.Add(dto);
            }

            var paginatedResponse = new PaginatedResponse<IEnumerable<EventParticipationDto>>
            {
                Items = participationDtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = participationDtos.Count,
            };

            return BaseResponse<PaginatedResponse<IEnumerable<EventParticipationDto>>>.SuccessResult(paginatedResponse);
        }
        catch (Exception ex)
        {
            return BaseResponse<PaginatedResponse<IEnumerable<EventParticipationDto>>>.InternalServerErrorResult($"Error retrieving event participations: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets all participations for a member (simplified for MVP0)
    /// </summary>
    public async Task<BaseResponse<PaginatedResponse<IEnumerable<EventParticipationDto>>>> GetMemberParticipationsAsync(
        Guid memberId, 
        int pageNumber = 1, 
        int pageSize = 10, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var participations = await _unitOfWork.Repository<MemberEventParticipation>()
                .GetAsync(p => p.MemberId == memberId, cancellationToken);

            var participationDtos = new List<EventParticipationDto>();
            foreach (var participation in participations)
            {
                var dto = _mapper.Map<EventParticipationDto>(participation);
                
                // Load related data
                var member = await _unitOfWork.Repository<Member>().GetByIdAsync(participation.MemberId, cancellationToken);
                var eventEntity = await _unitOfWork.Repository<OfficialEvent>().GetByIdAsync(participation.EventId, cancellationToken);
                
                dto.MemberName = member != null ? $"{member.FirstName} {member.LastName}" : "Unknown";
                dto.EventName = eventEntity?.Name ?? "Unknown";
                
                participationDtos.Add(dto);
            }

            var paginatedResponse = new PaginatedResponse<IEnumerable<EventParticipationDto>>
            {
                Items = participationDtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = participationDtos.Count,
            };

            return BaseResponse<PaginatedResponse<IEnumerable<EventParticipationDto>>>.SuccessResult(paginatedResponse);
        }
        catch (Exception ex)
        {
            return BaseResponse<PaginatedResponse<IEnumerable<EventParticipationDto>>>.InternalServerErrorResult($"Error retrieving member participations: {ex.Message}");
        }
    }

    /// <summary>
    /// Cancels event participation
    /// </summary>
    public async Task<BaseResponse<bool>> CancelParticipationAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var participation = await _unitOfWork.Repository<MemberEventParticipation>().GetByIdAsync(id, cancellationToken);
            if (participation == null)
            {
                return BaseResponse<bool>.ErrorResult("Participation not found");
            }

            participation.Status = ParticipationStatus.Cancelled;
            participation.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return BaseResponse<bool>.SuccessResult(true, "Participation cancelled successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.InternalServerErrorResult($"Error cancelling participation: {ex.Message}");
        }
    }

    #endregion

    #region Eligibility and Validation

    /// <summary>
    /// Checks if a member is eligible to participate in an event
    /// </summary>
    public async Task<BaseResponse<EventEligibilityDto>> CheckEligibilityAsync(Guid memberId, Guid eventId, CancellationToken cancellationToken = default)
    {
        try
        {
            // Get member
            var member = await _unitOfWork.Repository<Member>().GetByIdAsync(memberId, cancellationToken);
            if (member == null)
            {
                return BaseResponse<EventEligibilityDto>.ErrorResult("Member not found");
            }

            // Get event
            var eventEntity = await _unitOfWork.Repository<OfficialEvent>().GetByIdAsync(eventId, cancellationToken);
            if (eventEntity == null)
            {
                return BaseResponse<EventEligibilityDto>.ErrorResult("Event not found");
            }

            var eligibility = new EventEligibilityDto
            {
                MemberStatus = member.Status.ToString(),
                ClubStatus = "Unknown"
            };

            // Check if member is active
            if (member.Status != MemberStatus.Active)
            {
                eligibility.IsEligible = false;
                eligibility.Reason = "Member is not active";
                eligibility.Details = $"Member status: {member.Status}";
                return BaseResponse<EventEligibilityDto>.SuccessResult(eligibility);
            }

            // Check if member has active membership
            var membership = await _unitOfWork.Repository<Domain.Entities.Membership>()
                .GetFirstOrDefaultAsync(m => m.MemberId == memberId && m.IsActive, cancellationToken);

            if (membership == null)
            {
                // Check if member has leadership role (Regional/Distrital/Pastor)
                var assignments = await _unitOfWork.Repository<Assignment>()
                    .GetAsync(a => a.MemberId == memberId && a.IsActive, cancellationToken);

                var hasLeadershipRole = assignments.Any(a => 
                    a.RoleCatalog.Name.Contains("Regional") || 
                    a.RoleCatalog.Name.Contains("Distrital") || 
                    a.RoleCatalog.Name.Contains("Pastor"));

                if (hasLeadershipRole)
                {
                    eligibility.IsEligible = true;
                    eligibility.Reason = "Leadership role without membership";
                    eligibility.Details = "Member has leadership role and can participate in events";
                    eligibility.ClubStatus = "Leadership";
                    return BaseResponse<EventEligibilityDto>.SuccessResult(eligibility);
                }

                eligibility.IsEligible = false;
                eligibility.Reason = "No active membership";
                eligibility.Details = "Member must have an active membership to participate in events";
                return BaseResponse<EventEligibilityDto>.SuccessResult(eligibility);
            }

            // Check if club is active
            var clubValidation = await _clubStatusService.ValidateMemberClubOperationAsync(memberId, "Event Participation", cancellationToken);
            if (!clubValidation.IsSuccess)
            {
                eligibility.IsEligible = false;
                eligibility.Reason = "Club is inactive";
                eligibility.Details = clubValidation.Message ?? "Member's club is not active";
                eligibility.ClubStatus = "Inactive";
                return BaseResponse<EventEligibilityDto>.SuccessResult(eligibility);
            }

            // Check if event is still open for registration
            if (eventEntity.EndDate < DateTime.UtcNow)
            {
                eligibility.IsEligible = false;
                eligibility.Reason = "Event has ended";
                eligibility.Details = "Registration is closed for this event";
                return BaseResponse<EventEligibilityDto>.SuccessResult(eligibility);
            }

            // All checks passed
            eligibility.IsEligible = true;
            eligibility.Reason = "Member is eligible";
            eligibility.Details = "Member meets all requirements for event participation";
            eligibility.ClubStatus = "Active";

            return BaseResponse<EventEligibilityDto>.SuccessResult(eligibility);
        }
        catch (Exception ex)
        {
            return BaseResponse<EventEligibilityDto>.InternalServerErrorResult($"Error checking eligibility: {ex.Message}");
        }
    }

    /// <summary>
    /// Gets eligible members for an event (simplified for MVP0)
    /// </summary>
    public async Task<BaseResponse<PaginatedResponse<IEnumerable<EventEligibleMemberDto>>>> GetEligibleMembersAsync(
        Guid eventId, 
        int pageNumber = 1, 
        int pageSize = 10, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Get event
            var eventEntity = await _unitOfWork.Repository<OfficialEvent>().GetByIdAsync(eventId, cancellationToken);
            if (eventEntity == null)
            {
                return BaseResponse<PaginatedResponse<IEnumerable<EventEligibleMemberDto>>>.ErrorResult("Event not found");
            }

            // Get all active members
            var members = await _unitOfWork.Repository<Member>()
                .GetAsync(m => m.Status == MemberStatus.Active, cancellationToken);

            var eligibleMembers = new List<EventEligibleMemberDto>();

            foreach (var member in members)
            {
                var eligibility = await CheckEligibilityAsync(member.Id, eventId, cancellationToken);
                if (eligibility.IsSuccess && eligibility.Data?.IsEligible == true)
                {
                    // Get club info
                    var membership = await _unitOfWork.Repository<Domain.Entities.Membership>()
                        .GetFirstOrDefaultAsync(m => m.MemberId == member.Id && m.IsActive, cancellationToken);

                    var clubName = "Leadership";
                    var clubStatus = "Active";

                    if (membership != null)
                    {
                        var club = await _unitOfWork.Repository<Club>().GetByIdAsync(membership.ClubId, cancellationToken);
                        clubName = club?.Name ?? "Unknown";
                        clubStatus = club?.IsActive == true ? "Active" : "Inactive";
                    }

                    eligibleMembers.Add(new EventEligibleMemberDto
                    {
                        MemberId = member.Id,
                        MemberName = $"{member.FirstName} {member.LastName}",
                        MemberStatus = member.Status.ToString(),
                        ClubName = clubName,
                        ClubStatus = clubStatus,
                        EligibilityReason = eligibility.Data?.Reason ?? "Eligible"
                    });
                }
            }

            var paginatedResponse = new PaginatedResponse<IEnumerable<EventEligibleMemberDto>>
            {
                Items = eligibleMembers,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = eligibleMembers.Count,
            };

            return BaseResponse<PaginatedResponse<IEnumerable<EventEligibleMemberDto>>>.SuccessResult(paginatedResponse);
        }
        catch (Exception ex)
        {
            return BaseResponse<PaginatedResponse<IEnumerable<EventEligibleMemberDto>>>.InternalServerErrorResult($"Error retrieving eligible members: {ex.Message}");
        }
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Gets participant count for an event
    /// </summary>
    private async Task<int> GetParticipantCountAsync(Guid eventId, CancellationToken cancellationToken)
    {
        var participations = await _unitOfWork.Repository<MemberEventParticipation>()
            .GetAsync(p => p.EventId == eventId && p.Status != ParticipationStatus.Cancelled, cancellationToken);
        
        return participations.Count();
    }

    #endregion
}
