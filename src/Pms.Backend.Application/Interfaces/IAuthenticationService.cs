using Pms.Backend.Application.DTOs.Auth;
using Pms.Backend.Application.DTOs;

namespace Pms.Backend.Application.Interfaces;

/// <summary>
/// Interface para serviços de autenticação
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Autentica um usuário com e-mail e senha
    /// </summary>
    /// <param name="request">Dados de login</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resposta de login com token JWT</returns>
    Task<BaseResponse<LoginResponseDto>> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Valida um token JWT
    /// </summary>
    /// <param name="token">Token JWT</param>
    /// <returns>True se o token for válido</returns>
    bool ValidateToken(string token);

    /// <summary>
    /// Obtém informações do usuário a partir do token JWT
    /// </summary>
    /// <param name="token">Token JWT</param>
    /// <returns>Informações do usuário</returns>
    UserInfoDto? GetUserFromToken(string token);

    /// <summary>
    /// Gera um token JWT para um usuário
    /// </summary>
    /// <param name="user">Informações do usuário</param>
    /// <returns>Token JWT</returns>
    string GenerateToken(UserInfoDto user);

    /// <summary>
    /// Atualiza o token de um usuário
    /// </summary>
    /// <param name="token">Token atual</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Novo token JWT</returns>
    Task<BaseResponse<LoginResponseDto>> RefreshTokenAsync(string token, CancellationToken cancellationToken = default);

    /// <summary>
    /// Valida um token JWT e retorna informações do usuário
    /// </summary>
    /// <param name="token">Token JWT</param>
    /// <returns>Resposta com informações do usuário</returns>
    BaseResponse<object> ValidateTokenWithUserInfo(string token);

    /// <summary>
    /// Obtém informações do usuário a partir do token com tratamento de erro
    /// </summary>
    /// <param name="token">Token JWT</param>
    /// <returns>Resposta com informações do usuário</returns>
    BaseResponse<UserInfoDto> GetUserInfoFromToken(string token);

}
