using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pms.Backend.Application.DTOs.Contacts;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Domain.Enums;
using Pms.Backend.Infrastructure.Data;

namespace Pms.Backend.Infrastructure.Services;

/// <summary>
/// Serviço para gerenciamento de contatos
/// </summary>
public class ContactService : IContactService
{
    private readonly PmsDbContext _context;
    private readonly IMapper _mapper;
    private readonly IContactValidationService _validationService;

    /// <summary>
    /// Inicializa uma nova instância do ContactService
    /// </summary>
    /// <param name="context">Contexto do banco de dados</param>
    /// <param name="mapper">Mapeador AutoMapper</param>
    /// <param name="validationService">Serviço de validação</param>
    public ContactService(PmsDbContext context, IMapper mapper, IContactValidationService validationService)
    {
        _context = context;
        _mapper = mapper;
        _validationService = validationService;
    }

    /// <summary>
    /// Obtém todos os contatos de uma entidade
    /// </summary>
    /// <param name="entityId">ID da entidade</param>
    /// <param name="entityType">Tipo da entidade</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de contatos da entidade</returns>
    public async Task<IEnumerable<ContactDto>> GetContactsByEntityAsync(Guid entityId, string entityType, CancellationToken cancellationToken = default)
    {
        var contacts = await _context.Contacts
            .Where(c => c.EntityId == entityId && c.EntityType == entityType && !c.IsDeleted)
            .OrderBy(c => c.Priority)
            .ThenBy(c => c.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<ContactDto>>(contacts);
    }

    /// <summary>
    /// Obtém contatos por categoria
    /// </summary>
    /// <param name="entityId">ID da entidade</param>
    /// <param name="entityType">Tipo da entidade</param>
    /// <param name="category">Categoria do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de contatos da categoria</returns>
    public async Task<IEnumerable<ContactDto>> GetContactsByCategoryAsync(Guid entityId, string entityType, ContactCategory category, CancellationToken cancellationToken = default)
    {
        var contacts = await _context.Contacts
            .Where(c => c.EntityId == entityId && c.EntityType == entityType && c.Category == category && !c.IsDeleted)
            .OrderBy(c => c.Priority)
            .ThenBy(c => c.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<ContactDto>>(contacts);
    }

    /// <summary>
    /// Obtém contatos por tipo
    /// </summary>
    /// <param name="entityId">ID da entidade</param>
    /// <param name="entityType">Tipo da entidade</param>
    /// <param name="type">Tipo do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de contatos do tipo</returns>
    public async Task<IEnumerable<ContactDto>> GetContactsByTypeAsync(Guid entityId, string entityType, ContactType type, CancellationToken cancellationToken = default)
    {
        var contacts = await _context.Contacts
            .Where(c => c.EntityId == entityId && c.EntityType == entityType && c.Type == type && !c.IsDeleted)
            .OrderBy(c => c.Priority)
            .ThenBy(c => c.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<ContactDto>>(contacts);
    }

    /// <summary>
    /// Obtém contato por ID
    /// </summary>
    /// <param name="id">ID do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Contato encontrado ou null</returns>
    public async Task<ContactDto?> GetContactByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contact = await _context.Contacts
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, cancellationToken);

        return contact != null ? _mapper.Map<ContactDto>(contact) : null;
    }

    /// <summary>
    /// Obtém contato principal de uma categoria
    /// </summary>
    /// <param name="entityId">ID da entidade</param>
    /// <param name="entityType">Tipo da entidade</param>
    /// <param name="category">Categoria do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Contato principal ou null</returns>
    public async Task<ContactDto?> GetPrimaryContactAsync(Guid entityId, string entityType, ContactCategory category, CancellationToken cancellationToken = default)
    {
        var contact = await _context.Contacts
            .FirstOrDefaultAsync(c => c.EntityId == entityId && c.EntityType == entityType &&
                                    c.Category == category && c.IsPrimary && !c.IsDeleted, cancellationToken);

        return contact != null ? _mapper.Map<ContactDto>(contact) : null;
    }

    /// <summary>
    /// Cria um novo contato
    /// </summary>
    /// <param name="createDto">Dados para criação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Contato criado</returns>
    public async Task<ContactDto> CreateContactAsync(CreateContactDto createDto, CancellationToken cancellationToken = default)
    {
        // Validar dados
        var validationErrors = await ValidateContactAsync(createDto, cancellationToken);
        if (validationErrors.Any())
        {
            throw new InvalidOperationException($"Erros de validação: {string.Join(", ", validationErrors)}");
        }

        var contact = _mapper.Map<Contact>(createDto);
        contact.CreatedAtUtc = DateTime.UtcNow;
        contact.UpdatedAtUtc = DateTime.UtcNow;

        _context.Contacts.Add(contact);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ContactDto>(contact);
    }

    /// <summary>
    /// Atualiza um contato existente
    /// </summary>
    /// <param name="updateDto">Dados para atualização</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Contato atualizado</returns>
    public async Task<ContactDto> UpdateContactAsync(UpdateContactDto updateDto, CancellationToken cancellationToken = default)
    {
        // Validar dados
        var validationErrors = await ValidateContactAsync(updateDto, cancellationToken);
        if (validationErrors.Any())
        {
            throw new InvalidOperationException($"Erros de validação: {string.Join(", ", validationErrors)}");
        }

        var contact = await _context.Contacts
            .FirstOrDefaultAsync(c => c.Id == updateDto.Id && !c.IsDeleted, cancellationToken);

        if (contact == null)
        {
            throw new InvalidOperationException("Contato não encontrado");
        }

        _mapper.Map(updateDto, contact);
        contact.UpdatedAtUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ContactDto>(contact);
    }

    /// <summary>
    /// Remove um contato (soft delete)
    /// </summary>
    /// <param name="id">ID do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se removido com sucesso</returns>
    public async Task<bool> DeleteContactAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contact = await _context.Contacts
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, cancellationToken);

