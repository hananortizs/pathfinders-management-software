using System.Globalization;
using System.Text;
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
    /// <returns>CSV content as string</returns>
    public async Task<string> ExportMembersToCsvAsync(Guid clubId, CancellationToken cancellationToken = default)
    {
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

        return ConvertToCsv(exportData);
    }

    /// <summary>
    /// Exports timeline entries for a member to CSV format
    /// </summary>
    /// <param name="memberId">Member ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>CSV content as string</returns>
    public async Task<string> ExportTimelineToCsvAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
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

        return ConvertToCsv(exportData);
    }

    /// <summary>
    /// Exports event participations for a club within a date range to CSV format
    /// </summary>
    /// <param name="clubId">Club ID</param>
    /// <param name="startDate">Start date (inclusive)</param>
    /// <param name="endDate">End date (inclusive)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>CSV content as string</returns>
    public async Task<string> ExportParticipationsToCsvAsync(Guid clubId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
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

        return ConvertToCsv(exportData);
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
