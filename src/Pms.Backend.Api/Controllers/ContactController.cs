using Microsoft.AspNetCore.Mvc;
using Pms.Backend.Application.DTOs.Contacts;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Domain.Enums;

namespace Pms.Backend.Api.Controllers;

/// <summary>
/// Controller para gerenciamento de contatos
/// </summary>
[ApiController]
[Route("[controller]")]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;
    private readonly ILogger<ContactController> _logger;

    /// <summary>
    /// Inicializa uma nova instância do ContactController
    /// </summary>
    /// <param name="contactService">Serviço de contatos</param>
    /// <param name="logger">Logger</param>
    public ContactController(IContactService contactService, ILogger<ContactController> logger)
    {
        _contactService = contactService;
        _logger = logger;
    }

    /// <summary>
    /// Obtém todos os contatos de uma entidade
    /// </summary>
    /// <param name="entityId">ID da entidade</param>
    /// <param name="entityType">Tipo da entidade</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de contatos da entidade</returns>
    [HttpGet("entity/{entityId}")]
    [ProducesResponseType(typeof(IEnumerable<ContactDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetContactsByEntity(
        [FromRoute] Guid entityId,
        [FromQuery] string entityType,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Obtendo contatos para entidade {EntityId} do tipo {EntityType}", entityId, entityType);

            var contacts = await _contactService.GetContactsByEntityAsync(entityId, entityType, cancellationToken);
            return Ok(contacts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter contatos para entidade {EntityId} do tipo {EntityType}", entityId, entityType);
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Obtém contatos por categoria
    /// </summary>
    /// <param name="entityId">ID da entidade</param>
    /// <param name="entityType">Tipo da entidade</param>
    /// <param name="category">Categoria do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de contatos da categoria</returns>
    [HttpGet("entity/{entityId}/category/{category}")]
    [ProducesResponseType(typeof(IEnumerable<ContactDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetContactsByCategory(
        [FromRoute] Guid entityId,
        [FromRoute] ContactCategory category,
        [FromQuery] string entityType,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Obtendo contatos da categoria {Category} para entidade {EntityId} do tipo {EntityType}", category, entityId, entityType);

            var contacts = await _contactService.GetContactsByCategoryAsync(entityId, entityType, category, cancellationToken);
            return Ok(contacts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter contatos da categoria {Category} para entidade {EntityId} do tipo {EntityType}", category, entityId, entityType);
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Obtém contatos por tipo
    /// </summary>
    /// <param name="entityId">ID da entidade</param>
    /// <param name="entityType">Tipo da entidade</param>
    /// <param name="type">Tipo do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de contatos do tipo</returns>
    [HttpGet("entity/{entityId}/type/{type}")]
    [ProducesResponseType(typeof(IEnumerable<ContactDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetContactsByType(
        [FromRoute] Guid entityId,
        [FromRoute] ContactType type,
        [FromQuery] string entityType,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Obtendo contatos do tipo {Type} para entidade {EntityId} do tipo {EntityType}", type, entityId, entityType);

            var contacts = await _contactService.GetContactsByTypeAsync(entityId, entityType, type, cancellationToken);
            return Ok(contacts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter contatos do tipo {Type} para entidade {EntityId} do tipo {EntityType}", type, entityId, entityType);
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Obtém contato por ID
    /// </summary>
    /// <param name="id">ID do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Contato encontrado</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ContactDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetContactById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Obtendo contato {ContactId}", id);

            var contact = await _contactService.GetContactByIdAsync(id, cancellationToken);
            if (contact == null)
            {
                return NotFound($"Contato com ID {id} não encontrado");
            }

            return Ok(contact);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter contato {ContactId}", id);
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Obtém contato principal de uma categoria
    /// </summary>
    /// <param name="entityId">ID da entidade</param>
    /// <param name="entityType">Tipo da entidade</param>
    /// <param name="category">Categoria do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Contato principal</returns>
    [HttpGet("entity/{entityId}/primary/{category}")]
    [ProducesResponseType(typeof(ContactDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPrimaryContact(
        [FromRoute] Guid entityId,
        [FromRoute] ContactCategory category,
        [FromQuery] string entityType,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Obtendo contato principal da categoria {Category} para entidade {EntityId} do tipo {EntityType}", category, entityId, entityType);

            var contact = await _contactService.GetPrimaryContactAsync(entityId, entityType, category, cancellationToken);
            if (contact == null)
            {
                return NotFound($"Nenhum contato principal encontrado para a categoria {category}");
            }

            return Ok(contact);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter contato principal da categoria {Category} para entidade {EntityId} do tipo {EntityType}", category, entityId, entityType);
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Cria um novo contato
    /// </summary>
    /// <param name="createDto">Dados para criação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Contato criado</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ContactDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateContact(
        [FromBody] CreateContactDto createDto,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Criando novo contato para entidade {EntityId} do tipo {EntityType}", createDto.EntityId, createDto.EntityType);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contact = await _contactService.CreateContactAsync(createDto, cancellationToken);
            return CreatedAtAction(nameof(GetContactById), new { id = contact.Id }, contact);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao criar contato para entidade {EntityId} do tipo {EntityType}", createDto.EntityId, createDto.EntityType);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar contato para entidade {EntityId} do tipo {EntityType}", createDto.EntityId, createDto.EntityType);
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Atualiza um contato existente
    /// </summary>
    /// <param name="id">ID do contato</param>
    /// <param name="updateDto">Dados para atualização</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Contato atualizado</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ContactDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateContact(
        [FromRoute] Guid id,
        [FromBody] UpdateContactDto updateDto,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Atualizando contato {ContactId}", id);

            if (id != updateDto.Id)
            {
                return BadRequest("ID da rota não confere com o ID do corpo da requisição");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contact = await _contactService.UpdateContactAsync(updateDto, cancellationToken);
            return Ok(contact);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao atualizar contato {ContactId}", id);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar contato {ContactId}", id);
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Remove um contato
    /// </summary>
    /// <param name="id">ID do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da operação</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteContact(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Removendo contato {ContactId}", id);

            var result = await _contactService.DeleteContactAsync(id, cancellationToken);
            if (!result)
            {
                return NotFound($"Contato com ID {id} não encontrado");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover contato {ContactId}", id);
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Define um contato como principal
    /// </summary>
    /// <param name="id">ID do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da operação</returns>
    [HttpPatch("{id}/set-primary")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SetAsPrimary(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Definindo contato {ContactId} como principal", id);

            var result = await _contactService.SetAsPrimaryAsync(id, cancellationToken);
            if (!result)
            {
                return NotFound($"Contato com ID {id} não encontrado ou não pode ser definido como principal");
            }

            return Ok(new { message = "Contato definido como principal com sucesso" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao definir contato {ContactId} como principal", id);
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Remove a definição de contato principal
    /// </summary>
    /// <param name="id">ID do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da operação</returns>
    [HttpPatch("{id}/remove-primary")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemovePrimary(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Removendo definição de contato principal {ContactId}", id);

            var result = await _contactService.RemovePrimaryAsync(id, cancellationToken);
            if (!result)
            {
                return NotFound($"Contato com ID {id} não encontrado ou não é principal");
            }

            return Ok(new { message = "Definição de contato principal removida com sucesso" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover definição de contato principal {ContactId}", id);
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Ativa um contato
    /// </summary>
    /// <param name="id">ID do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da operação</returns>
    [HttpPatch("{id}/activate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ActivateContact(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Ativando contato {ContactId}", id);

            var result = await _contactService.ActivateContactAsync(id, cancellationToken);
            if (!result)
            {
                return NotFound($"Contato com ID {id} não encontrado");
            }

            return Ok(new { message = "Contato ativado com sucesso" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao ativar contato {ContactId}", id);
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Desativa um contato
    /// </summary>
    /// <param name="id">ID do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da operação</returns>
    [HttpPatch("{id}/deactivate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeactivateContact(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Desativando contato {ContactId}", id);

            var result = await _contactService.DeactivateContactAsync(id, cancellationToken);
            if (!result)
            {
                return NotFound($"Contato com ID {id} não encontrado");
            }

            return Ok(new { message = "Contato desativado com sucesso" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao desativar contato {ContactId}", id);
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Marca um contato como verificado
    /// </summary>
    /// <param name="id">ID do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da operação</returns>
    [HttpPatch("{id}/verify")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> MarkAsVerified(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Marcando contato {ContactId} como verificado", id);

            var result = await _contactService.MarkAsVerifiedAsync(id, cancellationToken);
            if (!result)
            {
                return NotFound($"Contato com ID {id} não encontrado");
            }

            return Ok(new { message = "Contato marcado como verificado com sucesso" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao marcar contato {ContactId} como verificado", id);
            return StatusCode(500, "Erro interno do servidor");
        }
    }

    /// <summary>
    /// Marca um contato como não verificado
    /// </summary>
    /// <param name="id">ID do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da operação</returns>
    [HttpPatch("{id}/unverify")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> MarkAsUnverified(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Marcando contato {ContactId} como não verificado", id);

            var result = await _contactService.MarkAsUnverifiedAsync(id, cancellationToken);
            if (!result)
            {
                return NotFound($"Contato com ID {id} não encontrado");
            }

            return Ok(new { message = "Contato marcado como não verificado com sucesso" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao marcar contato {ContactId} como não verificado", id);
            return StatusCode(500, "Erro interno do servidor");
        }
    }
}
