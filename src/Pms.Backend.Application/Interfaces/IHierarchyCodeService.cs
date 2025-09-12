using Pms.Backend.Application.DTOs;

namespace Pms.Backend.Application.Interfaces;

/// <summary>
/// Interface para serviços de códigos hierárquicos
/// </summary>
public interface IHierarchyCodeService
{
    /// <summary>
    /// Gera um código único para uma entidade hierárquica
    /// </summary>
    /// <param name="entityType">Tipo da entidade</param>
    /// <param name="parentCode">Código da entidade pai (opcional)</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Código único gerado</returns>
    Task<BaseResponse<string>> GenerateUniqueCodeAsync(string entityType, string? parentCode = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Valida se um código é único para uma entidade
    /// </summary>
    /// <param name="code">Código a ser validado</param>
    /// <param name="entityType">Tipo da entidade</param>
    /// <param name="entityId">ID da entidade (opcional, para exclusão na validação)</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se o código for único</returns>
    Task<BaseResponse<bool>> ValidateCodeUniquenessAsync(string code, string entityType, Guid? entityId = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gera o CodePath para uma entidade
    /// </summary>
    /// <param name="entityType">Tipo da entidade</param>
    /// <param name="entityId">ID da entidade</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>CodePath gerado</returns>
    Task<BaseResponse<string>> GenerateCodePathAsync(string entityType, Guid entityId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza o CodePath de uma entidade e recalcula em cascata
    /// </summary>
    /// <param name="entityType">Tipo da entidade</param>
    /// <param name="entityId">ID da entidade</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da operação</returns>
    Task<BaseResponse<bool>> UpdateCodePathAsync(string entityType, Guid entityId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Recalcula todos os CodePaths de uma hierarquia
    /// </summary>
    /// <param name="rootEntityType">Tipo da entidade raiz</param>
    /// <param name="rootEntityId">ID da entidade raiz</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da operação</returns>
    Task<BaseResponse<bool>> RecalculateHierarchyCodePathsAsync(string rootEntityType, Guid rootEntityId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém o código de uma entidade
    /// </summary>
    /// <param name="entityType">Tipo da entidade</param>
    /// <param name="entityId">ID da entidade</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Código da entidade</returns>
    Task<BaseResponse<string>> GetEntityCodeAsync(string entityType, Guid entityId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém o CodePath de uma entidade
    /// </summary>
    /// <param name="entityType">Tipo da entidade</param>
    /// <param name="entityId">ID da entidade</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>CodePath da entidade</returns>
    Task<BaseResponse<string>> GetEntityCodePathAsync(string entityType, Guid entityId, CancellationToken cancellationToken = default);
}
