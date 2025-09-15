using Pms.Backend.Application.DTOs;

namespace Pms.Backend.Application.Interfaces;

/// <summary>
/// Service interface for CSV exports
/// </summary>
public interface IExportService
{
    /// <summary>
    /// Exports members of a club to CSV format
    /// </summary>
    /// <param name="clubId">Club ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>CSV content as string wrapped in BaseResponse</returns>
    Task<BaseResponse<string>> ExportMembersToCsvAsync(Guid clubId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Exports timeline entries for a member to CSV format
    /// </summary>
    /// <param name="memberId">Member ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>CSV content as string wrapped in BaseResponse</returns>
    Task<BaseResponse<string>> ExportTimelineToCsvAsync(Guid memberId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Exports event participations for a club within a date range to CSV format
    /// </summary>
    /// <param name="clubId">Club ID</param>
    /// <param name="startDate">Start date (inclusive)</param>
    /// <param name="endDate">End date (inclusive)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>CSV content as string wrapped in BaseResponse</returns>
    Task<BaseResponse<string>> ExportParticipationsToCsvAsync(Guid clubId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates and parses date parameters for export operations
    /// </summary>
    /// <param name="startDateString">Start date string</param>
    /// <param name="endDateString">End date string</param>
    /// <returns>Validation result with parsed dates</returns>
    BaseResponse<(DateTime StartDate, DateTime EndDate)> ValidateDateParameters(string startDateString, string endDateString);
}
