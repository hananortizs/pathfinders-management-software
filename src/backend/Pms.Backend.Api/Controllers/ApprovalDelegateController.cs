using Microsoft.AspNetCore.Mvc;
using Pms.Backend.Application.DTOs.Assignments;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Controller para gerenciar delegações de aprovação
/// </summary>
[ApiController]
[Route("[controller]")]
public class ApprovalDelegateController : ControllerBase
{
    private readonly IApprovalDelegateService _approvalDelegateService;

    /// <summary>
    /// Inicializa uma nova instância do ApprovalDelegateController
    /// </summary>
    /// <param name="approvalDelegateService">Serviço de delegação de aprovação</param>
    public ApprovalDelegateController(IApprovalDelegateService approvalDelegateService)
    {
        _approvalDelegateService = approvalDelegateService;
    }

    /// <summary>
    /// Cria uma nova delegação de aprovação
    /// </summary>
    /// <param name="request">Solicitação de criação da delegação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>A delegação criada</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApprovalDelegateDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<ApprovalDelegateDto>> CreateDelegation([FromBody] CreateApprovalDelegateDto request, CancellationToken cancellationToken)
    {
        var delegation = await _approvalDelegateService.CreateDelegationAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetDelegationById), new { id = delegation.Id }, delegation);
    }

    /// <summary>
    /// Obtém uma delegação por ID
    /// </summary>
    /// <param name="id">ID da delegação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>A delegação</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApprovalDelegateDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApprovalDelegateDto>> GetDelegationById(Guid id, CancellationToken cancellationToken)
    {
        var delegation = await _approvalDelegateService.GetDelegationAsync(id, cancellationToken);
        return Ok(delegation);
    }

    /// <summary>
    /// Obtém todas as delegações para um escopo específico
    /// </summary>
    /// <param name="scopeType">Tipo do escopo</param>
    /// <param name="scopeId">ID do escopo</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de delegações</returns>
    [HttpGet("scope/{scopeType}/{scopeId}")]
    [ProducesResponseType(typeof(IEnumerable<ApprovalDelegateDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ApprovalDelegateDto>>> GetDelegationsByScope(ScopeType scopeType, Guid scopeId, CancellationToken cancellationToken)
    {
        var delegations = await _approvalDelegateService.GetDelegationsByScopeAsync(scopeType, scopeId, cancellationToken);
        return Ok(delegations);
    }

    /// <summary>
    /// Obtém todas as delegações ativas para um escopo específico
    /// </summary>
    /// <param name="scopeType">Tipo do escopo</param>
    /// <param name="scopeId">ID do escopo</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de delegações ativas</returns>
    [HttpGet("scope/{scopeType}/{scopeId}/active")]
    [ProducesResponseType(typeof(IEnumerable<ApprovalDelegateDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ApprovalDelegateDto>>> GetActiveDelegationsByScope(ScopeType scopeType, Guid scopeId, CancellationToken cancellationToken)
    {
        var delegations = await _approvalDelegateService.GetActiveDelegationsByScopeAsync(scopeType, scopeId, cancellationToken);
        return Ok(delegations);
    }

    /// <summary>
    /// Obtém todas as delegações onde um membro é o delegado
    /// </summary>
    /// <param name="memberId">ID do membro</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de delegações</returns>
    [HttpGet("delegate/{memberId}")]
    [ProducesResponseType(typeof(IEnumerable<ApprovalDelegateDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ApprovalDelegateDto>>> GetDelegationsByDelegate(Guid memberId, CancellationToken cancellationToken)
    {
        var delegations = await _approvalDelegateService.GetDelegationsByDelegateAsync(memberId, cancellationToken);
        return Ok(delegations);
    }

    /// <summary>
    /// Obtém todas as delegações onde um membro é o delegante
    /// </summary>
    /// <param name="memberId">ID do membro</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de delegações</returns>
    [HttpGet("delegator/{memberId}")]
    [ProducesResponseType(typeof(IEnumerable<ApprovalDelegateDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ApprovalDelegateDto>>> GetDelegationsByDelegator(Guid memberId, CancellationToken cancellationToken)
    {
        var delegations = await _approvalDelegateService.GetDelegationsByDelegatorAsync(memberId, cancellationToken);
        return Ok(delegations);
    }

    /// <summary>
    /// Atualiza uma delegação
    /// </summary>
    /// <param name="id">ID da delegação</param>
    /// <param name="request">Solicitação de atualização</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>A delegação atualizada</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApprovalDelegateDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<ApprovalDelegateDto>> UpdateDelegation(Guid id, [FromBody] UpdateApprovalDelegateDto request, CancellationToken cancellationToken)
    {
        var delegation = await _approvalDelegateService.UpdateDelegationAsync(id, request, cancellationToken);
        return Ok(delegation);
    }

    /// <summary>
    /// Finaliza uma delegação
    /// </summary>
    /// <param name="id">ID da delegação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado do sucesso</returns>
    [HttpPut("{id}/end")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> EndDelegation(Guid id, CancellationToken cancellationToken)
    {
        await _approvalDelegateService.EndDelegationAsync(id, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Exclui uma delegação
    /// </summary>
    /// <param name="id">ID da delegação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado do sucesso</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> DeleteDelegation(Guid id, CancellationToken cancellationToken)
    {
        await _approvalDelegateService.DeleteDelegationAsync(id, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Valida se uma delegação pode ser criada
    /// </summary>
    /// <param name="request">Solicitação da delegação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da validação</returns>
    [HttpPost("validate")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<bool>> ValidateDelegation([FromBody] CreateApprovalDelegateDto request, CancellationToken cancellationToken)
    {
        var isValid = await _approvalDelegateService.ValidateDelegationAsync(request, cancellationToken);
        return Ok(isValid);
    }

    /// <summary>
    /// Obtém o aprovador efetivo para uma função em um escopo específico
    /// </summary>
    /// <param name="role">Nome da função</param>
    /// <param name="scopeType">Tipo do escopo</param>
    /// <param name="scopeId">ID do escopo</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>A atribuição efetiva</returns>
    [HttpGet("effective-approver")]
    [ProducesResponseType(typeof(AssignmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AssignmentDto>> GetEffectiveApprover(
        [FromQuery] string role,
        [FromQuery] ScopeType scopeType,
        [FromQuery] Guid scopeId,
        CancellationToken cancellationToken)
    {
        var assignment = await _approvalDelegateService.GetEffectiveApproverAsync(role, scopeType, scopeId, cancellationToken);
        return Ok(assignment);
    }
}
