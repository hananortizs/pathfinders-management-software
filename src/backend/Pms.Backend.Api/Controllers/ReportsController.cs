using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.Interfaces;
using System.Text;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Controller for CSV exports and reports
/// </summary>
[ApiController]
[Route("[controller]")]
[Authorize] // Only authenticated users can access reports
public class ReportsController : BaseController
{
    private readonly IExportService _exportService;
    private readonly IReportsService _reportsService;

    /// <summary>
    /// Initializes a new instance of the ReportsController class
    /// </summary>
    /// <param name="exportService">Export service</param>
    /// <param name="reportsService">Reports service</param>
    public ReportsController(IExportService exportService, IReportsService reportsService)
    {
        _exportService = exportService;
        _reportsService = reportsService;
    }

    #region CSV Export Operations

    /// <summary>
    /// Exports members of a club to CSV format
    /// </summary>
    /// <param name="clubId">Club ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>CSV file with member data</returns>
    [HttpGet("members.csv")]
    [Authorize(Roles = "SystemAdmin,Director")] // SystemAdmin and Directors can export member data
    public async Task<IActionResult> ExportMembersCsv(
        [FromQuery] Guid clubId,
        CancellationToken cancellationToken = default)
    {
        var result = await _exportService.ExportMembersToCsvAsync(clubId, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return ProcessResponse(result);
        }

        var bytes = Encoding.UTF8.GetBytes(result.Data!);
        return ProcessFileResponse(result, bytes, "text/csv; charset=utf-8", $"members_club_{clubId}.csv");
    }

    /// <summary>
    /// Exports timeline entries for a member to CSV format
    /// </summary>
    /// <param name="memberId">Member ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>CSV file with timeline data</returns>
    [HttpGet("timeline.csv")]
    [Authorize(Roles = "SystemAdmin,Director")] // SystemAdmin and Directors can export timeline data
    public async Task<IActionResult> ExportTimelineCsv(
        [FromQuery] Guid memberId,
        CancellationToken cancellationToken = default)
    {
        var result = await _exportService.ExportTimelineToCsvAsync(memberId, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return ProcessResponse(result);
        }

        var bytes = Encoding.UTF8.GetBytes(result.Data!);
        return ProcessFileResponse(result, bytes, "text/csv; charset=utf-8", $"timeline_member_{memberId}.csv");
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
    [Authorize(Roles = "SystemAdmin,Director")] // SystemAdmin and Directors can export participation data
    public async Task<IActionResult> ExportParticipationsCsv(
        [FromQuery] Guid clubId,
        [FromQuery] string start,
        [FromQuery] string end,
        CancellationToken cancellationToken = default)
    {
        // Validar parâmetros de data
        var dateValidationResult = _exportService.ValidateDateParameters(start, end);
        if (!dateValidationResult.IsSuccess)
        {
            return ProcessResponse(dateValidationResult);
        }

        // Exportar participações
        var result = await _exportService.ExportParticipationsToCsvAsync(
            clubId, 
            dateValidationResult.Data.StartDate, 
            dateValidationResult.Data.EndDate, 
            cancellationToken);
        
        if (!result.IsSuccess)
        {
            return ProcessResponse(result);
        }

        var bytes = Encoding.UTF8.GetBytes(result.Data!);
        return ProcessFileResponse(result, bytes, "text/csv; charset=utf-8", $"participations_club_{clubId}_{start}_{end}.csv");
    }

    #endregion

    #region Basic Reports for Club Directors

    /// <summary>
    /// Gera relatório de membros por clube com estatísticas básicas
    /// </summary>
    /// <param name="clubId">ID do clube</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Relatório de membros do clube</returns>
    [HttpGet("club/{clubId}/members")]
    [Authorize(Roles = "SystemAdmin,Director")] // SystemAdmin and Directors can access reports
    [ProducesResponseType(typeof(BaseResponse<ClubMembersReportDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetClubMembersReport(Guid clubId, CancellationToken cancellationToken = default)
    {
        var result = await _reportsService.GetClubMembersReportAsync(clubId, cancellationToken);
        return ProcessResponse(result);
    }

    /// <summary>
    /// Gera relatório de capacidade das unidades de um clube
    /// </summary>
    /// <param name="clubId">ID do clube</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Relatório de capacidade das unidades</returns>
    [HttpGet("club/{clubId}/capacity")]
    [Authorize(Roles = "SystemAdmin,Director")] // SystemAdmin and Directors can access reports
    [ProducesResponseType(typeof(BaseResponse<ClubCapacityReportDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetClubCapacityReport(Guid clubId, CancellationToken cancellationToken = default)
    {
        var result = await _reportsService.GetClubCapacityReportAsync(clubId, cancellationToken);
        return ProcessResponse(result);
    }

    /// <summary>
    /// Gera relatório de membros por faixa etária
    /// </summary>
    /// <param name="clubId">ID do clube</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Relatório de membros por faixa etária</returns>
    [HttpGet("club/{clubId}/age-groups")]
    [Authorize(Roles = "SystemAdmin,Director")] // SystemAdmin and Directors can access reports
    [ProducesResponseType(typeof(BaseResponse<AgeGroupReportDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAgeGroupReport(Guid clubId, CancellationToken cancellationToken = default)
    {
        var result = await _reportsService.GetAgeGroupReportAsync(clubId, cancellationToken);
        return ProcessResponse(result);
    }

    /// <summary>
    /// Gera relatório de membros ativos/inativos
    /// </summary>
    /// <param name="clubId">ID do clube</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Relatório de status dos membros</returns>
    [HttpGet("club/{clubId}/status")]
    [Authorize(Roles = "SystemAdmin,Director")] // SystemAdmin and Directors can access reports
    [ProducesResponseType(typeof(BaseResponse<MemberStatusReportDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMemberStatusReport(Guid clubId, CancellationToken cancellationToken = default)
    {
        var result = await _reportsService.GetMemberStatusReportAsync(clubId, cancellationToken);
        return ProcessResponse(result);
    }

    #endregion
}