        if (contact == null)
        {
            return false;
        }

        contact.IsDeleted = true;
        contact.DeletedAtUtc = DateTime.UtcNow;
        contact.UpdatedAtUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <summary>
    /// Define um contato como principal
    /// </summary>
    /// <param name="id">ID do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se definido com sucesso</returns>
    public async Task<bool> SetAsPrimaryAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contact = await _context.Contacts
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, cancellationToken);

        if (contact == null)
        {
            return false;
        }

        // Verificar se pode ser principal
        if (!contact.CanBePrimary())
        {
            return false;
        }

        // Remover outros contatos principais da mesma categoria
        var otherPrimaryContacts = await _context.Contacts
            .Where(c => c.EntityId == contact.EntityId &&
                       c.EntityType == contact.EntityType &&
                       c.Category == contact.Category &&
                       c.IsPrimary &&
                       c.Id != contact.Id &&
                       !c.IsDeleted)
            .ToListAsync(cancellationToken);

        foreach (var otherContact in otherPrimaryContacts)
        {
            otherContact.IsPrimary = false;
            otherContact.UpdatedAtUtc = DateTime.UtcNow;
        }

        // Definir como principal
        contact.SetAsPrimary();
        contact.UpdatedAtUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <summary>
    /// Remove a definição de contato principal
    /// </summary>
    /// <param name="id">ID do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se removido com sucesso</returns>
    public async Task<bool> RemovePrimaryAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contact = await _context.Contacts
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, cancellationToken);

        if (contact == null || !contact.IsPrimary)
        {
            return false;
        }

        contact.RemovePrimary();
        contact.UpdatedAtUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <summary>
    /// Ativa um contato
    /// </summary>
    /// <param name="id">ID do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se ativado com sucesso</returns>
    public async Task<bool> ActivateContactAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contact = await _context.Contacts
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, cancellationToken);

        if (contact == null)
        {
            return false;
        }

        contact.Activate();
        contact.UpdatedAtUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <summary>
    /// Desativa um contato
    /// </summary>
    /// <param name="id">ID do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se desativado com sucesso</returns>
    public async Task<bool> DeactivateContactAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contact = await _context.Contacts
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, cancellationToken);

        if (contact == null)
        {
            return false;
        }

        contact.Deactivate();
        contact.UpdatedAtUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <summary>
    /// Marca um contato como verificado
    /// </summary>
    /// <param name="id">ID do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se marcado com sucesso</returns>
    public async Task<bool> MarkAsVerifiedAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contact = await _context.Contacts
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, cancellationToken);

        if (contact == null)
        {
            return false;
        }

        contact.MarkAsVerified();
        contact.UpdatedAtUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <summary>
    /// Marca um contato como não verificado
    /// </summary>
    /// <param name="id">ID do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se marcado com sucesso</returns>
    public async Task<bool> MarkAsUnverifiedAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contact = await _context.Contacts
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, cancellationToken);

        if (contact == null)
        {
            return false;
        }

        contact.MarkAsUnverified();
        contact.UpdatedAtUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <summary>
    /// Valida se um contato pode ser criado
    /// </summary>
    /// <param name="createDto">Dados para criação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de erros de validação</returns>
    public async Task<IEnumerable<string>> ValidateContactAsync(CreateContactDto createDto, CancellationToken cancellationToken = default)
    {
        var errors = new List<string>();

        // Validar se a entidade existe
        var entityExists = await _validationService.ValidateEntityExistsAsync(createDto.EntityId, createDto.EntityType, cancellationToken);
        if (!entityExists)
        {
            errors.Add("A entidade especificada não existe");
        }

        // Validar se o tipo de entidade é válido
        if (!_validationService.ValidateEntityCanHaveContacts(createDto.EntityType))
        {
            errors.Add("Tipo de entidade inválido");
        }

        // Validar se o valor é único
        var isUnique = await _validationService.ValidateContactValueUniqueAsync(null, createDto.EntityId, createDto.EntityType, createDto.Type, createDto.Value, cancellationToken);
        if (!isUnique)
        {
            errors.Add("Já existe um contato com este valor para esta entidade");
        }

        // Validar se pode ser principal
        if (createDto.IsPrimary)
        {
            var canBePrimary = await _validationService.ValidatePrimaryContactAsync(null, createDto.EntityId, createDto.EntityType, createDto.Category, true, cancellationToken);
            if (!canBePrimary)
            {
                errors.Add("Já existe um contato principal para esta categoria");
            }
        }

        // Validar valor do contato
        var contact = _mapper.Map<Contact>(createDto);
        if (!contact.IsValid())
        {
            errors.AddRange(contact.GetValidationErrors());
        }

        return errors;
    }

    /// <summary>
    /// Valida se um contato pode ser atualizado
    /// </summary>
    /// <param name="updateDto">Dados para atualização</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de erros de validação</returns>
    public async Task<IEnumerable<string>> ValidateContactAsync(UpdateContactDto updateDto, CancellationToken cancellationToken = default)
    {
        var errors = new List<string>();

        // Validar se a entidade existe
        var entityExists = await _validationService.ValidateEntityExistsAsync(updateDto.EntityId, updateDto.EntityType, cancellationToken);
        if (!entityExists)
        {
            errors.Add("A entidade especificada não existe");
        }

        // Validar se o tipo de entidade é válido
        if (!_validationService.ValidateEntityCanHaveContacts(updateDto.EntityType))
        {
            errors.Add("Tipo de entidade inválido");
        }

        // Validar se o valor é único
        var isUnique = await _validationService.ValidateContactValueUniqueAsync(updateDto.Id, updateDto.EntityId, updateDto.EntityType, updateDto.Type, updateDto.Value, cancellationToken);
        if (!isUnique)
        {
            errors.Add("Já existe um contato com este valor para esta entidade");
        }

        // Validar se pode ser principal
        if (updateDto.IsPrimary)
        {
            var canBePrimary = await _validationService.ValidatePrimaryContactAsync(updateDto.Id, updateDto.EntityId, updateDto.EntityType, updateDto.Category, true, cancellationToken);
            if (!canBePrimary)
            {
                errors.Add("Já existe um contato principal para esta categoria");
            }
        }

        // Validar valor do contato
        var contact = _mapper.Map<Contact>(updateDto);
        if (!contact.IsValid())
        {
            errors.AddRange(contact.GetValidationErrors());
        }

        return errors;
    }
}
