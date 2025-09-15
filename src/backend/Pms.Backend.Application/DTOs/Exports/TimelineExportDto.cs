namespace Pms.Backend.Application.DTOs.Exports;

/// <summary>
/// DTO for timeline export to CSV
/// </summary>
public class TimelineExportDto
{
    /// <summary>
    /// Timeline entry ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Member name
    /// </summary>
    public string MemberName { get; set; } = string.Empty;

    /// <summary>
    /// Member email
    /// </summary>
    public string MemberEmail { get; set; } = string.Empty;

    /// <summary>
    /// Entry type
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Entry title
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Entry description
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Event date (in America/Sao_Paulo timezone)
    /// </summary>
    public DateTime EventDate { get; set; }

    /// <summary>
    /// Additional data (JSON)
    /// </summary>
    public string? Data { get; set; }

    /// <summary>
    /// Related membership ID (if applicable)
    /// </summary>
    public Guid? MembershipId { get; set; }

    /// <summary>
    /// Related assignment ID (if applicable)
    /// </summary>
    public Guid? AssignmentId { get; set; }

    /// <summary>
    /// Related event ID (if applicable)
    /// </summary>
    public Guid? EventId { get; set; }

    /// <summary>
    /// Related event name (if applicable)
    /// </summary>
    public string? EventName { get; set; }

    /// <summary>
    /// Created date (in America/Sao_Paulo timezone)
    /// </summary>
    public DateTime CreatedDate { get; set; }
}
