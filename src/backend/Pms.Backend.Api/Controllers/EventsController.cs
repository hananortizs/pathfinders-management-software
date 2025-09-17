using Microsoft.AspNetCore.Mvc;
using Pms.Backend.Application.DTOs.Events;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Controller for managing official events and participations
/// </summary>
[ApiController]
[Route("events")]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly ILogger<EventsController> _logger;

    /// <summary>
    /// Initializes a new instance of the EventsController
    /// </summary>
    /// <param name="eventService">Event service</param>
    /// <param name="logger">Logger</param>
    public EventsController(IEventService eventService, ILogger<EventsController> logger)
    {
        _eventService = eventService;
        _logger = logger;
    }

    #region Event CRUD Operations

    /// <summary>
    /// Creates a new official event
    /// </summary>
    /// <param name="request">Event creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created event</returns>
    [HttpPost]
    public async Task<IActionResult> CreateEventAsync([FromBody] CreateEventDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new event: {EventName}", request.Name);

            var result = await _eventService.CreateEventAsync(request, cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to create event: {Error}", result.Message);
                return StatusCode(result.StatusCode, result);
            }

            _logger.LogInformation("Event created successfully with ID: {EventId}", result.Data?.Id);
            return CreatedAtAction(nameof(GetEventAsync), new { id = result.Data?.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating event");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Gets an event by ID
    /// </summary>
    /// <param name="id">Event ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Event details</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEventAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving event with ID: {EventId}", id);

            var result = await _eventService.GetEventAsync(id, cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Event not found: {EventId}", id);
                return NotFound(result);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving event: {EventId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Gets all events with pagination and filters
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="organizerLevel">Filter by organizer level (optional)</param>
    /// <param name="organizerId">Filter by organizer ID (optional)</param>
    /// <param name="isActive">Filter by active status (optional)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of events</returns>
    [HttpGet]
    public async Task<IActionResult> GetEventsAsync(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] OrganizerLevel? organizerLevel = null,
        [FromQuery] Guid? organizerId = null,
        [FromQuery] bool? isActive = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving events - Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);

            var result = await _eventService.GetEventsAsync(pageNumber, pageSize, organizerLevel, organizerId, isActive, cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to retrieve events: {Error}", result.Message);
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving events");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Updates an existing event
    /// </summary>
    /// <param name="id">Event ID</param>
    /// <param name="request">Update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated event</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEventAsync(Guid id, [FromBody] UpdateEventDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating event with ID: {EventId}", id);

            var result = await _eventService.UpdateEventAsync(id, request, cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to update event {EventId}: {Error}", id, result.Message);
                return StatusCode(result.StatusCode, result);
            }

            _logger.LogInformation("Event updated successfully: {EventId}", id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating event: {EventId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Deactivates an event (soft delete)
    /// </summary>
    /// <param name="id">Event ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success response</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeactivateEventAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deactivating event with ID: {EventId}", id);

            var result = await _eventService.DeactivateEventAsync(id, cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to deactivate event {EventId}: {Error}", id, result.Message);
                return StatusCode(result.StatusCode, result);
            }

            _logger.LogInformation("Event deactivated successfully: {EventId}", id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deactivating event: {EventId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    #endregion

    #region Event Participation Operations

    /// <summary>
    /// Registers a member for an event
    /// </summary>
    /// <param name="request">Participation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created participation</returns>
    [HttpPost("participations")]
    public async Task<IActionResult> RegisterForEventAsync([FromBody] CreateEventParticipationDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Registering member {MemberId} for event {EventId}", request.MemberId, request.EventId);

            var result = await _eventService.RegisterForEventAsync(request, cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to register for event: {Error}", result.Message);
                return StatusCode(result.StatusCode, result);
            }

            _logger.LogInformation("Member registered successfully for event");
            return CreatedAtAction(nameof(GetParticipationAsync), new { id = result.Data?.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering for event");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Gets a participation by ID
    /// </summary>
    /// <param name="id">Participation ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Participation details</returns>
    [HttpGet("participations/{id}")]
    public Task<IActionResult> GetParticipationAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving participation with ID: {ParticipationId}", id);

            // This would need to be implemented in the service
            return Task.FromResult<IActionResult>(NotFound("Not implemented yet"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving participation: {ParticipationId}", id);
            return Task.FromResult<IActionResult>(StatusCode(500, "Internal server error"));
        }
    }

    /// <summary>
    /// Updates event participation
    /// </summary>
    /// <param name="id">Participation ID</param>
    /// <param name="request">Update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated participation</returns>
    [HttpPut("participations/{id}")]
    public async Task<IActionResult> UpdateParticipationAsync(Guid id, [FromBody] UpdateEventParticipationDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating participation with ID: {ParticipationId}", id);

            var result = await _eventService.UpdateParticipationAsync(id, request, cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to update participation {ParticipationId}: {Error}", id, result.Message);
                return StatusCode(result.StatusCode, result);
            }

            _logger.LogInformation("Participation updated successfully: {ParticipationId}", id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating participation: {ParticipationId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Gets all participations for an event
    /// </summary>
    /// <param name="eventId">Event ID</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of participations</returns>
    [HttpGet("{eventId}/participations")]
    public async Task<IActionResult> GetEventParticipationsAsync(
        Guid eventId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving participations for event {EventId}", eventId);

            var result = await _eventService.GetEventParticipationsAsync(eventId, pageNumber, pageSize, cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to retrieve event participations: {Error}", result.Message);
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving event participations: {EventId}", eventId);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Gets all participations for a member
    /// </summary>
    /// <param name="memberId">Member ID</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of participations</returns>
    [HttpGet("members/{memberId}/participations")]
    public async Task<IActionResult> GetMemberParticipationsAsync(
        Guid memberId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving participations for member {MemberId}", memberId);

            var result = await _eventService.GetMemberParticipationsAsync(memberId, pageNumber, pageSize, cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to retrieve member participations: {Error}", result.Message);
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving member participations: {MemberId}", memberId);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Cancels event participation
    /// </summary>
    /// <param name="id">Participation ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success response</returns>
    [HttpDelete("participations/{id}")]
    public async Task<IActionResult> CancelParticipationAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Cancelling participation with ID: {ParticipationId}", id);

            var result = await _eventService.CancelParticipationAsync(id, cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to cancel participation {ParticipationId}: {Error}", id, result.Message);
                return StatusCode(result.StatusCode, result);
            }

            _logger.LogInformation("Participation cancelled successfully: {ParticipationId}", id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling participation: {ParticipationId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    #endregion

    #region Eligibility and Validation

    /// <summary>
    /// Checks if a member is eligible to participate in an event
    /// </summary>
    /// <param name="memberId">Member ID</param>
    /// <param name="eventId">Event ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Eligibility result</returns>
    [HttpGet("eligibility/{memberId}/{eventId}")]
    public async Task<IActionResult> CheckEligibilityAsync(Guid memberId, Guid eventId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Checking eligibility for member {MemberId} and event {EventId}", memberId, eventId);

            var result = await _eventService.CheckEligibilityAsync(memberId, eventId, cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to check eligibility: {Error}", result.Message);
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking eligibility for member {MemberId} and event {EventId}", memberId, eventId);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Gets eligible members for an event
    /// </summary>
    /// <param name="eventId">Event ID</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of eligible members</returns>
    [HttpGet("{eventId}/eligible-members")]
    public async Task<IActionResult> GetEligibleMembersAsync(
        Guid eventId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving eligible members for event {EventId}", eventId);

            var result = await _eventService.GetEligibleMembersAsync(eventId, pageNumber, pageSize, cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Failed to retrieve eligible members: {Error}", result.Message);
                return StatusCode(result.StatusCode, result);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving eligible members for event {EventId}", eventId);
            return StatusCode(500, "Internal server error");
        }
    }

    #endregion
}
