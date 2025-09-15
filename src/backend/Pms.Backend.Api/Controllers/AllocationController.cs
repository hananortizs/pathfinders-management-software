using Microsoft.AspNetCore.Mvc;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Membership;
using Pms.Backend.Application.Interfaces;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Controller para operações de alocação de membros em unidades
/// Gerencia a alocação automática e manual de membros em unidades baseado nas regras de negócio
/// </summary>
[ApiController]
[Route("allocation")]
public class AllocationController : ControllerBase
{
    private readonly IAllocationService _allocationService;

    /// <summary>
    /// Inicializa uma nova instância do AllocationController
    /// </summary>
    /// <param name="allocationService">Serviço de alocação</param>
    public AllocationController(IAllocationService allocationService)
    {
        _allocationService = allocationService;
    }

    /// <summary>
    /// Aloca automaticamente um membro em uma unidade
    /// </summary>
    /// <param name="membershipId">ID da membership</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da alocação</returns>
    [HttpPost("auto-allocate/{membershipId:guid}")]
    [ProducesResponseType(typeof(BaseResponse<AllocationResultDto>), 200)]
    [ProducesResponseType(typeof(BaseResponse<AllocationResultDto>), 422)]
    [ProducesResponseType(typeof(BaseResponse<AllocationResultDto>), 400)]
    public async Task<IActionResult> AutoAllocateMember(
        [FromRoute] Guid membershipId,
        CancellationToken cancellationToken = default)
    {
        var result = await _allocationService.AllocateMemberAsync(membershipId, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Aloca um membro em uma unidade específica
    /// </summary>
    /// <param name="membershipId">ID da membership</param>
    /// <param name="unitId">ID da unidade</param>
    /// <param name="reason">Motivo da alocação manual (opcional)</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da alocação</returns>
    [HttpPost("allocate/{membershipId:guid}/unit/{unitId:guid}")]
    [ProducesResponseType(typeof(BaseResponse<MembershipDto>), 200)]
    [ProducesResponseType(typeof(BaseResponse<MembershipDto>), 400)]
    public async Task<IActionResult> AllocateToSpecificUnit(
        [FromRoute] Guid membershipId,
        [FromRoute] Guid unitId,
        [FromQuery] string? reason = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _allocationService.AllocateToSpecificUnitAsync(membershipId, unitId, reason, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Realoca um membro para uma nova unidade
    /// </summary>
    /// <param name="membershipId">ID da membership</param>
    /// <param name="newUnitId">ID da nova unidade</param>
    /// <param name="reason">Motivo da realocação (opcional)</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da realocação</returns>
    [HttpPost("reallocate/{membershipId:guid}/unit/{newUnitId:guid}")]
    [ProducesResponseType(typeof(BaseResponse<MembershipDto>), 200)]
    [ProducesResponseType(typeof(BaseResponse<MembershipDto>), 400)]
    public async Task<IActionResult> ReallocateMember(
        [FromRoute] Guid membershipId,
        [FromRoute] Guid newUnitId,
        [FromQuery] string? reason = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _allocationService.ReallocateMemberAsync(membershipId, newUnitId, reason, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Verifica se um membro precisa ser realocado por aniversário
    /// </summary>
    /// <param name="membershipId">ID da membership</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da verificação</returns>
    [HttpGet("check-birthday-reallocation/{membershipId:guid}")]
    [ProducesResponseType(typeof(BaseResponse<ReallocationCheckResultDto>), 200)]
    [ProducesResponseType(typeof(BaseResponse<ReallocationCheckResultDto>), 400)]
    public async Task<IActionResult> CheckBirthdayReallocation(
        [FromRoute] Guid membershipId,
        CancellationToken cancellationToken = default)
    {
        var result = await _allocationService.CheckBirthdayReallocationAsync(membershipId, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Obtém as unidades compatíveis para um membro
    /// </summary>
    /// <param name="memberId">ID do membro</param>
    /// <param name="clubId">ID do clube</param>
    /// <param name="referenceDate">Data de referência para cálculo de idade (opcional)</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de unidades compatíveis</returns>
    [HttpGet("compatible-units")]
    [ProducesResponseType(typeof(BaseResponse<List<CompatibleUnitDto>>), 200)]
    [ProducesResponseType(typeof(BaseResponse<List<CompatibleUnitDto>>), 400)]
    public async Task<IActionResult> GetCompatibleUnits(
        [FromQuery] Guid memberId,
        [FromQuery] Guid clubId,
        [FromQuery] DateTime? referenceDate = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _allocationService.GetCompatibleUnitsAsync(memberId, clubId, referenceDate, cancellationToken);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// Verifica se uma unidade tem capacidade disponível
    /// </summary>
    /// <param name="unitId">ID da unidade</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se tem capacidade, false caso contrário</returns>
    [HttpGet("unit/{unitId:guid}/capacity")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(typeof(BaseResponse<bool>), 400)]
    public async Task<IActionResult> CheckUnitCapacity(
        [FromRoute] Guid unitId,
        CancellationToken cancellationToken = default)
    {
        var hasCapacity = await _allocationService.HasAvailableCapacityAsync(unitId, cancellationToken);
        return Ok(hasCapacity);
    }

    /// <summary>
    /// Obtém o número atual de membros em uma unidade
    /// </summary>
    /// <param name="unitId">ID da unidade</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Número de membros ativos</returns>
    [HttpGet("unit/{unitId:guid}/member-count")]
    [ProducesResponseType(typeof(int), 200)]
    [ProducesResponseType(typeof(BaseResponse<int>), 400)]
    public async Task<IActionResult> GetUnitMemberCount(
        [FromRoute] Guid unitId,
        CancellationToken cancellationToken = default)
    {
        var count = await _allocationService.GetCurrentMemberCountAsync(unitId, cancellationToken);
        return Ok(count);
    }
}
