using System.ComponentModel.DataAnnotations;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Application.DTOs.Events;

/// <summary>
/// Data Transfer Object for OfficialEvent entity
/// </summary>
public class EventDto
{
    /// <summary>
    /// Event ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Name of the event
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the event
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// When the event starts
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// When the event ends
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Where the event takes place
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// Level of the organizer
    /// </summary>
    public OrganizerLevel OrganizerLevel { get; set; }

    /// <summary>
    /// ID of the organizing entity
    /// </summary>
    public Guid OrganizerId { get; set; }

    /// <summary>
    /// Name of the organizing entity
    /// </summary>
    public string OrganizerName { get; set; } = string.Empty;

    /// <summary>
    /// Fee amount for the event (null = free)
    /// </summary>
    public decimal? FeeAmount { get; set; }

    /// <summary>
    /// Currency for the fee (default: BRL)
    /// </summary>
    public string FeeCurrency { get; set; } = "BRL";

    /// <summary>
    /// Indicates if the event is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Number of registered participants
    /// </summary>
    public int ParticipantCount { get; set; }

    /// <summary>
    /// Checks if the event is currently happening
    /// </summary>
    public bool IsCurrentlyHappening { get; set; }

    /// <summary>
    /// Checks if the event has ended
    /// </summary>
    public bool HasEnded { get; set; }

    /// <summary>
    /// Creation date
    /// </summary>
    public DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime UpdatedAtUtc { get; set; }
}

/// <summary>
/// Data Transfer Object for creating a new event
/// </summary>
public class CreateEventDto
{
    /// <summary>
    /// Name of the event
    /// </summary>
    [Required(ErrorMessage = "Event name is required")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Event name must be between 3 and 200 characters")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the event
    /// </summary>
    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    public string? Description { get; set; }

    /// <summary>
    /// When the event starts
    /// </summary>
    [Required(ErrorMessage = "Start date is required")]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// When the event ends
    /// </summary>
    [Required(ErrorMessage = "End date is required")]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Where the event takes place
    /// </summary>
    [StringLength(300, ErrorMessage = "Location cannot exceed 300 characters")]
    public string? Location { get; set; }

    /// <summary>
    /// Level of the organizer
    /// </summary>
    [Required(ErrorMessage = "Organizer level is required")]
    public OrganizerLevel OrganizerLevel { get; set; }

    /// <summary>
    /// ID of the organizing entity
    /// </summary>
    [Required(ErrorMessage = "Organizer ID is required")]
    public Guid OrganizerId { get; set; }

    /// <summary>
    /// Fee amount for the event (null = free)
    /// </summary>
    public decimal? FeeAmount { get; set; }

    /// <summary>
    /// Currency for the fee (default: BRL)
    /// </summary>
    [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency must be exactly 3 characters")]
    public string FeeCurrency { get; set; } = "BRL";
}

/// <summary>
/// Data Transfer Object for updating an event
/// </summary>
public class UpdateEventDto
{
    /// <summary>
    /// Name of the event
    /// </summary>
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Event name must be between 3 and 200 characters")]
    public string? Name { get; set; }

    /// <summary>
    /// Description of the event
    /// </summary>
    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    public string? Description { get; set; }

    /// <summary>
    /// When the event starts
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// When the event ends
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Where the event takes place
    /// </summary>
    [StringLength(300, ErrorMessage = "Location cannot exceed 300 characters")]
    public string? Location { get; set; }

    /// <summary>
    /// Fee amount for the event (null = free)
    /// </summary>
    public decimal? FeeAmount { get; set; }

    /// <summary>
    /// Currency for the fee
    /// </summary>
    [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency must be exactly 3 characters")]
    public string? FeeCurrency { get; set; }

    /// <summary>
    /// Indicates if the event is active
    /// </summary>
    public bool? IsActive { get; set; }
}

/// <summary>
/// Data Transfer Object for event participation
/// </summary>
public class EventParticipationDto
{
    /// <summary>
    /// Participation ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Member ID
    /// </summary>
    public Guid MemberId { get; set; }

    /// <summary>
    /// Member name
    /// </summary>
    public string MemberName { get; set; } = string.Empty;

    /// <summary>
    /// Event ID
    /// </summary>
    public Guid EventId { get; set; }

    /// <summary>
    /// Event name
    /// </summary>
    public string EventName { get; set; } = string.Empty;

    /// <summary>
    /// When the participation was registered
    /// </summary>
    public DateTime RegisteredAtUtc { get; set; }

    /// <summary>
    /// Status of the participation
    /// </summary>
    public ParticipationStatus Status { get; set; }

    /// <summary>
    /// Fee paid for the event (if any)
    /// </summary>
    public decimal? FeePaid { get; set; }

    /// <summary>
    /// Currency of the fee paid
    /// </summary>
    public string? FeeCurrency { get; set; }

    /// <summary>
    /// When the fee was paid
    /// </summary>
    public DateTime? FeePaidAtUtc { get; set; }

    /// <summary>
    /// Notes about the participation
    /// </summary>
    public string? Notes { get; set; }
}

/// <summary>
/// Data Transfer Object for creating event participation
/// </summary>
public class CreateEventParticipationDto
{
    /// <summary>
    /// Member ID
    /// </summary>
    [Required(ErrorMessage = "Member ID is required")]
    public Guid MemberId { get; set; }

    /// <summary>
    /// Event ID
    /// </summary>
    [Required(ErrorMessage = "Event ID is required")]
    public Guid EventId { get; set; }

    /// <summary>
    /// Notes about the participation
    /// </summary>
    [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
    public string? Notes { get; set; }
}

/// <summary>
/// Data Transfer Object for updating event participation
/// </summary>
public class UpdateEventParticipationDto
{
    /// <summary>
    /// Status of the participation
    /// </summary>
    public ParticipationStatus? Status { get; set; }

    /// <summary>
    /// Fee paid for the event (if any)
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Fee paid must be positive")]
    public decimal? FeePaid { get; set; }

    /// <summary>
    /// Currency of the fee paid
    /// </summary>
    [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency must be exactly 3 characters")]
    public string? FeeCurrency { get; set; }

    /// <summary>
    /// Notes about the participation
    /// </summary>
    [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
    public string? Notes { get; set; }
}
