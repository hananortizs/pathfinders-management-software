using Pms.Backend.Application.DTOs.Contacts;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Domain.Enums;

namespace Pms.Backend.Application.Interfaces;

/// <summary>
/// Interface para serviços de contato
/// </summary>
public interface IContactService
{
    /// <summary>
    /// Obtém todos os contatos de uma entidade
    /// </summary>
    /// <param name="entityId">ID da entidade</param>
    /// <param name="entityType">Tipo da entidade</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de contatos da entidade</returns>
    Task<IEnumerable<ContactDto>> GetContactsByEntityAsync(Guid entityId, string entityType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém contatos por categoria
    /// </summary>
    /// <param name="entityId">ID da entidade</param>
    /// <param name="entityType">Tipo da entidade</param>
    /// <param name="category">Categoria do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de contatos da categoria</returns>
    Task<IEnumerable<ContactDto>> GetContactsByCategoryAsync(Guid entityId, string entityType, ContactCategory category, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém contatos por tipo
    /// </summary>
    /// <param name="entityId">ID da entidade</param>
    /// <param name="entityType">Tipo da entidade</param>
    /// <param name="type">Tipo do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de contatos do tipo</returns>
    Task<IEnumerable<ContactDto>> GetContactsByTypeAsync(Guid entityId, string entityType, ContactType type, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém contato por ID
    /// </summary>
    /// <param name="id">ID do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Contato encontrado ou null</returns>
    Task<ContactDto?> GetContactByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém contato principal de uma categoria
    /// </summary>
    /// <param name="entityId">ID da entidade</param>
    /// <param name="entityType">Tipo da entidade</param>
    /// <param name="category">Categoria do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Contato principal ou null</returns>
    Task<ContactDto?> GetPrimaryContactAsync(Guid entityId, string entityType, ContactCategory category, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cria um novo contato
    /// </summary>
    /// <param name="createDto">Dados para criação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Contato criado</returns>
    Task<ContactDto> CreateContactAsync(CreateContactDto createDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza um contato existente
    /// </summary>
    /// <param name="updateDto">Dados para atualização</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Contato atualizado</returns>
    Task<ContactDto> UpdateContactAsync(UpdateContactDto updateDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove um contato (soft delete)
    /// </summary>
    /// <param name="id">ID do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se removido com sucesso</returns>
    Task<bool> DeleteContactAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Define um contato como principal
    /// </summary>
    /// <param name="id">ID do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se definido com sucesso</returns>
    Task<bool> SetAsPrimaryAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove a definição de contato principal
    /// </summary>
    /// <param name="id">ID do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se removido com sucesso</returns>
    Task<bool> RemovePrimaryAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ativa um contato
    /// </summary>
    /// <param name="id">ID do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se ativado com sucesso</returns>
    Task<bool> ActivateContactAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Desativa um contato
    /// </summary>
    /// <param name="id">ID do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se desativado com sucesso</returns>
    Task<bool> DeactivateContactAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Marca um contato como verificado
    /// </summary>
    /// <param name="id">ID do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se marcado com sucesso</returns>
    Task<bool> MarkAsVerifiedAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Marca um contato como não verificado
    /// </summary>
    /// <param name="id">ID do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se marcado com sucesso</returns>
    Task<bool> MarkAsUnverifiedAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Valida se um contato pode ser criado
    /// </summary>
    /// <param name="createDto">Dados para criação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de erros de validação</returns>
    Task<IEnumerable<string>> ValidateContactAsync(CreateContactDto createDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Valida se um contato pode ser atualizado
    /// </summary>
    /// <param name="updateDto">Dados para atualização</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de erros de validação</returns>
    Task<IEnumerable<string>> ValidateContactAsync(UpdateContactDto updateDto, CancellationToken cancellationToken = default);
}
