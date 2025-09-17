using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Validation;
using Pms.Backend.Application.Interfaces.Validation;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Controller para validação de gating por lenço
/// </summary>
[ApiController]
[Route("pms-loc/scarf-gating")]
[Authorize]
public class ScarfGatingController : BaseController
{
    private readonly IScarfGatingService _scarfGatingService;

    /// <summary>
    /// Inicializa uma nova instância do ScarfGatingController
    /// </summary>
    /// <param name="scarfGatingService">Serviço de validação de lenço</param>
    public ScarfGatingController(IScarfGatingService scarfGatingService)
    {
        _scarfGatingService = scarfGatingService;
    }

    /// <summary>
    /// Valida se um membro pode realizar operações de progresso baseado no lenço
    /// </summary>
    /// <param name="memberId">ID do membro</param>
    /// <param name="operationType">Tipo de operação (classe, especialidade, mestrado, etc.)</param>
    /// <returns>Resultado da validação</returns>
    [HttpPost("validate-progress/{memberId}")]
    [ProducesResponseType(typeof(BaseResponse<bool>), 200)]
    [ProducesResponseType(typeof(BaseResponse<bool>), 400)]
    [ProducesResponseType(typeof(BaseResponse<bool>), 404)]
    public async Task<IActionResult> ValidateProgressRequirement(Guid memberId, [FromBody] string operationType)
    {
        try
        {
            // TODO: Buscar membro do banco de dados
            // Por enquanto, vamos simular um membro para teste
            var member = new Member
            {
                Id = memberId,
                ScarfInvested = false // Simular membro sem lenço
            };

            var result = _scarfGatingService.ValidateScarfRequirement(member, operationType);
            
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, BaseResponse<bool>.InternalServerErrorResult(
                $"Erro interno ao validar requisito de lenço: {ex.Message}",
                true
            ));
        }
    }

    /// <summary>
    /// Valida se um membro pode assumir cargos de liderança baseado no lenço
    /// </summary>
    /// <param name="memberId">ID do membro</param>
    /// <param name="roleName">Nome do cargo de liderança</param>
    /// <returns>Resultado da validação</returns>
    [HttpPost("validate-leadership/{memberId}")]
    [ProducesResponseType(typeof(BaseResponse<bool>), 200)]
    [ProducesResponseType(typeof(BaseResponse<bool>), 400)]
    [ProducesResponseType(typeof(BaseResponse<bool>), 404)]
    public async Task<IActionResult> ValidateLeadershipRequirement(Guid memberId, [FromBody] string roleName)
    {
        try
        {
            // TODO: Buscar membro do banco de dados
            // Por enquanto, vamos simular um membro para teste
            var member = new Member
            {
                Id = memberId,
                ScarfInvested = false // Simular membro sem lenço
            };

            var result = _scarfGatingService.ValidateLeadershipScarfRequirement(member, roleName);
            
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, BaseResponse<bool>.InternalServerErrorResult(
                $"Erro interno ao validar requisito de lenço para liderança: {ex.Message}",
                true
            ));
        }
    }

    /// <summary>
    /// Verifica se um membro possui lenço investido
    /// </summary>
    /// <param name="memberId">ID do membro</param>
    /// <returns>Informações sobre a investidura de lenço</returns>
    [HttpGet("check/{memberId}")]
    [ProducesResponseType(typeof(BaseResponse<ScarfInvestitureInfo?>), 200)]
    [ProducesResponseType(typeof(BaseResponse<ScarfInvestitureInfo?>), 404)]
    public async Task<IActionResult> CheckScarfInvestiture(Guid memberId)
    {
        try
        {
            // TODO: Buscar membro do banco de dados
            // Por enquanto, vamos simular um membro para teste
            var member = new Member
            {
                Id = memberId,
                ScarfInvested = true,
                ScarfInvestedAt = DateTime.UtcNow.AddMonths(-6),
                ScarfChurch = "IASD Vila Medeiros",
                ScarfPastor = "Pastor Arilton Ferreira",
                ScarfDate = DateTime.UtcNow.AddMonths(-6)
            };

            var info = _scarfGatingService.GetScarfInvestitureInfo(member);
            
            return Ok(BaseResponse<ScarfInvestitureInfo?>.SuccessResult(info, "Informações de lenço obtidas com sucesso"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, BaseResponse<ScarfInvestitureInfo?>.InternalServerErrorResult(
                $"Erro interno ao verificar investidura de lenço: {ex.Message}",
                true
            ));
        }
    }
}
