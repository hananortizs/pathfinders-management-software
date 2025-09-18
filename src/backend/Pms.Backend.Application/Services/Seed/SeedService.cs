using Microsoft.Extensions.Logging;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Domain.Entities.Hierarchy;
using Pms.Backend.Domain.Enums;
using System.Text;

namespace Pms.Backend.Application.Services.Seed;

/// <summary>
/// Serviço para seeds e dados iniciais
/// </summary>
public class SeedService : ISeedService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SeedService> _logger;
    private readonly IHierarchyCodeService _hierarchyCodeService;

    /// <summary>
    /// Inicializa uma nova instância do SeedService
    /// </summary>
    /// <param name="unitOfWork">Unit of Work</param>
    /// <param name="logger">Logger</param>
    /// <param name="hierarchyCodeService">Serviço de códigos hierárquicos</param>
    public SeedService(IUnitOfWork unitOfWork, ILogger<SeedService> logger, IHierarchyCodeService hierarchyCodeService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _hierarchyCodeService = hierarchyCodeService;
    }

    /// <summary>
    /// Cria o SystemAdmin inicial
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da operação</returns>
    public async Task<BaseResponse<bool>> CreateSystemAdminAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Criando SystemAdmin inicial...");

            // Verificar se já existe um SystemAdmin
            var existingSystemAdmin = await _unitOfWork.Repository<Member>()
                .GetAsync(m => m.Contacts.Any(c => c.Type == ContactType.Email && c.Value == "system@pms.com"), cancellationToken);

            if (existingSystemAdmin.Any())
            {
                _logger.LogInformation("SystemAdmin já existe, pulando criação");
                return BaseResponse<bool>.SuccessResult(true, "SystemAdmin já existe");
            }

            // Criar SystemAdmin
            var systemAdmin = new Member
            {
                FirstName = "System",
                LastName = "Administrator",
                DateOfBirth = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Gender = MemberGender.Male,
                Cpf = "00000000000",
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow
            };

            // Adicionar contato de email
            systemAdmin.Contacts.Add(new Contact
            {
                Type = ContactType.Email,
                Value = "system@pms.com",
                EntityId = systemAdmin.Id,
                EntityType = "Member",
                IsPrimary = true,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow
            });

            // Adicionar credenciais de login
            var userCredential = new UserCredential
            {
                MemberId = systemAdmin.Id,
                Email = "system@pms.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("SystemAdmin123!"),
                IsEmailVerified = true,
                IsActive = true,
                FailedLoginAttempts = 0,
                LockedOutUntilUtc = null,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow
            };

            // Adicionar role de SystemAdmin
            var systemAdminRole = await GetOrCreateRole("SystemAdmin", cancellationToken);
            var systemAdminAssignment = new Assignment
            {
                MemberId = systemAdmin.Id,
                RoleId = systemAdminRole.Id,
                Role = "SystemAdmin",
                ScopeType = ScopeType.Division, // Maior nível da hierarquia
                ScopeId = Guid.Empty,
                StartDate = DateTime.UtcNow,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow
            };

            await _unitOfWork.Repository<Member>().AddAsync(systemAdmin, cancellationToken);
            await _unitOfWork.Repository<UserCredential>().AddAsync(userCredential, cancellationToken);
            await _unitOfWork.Repository<Assignment>().AddAsync(systemAdminAssignment, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("SystemAdmin criado com sucesso");
            return BaseResponse<bool>.SuccessResult(true, "SystemAdmin criado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar SystemAdmin");
            return BaseResponse<bool>.ErrorResult("Erro ao criar SystemAdmin");
        }
    }

    /// <summary>
    /// Cria a hierarquia inicial (Division, Union, Association, Region, District)
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da operação</returns>
    public async Task<BaseResponse<bool>> CreateInitialHierarchyAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Criando hierarquia inicial...");

            // Verificar se já existe uma divisão
            var existingDivision = await _unitOfWork.Repository<Division>()
                .GetAsync(d => d.Code == "DIV01", cancellationToken);

            if (existingDivision.Any())
            {
                _logger.LogInformation("Hierarquia já existe, pulando criação");
                return BaseResponse<bool>.SuccessResult(true, "Hierarquia já existe");
            }

            // Criar Division
            var division = new Division
            {
                Code = "DIV01",
                Name = "Divisão Sul Americana",
                Description = "Divisão Sul Americana da Igreja Adventista do Sétimo Dia",
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow
            };

            // Criar Union
            var union = new Union
            {
                Code = "UN01",
                Name = "União Central Brasileira",
                Description = "União Central Brasileira da Igreja Adventista do Sétimo Dia",
                DivisionId = division.Id,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow
            };

            // Criar Association
            var association = new Association
            {
                Code = "APL",
                Name = "Associação Paulista Leste",
                Description = "Associação Paulista Leste da Igreja Adventista do Sétimo Dia",
                UnionId = union.Id,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow
            };

            // Criar Region
            var region = new Region
            {
                Code = "3APL",
                Name = "3ª Região - Tigre",
                Description = "3ª Região - Tigre da Associação Paulista Leste",
                AssociationId = association.Id,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow
            };

            // Criar District
            var district = new District
            {
                Code = "DVM",
                Name = "Distrito de Vila Medeiros",
                Description = "Distrito de Vila Medeiros da 3ª Região - Tigre",
                RegionId = region.Id,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow
            };

            // Salvar entidades
            await _unitOfWork.Repository<Division>().AddAsync(division, cancellationToken);
            await _unitOfWork.Repository<Union>().AddAsync(union, cancellationToken);
            await _unitOfWork.Repository<Association>().AddAsync(association, cancellationToken);
            await _unitOfWork.Repository<Region>().AddAsync(region, cancellationToken);
            await _unitOfWork.Repository<District>().AddAsync(district, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Hierarquia inicial criada com sucesso");
            return BaseResponse<bool>.SuccessResult(true, "Hierarquia inicial criada com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar hierarquia inicial");
            return BaseResponse<bool>.ErrorResult("Erro ao criar hierarquia inicial");
        }
    }

    /// <summary>
    /// Cria o usuário Hanan Del Chiaro como SystemAdmin e Distrital
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da operação</returns>
    public async Task<BaseResponse<bool>> CreateHananUserAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Criando usuário Hanan Del Chiaro...");

            // Verificar se já existe por email
            var existingHananByEmail = await _unitOfWork.Repository<Member>()
                .GetAsync(m => m.Contacts.Any(c => c.Type == ContactType.Email && c.Value == "hanan.ortiz.hnnrtz@gmail.com"), cancellationToken);

            if (existingHananByEmail.Any())
            {
                _logger.LogInformation("Usuário Hanan já existe (por email), pulando criação");
                return BaseResponse<bool>.SuccessResult(true, "Usuário Hanan já existe");
            }

            // Verificar se já existe por nome completo (backup)
            var existingHananByName = await _unitOfWork.Repository<Member>()
                .GetAsync(m => m.FirstName == "Hanan" && m.LastName == "Del Chiaro", cancellationToken);

            if (existingHananByName.Any())
            {
                _logger.LogInformation("Usuário Hanan já existe (por nome), pulando criação");
                return BaseResponse<bool>.SuccessResult(true, "Usuário Hanan já existe");
            }

            // Verificar se já existe UserCredential com o email
            var existingCredential = await _unitOfWork.Repository<UserCredential>()
                .GetAsync(uc => uc.Email == "hanan.ortiz.hnnrtz@gmail.com", cancellationToken);

            if (existingCredential.Any())
            {
                _logger.LogInformation("Credenciais do usuário Hanan já existem, pulando criação");
                return BaseResponse<bool>.SuccessResult(true, "Usuário Hanan já existe");
            }

            // Obter o distrito criado
            var district = await _unitOfWork.Repository<District>()
                .GetAsync(d => d.Code == "DVM", cancellationToken);
            var districtEntity = district.FirstOrDefault();

            if (districtEntity == null)
            {
                return BaseResponse<bool>.ErrorResult("Distrito não encontrado. Execute CreateInitialHierarchyAsync primeiro");
            }

            // Criar Hanan
            var hanan = new Member
            {
                FirstName = "Hanan",
                MiddleNames = "Ferreira Ortiz Spinola",
                LastName = "Del Chiaro",
                DateOfBirth = new DateTime(2002, 6, 14, 0, 0, 0, DateTimeKind.Utc), // Ajustar conforme necessário
                Gender = MemberGender.Male,
                Cpf = "44732518885", // Ajustar conforme necessário
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow
            };

            // Adicionar contato de email
            hanan.Contacts.Add(new Contact
            {
                Type = ContactType.Email,
                Value = "hanan.ortiz.hnnrtz@gmail.com",
                EntityId = hanan.Id,
                EntityType = "Member",
                IsPrimary = true,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow
            });

            // Adicionar contato de telefone
            hanan.Contacts.Add(new Contact
            {
                Type = ContactType.Mobile,
                Value = "+5511952404468",
                EntityId = hanan.Id,
                EntityType = "Member",
                IsPrimary = true,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow
            });

            // Adicionar credenciais de login
            var userCredential = new UserCredential
            {
                MemberId = hanan.Id,
                Email = "hanan.ortiz.hnnrtz@gmail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Hanan123!@"),
                IsEmailVerified = true,
                IsActive = true,
                FailedLoginAttempts = 0,
                LockedOutUntilUtc = null,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow
            };

            // Adicionar role de SystemAdmin
            var systemAdminRole = await GetOrCreateRole("SystemAdmin", cancellationToken);
            var systemAdminAssignment = new Assignment
            {
                MemberId = hanan.Id,
                RoleId = systemAdminRole.Id,
                Role = "SystemAdmin",
                ScopeType = ScopeType.Division, // Maior nível da hierarquia
                ScopeId = Guid.Empty,
                StartDate = DateTime.UtcNow,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow
            };

            // Adicionar role de Distrital
            var distritalRole = await GetOrCreateRole("Distrital", cancellationToken);
            var distritalAssignment = new Assignment
            {
                MemberId = hanan.Id,
                RoleId = distritalRole.Id,
                Role = "Distrital",
                ScopeType = ScopeType.District,
                ScopeId = districtEntity.Id,
                StartDate = DateTime.UtcNow,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow
            };

            // Verificação final antes de salvar - garantir que não há duplicação
            var finalCheck = await _unitOfWork.Repository<Member>()
                .GetAsync(m => m.FirstName == "Hanan" && m.LastName == "Del Chiaro", cancellationToken);

            if (finalCheck.Any())
            {
                _logger.LogInformation("Usuário Hanan foi criado por outro processo, pulando criação");
                return BaseResponse<bool>.SuccessResult(true, "Usuário Hanan já existe");
            }

            await _unitOfWork.Repository<Member>().AddAsync(hanan, cancellationToken);
            await _unitOfWork.Repository<UserCredential>().AddAsync(userCredential, cancellationToken);
            await _unitOfWork.Repository<Assignment>().AddAsync(systemAdminAssignment, cancellationToken);
            await _unitOfWork.Repository<Assignment>().AddAsync(distritalAssignment, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Usuário Hanan Del Chiaro criado com sucesso");
            return BaseResponse<bool>.SuccessResult(true, "Usuário Hanan Del Chiaro criado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar usuário Hanan Del Chiaro");
            return BaseResponse<bool>.ErrorResult("Erro ao criar usuário Hanan Del Chiaro");
        }
    }

    /// <summary>
    /// Cria a igreja IASD Jardim Brasil
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da operação</returns>
    public async Task<BaseResponse<bool>> CreateChurchAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Criando igreja IASD Jardim Brasil...");

            // Verificar se já existe
            if (await HasChurchAsync(cancellationToken))
            {
                _logger.LogInformation("Igreja IASD Jardim Brasil já existe, pulando criação");
                return BaseResponse<bool>.SuccessResult(true, "Igreja já existe");
            }

            // Obter o distrito DVM
            var district = await _unitOfWork.Repository<District>()
                .GetAsync(d => d.Code == "DVM", cancellationToken);
            var districtEntity = district.FirstOrDefault();

            if (districtEntity == null)
            {
                return BaseResponse<bool>.ErrorResult("Distrito DVM não encontrado. Execute CreateInitialHierarchyAsync primeiro");
            }

            // Criar a igreja
            var church = new Church
            {
                Name = "IASD Jardim Brasil",
                DistrictId = districtEntity.Id,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow
            };

            await _unitOfWork.Repository<Church>().AddAsync(church, cancellationToken);

            // Criar endereço da igreja
            var address = new Pms.Backend.Domain.Entities.Address
            {
                Street = "Av. Mendes da Rocha",
                Number = "528",
                Complement = "Jardim Brasil (Zona Norte)",
                Neighborhood = "Jardim Brasil",
                City = "São Paulo",
                State = "SP",
                Country = "Brasil",
                Cep = "02227000", // CEP sem formatação para o banco
                Type = AddressType.Church,
                IsPrimary = true,
                EntityId = church.Id,
                EntityType = "Church",
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow
            };

            await _unitOfWork.Repository<Pms.Backend.Domain.Entities.Address>().AddAsync(address, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Igreja IASD Jardim Brasil criada com sucesso");
            return BaseResponse<bool>.SuccessResult(true, "Igreja criada com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar igreja IASD Jardim Brasil");
            return BaseResponse<bool>.ErrorResult("Erro ao criar igreja");
        }
    }

    /// <summary>
    /// Cria o clube Pássaro Celeste (PAC)
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da operação</returns>
    public async Task<BaseResponse<bool>> CreateClubAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Criando clube Pássaro Celeste (PAC)...");

            // Verificar se já existe
            if (await HasClubAsync(cancellationToken))
            {
                _logger.LogInformation("Clube Pássaro Celeste já existe, pulando criação");
                return BaseResponse<bool>.SuccessResult(true, "Clube já existe");
            }

            // Obter o distrito DVM
            var district = await _unitOfWork.Repository<District>()
                .GetAsync(d => d.Code == "DVM", cancellationToken);
            var districtEntity = district.FirstOrDefault();

            if (districtEntity == null)
            {
                return BaseResponse<bool>.ErrorResult("Distrito DVM não encontrado. Execute CreateInitialHierarchyAsync primeiro");
            }

            // Obter a igreja IASD Jardim Brasil
            var church = await _unitOfWork.Repository<Church>()
                .GetAsync(c => c.Name == "IASD Jardim Brasil", cancellationToken);
            var churchEntity = church.FirstOrDefault();

            if (churchEntity == null)
            {
                return BaseResponse<bool>.ErrorResult("Igreja IASD Jardim Brasil não encontrada. Execute CreateChurchAsync primeiro");
            }

            // Criar o clube
            var club = new Club
            {
                Code = "PAC",
                Name = "Pássaro Celeste",
                Description = "Clube Pássaro Celeste da IASD Jardim Brasil",
                DistrictId = districtEntity.Id,
                ChurchId = churchEntity.Id,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow
            };

            await _unitOfWork.Repository<Club>().AddAsync(club, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Clube Pássaro Celeste (PAC) criado com sucesso");
            return BaseResponse<bool>.SuccessResult(true, "Clube criado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar clube Pássaro Celeste");
            return BaseResponse<bool>.ErrorResult("Erro ao criar clube");
        }
    }

    /// <summary>
    /// Executa todos os seeds necessários para o MVP0
    /// </summary>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da operação</returns>
    public async Task<BaseResponse<bool>> SeedAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Verificando e executando seeds para MVP0...");

            // Verificar se já existem dados
            var hasSystemAdmin = await HasSystemAdminAsync(cancellationToken);
            var hasHierarchy = await HasInitialHierarchyAsync(cancellationToken);
            var hasHananUser = await HasHananUserAsync(cancellationToken);
            var hasChurch = await HasChurchAsync(cancellationToken);
            var hasClub = await HasClubAsync(cancellationToken);

            if (hasSystemAdmin && hasHierarchy && hasHananUser && hasChurch && hasClub)
            {
                _logger.LogInformation("Todos os seeds já foram executados anteriormente");
                return BaseResponse<bool>.SuccessResult(true, "Todos os seeds já foram executados anteriormente");
            }

            // 1. Criar SystemAdmin (se não existir)
            if (!hasSystemAdmin)
            {
                var systemAdminResult = await CreateSystemAdminAsync(cancellationToken);
                if (!systemAdminResult.IsSuccess)
                {
                    return systemAdminResult;
                }
            }

            // 2. Criar hierarquia inicial (se não existir)
            if (!hasHierarchy)
            {
                var hierarchyResult = await CreateInitialHierarchyAsync(cancellationToken);
                if (!hierarchyResult.IsSuccess)
                {
                    return hierarchyResult;
                }
            }

            // 3. Criar usuário Hanan (se não existir)
            if (!hasHananUser)
            {
                var hananResult = await CreateHananUserAsync(cancellationToken);
                if (!hananResult.IsSuccess)
                {
                    return hananResult;
                }
            }

            // 4. Criar igreja (se não existir)
            if (!hasChurch)
            {
                var churchResult = await CreateChurchAsync(cancellationToken);
                if (!churchResult.IsSuccess)
                {
                    return churchResult;
                }
            }

            // 5. Criar clube (se não existir)
            if (!hasClub)
            {
                var clubResult = await CreateClubAsync(cancellationToken);
                if (!clubResult.IsSuccess)
                {
                    return clubResult;
                }
            }

            _logger.LogInformation("Seeds executados com sucesso");
            return BaseResponse<bool>.SuccessResult(true, "Seeds executados com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao executar seeds");
            return BaseResponse<bool>.ErrorResult("Erro ao executar seeds");
        }
    }

    #region Private Methods

    /// <summary>
    /// Verifica se já existe um SystemAdmin
    /// </summary>
    private async Task<bool> HasSystemAdminAsync(CancellationToken cancellationToken)
    {
        var systemAdmin = await _unitOfWork.Repository<Member>()
            .GetAsync(m => m.Contacts.Any(c => c.Type == ContactType.Email && c.Value == "system@pms.com"), cancellationToken);
        return systemAdmin.Any();
    }

    /// <summary>
    /// Verifica se já existe a hierarquia inicial
    /// </summary>
    private async Task<bool> HasInitialHierarchyAsync(CancellationToken cancellationToken)
    {
        var district = await _unitOfWork.Repository<District>()
            .GetAsync(d => d.Code == "DVM", cancellationToken);
        return district.Any();
    }

    /// <summary>
    /// Verifica se já existe o usuário Hanan
    /// </summary>
    private async Task<bool> HasHananUserAsync(CancellationToken cancellationToken)
    {
        // Verificar por email no Member
        var hananByEmail = await _unitOfWork.Repository<Member>()
            .GetAsync(m => m.Contacts.Any(c => c.Type == ContactType.Email && c.Value == "hanan.ortiz.hnnrtz@gmail.com"), cancellationToken);

        if (hananByEmail.Any())
        {
            return true;
        }

        // Verificar por nome completo (backup)
        var hananByName = await _unitOfWork.Repository<Member>()
            .GetAsync(m => m.FirstName == "Hanan" && m.LastName == "Del Chiaro", cancellationToken);

        return hananByName.Any();
    }

    /// <summary>
    /// Verifica se já existe a igreja IASD Jardim Brasil
    /// </summary>
    private async Task<bool> HasChurchAsync(CancellationToken cancellationToken)
    {
        var church = await _unitOfWork.Repository<Church>()
            .GetAsync(c => c.Name == "IASD Jardim Brasil", cancellationToken);
        return church.Any();
    }

    /// <summary>
    /// Verifica se já existe o clube Pássaro Celeste
    /// </summary>
    private async Task<bool> HasClubAsync(CancellationToken cancellationToken)
    {
        var club = await _unitOfWork.Repository<Club>()
            .GetAsync(c => c.Code == "PAC", cancellationToken);
        return club.Any();
    }

    /// <summary>
    /// Obtém ou cria uma role
    /// </summary>
    private async Task<RoleCatalog> GetOrCreateRole(string roleName, CancellationToken cancellationToken)
    {
        var existingRole = await _unitOfWork.Repository<RoleCatalog>()
            .GetAsync(r => r.Name == roleName, cancellationToken);

        if (existingRole.Any())
        {
            return existingRole.First();
        }

        var newRole = new RoleCatalog
        {
            Name = roleName,
            Description = $"Role {roleName}",
            IsActive = true,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };

        await _unitOfWork.Repository<RoleCatalog>().AddAsync(newRole, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return newRole;
    }

    #endregion
}
