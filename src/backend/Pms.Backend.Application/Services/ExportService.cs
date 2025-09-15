using System.Globalization;
using System.Text;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Exports;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Application.Services;

/// <summary>
/// Service for CSV exports
/// </summary>
public class ExportService : IExportService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly TimeZoneInfo _brazilTimeZone;

    /// <summary>
    /// Initializes a new instance of the ExportService class
    /// </summary>
    /// <param name="unitOfWork">Unit of work for data access</param>
    public ExportService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _brazilTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
    }

    /// <summary>
    /// Exports members of a club to CSV format
    /// </summary>
    /// <param name="clubId">Club ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>CSV content as string wrapped in BaseResponse</returns>
    public async Task<BaseResponse<string>> ExportMembersToCsvAsync(Guid clubId, CancellationToken cancellationToken = default)
    {
        try
        {
            // Verificar se o clube existe
            var club = await _unitOfWork.Repository<Club>().GetByIdAsync(clubId, cancellationToken);
            if (club == null)
            {
                return BaseResponse<string>.NotFoundResult("Clube não encontrado");
            }

            var members = await _unitOfWork.Repository<Member>().GetAllWithIncludesAsync(
                m => m.Memberships.Any(mem => mem.ClubId == clubId && mem.IsActive),
                new[]
                {
                    "Memberships",
                    "Memberships.Unit",
                "Memberships.Club",
                "Memberships.Club.District",
                "Memberships.Club.District.Region",
                "Memberships.Club.District.Region.Association",
                "Memberships.Club.District.Region.Association.Union",
                "Memberships.Club.District.Region.Association.Union.Division"
            }
        );

        var exportData = members.Select(m =>
        {
            var activeMembership = m.Memberships.FirstOrDefault(mem => mem.ClubId == clubId && mem.IsActive);
            return new MemberExportDto
            {
                Id = m.Id,
                Name = m.DisplayName,
                Email = m.PrimaryEmail ?? "",
                Phone = m.PrimaryPhone ?? "",
                BirthDate = m.DateOfBirth,
                Gender = m.Gender.ToString(),
                Status = m.Status.ToString(),
                BaptismDate = m.BaptismDate,
                ScarfInvestitureDate = m.ScarfDate,
                CurrentUnitName = activeMembership?.Unit?.Name,
                CurrentUnitGender = activeMembership?.Unit?.Gender.ToString(),
                CurrentUnitAgeRange = activeMembership?.Unit != null
                    ? $"{activeMembership.Unit.AgeMin}-{activeMembership.Unit.AgeMax}"
                    : null,
                MembershipStartDate = activeMembership?.StartDate,
                MembershipEndDate = activeMembership?.EndDate,
                ClubName = activeMembership?.Club?.Name ?? string.Empty,
                ClubCode = activeMembership?.Club?.Code ?? string.Empty,
                DistrictName = activeMembership?.Club?.District?.Name ?? string.Empty,
                RegionName = activeMembership?.Club?.District?.Region?.Name ?? string.Empty,
                AssociationName = activeMembership?.Club?.District?.Region?.Association?.Name ?? string.Empty,
                UnionName = activeMembership?.Club?.District?.Region?.Association?.Union?.Name ?? string.Empty,
                DivisionName = activeMembership?.Club?.District?.Region?.Association?.Union?.Division?.Name ?? string.Empty
            };
        }).ToList();

            // Se não há membros, adicionar comentário informativo
            var csv = ConvertToCsv(exportData);
            if (!exportData.Any())
            {
                csv += $"\n# Nenhum membro encontrado para o clube '{club.Name}' (ID: {clubId})";
            }

            return BaseResponse<string>.SuccessResult(csv, "Exportação realizada com sucesso");
        }
        catch (Exception ex)
        {
            return BaseResponse<string>.InternalServerErrorResult($"Erro ao exportar membros: {ex.Message}", true);
        }
    }

    /// <summary>
    /// Exports timeline entries for a member to CSV format
    /// </summary>
    /// <param name="memberId">Member ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>CSV content as string wrapped in BaseResponse</returns>
    public async Task<BaseResponse<string>> ExportTimelineToCsvAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        try
        {
            // Verificar se o membro existe
            var member = await _unitOfWork.Repository<Member>().GetByIdAsync(memberId, cancellationToken);
            if (member == null)
            {
                return BaseResponse<string>.NotFoundResult("Membro não encontrado");
            }

            var timelineEntries = await _unitOfWork.Repository<TimelineEntry>().GetAllWithIncludesAsync(
                te => te.MemberId == memberId,
                new[] { "Member", "Event" },
                cancellationToken
            );

            var exportData = timelineEntries.Select(te => new TimelineExportDto
            {
                Id = te.Id,
                MemberName = te.Member.DisplayName,
                MemberEmail = te.Member.PrimaryEmail ?? "",
                Type = te.Type.ToString(),
                Title = te.Title,
                Description = te.Description,
                EventDate = TimeZoneInfo.ConvertTimeFromUtc(te.EventDateUtc, _brazilTimeZone),
                Data = te.Data,
                MembershipId = te.MembershipId,
                AssignmentId = te.AssignmentId,
                EventId = te.EventId,
                EventName = te.Event?.Name,
                CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(te.CreatedAtUtc, _brazilTimeZone)
            }).ToList();

            var csv = ConvertToCsv(exportData);
            if (!exportData.Any())
            {
                csv += $"\n# Nenhuma entrada de timeline encontrada para o membro (ID: {memberId})";
            }

            return BaseResponse<string>.SuccessResult(csv, "Exportação de timeline realizada com sucesso");
        }
        catch (Exception ex)
        {
            return BaseResponse<string>.InternalServerErrorResult($"Erro ao exportar timeline: {ex.Message}", true);
        }
    }

    /// <summary>
    /// Exports event participations for a club within a date range to CSV format
    /// </summary>
    /// <param name="clubId">Club ID</param>
    /// <param name="startDate">Start date (inclusive)</param>
    /// <param name="endDate">End date (inclusive)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>CSV content as string wrapped in BaseResponse</returns>
    public async Task<BaseResponse<string>> ExportParticipationsToCsvAsync(Guid clubId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        try
        {
            // Verificar se o clube existe
            var club = await _unitOfWork.Repository<Club>().GetByIdAsync(clubId, cancellationToken);
            if (club == null)
            {
                return BaseResponse<string>.NotFoundResult("Clube não encontrado");
            }

            // Validar datas
            if (startDate > endDate)
            {
                return BaseResponse<string>.UnprocessableEntityResult("Data de início deve ser anterior ou igual à data de fim");
            }

            var participations = await _unitOfWork.Repository<MemberEventParticipation>().GetAllWithIncludesAsync(
            mep => mep.Member.Memberships.Any(mem => mem.ClubId == clubId && mem.IsActive) &&
                   mep.Event.StartDate >= startDate &&
                   mep.Event.EndDate <= endDate,
            new[]
            {
                "Member",
                "Member.Memberships",
                "Member.Memberships.Unit",
                "Member.Memberships.Club",
                "Member.Memberships.Club.District",
                "Member.Memberships.Club.District.Region",
                "Member.Memberships.Club.District.Region.Association",
                "Member.Memberships.Club.District.Region.Association.Union",
                "Member.Memberships.Club.District.Region.Association.Union.Division",
                "Event"
            }
        );

        var exportData = participations.Select(mep =>
        {
            var activeMembership = mep.Member.Memberships.FirstOrDefault(mem => mem.ClubId == clubId && mem.IsActive);
            return new ParticipationExportDto
            {
                Id = mep.Id,
                MemberName = mep.Member.DisplayName,
                MemberEmail = mep.Member.PrimaryEmail ?? "",
                MemberBirthDate = mep.Member.DateOfBirth,
                MemberGender = mep.Member.Gender.ToString(),
                EventName = mep.Event.Name,
                EventDescription = mep.Event.Description ?? string.Empty,
                EventStartDate = TimeZoneInfo.ConvertTimeFromUtc(mep.Event.StartDate, _brazilTimeZone),
                EventEndDate = TimeZoneInfo.ConvertTimeFromUtc(mep.Event.EndDate, _brazilTimeZone),
                EventLocation = mep.Event.Location,
                EventFee = mep.Event.FeeAmount,
                RegistrationDate = TimeZoneInfo.ConvertTimeFromUtc(mep.RegisteredAtUtc, _brazilTimeZone),
                Status = mep.Status.ToString(),
                CurrentUnitName = activeMembership?.Unit?.Name,
                ClubName = activeMembership?.Club?.Name ?? string.Empty,
                ClubCode = activeMembership?.Club?.Code ?? string.Empty,
                DistrictName = activeMembership?.Club?.District?.Name ?? string.Empty,
                RegionName = activeMembership?.Club?.District?.Region?.Name ?? string.Empty,
                AssociationName = activeMembership?.Club?.District?.Region?.Association?.Name ?? string.Empty,
                UnionName = activeMembership?.Club?.District?.Region?.Association?.Union?.Name ?? string.Empty,
                DivisionName = activeMembership?.Club?.District?.Region?.Association?.Union?.Division?.Name ?? string.Empty
            };
            }).ToList();

            var csv = ConvertToCsv(exportData);
            if (!exportData.Any())
            {
                csv += $"\n# Nenhuma participação encontrada para o clube '{club.Name}' no período de {startDate:dd/MM/yyyy} a {endDate:dd/MM/yyyy}";
            }

            return BaseResponse<string>.SuccessResult(csv, "Exportação de participações realizada com sucesso");
        }
        catch (Exception ex)
        {
            return BaseResponse<string>.InternalServerErrorResult($"Erro ao exportar participações: {ex.Message}", true);
        }
    }

    /// <summary>
    /// Validates and parses date parameters for export operations
    /// </summary>
    /// <param name="startDateString">Start date string</param>
    /// <param name="endDateString">End date string</param>
    /// <returns>Validation result with parsed dates</returns>
    public BaseResponse<(DateTime StartDate, DateTime EndDate)> ValidateDateParameters(string startDateString, string endDateString)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(startDateString) || string.IsNullOrWhiteSpace(endDateString))
            {
                return BaseResponse<(DateTime, DateTime)>.UnprocessableEntityResult("Datas de início e fim são obrigatórias");
            }

            if (!DateTime.TryParse(startDateString, out var startDate))
            {
                return BaseResponse<(DateTime, DateTime)>.UnprocessableEntityResult("Data de início inválida. Use o formato dd/MM/yyyy");
            }

            if (!DateTime.TryParse(endDateString, out var endDate))
            {
                return BaseResponse<(DateTime, DateTime)>.UnprocessableEntityResult("Data de fim inválida. Use o formato dd/MM/yyyy");
            }

            if (startDate > endDate)
            {
                return BaseResponse<(DateTime, DateTime)>.UnprocessableEntityResult("Data de início deve ser anterior ou igual à data de fim");
            }

            return BaseResponse<(DateTime, DateTime)>.SuccessResult((startDate, endDate), "Datas validadas com sucesso");
        }
        catch (Exception ex)
        {
            return BaseResponse<(DateTime, DateTime)>.InternalServerErrorResult($"Erro ao validar datas: {ex.Message}");
        }
    }

    /// <summary>
    /// Converts a list of objects to CSV format
    /// </summary>
    /// <typeparam name="T">Type of objects to convert</typeparam>
    /// <param name="data">List of objects</param>
    /// <returns>CSV content as string</returns>
    private static string ConvertToCsv<T>(IEnumerable<T> data)
    {
        var csv = new StringBuilder();
        var properties = typeof(T).GetProperties();

        // Add header row
        var header = string.Join(";", properties.Select(p => p.Name));
        csv.AppendLine(header);

        // Add data rows
        foreach (var item in data)
        {
            var values = properties.Select(p =>
            {
                var value = p.GetValue(item);
                if (value == null) return string.Empty;

                // Handle DateTime values
                if (value is DateTime dateTime)
                {
                    return dateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                }

                // Handle decimal values
                if (value is decimal decimalValue)
                {
                    return decimalValue.ToString("F2", CultureInfo.InvariantCulture);
                }

                // Handle other values
                var stringValue = value.ToString() ?? string.Empty;

                // Escape semicolons and quotes
                if (stringValue.Contains(';') || stringValue.Contains('"') || stringValue.Contains('\n'))
                {
                    stringValue = $"\"{stringValue.Replace("\"", "\"\"")}\"";
                }

                return stringValue;
            });

            csv.AppendLine(string.Join(";", values));
        }

        return csv.ToString();
    }
}
