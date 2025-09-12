using Microsoft.AspNetCore.Mvc;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Auth;
using Pms.Backend.Application.DTOs.Members;
using Pms.Backend.Application.Interfaces;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Controller for member management operations
/// </summary>
[ApiController]
[Route("[controller]")]
public partial class MemberController : ControllerBase
{
    private readonly IMemberService _memberService;

    /// <summary>
    /// Initializes a new instance of the MemberController
    /// </summary>
    /// <param name="memberService">Member service for business logic</param>
    public MemberController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    #region Member CRUD Operations

    /// <summary>
    /// Gets a specific member by ID
    /// </summary>
    /// <param name="id">Member ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Member information</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMember(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _memberService.GetMemberAsync(id, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets a paginated list of members with optimized response structure (no repetitive fields)
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Optimized paginated list of members</returns>
    [HttpGet]
    public async Task<IActionResult> GetMembers(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _memberService.GetMembersOptimizedAsync(pageNumber, pageSize, cancellationToken);
        return Ok(result);
    }


    /// <summary>
    /// Gets members by club ID
    /// </summary>
    /// <param name="clubId">Club ID</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of members in the club</returns>
    [HttpGet("club/{clubId}")]
    public async Task<IActionResult> GetMembersByClub(
        Guid clubId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _memberService.GetMembersByClubAsync(clubId, pageNumber, pageSize, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Cria um novo membro com informações completas incluindo endereço, informações médicas, contatos, etc.
    /// </summary>
    /// <remarks>
    /// Este endpoint permite criar um membro com todas as informações necessárias:
    ///
    /// **Regras de Negócio:**
    /// - Email é obrigatório: deve estar em `contactInfo` (tipo 3) OU em `loginInfo`
    /// - Se não houver email em `contactInfo`, o sistema cria automaticamente um contato de email usando `loginInfo.email`
    /// - CPF deve ser único no sistema (se fornecido)
    /// - Membro deve ter pelo menos 10 anos de idade
    /// - Senha deve ter pelo menos 10 caracteres (se fornecida)
    ///
    /// **Tipos de Contato:**
    /// - 1: Mobile (Celular)
    /// - 2: Landline (Telefone Fixo)
    /// - 3: Email
    /// - 4: WhatsApp
    /// - 5-11: Redes Sociais (Facebook, Instagram, YouTube, etc.)
    ///
    /// **Exemplo de Payload:**
    /// ```json
    /// {
    ///   "firstName": "João",
    ///   "lastName": "Silva",
    ///   "middleNames": "Santos",
    ///   "socialName": "João Silva",
    ///   "dateOfBirth": "1990-05-15T00:00:00.000Z",
    ///   "gender": 1,
    ///   "cpf": "12345678901",
    ///   "rg": "123456789",
    ///   "contactInfo": [
    ///     {
    ///       "type": 1,
    ///       "value": "+5511999999999",
    ///       "isPrimary": true,
    ///       "description": "Telefone celular"
    ///     }
    ///   ],
    ///   "loginInfo": {
    ///     "email": "joao.silva@email.com",
    ///     "password": "MinhaSenh@123",
    ///     "confirmPassword": "MinhaSenh@123"
    ///   },
    ///   "address": {
    ///     "postalCode": "01234567",
    ///     "street": "Rua das Flores, 123",
    ///     "neighborhood": "Centro",
    ///     "city": "São Paulo",
    ///     "state": "SP",
    ///     "country": "Brasil",
    ///     "isPrimary": true
    ///   }
    /// }
    /// ```
    /// </remarks>
    /// <param name="dto">Dados completos para criação do membro</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>
    /// **201 Created**: Membro criado com sucesso
    ///
    /// **400 Bad Request**: Dados inválidos ou regras de negócio violadas
    /// - "Pelo menos um contato de email é obrigatório (forneça no contactInfo ou loginInfo)"
    /// - "CPF já existe"
    /// - "Email {email} já existe"
    /// - "Membro deve ter pelo menos 10 anos de idade"
    /// - "Senha deve ter pelo menos 10 caracteres"
    ///
    /// **401 Unauthorized**: Token JWT inválido ou expirado
    ///
    /// **500 Internal Server Error**: Erro interno do servidor
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(BaseResponse<MemberDto>), 201)]
    [ProducesResponseType(typeof(BaseResponse<object>), 400)]
    [ProducesResponseType(typeof(BaseResponse<object>), 401)]
    [ProducesResponseType(typeof(BaseResponse<object>), 500)]
    public async Task<IActionResult> CreateMember([FromBody] CreateMemberCompleteDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _memberService.CreateMemberCompleteAsync(dto, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Created($"/member/{result.Data?.Id}", result);
    }


    /// <summary>
    /// Updates an existing member
    /// </summary>
    /// <param name="id">Member ID</param>
    /// <param name="dto">Member update data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated member information</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMember(Guid id, [FromBody] UpdateMemberDto dto, CancellationToken cancellationToken = default)
    {
        var result = await _memberService.UpdateMemberAsync(id, dto, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a member (soft delete)
    /// </summary>
    /// <param name="id">Member ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Deletion result</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMember(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _memberService.DeleteMemberAsync(id, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Hard deletes a member and all related data (permanent deletion)
    /// </summary>
    /// <param name="id">Member ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Hard deletion result</returns>
    [HttpDelete("{id}/hard")]
    public async Task<IActionResult> HardDeleteMember(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _memberService.HardDeleteMemberAsync(id, cancellationToken);
        return Ok(result);
    }

    #endregion

    #region Authentication Operations

    /// <summary>
    /// Changes user password
    /// </summary>
    /// <param name="memberId">Member ID</param>
    /// <param name="request">Password change request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Password change result</returns>
    [HttpPost("{memberId}/change-password")]
    public async Task<IActionResult> ChangePassword(Guid memberId, [FromBody] ChangePasswordRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _memberService.ChangePasswordAsync(memberId, request, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Initiates password reset
    /// </summary>
    /// <param name="request">Password reset request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Password reset initiation result</returns>
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _memberService.ResetPasswordAsync(request, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Confirms password reset
    /// </summary>
    /// <param name="request">Password reset confirmation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Password reset confirmation result</returns>
    [HttpPost("reset-password/confirm")]
    public async Task<IActionResult> ResetPasswordConfirm([FromBody] ResetPasswordConfirmDto request, CancellationToken cancellationToken = default)
    {
        var result = await _memberService.ResetPasswordConfirmAsync(request, cancellationToken);
        return Ok(result);
    }

    #endregion

    #region Member Invitation and Activation

    /// <summary>
    /// Invites a new member
    /// </summary>
    /// <param name="request">Member invitation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Invitation result</returns>
    [HttpPost("invite")]
    public async Task<IActionResult> InviteMember([FromBody] InviteMemberRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _memberService.InviteMemberAsync(request, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Activates a member account
    /// </summary>
    /// <param name="request">Member activation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Activation result with JWT token</returns>
    [HttpPost("activate")]
    public async Task<IActionResult> ActivateMember([FromBody] ActivateMemberRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _memberService.ActivateMemberAsync(request, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Resends activation email
    /// </summary>
    /// <param name="memberId">Member ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Resend result</returns>
    [HttpPost("{memberId}/resend-activation")]
    public async Task<IActionResult> ResendActivationEmail(Guid memberId, CancellationToken cancellationToken = default)
    {
        var result = await _memberService.ResendActivationEmailAsync(memberId, cancellationToken);
        return Ok(result);
    }

    #endregion
}
