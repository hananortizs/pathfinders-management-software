using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Pms.Backend.Application.DTOs.Auth;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Members;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pms.Backend.Application.Services.Auth;

/// <summary>
/// Serviço de autenticação JWT
/// </summary>
public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AuthService> _logger;
    private readonly IMemberValidationService _validationService;

    /// <summary>
    /// Inicializa uma nova instância do AuthService
    /// </summary>
    /// <param name="configuration">Configuração da aplicação</param>
    /// <param name="unitOfWork">Unit of Work</param>
    /// <param name="logger">Logger</param>
    /// <param name="validationService">Serviço de validação de membros</param>
    public AuthService(
        IConfiguration configuration,
        IUnitOfWork unitOfWork,
        ILogger<AuthService> logger,
        IMemberValidationService validationService)
    {
        _configuration = configuration;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _validationService = validationService;
    }

    /// <summary>
    /// Autentica um usuário com e-mail e senha
    /// </summary>
    /// <param name="request">Dados de login</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resposta de login com token JWT</returns>
    public async Task<BaseResponse<DTOs.Auth.LoginResponseDto>> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Tentativa de login para e-mail: {Email}", request.Email);

            // Buscar usuário por e-mail através dos contatos
            // Primeiro buscar contatos de email que correspondem
            var emailContacts = await _unitOfWork.Repository<Contact>()
                .GetAsync(c => c.Type == Domain.Enums.ContactType.Email && c.Value == request.Email && c.EntityType == "Member", cancellationToken);

            if (!emailContacts.Any())
            {
                _logger.LogWarning("Nenhum contato de email encontrado para: {Email}", request.Email);
                return BaseResponse<LoginResponseDto>.UnauthorizedResult("E-mail ou senha inválidos");
            }

            var memberIds = emailContacts.Select(c => c.EntityId).ToList();
            var members = await _unitOfWork.Repository<Member>()
                .GetAsync(m => memberIds.Contains(m.Id), cancellationToken);

            var member = members.FirstOrDefault();
            if (member == null)
            {
                _logger.LogWarning("Usuário não encontrado para e-mail: {Email}. Total de membros encontrados: {Count}", request.Email, members.Count());
                return BaseResponse<LoginResponseDto>.UnauthorizedResult("E-mail ou senha inválidos");
            }

            _logger.LogInformation("Usuário encontrado: {MemberId} - {FirstName} {LastName}", member.Id, member.FirstName, member.LastName);

            // Buscar credenciais do usuário
            var userCredentials = await _unitOfWork.Repository<UserCredential>()
                .GetAsync(uc => uc.MemberId == member.Id, cancellationToken);

            var userCredential = userCredentials.FirstOrDefault();
            if (userCredential == null)
            {
                _logger.LogWarning("Credenciais não encontradas para e-mail: {Email}", request.Email);
                return BaseResponse<LoginResponseDto>.UnauthorizedResult("E-mail ou senha inválidos");
            }

            // Verificar se o usuário pode fazer login baseado no status
            if (member.Status != Domain.Entities.MemberStatus.Pending && !userCredential.IsActive)
            {
                _logger.LogWarning("Conta inativa para e-mail: {Email}. Status: {Status}", request.Email, member.Status);
                return BaseResponse<LoginResponseDto>.ForbiddenResult("Conta inativa. Entre em contato com o administrador.");
            }

            // Se o usuário tem status Pending, validar dados para informar o que está pendente
            MemberValidationResult? validationResult = null;
            if (member.Status == Domain.Entities.MemberStatus.Pending)
            {
                validationResult = await _validationService.ValidateMemberDataAsync(member, cancellationToken);
                _logger.LogInformation("Usuário com status Pending. Dados pendentes: {PendingDataTypes}",
                    string.Join(", ", validationResult.PendingDataTypes));
            }

            // Verificar senha
            if (!BCrypt.Net.BCrypt.Verify(request.Password, userCredential.PasswordHash))
            {
                _logger.LogWarning("Senha incorreta para e-mail: {Email}", request.Email);
                return BaseResponse<LoginResponseDto>.UnauthorizedResult("E-mail ou senha inválidos");
            }

            // Buscar papéis e escopos do usuário
            var assignments = await _unitOfWork.Repository<Assignment>()
                .GetAsync(a => a.MemberId == member.Id && a.IsActive, cancellationToken);

            // Usar o campo Role diretamente em vez de RoleCatalog.Name
            var roles = assignments.Select(a => a.Role).ToList();
            var scopes = assignments.Select(a => $"{a.ScopeType}:{a.ScopeId}").ToList();

            // Criar informações do usuário
            var userInfo = new UserInfoDto
            {
                Id = member.Id,
                Email = request.Email,
                FirstName = member.FirstName,
                LastName = member.LastName,
                Roles = roles,
                Scopes = scopes,
                IsActive = userCredential.IsActive,
                CreatedAtUtc = member.CreatedAtUtc,
                UpdatedAtUtc = member.UpdatedAtUtc
            };

            // Gerar token JWT
            var token = GenerateToken(userInfo);
            var expiresIn = _configuration.GetValue<int>("Jwt:ExpirationMinutes") * 60;

            // Criar informações básicas do membro
            var memberBasicInfo = new MemberBasicInfoDto
            {
                Id = member.Id,
                Status = member.Status.ToString()
            };

            // Criar dados pendentes se necessário
            PendingDataDto? pendingData = null;
            if (member.Status == Domain.Entities.MemberStatus.Pending && validationResult != null)
            {
                var activationChecklist = new ActivationChecklistDto
                {
                    HasCompleteAddress = validationResult.PendingDataTypes.Contains("Address"),
                    HasContactEmail = validationResult.PendingDataTypes.Contains("ContactEmail"),
                    HasContactMobile = validationResult.PendingDataTypes.Contains("ContactMobile")
                };

                pendingData = new PendingDataDto
                {
                    ActivationRequired = activationChecklist.GetMissingActivationFields(),
                    OperationRequired = activationChecklist.GetMissingOperationFields(
                        validationResult.PendingDataTypes.Contains("ScarfInvestiture"),
                        validationResult.PendingDataTypes.Contains("BaptismInfo")
                    ),
                    Optional = activationChecklist.GetOptionalFields(
                        validationResult.PendingDataTypes.Contains("MedicalInfo")
                    ),
                    BlockingWrites = true
                };
            }

            var response = new DTOs.Auth.LoginResponseDto
            {
                AccessToken = token,
                TokenType = "Bearer",
                ExpiresIn = expiresIn,
                Member = memberBasicInfo,
                Pending = pendingData
            };

            _logger.LogInformation("Login bem-sucedido para usuário: {UserId}", member.Id);
            return BaseResponse<LoginResponseDto>.SuccessResult(response, "Login realizado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro durante o processo de login para e-mail: {Email}", request.Email);
            return BaseResponse<LoginResponseDto>.InternalServerErrorResult("Erro interno do servidor", true);
        }
    }

    /// <summary>
    /// Valida um token JWT
    /// </summary>
    /// <param name="token">Token JWT</param>
    /// <returns>True se o token for válido</returns>
    public bool ValidateToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]!);
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            _logger.LogDebug("Validando token JWT com Issuer: {Issuer}, Audience: {Audience}", issuer, audience);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            _logger.LogDebug("Token JWT válido");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Token JWT inválido: {Error}", ex.Message);
            return false;
        }
    }

    /// <summary>
    /// Obtém informações do usuário a partir do token JWT
    /// </summary>
    /// <param name="token">Token JWT</param>
    /// <returns>Informações do usuário</returns>
    public UserInfoDto? GetUserFromToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            var userIdClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "sub");
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                return null;

            var emailClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "email");
            var firstNameClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "given_name");
            var lastNameClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "family_name");
            var rolesClaim = jwtToken.Claims.Where(x => x.Type == "role").Select(x => x.Value).ToList();
            var scopesClaim = jwtToken.Claims.Where(x => x.Type == "scope").Select(x => x.Value).ToList();
            var isActiveClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "is_active");
            var createdAtClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "created_at");
            var updatedAtClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "updated_at");

            return new UserInfoDto
            {
                Id = userId,
                Email = emailClaim?.Value ?? string.Empty,
                FirstName = firstNameClaim?.Value ?? string.Empty,
                LastName = lastNameClaim?.Value ?? string.Empty,
                Roles = rolesClaim,
                Scopes = scopesClaim,
                IsActive = bool.TryParse(isActiveClaim?.Value, out var isActive) && isActive,
                CreatedAtUtc = DateTime.TryParse(createdAtClaim?.Value, out var createdAt) ? createdAt : DateTime.UtcNow,
                UpdatedAtUtc = DateTime.TryParse(updatedAtClaim?.Value, out var updatedAt) ? updatedAt : DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Erro ao extrair informações do usuário do token JWT");
            return null;
        }
    }

    /// <summary>
    /// Gera um token JWT para um usuário
    /// </summary>
    /// <param name="user">Informações do usuário</param>
    /// <returns>Token JWT</returns>
    public string GenerateToken(UserInfoDto user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]!);
        var expiresIn = _configuration.GetValue<int>("Jwt:ExpirationMinutes");

        var claims = new List<Claim>
        {
            new("sub", user.Id.ToString()),
            new("email", user.Email),
            new("given_name", user.FirstName),
            new("family_name", user.LastName),
            new("name", user.FullName),
            new("is_active", user.IsActive.ToString()),
            new("created_at", user.CreatedAtUtc.ToString("O")),
            new("updated_at", user.UpdatedAtUtc.ToString("O"))
        };

        // Adicionar papéis
        foreach (var role in user.Roles)
        {
            claims.Add(new Claim("role", role));
        }

        // Adicionar escopos
        foreach (var scope in user.Scopes)
        {
            claims.Add(new Claim("scope", scope));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expiresIn),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// Atualiza o token de um usuário
    /// </summary>
    /// <param name="token">Token atual</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Novo token JWT</returns>
    public async Task<BaseResponse<LoginResponseDto>> RefreshTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!ValidateToken(token))
            {
                return BaseResponse<LoginResponseDto>.UnauthorizedResult("Token inválido");
            }

            var userInfo = GetUserFromToken(token);
            if (userInfo == null)
            {
                return BaseResponse<LoginResponseDto>.UnauthorizedResult("Não foi possível extrair informações do usuário");
            }

            // Verificar se o usuário ainda está ativo
            var member = await _unitOfWork.Repository<Member>().GetByIdAsync(userInfo.Id, cancellationToken);
            if (member == null)
            {
                return BaseResponse<LoginResponseDto>.NotFoundResult("Usuário não encontrado");
            }

            var userCredentials = await _unitOfWork.Repository<UserCredential>()
                .GetAsync(uc => uc.MemberId == member.Id, cancellationToken);

            var userCredential = userCredentials.FirstOrDefault();
            if (userCredential == null || !userCredential.IsActive)
            {
                return BaseResponse<LoginResponseDto>.ForbiddenResult("Usuário inativo");
            }

            // Atualizar informações do usuário
            userInfo.UpdatedAtUtc = member.UpdatedAtUtc;

            // Gerar novo token
            var newToken = GenerateToken(userInfo);
            var expiresIn = _configuration.GetValue<int>("Jwt:ExpirationMinutes") * 60;

            var response = new DTOs.Auth.LoginResponseDto
            {
                AccessToken = newToken,
                TokenType = "Bearer",
                ExpiresIn = expiresIn
            };

            return BaseResponse<LoginResponseDto>.SuccessResult(response, "Token atualizado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar token JWT");
            return BaseResponse<LoginResponseDto>.InternalServerErrorResult("Erro interno do servidor", true);
        }
    }

    /// <summary>
    /// Valida um token JWT e retorna informações do usuário
    /// </summary>
    /// <param name="token">Token JWT</param>
    /// <returns>Resposta com informações do usuário</returns>
    public BaseResponse<object> ValidateTokenWithUserInfo(string token)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return BaseResponse<object>.UnprocessableEntityResult("Token é obrigatório");
            }

            var isValid = ValidateToken(token);
            if (!isValid)
            {
                return BaseResponse<object>.UnauthorizedResult("Token inválido");
            }

            var userInfo = GetUserFromToken(token);
            if (userInfo == null)
            {
                return BaseResponse<object>.UnauthorizedResult("Não foi possível extrair informações do usuário");
            }

            return BaseResponse<object>.SuccessResult(new { IsValid = true, User = userInfo }, "Token válido");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao validar token JWT");
            return BaseResponse<object>.InternalServerErrorResult("Erro interno do servidor", true);
        }
    }

    /// <summary>
    /// Obtém informações do usuário a partir do token com tratamento de erro
    /// </summary>
    /// <param name="token">Token JWT</param>
    /// <returns>Resposta com informações do usuário</returns>
    public BaseResponse<UserInfoDto> GetUserInfoFromToken(string token)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return BaseResponse<UserInfoDto>.UnprocessableEntityResult("Token é obrigatório");
            }

            if (!ValidateToken(token))
            {
                return BaseResponse<UserInfoDto>.UnauthorizedResult("Token inválido");
            }

            var userInfo = GetUserFromToken(token);
            if (userInfo == null)
            {
                return BaseResponse<UserInfoDto>.UnauthorizedResult("Não foi possível extrair informações do usuário");
            }

            return BaseResponse<UserInfoDto>.SuccessResult(userInfo, "Informações do usuário obtidas com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter informações do usuário");
            return BaseResponse<UserInfoDto>.InternalServerErrorResult("Erro interno do servidor", true);
        }
    }

}
