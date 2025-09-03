using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pms.Backend.Application.Interfaces;
using System.Text;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Controller for CSV exports and reports
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize] // Only authenticated users can access reports
public class ReportsController : ControllerBase
{
    private readonly IExportService _exportService;

    /// <summary>
    /// Initializes a new instance of the ReportsController class
    /// </summary>
    /// <param name="exportService">Export service</param>
    public ReportsController(IExportService exportService)
    {
        _exportService = exportService;
    }

    /// <summary>
    /// Exports members of a club to CSV format
    /// </summary>
    /// <param name="clubId">Club ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>CSV file with member data</returns>
    [HttpGet("members.csv")]
    [Authorize(Roles = "Director")] // Only Directors can export member data
    public async Task<IActionResult> ExportMembersCsv(
        [FromQuery] Guid clubId,
        CancellationToken cancellationToken = default)
    {
        if (clubId == Guid.Empty)
        {
            return BadRequest("clubId parameter is required");
        }

        try
        {
            var csvContent = await _exportService.ExportMembersToCsvAsync(clubId, cancellationToken);
            var bytes = Encoding.UTF8.GetBytes(csvContent);

            return File(bytes, "text/csv; charset=utf-8", $"members_club_{clubId}.csv");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error generating members export: {ex.Message}");
        }
    }

    /// <summary>
    /// Exports timeline entries for a member to CSV format
    /// </summary>
    /// <param name="memberId">Member ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>CSV file with timeline data</returns>
    [HttpGet("timeline.csv")]
    [Authorize(Roles = "Director")] // Only Directors can export timeline data
    public async Task<IActionResult> ExportTimelineCsv(
        [FromQuery] Guid memberId,
        CancellationToken cancellationToken = default)
    {
        if (memberId == Guid.Empty)
        {
            return BadRequest("memberId parameter is required");
        }

        try
        {
            var csvContent = await _exportService.ExportTimelineToCsvAsync(memberId, cancellationToken);
            var bytes = Encoding.UTF8.GetBytes(csvContent);

            return File(bytes, "text/csv; charset=utf-8", $"timeline_member_{memberId}.csv");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error generating timeline export: {ex.Message}");
        }
    }

    /// <summary>
    /// Exports event participations for a club within a date range to CSV format
    /// </summary>
    /// <param name="clubId">Club ID</param>
    /// <param name="start">Start date (inclusive) - format: yyyy-MM-dd</param>
    /// <param name="end">End date (inclusive) - format: yyyy-MM-dd</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>CSV file with participation data</returns>
    [HttpGet("participations.csv")]
    [Authorize(Roles = "Director")] // Only Directors can export participation data
    public async Task<IActionResult> ExportParticipationsCsv(
        [FromQuery] Guid clubId,
        [FromQuery] string start,
        [FromQuery] string end,
        CancellationToken cancellationToken = default)
    {
        if (clubId == Guid.Empty)
        {
            return BadRequest("clubId parameter is required");
        }

        if (string.IsNullOrEmpty(start) || string.IsNullOrEmpty(end))
        {
            return BadRequest("start and end date parameters are required (format: yyyy-MM-dd)");
        }

        try
        {
            var startDate = DateTime.ParseExact(start, "yyyy-MM-dd", null);
            var endDate = DateTime.ParseExact(end, "yyyy-MM-dd", null);

            if (startDate > endDate)
            {
                return BadRequest("start date must be before or equal to end date");
            }

            var csvContent = await _exportService.ExportParticipationsToCsvAsync(clubId, startDate, endDate, cancellationToken);
            var bytes = Encoding.UTF8.GetBytes(csvContent);

            return File(bytes, "text/csv; charset=utf-8", $"participations_club_{clubId}_{start}_{end}.csv");
        }
        catch (FormatException)
        {
            return BadRequest("Invalid date format. Use yyyy-MM-dd format");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error generating participations export: {ex.Message}");
        }
    }
}
