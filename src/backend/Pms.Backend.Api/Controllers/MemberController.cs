using Microsoft.AspNetCore.Mvc;
using Pms.Backend.Application.DTOs.Members;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Application.DTOs;

namespace Pms.Backend.Api.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de membros
    /// </summary>
    [ApiController]
    [Route("pms-loc/members")]
    public partial class MemberController : BaseController
    {
        private readonly IMemberService _memberService;
        private readonly IAuthService _authService;
        private readonly ILogger<MemberController> _logger;

        /// <summary>
        /// Inicializa uma nova instância da classe MemberController
        /// </summary>
        /// <param name="memberService">Serviço de membros</param>
        /// <param name="authService">Serviço de autenticação</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public MemberController(
            IMemberService memberService,
            IAuthService authService,
            ILogger<MemberController> logger)
        {
            _memberService = memberService;
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Lista membros com filtros e paginação (método GET para compatibilidade)
        /// </summary>
        /// <param name="userLevel">Nível do usuário</param>
        /// <param name="groupingStrategy">Estratégia de agrupamento</param>
        /// <param name="page">Página atual</param>
        /// <param name="pageSize">Tamanho da página</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        /// <returns>Lista paginada de membros</returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<MemberListResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMembers(
            [FromQuery] string userLevel = "Admin",
            [FromQuery] string groupingStrategy = "hierarchical",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Listando membros - UserLevel: {UserLevel}, Grouping: {GroupingStrategy}, Page: {Page}",
                userLevel, groupingStrategy, page);

            var request = new GetMembersRequestDto
            {
                UserLevel = userLevel,
                GroupingStrategy = groupingStrategy,
                Page = page,
                PageSize = pageSize
            };

            var result = await _memberService.GetMembersAsync(request, cancellationToken);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Busca membros com filtros avançados (método POST recomendado)
        /// </summary>
        /// <param name="request">Parâmetros de filtro e paginação</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        /// <returns>Lista paginada de membros</returns>
        [HttpPost("search")]
        [ProducesResponseType(typeof(BaseResponse<MemberListResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchMembers(
            [FromBody] GetMembersRequestDto request,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Buscando membros com filtros avançados: {Filters}", request);

            var result = await _memberService.GetMembersAsync(request, cancellationToken);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Obtém detalhes de um membro específico
        /// </summary>
        /// <param name="id">ID do membro</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        /// <returns>Detalhes do membro</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(BaseResponse<MemberDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMemberById(Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Buscando membro {MemberId}", id);

            var result = await _memberService.GetMemberAsync(id, cancellationToken);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Cria um novo membro
        /// </summary>
        /// <param name="request">Dados do novo membro</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        /// <returns>Membro criado</returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<MemberDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateMember(
            [FromBody] CreateMemberCompleteDto request,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Criando novo membro");

            var result = await _memberService.CreateMemberCompleteAsync(request, cancellationToken);
            return ProcessResponseWithCreatedAtAction(result, nameof(GetMemberById), new { id = result.Data?.Id });
        }

        /// <summary>
        /// Atualiza um membro existente
        /// </summary>
        /// <param name="id">ID do membro</param>
        /// <param name="request">Dados atualizados</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        /// <returns>Membro atualizado</returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(BaseResponse<MemberDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateMember(
            Guid id,
            [FromBody] UpdateMemberDto request,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Atualizando membro {MemberId}", id);

            var result = await _memberService.UpdateMemberAsync(id, request, cancellationToken);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Exclui um membro (soft delete)
        /// </summary>
        /// <param name="id">ID do membro</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        /// <returns>Confirmação da exclusão</returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMember(Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Excluindo membro {MemberId}", id);

            var result = await _memberService.DeleteMemberAsync(id, cancellationToken);
            return ProcessResponse(result);
        }

    }
}
