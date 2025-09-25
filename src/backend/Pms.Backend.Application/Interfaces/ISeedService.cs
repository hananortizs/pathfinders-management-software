using Pms.Backend.Application.DTOs;

namespace Pms.Backend.Application.Interfaces;

/// <summary>
/// Interface para serviços de seeds e dados iniciais
/// </summary>
public interface ISeedService
{
    /// <summary>
    /// Cria o SystemAdmin inicial
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da operação</returns>
    Task<BaseResponse<bool>> CreateSystemAdminAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Cria a hierarquia inicial (Division, Union, Association, Region, District)
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da operação</returns>
    Task<BaseResponse<bool>> CreateInitialHierarchyAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Cria o usuário Hanan Del Chiaro como SystemAdmin e Distrital
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da operação</returns>
    Task<BaseResponse<bool>> CreateHananUserAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Cria a unidade Falcão para o clube Pássaro Celeste
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da operação</returns>
    Task<BaseResponse<bool>> CreateFalconUnitAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Cria o membro Marcelo Martins como diretor do clube Pássaro Celeste
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da operação</returns>
    Task<BaseResponse<bool>> CreateMarceloDirectorAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Cria o membro Ricardo Ferreira Ortiz Gonzaga e associa à unidade Falcão
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da operação</returns>
    Task<BaseResponse<bool>> CreateRicardoMemberAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Executa todos os seeds necessários para o MVP0
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da operação</returns>
    Task<BaseResponse<bool>> SeedAllAsync(CancellationToken cancellationToken = default);
}
