using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Events;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Application.Interfaces;

/// <summary>
/// Service interface for event management operations
/// Handles official events, participations, and eligibility validation
/// </summary>
public interface IEventService
{
    #region Event CRUD Operations

    /// <summary>
    /// Creates a new official event
    /// </summary>
    /// <param name="request">The event creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created event</returns>
    Task<BaseResponse<EventDto>> CreateEventAsync(CreateEventDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an event by ID
    /// </summary>
    /// <param name="id">The event ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The event</returns>
    Task<BaseResponse<EventDto>> GetEventAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all events with pagination
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="organizerLevel">Filter by organizer level (optional)</param>
    /// <param name="organizerId">Filter by organizer ID (optional)</param>
    /// <param name="isActive">Filter by active status (optional)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of events</returns>
    Task<BaseResponse<PaginatedResponse<IEnumerable<EventDto>>>> GetEventsAsync(
        int pageNumber = 1, 
        int pageSize = 10, 
        OrganizerLevel? organizerLevel = null, 
        Guid? organizerId = null, 
        bool? isActive = null, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing event
    /// </summary>
    /// <param name="id">The event ID</param>
    /// <param name="request">The update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated event</returns>
    Task<BaseResponse<EventDto>> UpdateEventAsync(Guid id, UpdateEventDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deactivates an event (soft delete)
    /// </summary>
    /// <param name="id">The event ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success response</returns>
    Task<BaseResponse<bool>> DeactivateEventAsync(Guid id, CancellationToken cancellationToken = default);

    #endregion

    #region Event Participation Operations

    /// <summary>
    /// Registers a member for an event
    /// </summary>
    /// <param name="request">The participation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created participation</returns>
    Task<BaseResponse<EventParticipationDto>> RegisterForEventAsync(CreateEventParticipationDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates event participation
    /// </summary>
    /// <param name="id">The participation ID</param>
    /// <param name="request">The update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated participation</returns>
    Task<BaseResponse<EventParticipationDto>> UpdateParticipationAsync(Guid id, UpdateEventParticipationDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all participations for an event
    /// </summary>
    /// <param name="eventId">The event ID</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of participations</returns>
    Task<BaseResponse<PaginatedResponse<IEnumerable<EventParticipationDto>>>> GetEventParticipationsAsync(
        Guid eventId, 
        int pageNumber = 1, 
        int pageSize = 10, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all participations for a member
    /// </summary>
    /// <param name="memberId">The member ID</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of participations</returns>
    Task<BaseResponse<PaginatedResponse<IEnumerable<EventParticipationDto>>>> GetMemberParticipationsAsync(
        Guid memberId, 
        int pageNumber = 1, 
        int pageSize = 10, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancels event participation
    /// </summary>
    /// <param name="id">The participation ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success response</returns>
    Task<BaseResponse<bool>> CancelParticipationAsync(Guid id, CancellationToken cancellationToken = default);

    #endregion

    #region Eligibility and Validation

    /// <summary>
    /// Checks if a member is eligible to participate in an event
    /// </summary>
    /// <param name="memberId">The member ID</param>
    /// <param name="eventId">The event ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Eligibility result with details</returns>
    Task<BaseResponse<EventEligibilityDto>> CheckEligibilityAsync(Guid memberId, Guid eventId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets eligible members for an event
    /// </summary>
    /// <param name="eventId">The event ID</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of eligible members</returns>
    Task<BaseResponse<PaginatedResponse<IEnumerable<EventEligibleMemberDto>>>> GetEligibleMembersAsync(
        Guid eventId, 
        int pageNumber = 1, 
        int pageSize = 10, 
        CancellationToken cancellationToken = default);

    #endregion
}

/// <summary>
/// Data Transfer Object for event eligibility check
/// </summary>
public class EventEligibilityDto
{
    /// <summary>
    /// Indicates if the member is eligible
    /// </summary>
    public bool IsEligible { get; set; }

    /// <summary>
    /// Reason for eligibility or ineligibility
    /// </summary>
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// Additional details about eligibility
    /// </summary>
    public string? Details { get; set; }

    /// <summary>
    /// Member's current status
    /// </summary>
    public string MemberStatus { get; set; } = string.Empty;

    /// <summary>
    /// Member's club status
    /// </summary>
    public string ClubStatus { get; set; } = string.Empty;
}

/// <summary>
/// Data Transfer Object for eligible members
/// </summary>
public class EventEligibleMemberDto
{
    /// <summary>
    /// Member ID
    /// </summary>
    public Guid MemberId { get; set; }

    /// <summary>
    /// Member name
    /// </summary>
    public string MemberName { get; set; } = string.Empty;

    /// <summary>
    /// Member status
    /// </summary>
    public string MemberStatus { get; set; } = string.Empty;

    /// <summary>
    /// Club name
    /// </summary>
    public string ClubName { get; set; } = string.Empty;

    /// <summary>
    /// Club status
    /// </summary>
    public string ClubStatus { get; set; } = string.Empty;

    /// <summary>
    /// Eligibility reason
    /// </summary>
    public string EligibilityReason { get; set; } = string.Empty;
}
