using Microsoft.Extensions.Logging;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Domain.Enums;

namespace Pms.Backend.Application.Services.Reports;

/// <summary>
/// Serviço para geração de relatórios
/// </summary>
public class ReportsService : IReportsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ReportsService> _logger;

    /// <summary>
    /// Inicializa uma nova instância do ReportsService
    /// </summary>
    /// <param name="unitOfWork">Unit of Work para acesso aos dados</param>
    /// <param name="logger">Logger</param>
    public ReportsService(IUnitOfWork unitOfWork, ILogger<ReportsService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Gera relatório de membros por clube com estatísticas básicas
    /// </summary>
    /// <param name="clubId">ID do clube</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Relatório de membros do clube</returns>
    public async Task<BaseResponse<ClubMembersReportDto>> GetClubMembersReportAsync(Guid clubId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Gerando relatório de membros para clube {ClubId}", clubId);

            // Verificar se o clube existe
            var club = await _unitOfWork.Repository<Club>().GetByIdAsync(clubId, cancellationToken);
            if (club == null)
            {
                return BaseResponse<ClubMembersReportDto>.ErrorResult("Clube não encontrado");
            }

            // Obter membros do clube
            var memberships = await _unitOfWork.Repository<Domain.Entities.Membership>()
                .GetAsync(m => m.ClubId == clubId && m.IsActive, cancellationToken);

            var memberIds = memberships.Select(m => m.MemberId).ToList();
            var members = await _unitOfWork.Repository<Member>()
                .GetAsync(m => memberIds.Contains(m.Id) && !m.IsDeleted, cancellationToken);

            // Calcular estatísticas
            var totalMembers = members.Count();
            var activeMembers = members.Count(m => m.Status == MemberStatus.Active);
            var inactiveMembers = members.Count(m => m.Status == MemberStatus.Inactive);
            var maleMembers = members.Count(m => m.Gender == MemberGender.Male);
            var femaleMembers = members.Count(m => m.Gender == MemberGender.Female);

            // Calcular faixas etárias
            var currentDate = DateTime.UtcNow;
            var ageGroups = new Dictionary<string, int>
            {
                ["10-12 anos"] = 0,
                ["13-15 anos"] = 0,
                ["16-18 anos"] = 0,
                ["19+ anos"] = 0
            };

            foreach (var member in members)
            {
                if (member.DateOfBirth != default)
                {
                    var age = currentDate.Year - member.DateOfBirth.Year;
                    if (age >= 10 && age <= 12) ageGroups["10-12 anos"]++;
                    else if (age >= 13 && age <= 15) ageGroups["13-15 anos"]++;
                    else if (age >= 16 && age <= 18) ageGroups["16-18 anos"]++;
                    else if (age >= 19) ageGroups["19+ anos"]++;
                }
            }

            var report = new ClubMembersReportDto
            {
                ClubId = clubId,
                ClubName = club.Name,
                GeneratedAt = DateTime.UtcNow,
                TotalMembers = totalMembers,
                ActiveMembers = activeMembers,
                InactiveMembers = inactiveMembers,
                MaleMembers = maleMembers,
                FemaleMembers = femaleMembers,
                AgeGroups = ageGroups,
                Members = members.Select(m => new MemberReportItemDto
                {
                    Id = m.Id,
                    FirstName = m.FirstName,
                    LastName = m.LastName,
                    SocialName = m.SocialName,
                    DateOfBirth = m.DateOfBirth,
                    Gender = m.Gender.ToString(),
                    Status = m.Status.ToString(),
                    Cpf = m.Cpf,
                    Rg = m.Rg,
                    CreatedAt = m.CreatedAtUtc
                }).ToList()
            };

            return BaseResponse<ClubMembersReportDto>.SuccessResult(report, "Relatório de membros gerado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar relatório de membros para clube {ClubId}", clubId);
            return BaseResponse<ClubMembersReportDto>.ErrorResult($"Erro ao gerar relatório: {ex.Message}");
        }
    }

    /// <summary>
    /// Gera relatório de capacidade das unidades de um clube
    /// </summary>
    /// <param name="clubId">ID do clube</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Relatório de capacidade das unidades</returns>
    public async Task<BaseResponse<ClubCapacityReportDto>> GetClubCapacityReportAsync(Guid clubId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Gerando relatório de capacidade para clube {ClubId}", clubId);

            // Verificar se o clube existe
            var club = await _unitOfWork.Repository<Club>().GetByIdAsync(clubId, cancellationToken);
            if (club == null)
            {
                return BaseResponse<ClubCapacityReportDto>.ErrorResult("Clube não encontrado");
            }

            // Obter unidades do clube
            var units = await _unitOfWork.Repository<Unit>()
                .GetAsync(u => u.ClubId == clubId && !u.IsDeleted, cancellationToken);

            var capacityReport = new List<UnitCapacityReportDto>();

            foreach (var unit in units)
            {
                // Obter membros alocados na unidade
                var memberships = await _unitOfWork.Repository<Domain.Entities.Membership>()
                    .GetAsync(m => m.ClubId == clubId && m.UnitId == unit.Id && m.IsActive, cancellationToken);

                var currentMembers = memberships.Count();
                var capacity = unit.Capacity ?? 10; // Capacidade padrão de 10 se não definida
                var availableSlots = Math.Max(0, capacity - currentMembers);
                var utilizationPercentage = capacity > 0 ? (double)currentMembers / capacity * 100 : 0;

                capacityReport.Add(new UnitCapacityReportDto
                {
                    UnitId = unit.Id,
                    UnitName = unit.Name,
                    UnitType = "Unidade", // Unit não tem propriedade Type
                    Capacity = capacity,
                    CurrentMembers = currentMembers,
                    AvailableSlots = availableSlots,
                    UtilizationPercentage = Math.Round(utilizationPercentage, 2),
                    IsFull = currentMembers >= capacity,
                    IsNearCapacity = utilizationPercentage >= 80
                });
            }

            var report = new ClubCapacityReportDto
            {
                ClubId = clubId,
                ClubName = club.Name,
                GeneratedAt = DateTime.UtcNow,
                TotalUnits = units.Count(),
                TotalCapacity = capacityReport.Sum(u => u.Capacity),
                TotalMembers = capacityReport.Sum(u => u.CurrentMembers),
                TotalAvailableSlots = capacityReport.Sum(u => u.AvailableSlots),
                AverageUtilization = capacityReport.Any() ? capacityReport.Average(u => u.UtilizationPercentage) : 0,
                Units = capacityReport
            };

            return BaseResponse<ClubCapacityReportDto>.SuccessResult(report, "Relatório de capacidade gerado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar relatório de capacidade para clube {ClubId}", clubId);
            return BaseResponse<ClubCapacityReportDto>.ErrorResult($"Erro ao gerar relatório: {ex.Message}");
        }
    }

    /// <summary>
    /// Gera relatório de membros por faixa etária
    /// </summary>
    /// <param name="clubId">ID do clube</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Relatório de membros por faixa etária</returns>
    public async Task<BaseResponse<AgeGroupReportDto>> GetAgeGroupReportAsync(Guid clubId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Gerando relatório de faixas etárias para clube {ClubId}", clubId);

            // Verificar se o clube existe
            var club = await _unitOfWork.Repository<Club>().GetByIdAsync(clubId, cancellationToken);
            if (club == null)
            {
                return BaseResponse<AgeGroupReportDto>.ErrorResult("Clube não encontrado");
            }

            // Obter membros do clube
            var memberships = await _unitOfWork.Repository<Domain.Entities.Membership>()
                .GetAsync(m => m.ClubId == clubId && m.IsActive, cancellationToken);

            var memberIds = memberships.Select(m => m.MemberId).ToList();
            var members = await _unitOfWork.Repository<Member>()
                .GetAsync(m => memberIds.Contains(m.Id) && !m.IsDeleted, cancellationToken);

            var currentDate = DateTime.UtcNow;
            var ageGroups = new Dictionary<string, AgeGroupDetailDto>
            {
                ["10-12 anos"] = new AgeGroupDetailDto { MinAge = 10, MaxAge = 12, Members = new List<MemberReportItemDto>() },
                ["13-15 anos"] = new AgeGroupDetailDto { MinAge = 13, MaxAge = 15, Members = new List<MemberReportItemDto>() },
                ["16-18 anos"] = new AgeGroupDetailDto { MinAge = 16, MaxAge = 18, Members = new List<MemberReportItemDto>() },
                ["19+ anos"] = new AgeGroupDetailDto { MinAge = 19, MaxAge = 999, Members = new List<MemberReportItemDto>() }
            };

            foreach (var member in members)
            {
                if (member.DateOfBirth != default)
                {
                    var age = currentDate.Year - member.DateOfBirth.Year;
                    var memberItem = new MemberReportItemDto
                    {
                        Id = member.Id,
                        FirstName = member.FirstName,
                        LastName = member.LastName,
                        SocialName = member.SocialName,
                        DateOfBirth = member.DateOfBirth,
                        Gender = member.Gender.ToString(),
                        Status = member.Status.ToString(),
                        Cpf = member.Cpf,
                        Rg = member.Rg,
                        CreatedAt = member.CreatedAtUtc
                    };

                    if (age >= 10 && age <= 12) ageGroups["10-12 anos"].Members.Add(memberItem);
                    else if (age >= 13 && age <= 15) ageGroups["13-15 anos"].Members.Add(memberItem);
                    else if (age >= 16 && age <= 18) ageGroups["16-18 anos"].Members.Add(memberItem);
                    else if (age >= 19) ageGroups["19+ anos"].Members.Add(memberItem);
                }
            }

            // Calcular estatísticas
            foreach (var group in ageGroups.Values)
            {
                group.Count = group.Members.Count;
                group.MaleCount = group.Members.Count(m => m.Gender == "Masculino");
                group.FemaleCount = group.Members.Count(m => m.Gender == "Feminino");
                group.ActiveCount = group.Members.Count(m => m.Status == "Active");
                group.InactiveCount = group.Members.Count(m => m.Status == "Inactive");
            }

            var report = new AgeGroupReportDto
            {
                ClubId = clubId,
                ClubName = club.Name,
                GeneratedAt = DateTime.UtcNow,
                TotalMembers = members.Count(),
                AgeGroups = ageGroups
            };

            return BaseResponse<AgeGroupReportDto>.SuccessResult(report, "Relatório de faixas etárias gerado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar relatório de faixas etárias para clube {ClubId}", clubId);
            return BaseResponse<AgeGroupReportDto>.ErrorResult($"Erro ao gerar relatório: {ex.Message}");
        }
    }

    /// <summary>
    /// Gera relatório de membros ativos/inativos
    /// </summary>
    /// <param name="clubId">ID do clube</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Relatório de status dos membros</returns>
    public async Task<BaseResponse<MemberStatusReportDto>> GetMemberStatusReportAsync(Guid clubId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Gerando relatório de status para clube {ClubId}", clubId);

            // Verificar se o clube existe
            var club = await _unitOfWork.Repository<Club>().GetByIdAsync(clubId, cancellationToken);
            if (club == null)
            {
                return BaseResponse<MemberStatusReportDto>.ErrorResult("Clube não encontrado");
            }

            // Obter membros do clube
            var memberships = await _unitOfWork.Repository<Domain.Entities.Membership>()
                .GetAsync(m => m.ClubId == clubId && m.IsActive, cancellationToken);

            var memberIds = memberships.Select(m => m.MemberId).ToList();
            var members = await _unitOfWork.Repository<Member>()
                .GetAsync(m => memberIds.Contains(m.Id) && !m.IsDeleted, cancellationToken);

            var activeMembers = members.Where(m => m.Status == MemberStatus.Active).ToList();
            var inactiveMembers = members.Where(m => m.Status == MemberStatus.Inactive).ToList();

            var report = new MemberStatusReportDto
            {
                ClubId = clubId,
                ClubName = club.Name,
                GeneratedAt = DateTime.UtcNow,
                TotalMembers = members.Count(),
                ActiveMembers = activeMembers.Count(),
                InactiveMembers = inactiveMembers.Count(),
                ActivePercentage = members.Any() ? Math.Round((double)activeMembers.Count() / members.Count() * 100, 2) : 0,
                InactivePercentage = members.Any() ? Math.Round((double)inactiveMembers.Count() / members.Count() * 100, 2) : 0,
                ActiveMembersList = activeMembers.Select(m => new MemberReportItemDto
                {
                    Id = m.Id,
                    FirstName = m.FirstName,
                    LastName = m.LastName,
                    SocialName = m.SocialName,
                    DateOfBirth = m.DateOfBirth,
                    Gender = m.Gender.ToString(),
                    Status = m.Status.ToString(),
                    Cpf = m.Cpf,
                    Rg = m.Rg,
                    CreatedAt = m.CreatedAtUtc
                }).ToList(),
                InactiveMembersList = inactiveMembers.Select(m => new MemberReportItemDto
                {
                    Id = m.Id,
                    FirstName = m.FirstName,
                    LastName = m.LastName,
                    SocialName = m.SocialName,
                    DateOfBirth = m.DateOfBirth,
                    Gender = m.Gender.ToString(),
                    Status = m.Status.ToString(),
                    Cpf = m.Cpf,
                    Rg = m.Rg,
                    CreatedAt = m.CreatedAtUtc
                }).ToList()
            };

            return BaseResponse<MemberStatusReportDto>.SuccessResult(report, "Relatório de status gerado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar relatório de status para clube {ClubId}", clubId);
            return BaseResponse<MemberStatusReportDto>.ErrorResult($"Erro ao gerar relatório: {ex.Message}");
        }
    }
}
