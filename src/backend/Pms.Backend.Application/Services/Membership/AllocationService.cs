using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Membership;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Domain.Enums;

namespace Pms.Backend.Application.Services.Membership;

/// <summary>
/// Serviço para alocação de membros em unidades
/// Implementa as regras de negócio definidas para alocação automática e manual
/// </summary>
public class AllocationService : IAllocationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILoggingService _loggingService;

    /// <summary>
    /// Inicializa uma nova instância do AllocationService
    /// </summary>
    /// <param name="unitOfWork">Unidade de trabalho para acesso aos dados</param>
    /// <param name="mapper">Mapeador AutoMapper</param>
    /// <param name="loggingService">Serviço centralizado de logging</param>
    public AllocationService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILoggingService loggingService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _loggingService = loggingService;
    }

    /// <summary>
    /// Aloca automaticamente um membro em uma unidade baseado nas regras de negócio
    /// </summary>
    public async Task<BaseResponse<AllocationResultDto>> AllocateMemberAsync(Guid membershipId, CancellationToken cancellationToken = default)
    {
        try
        {
            _loggingService.LogInformation("Iniciando alocação automática para membership {MembershipId}", membershipId);

            // Buscar membership
            var membership = await _unitOfWork.Repository<Domain.Entities.Membership>()
                .GetFirstOrDefaultAsync(
                    m => m.Id == membershipId,
                    cancellationToken);

            if (membership == null)
            {
                return BaseResponse<AllocationResultDto>.ErrorResult("Membership não encontrada");
            }

            // Buscar membro e clube separadamente
            var member = await _unitOfWork.Repository<Member>()
                .GetFirstOrDefaultAsync(m => m.Id == membership.MemberId, cancellationToken);

            if (member == null)
            {
                return BaseResponse<AllocationResultDto>.ErrorResult("Membro não encontrado");
            }

            // Calcular idade de referência (1º de junho do ano da matrícula)
            var referenceDate = GetReferenceDateForAllocation(membership);
            var memberAge = member.GetAgeOnJuneFirst(referenceDate.Year);

            _loggingService.LogInformation("Membro {MemberId} tem {Age} anos em {ReferenceDate}",
                membership.MemberId, memberAge, referenceDate.ToString("yyyy-MM-dd"));

            // Verificar se membro tem ≥16 anos (não pode ser alocado em unidade infanto-juvenil)
            if (memberAge >= 16)
            {
                _loggingService.LogInformation("Membro {MemberId} tem {Age} anos - não pode ser alocado em unidade infanto-juvenil",
                    membership.MemberId, memberAge);

                return new BaseResponse<AllocationResultDto>
                {
                    IsSuccess = false,
                    Message = "Membros com 16 anos ou mais não podem ser alocados em unidades infanto-juvenis",
                    Data = new AllocationResultDto
                    {
                        Success = false,
                        Message = "Membros com 16 anos ou mais não podem ser alocados em unidades infanto-juvenis",
                        RequiresTask = true,
                        TaskType = "MemberOverAge",
                        TaskDescription = $"Membro {member.FullName} tem {memberAge} anos e não pode ser alocado em unidade infanto-juvenil"
                    }
                };
            }

            // Buscar unidades compatíveis
            var compatibleUnits = await GetCompatibleUnitsInternalAsync(
                membership.MemberId,
                membership.ClubId,
                referenceDate,
                cancellationToken);

            if (!compatibleUnits.Any())
            {
                _loggingService.LogInformation("Nenhuma unidade compatível encontrada para membro {MemberId}", membership.MemberId);

                return new BaseResponse<AllocationResultDto>
                {
                    IsSuccess = false,
                    Message = "Nenhuma unidade compatível encontrada",
                    Data = new AllocationResultDto
                    {
                        Success = false,
                        Message = "Nenhuma unidade compatível encontrada",
                        RequiresTask = true,
                        TaskType = "AllocateUnit",
                        TaskDescription = $"Alocar membro {member.FullName} em unidade compatível"
                    }
                };
            }

            // Aplicar regras de alocação
            var availableUnits = compatibleUnits.Where(u => u.HasAvailableCapacity).ToList();

            if (availableUnits.Count == 1)
            {
                // Alocação automática - apenas uma unidade disponível
                var selectedUnit = availableUnits.First();
                _loggingService.LogInformation("Alocação automática: membro {MemberId} será alocado na unidade {UnitId}",
                    membership.MemberId, selectedUnit.Id);

                membership.UnitId = selectedUnit.Id;
                membership.UpdatedAtUtc = DateTime.UtcNow;

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var membershipDto = _mapper.Map<MembershipDto>(membership);

                return new BaseResponse<AllocationResultDto>
                {
                    IsSuccess = true,
                    Message = $"Membro alocado automaticamente na unidade {selectedUnit.Name}",
                    Data = new AllocationResultDto
                    {
                        Success = true,
                        Message = $"Membro alocado automaticamente na unidade {selectedUnit.Name}",
                        Membership = membershipDto
                    }
                };
            }
            else if (availableUnits.Count > 1)
            {
                // Múltiplas opções - retornar 422 com opções ordenadas
                var orderedUnits = availableUnits
                    .OrderBy(u => u.Priority) // Menor lotação primeiro
                    .ThenBy(u => u.Name) // Tie-break por nome
                    .ToList();

                _loggingService.LogInformation("Múltiplas unidades disponíveis para membro {MemberId}: {Count} opções",
                    membership.MemberId, orderedUnits.Count);

                return new BaseResponse<AllocationResultDto>
                {
                    IsSuccess = false,
                    Message = "Múltiplas unidades compatíveis disponíveis",
                    Data = new AllocationResultDto
                    {
                        Success = false,
                        Message = "Múltiplas unidades compatíveis disponíveis",
                        CompatibleUnits = orderedUnits,
                        RequiresTask = true,
                        TaskType = "ChooseUnit",
                        TaskDescription = $"Escolher unidade para membro {member.FullName} entre {orderedUnits.Count} opções"
                    }
                };
            }
            else
            {
                // Nenhuma unidade com capacidade disponível
                _loggingService.LogInformation("Nenhuma unidade com capacidade disponível para membro {MemberId}", membership.MemberId);

                return new BaseResponse<AllocationResultDto>
                {
                    IsSuccess = false,
                    Message = "Nenhuma unidade com capacidade disponível",
                    Data = new AllocationResultDto
                    {
                        Success = false,
                        Message = "Nenhuma unidade com capacidade disponível",
                        CompatibleUnits = compatibleUnits,
                        RequiresTask = true,
                        TaskType = "CapacityExceeded",
                        TaskDescription = $"Capacidade excedida - todas as unidades compatíveis estão lotadas para membro {member.FullName}"
                    }
                };
            }
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Erro ao alocar membro {MembershipId}", membershipId);
            return BaseResponse<AllocationResultDto>.ErrorResult($"Erro ao alocar membro: {ex.Message}");
        }
    }

    /// <summary>
    /// Aloca um membro em uma unidade específica
    /// </summary>
    public async Task<BaseResponse<MembershipDto>> AllocateToSpecificUnitAsync(Guid membershipId, Guid unitId, string? reason = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _loggingService.LogInformation("Alocação manual: membership {MembershipId} para unidade {UnitId}", membershipId, unitId);

            var membership = await _unitOfWork.Repository<Domain.Entities.Membership>()
                .GetFirstOrDefaultAsync(m => m.Id == membershipId, cancellationToken);

            if (membership == null)
            {
                return BaseResponse<MembershipDto>.ErrorResult("Membership não encontrada");
            }

            // Validar unidade
            var unit = await _unitOfWork.Repository<Unit>().GetFirstOrDefaultAsync(
                u => u.Id == unitId && u.ClubId == membership.ClubId, cancellationToken);

            if (unit == null)
            {
                return BaseResponse<MembershipDto>.ErrorResult("Unidade não encontrada ou não pertence ao mesmo clube");
            }

            // Buscar membro para validação
            var member = await _unitOfWork.Repository<Member>()
                .GetFirstOrDefaultAsync(m => m.Id == membership.MemberId, cancellationToken);

            if (member == null)
            {
                return BaseResponse<MembershipDto>.ErrorResult("Membro não encontrado");
            }

            // Validar compatibilidade de gênero
            if (!IsGenderCompatible(member.Gender, unit.Gender))
            {
                return BaseResponse<MembershipDto>.ErrorResult("Gênero da unidade não é compatível com o gênero do membro");
            }

            // Validar capacidade
            var hasCapacity = await HasAvailableCapacityAsync(unitId, cancellationToken);
            if (!hasCapacity)
            {
                return BaseResponse<MembershipDto>.ErrorResult("Unidade está em capacidade máxima");
            }

            // Realizar alocação
            membership.UnitId = unitId;
            membership.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // TODO: Registrar no Timeline se reason fornecida
            if (!string.IsNullOrEmpty(reason))
            {
                _loggingService.LogInformation("Alocação manual realizada com motivo: {Reason}", reason);
            }

            var dto = _mapper.Map<MembershipDto>(membership);
            return BaseResponse<MembershipDto>.SuccessResult(dto, $"Membro alocado na unidade {unit.Name}");
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Erro ao alocar membro {MembershipId} na unidade {UnitId}", membershipId, unitId);
            return BaseResponse<MembershipDto>.ErrorResult($"Erro ao alocar membro: {ex.Message}");
        }
    }

    /// <summary>
    /// Realoca um membro para uma nova unidade
    /// </summary>
    public async Task<BaseResponse<MembershipDto>> ReallocateMemberAsync(Guid membershipId, Guid newUnitId, string? reason = null, CancellationToken cancellationToken = default)
    {
        try
        {
            _loggingService.LogInformation("Realocação: membership {MembershipId} para unidade {NewUnitId}", membershipId, newUnitId);

            var membership = await _unitOfWork.Repository<Domain.Entities.Membership>()
                .GetFirstOrDefaultAsync(m => m.Id == membershipId, cancellationToken);

            if (membership == null)
            {
                return BaseResponse<MembershipDto>.ErrorResult("Membership não encontrada");
            }

            var oldUnitId = membership.UnitId;

            // Validar nova unidade
            var newUnit = await _unitOfWork.Repository<Unit>().GetFirstOrDefaultAsync(
                u => u.Id == newUnitId && u.ClubId == membership.ClubId, cancellationToken);

            if (newUnit == null)
            {
                return BaseResponse<MembershipDto>.ErrorResult("Nova unidade não encontrada ou não pertence ao mesmo clube");
            }

            // Buscar membro para validação
            var member = await _unitOfWork.Repository<Member>()
                .GetFirstOrDefaultAsync(m => m.Id == membership.MemberId, cancellationToken);

            if (member == null)
            {
                return BaseResponse<MembershipDto>.ErrorResult("Membro não encontrado");
            }

            // Validar compatibilidade
            if (!IsGenderCompatible(member.Gender, newUnit.Gender))
            {
                return BaseResponse<MembershipDto>.ErrorResult("Gênero da nova unidade não é compatível com o gênero do membro");
            }

            // Validar capacidade
            var hasCapacity = await HasAvailableCapacityAsync(newUnitId, cancellationToken);
            if (!hasCapacity)
            {
                return BaseResponse<MembershipDto>.ErrorResult("Nova unidade está em capacidade máxima");
            }

            // Realizar realocação
            membership.UnitId = newUnitId;
            membership.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // TODO: Registrar no Timeline
            _loggingService.LogInformation("Realocação realizada: membro {MemberId} de unidade {OldUnitId} para {NewUnitId}",
                membership.MemberId, oldUnitId, newUnitId);

            var dto = _mapper.Map<MembershipDto>(membership);
            return BaseResponse<MembershipDto>.SuccessResult(dto, $"Membro realocado para unidade {newUnit.Name}");
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Erro ao realocar membro {MembershipId} para unidade {NewUnitId}", membershipId, newUnitId);
            return BaseResponse<MembershipDto>.ErrorResult($"Erro ao realocar membro: {ex.Message}");
        }
    }

    /// <summary>
    /// Verifica se um membro precisa ser realocado por aniversário
    /// </summary>
    public async Task<BaseResponse<ReallocationCheckResultDto>> CheckBirthdayReallocationAsync(Guid membershipId, CancellationToken cancellationToken = default)
    {
        try
        {
            var membership = await _unitOfWork.Repository<Domain.Entities.Membership>()
                .GetFirstOrDefaultAsync(m => m.Id == membershipId, cancellationToken);

            if (membership == null)
            {
                return BaseResponse<ReallocationCheckResultDto>.ErrorResult("Membership não encontrada");
            }

            if (membership.UnitId == null)
            {
                return BaseResponse<ReallocationCheckResultDto>.ErrorResult("Membro não está alocado em nenhuma unidade");
            }

            // Buscar unidade atual
            var currentUnit = await _unitOfWork.Repository<Unit>()
                .GetFirstOrDefaultAsync(u => u.Id == membership.UnitId, cancellationToken);

            if (currentUnit == null)
            {
                return BaseResponse<ReallocationCheckResultDto>.ErrorResult("Unidade atual não encontrada");
            }

            // Buscar membro
            var member = await _unitOfWork.Repository<Member>()
                .GetFirstOrDefaultAsync(m => m.Id == membership.MemberId, cancellationToken);

            if (member == null)
            {
                return BaseResponse<ReallocationCheckResultDto>.ErrorResult("Membro não encontrado");
            }

            // Calcular idade atual
            var currentAge = member.CurrentAge;

            // Verificar se ainda é compatível com a unidade atual
            var isStillCompatible = IsAgeCompatible(currentAge, currentUnit.AgeMin, currentUnit.AgeMax);

            if (isStillCompatible)
            {
            return new BaseResponse<ReallocationCheckResultDto>
            {
                IsSuccess = true,
                Message = "Membro ainda é compatível com a unidade atual",
                Data = new ReallocationCheckResultDto
                {
                    NeedsReallocation = false,
                    CurrentUnit = _mapper.Map<CompatibleUnitDto>(currentUnit)
                }
            };
            }

            // Buscar unidades compatíveis
            var compatibleUnits = await GetCompatibleUnitsInternalAsync(
                membership.MemberId,
                membership.ClubId,
                DateTime.UtcNow,
                cancellationToken);

            var availableUnits = compatibleUnits.Where(u => u.HasAvailableCapacity).ToList();

            var result = new ReallocationCheckResultDto
            {
                NeedsReallocation = true,
                Reason = $"Membro tem {currentAge} anos e não é mais compatível com a unidade atual (faixa: {currentUnit.AgeMin}-{currentUnit.AgeMax})",
                CurrentUnit = _mapper.Map<CompatibleUnitDto>(currentUnit),
                CompatibleUnits = availableUnits,
                RequiresTask = true
            };

            if (availableUnits.Count == 1)
            {
                result.TaskType = "BirthdayReallocation";
                result.TaskDescription = $"Realocar membro {member.FullName} por aniversário para unidade {availableUnits.First().Name}";
            }
            else if (availableUnits.Count > 1)
            {
                result.TaskType = "ChooseReallocationUnit";
                result.TaskDescription = $"Escolher unidade para realocação por aniversário do membro {member.FullName}";
            }
            else
            {
                result.TaskType = "NoCompatibleUnit";
                result.TaskDescription = $"Nenhuma unidade compatível disponível para realocação por aniversário do membro {member.FullName}";
            }

            return new BaseResponse<ReallocationCheckResultDto>
            {
                IsSuccess = true,
                Message = "Membro precisa ser realocado por aniversário",
                Data = result
            };
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Erro ao verificar realocação por aniversário para membership {MembershipId}", membershipId);
            return BaseResponse<ReallocationCheckResultDto>.ErrorResult($"Erro ao verificar realocação: {ex.Message}");
        }
    }

    /// <summary>
    /// Obtém as unidades compatíveis para um membro
    /// </summary>
    public async Task<BaseResponse<List<CompatibleUnitDto>>> GetCompatibleUnitsAsync(Guid memberId, Guid clubId, DateTime? referenceDate = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var units = await GetCompatibleUnitsInternalAsync(memberId, clubId, referenceDate, cancellationToken);
            return BaseResponse<List<CompatibleUnitDto>>.SuccessResult(units);
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Erro ao obter unidades compatíveis para membro {MemberId}", memberId);
            return BaseResponse<List<CompatibleUnitDto>>.ErrorResult($"Erro ao obter unidades compatíveis: {ex.Message}");
        }
    }

    /// <summary>
    /// Valida se uma unidade tem capacidade disponível
    /// </summary>
    public async Task<bool> HasAvailableCapacityAsync(Guid unitId, CancellationToken cancellationToken = default)
    {
        var unit = await _unitOfWork.Repository<Unit>().GetFirstOrDefaultAsync(
            u => u.Id == unitId, cancellationToken);

        if (unit == null) return false;

        var currentCount = await GetCurrentMemberCountAsync(unitId, cancellationToken);
        return unit.Capacity == null || currentCount < unit.Capacity.Value;
    }

    /// <summary>
    /// Obtém o número atual de membros em uma unidade
    /// </summary>
    public async Task<int> GetCurrentMemberCountAsync(Guid unitId, CancellationToken cancellationToken = default)
    {
        var memberships = await _unitOfWork.Repository<Domain.Entities.Membership>()
            .GetAllAsync(m => m.UnitId == unitId && m.IsActive, cancellationToken);
        return memberships.Count();
    }

    #region Private Methods

    /// <summary>
    /// Obtém unidades compatíveis internamente
    /// </summary>
    private async Task<List<CompatibleUnitDto>> GetCompatibleUnitsInternalAsync(Guid memberId, Guid clubId, DateTime? referenceDate, CancellationToken cancellationToken)
    {
        var member = await _unitOfWork.Repository<Member>().GetFirstOrDefaultAsync(
            m => m.Id == memberId, cancellationToken);

        if (member == null) return new List<CompatibleUnitDto>();

        var actualReferenceDate = referenceDate ?? GetReferenceDateForAllocation(null);
        var memberAge = member.GetAgeOnJuneFirst(actualReferenceDate.Year);

        var units = await _unitOfWork.Repository<Unit>()
            .GetAllAsync(u => u.ClubId == clubId, cancellationToken);

        var compatibleUnits = new List<CompatibleUnitDto>();

        foreach (var unit in units)
        {
            if (!IsGenderCompatible(member.Gender, unit.Gender))
                continue;

            if (!IsAgeCompatible(memberAge, unit.AgeMin, unit.AgeMax))
                continue;

            var currentCount = await GetCurrentMemberCountAsync(unit.Id, cancellationToken);
            var hasCapacity = unit.Capacity == null || currentCount < unit.Capacity.Value;
            var occupancyPercentage = unit.Capacity.HasValue ? (double)currentCount / unit.Capacity.Value * 100 : 0;

            compatibleUnits.Add(new CompatibleUnitDto
            {
                Id = unit.Id,
                Name = unit.Name,
                Description = unit.Description,
                Gender = unit.Gender.ToString(),
                AgeMin = unit.AgeMin,
                AgeMax = unit.AgeMax,
                Capacity = unit.Capacity,
                CurrentMemberCount = currentCount,
                HasAvailableCapacity = hasCapacity,
                OccupancyPercentage = occupancyPercentage,
                Priority = currentCount // Menor lotação = maior prioridade
            });
        }

        return compatibleUnits;
    }

    /// <summary>
    /// Verifica se os gêneros são compatíveis
    /// </summary>
    private static bool IsGenderCompatible(MemberGender memberGender, UnitGender unitGender)
    {
        return (memberGender == MemberGender.Masculino && unitGender == UnitGender.Masculina) ||
               (memberGender == MemberGender.Feminino && unitGender == UnitGender.Feminina);
    }

    /// <summary>
    /// Verifica se a idade é compatível com a faixa da unidade
    /// </summary>
    private static bool IsAgeCompatible(int age, int ageMin, int ageMax)
    {
        return age >= ageMin && age <= ageMax;
    }

    /// <summary>
    /// Obtém a data de referência para alocação (1º de junho do ano da matrícula)
    /// </summary>
    private static DateTime GetReferenceDateForAllocation(Domain.Entities.Membership? membership)
    {
        var currentYear = DateTime.UtcNow.Year;

        // Se membership fornecida, usar o ano da matrícula
        if (membership != null)
        {
            var membershipYear = membership.CreatedAtUtc.Year;
            return new DateTime(membershipYear, 6, 1);
        }

        // Caso contrário, usar ano atual
        return new DateTime(currentYear, 6, 1);
    }

    #endregion
}
