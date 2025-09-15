using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Auth;
using Pms.Backend.Application.DTOs.Members;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Domain.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Pms.Backend.Application.Services.Members;

/// <summary>
/// Service implementation for member management operations
/// </summary>
public partial class MemberService : IMemberService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly IMemberValidationService _validationService;

    /// <summary>
    /// Initializes a new instance of the MemberService
    /// </summary>
    /// <param name="unitOfWork">Unit of work for data access</param>
    /// <param name="mapper">AutoMapper instance for object mapping</param>
    /// <param name="configuration">Configuration instance for JWT settings</param>
    /// <param name="validationService">Service for member data validation</param>
    public MemberService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, IMemberValidationService validationService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _configuration = configuration;
        _validationService = validationService;
    }

    #region Member CRUD Operations

    /// <summary>
    /// Retrieves a specific Member by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the Member.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the MemberDto if found, or an error.</returns>
    public async Task<BaseResponse<MemberDto>> GetMemberAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var member = await _unitOfWork.Repository<Member>().GetByIdAsync(id, cancellationToken);
            if (member == null)
            {
                return BaseResponse<MemberDto>.NotFoundResult("Membro não encontrado");
            }

            // Carregar contatos para calcular PrimaryEmail e PrimaryPhone
            var contacts = await _unitOfWork.Repository<Contact>()
                .GetAsync(c => c.EntityId == member.Id && c.EntityType == "Member" && !c.IsDeleted, cancellationToken);
            member.Contacts = contacts.ToList();

            // Debug: Log dos contatos carregados
            Console.WriteLine($"Member {member.FirstName} {member.LastName} - Contatos carregados: {member.Contacts.Count}");
            foreach (var contact in member.Contacts)
            {
                Console.WriteLine($"  - Tipo: {contact.Type}, Valor: {contact.Value}, IsPrimary: {contact.IsPrimary}, IsActive: {contact.IsActive}");
            }
            Console.WriteLine($"  - PrimaryEmail: {member.PrimaryEmail}");
            Console.WriteLine($"  - PrimaryPhone: {member.PrimaryPhone}");

            var dto = _mapper.Map<MemberDto>(member);
            return BaseResponse<MemberDto>.SuccessResult(dto);
        }
        catch (Exception ex)
        {
            return BaseResponse<MemberDto>.InternalServerErrorResult($"Erro ao recuperar membro: {ex.Message}", true);
        }
    }

    /// <summary>
    /// Retrieves a paginated list of Members.
    /// </summary>
    /// <param name="pageNumber">The page number for pagination (default is 1).</param>
    /// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing a PaginatedResponse of MemberDto.</returns>
    public async Task<BaseResponse<PaginatedResponse<IEnumerable<MemberDto>>>> GetMembersAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            var (items, totalCount) = await _unitOfWork.Repository<Member>().GetPagedAsync(pageNumber, pageSize, null, cancellationToken);
            var dtos = _mapper.Map<IEnumerable<MemberDto>>(items);

            var paginatedResponse = new PaginatedResponse<IEnumerable<MemberDto>>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return BaseResponse<PaginatedResponse<IEnumerable<MemberDto>>>.SuccessResult(paginatedResponse);
        }
        catch (Exception ex)
        {
            return BaseResponse<PaginatedResponse<IEnumerable<MemberDto>>>.ErrorResult($"Error retrieving members: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves a paginated list of Members by Club ID.
    /// </summary>
    /// <param name="clubId">The ID of the Club.</param>
    /// <param name="pageNumber">The page number for pagination (default is 1).</param>
    /// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing a PaginatedResponse of MemberDto.</returns>
    public async Task<BaseResponse<PaginatedResponse<IEnumerable<MemberDto>>>> GetMembersByClubAsync(Guid clubId, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            // Verify club exists
            var club = await _unitOfWork.Repository<Club>().GetByIdAsync(clubId, cancellationToken);
            if (club == null)
            {
                return BaseResponse<PaginatedResponse<IEnumerable<MemberDto>>>.ErrorResult("Club not found");
            }

            var (items, totalCount) = await _unitOfWork.Repository<Member>().GetPagedAsync(
                pageNumber,
                pageSize,
                m => m.Memberships.Any(mem => mem.ClubId == clubId && mem.IsActive),
                cancellationToken);

            var dtos = _mapper.Map<IEnumerable<MemberDto>>(items);

            var paginatedResponse = new PaginatedResponse<IEnumerable<MemberDto>>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return BaseResponse<PaginatedResponse<IEnumerable<MemberDto>>>.SuccessResult(paginatedResponse);
        }
        catch (Exception ex)
        {
            return BaseResponse<PaginatedResponse<IEnumerable<MemberDto>>>.ErrorResult($"Error retrieving members by club: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves a paginated list of Members with optimized response structure.
    /// </summary>
    /// <param name="pageNumber">The page number for pagination (default is 1).</param>
    /// <param name="pageSize">The number of items per page (default is 10).</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing an OptimizedMemberListResponse.</returns>
    public async Task<BaseResponse<OptimizedMemberListResponse>> GetMembersOptimizedAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        try
        {
            var members = await _unitOfWork.Repository<Member>()
                .GetAsync(m => !m.IsDeleted, cancellationToken);

            var totalCount = members.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var hasPreviousPage = pageNumber > 1;
            var hasNextPage = pageNumber < totalPages;

            var pagedMembers = members
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Carregar contatos para cada membro para calcular PrimaryEmail e PrimaryPhone
            foreach (var member in pagedMembers)
            {
                var contacts = await _unitOfWork.Repository<Contact>()
                    .GetAsync(c => c.EntityId == member.Id && c.EntityType == "Member" && !c.IsDeleted, cancellationToken);
                member.Contacts = contacts.ToList();

                // Debug: Log dos contatos carregados
                Console.WriteLine($"Member {member.FirstName} {member.LastName} - Contatos carregados: {member.Contacts.Count}");
                foreach (var contact in member.Contacts)
                {
                    Console.WriteLine($"  - Tipo: {contact.Type}, Valor: {contact.Value}, IsPrimary: {contact.IsPrimary}, IsActive: {contact.IsActive}");
                }
                Console.WriteLine($"  - PrimaryEmail: {member.PrimaryEmail}");
                Console.WriteLine($"  - PrimaryPhone: {member.PrimaryPhone}");
            }

            var memberListDtos = _mapper.Map<List<MemberListDto>>(pagedMembers);

            var optimizedResponse = new OptimizedMemberListResponse
            {
                Items = memberListDtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                HasPreviousPage = hasPreviousPage,
                HasNextPage = hasNextPage
            };

            return BaseResponse<OptimizedMemberListResponse>.SuccessResult(optimizedResponse);
        }
        catch (Exception ex)
        {
            return BaseResponse<OptimizedMemberListResponse>.ErrorResult($"Erro ao recuperar membros: {ex.Message}");
        }
    }


    /// <summary>
    /// Creates a new Member with complete information including address, medical info, contacts, etc.
    /// Valida sub-objetos se preenchidos e faz inserção sequencial com rollback em caso de erro.
    /// </summary>
    /// <param name="dto">The data transfer object containing the Member's complete details.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the created MemberDto if successful, or an error.</returns>
    public async Task<BaseResponse<MemberDto>> CreateMemberCompleteAsync(CreateMemberCompleteDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            // 1. Validações básicas do membro
            var nameValidation = ValidateMemberName(dto.FirstName, dto.MiddleNames, dto.LastName);
            if (!nameValidation.IsSuccess)
            {
                return BaseResponse<MemberDto>.ErrorResult(nameValidation.Message ?? "Erro de validação de nome");
            }

            // Validar CPF se fornecido
            if (!string.IsNullOrEmpty(dto.Cpf))
            {
                var cpfAvailable = await IsCpfAvailableAsync(dto.Cpf, null, cancellationToken);
                if (!cpfAvailable.IsSuccess)
                {
                    return BaseResponse<MemberDto>.ErrorResult(cpfAvailable.Message ?? "Erro na validação do CPF");
                }
                if (!cpfAvailable.Data)
                {
                    return BaseResponse<MemberDto>.ErrorResult("CPF já existe");
                }
            }

            // Validar idade (mínimo 10 anos)
            var age = CalculateAge(dto.DateOfBirth);
            if (age < 10)
            {
                return BaseResponse<MemberDto>.ErrorResult("Membro deve ter pelo menos 10 anos de idade");
            }

            // 1.5. Validação de dados mínimos obrigatórios
            var validationResult = await _validationService.ValidateCreateMemberDtoAsync(dto, cancellationToken);
            // Sempre permitir criação - a validação apenas determina o status do membro

            // 2. Validação de e-mail obrigatório (considerando loginInfo)
            var hasEmailInContacts = dto.Contacts?.Any(c => c.Type == ContactType.Email) ?? false;
            var hasEmailInLogin = !string.IsNullOrEmpty(dto.LoginInfo?.Email);


            if (!hasEmailInContacts && !hasEmailInLogin)
            {
                return BaseResponse<MemberDto>.ErrorResult("Pelo menos um contato de email é obrigatório (forneça no contactInfo ou loginInfo)");
            }

            // 2.1. Validações de contatos se preenchidos
            if (dto.Contacts != null && dto.Contacts.Any())
            {
                // Verificar emails duplicados nos contatos fornecidos
                var emailContacts = dto.Contacts.Where(c => c.Type == ContactType.Email).ToList();
                foreach (var emailContact in emailContacts)
                {
                    var emailAvailable = await IsEmailAvailableAsync(emailContact.Value, null, cancellationToken);
                    if (!emailAvailable.Data)
                    {
                        return BaseResponse<MemberDto>.ErrorResult($"Email {emailContact.Value} já existe");
                    }
                }
            }


            // 2.4. Verificação de disponibilidade do email no loginInfo
            if (dto.LoginInfo != null && !string.IsNullOrEmpty(dto.LoginInfo.Email))
            {
                var emailAvailable = await IsEmailAvailableAsync(dto.LoginInfo.Email, null, cancellationToken);
                if (!emailAvailable.Data)
                {
                    return BaseResponse<MemberDto>.ErrorResult($"Email {dto.LoginInfo.Email} já existe");
                }
            }

            // 3. Criar membro principal
            var member = _mapper.Map<Member>(dto);
            member.Id = Guid.NewGuid();

            // Limpar CPF se fornecido (remover pontos, traços e espaços)
            if (!string.IsNullOrEmpty(member.Cpf))
            {
                member.Cpf = member.Cpf.Replace(".", "").Replace("-", "").Replace(" ", "");
            }

            // Definir status baseado na validação
            member.Status = validationResult.CanBeActivated ? MemberStatus.Active : MemberStatus.Pending;
            member.CreatedAtUtc = DateTime.UtcNow;
            member.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.Repository<Member>().AddAsync(member, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 4. Criar contatos se fornecidos + contato de e-mail automático do loginInfo
            var contactsToCreate = new List<Contact>();

            // 4.1. Adicionar contatos fornecidos pelo usuário
            if (dto.Contacts != null && dto.Contacts.Any())
            {
                foreach (var contactDto in dto.Contacts)
                {
                    var contact = _mapper.Map<Contact>(contactDto);
                    contact.Id = Guid.NewGuid();
                    contact.EntityId = member.Id;
                    contact.EntityType = "Member";
                    contact.CreatedAtUtc = DateTime.UtcNow;
                    contact.UpdatedAtUtc = DateTime.UtcNow;

                    contactsToCreate.Add(contact);
                }
            }

            // 4.2. Adicionar contato de e-mail automaticamente a partir do loginInfo
            if (dto.LoginInfo != null && !string.IsNullOrEmpty(dto.LoginInfo.Email))
            {
                // Verificar se já não existe um contato de e-mail nos contatos fornecidos
                var hasEmailContact = dto.Contacts?.Any(c => c.Type == ContactType.Email) ?? false;

                if (!hasEmailContact)
                {
                    var emailContact = new Contact
                    {
                        Id = Guid.NewGuid(),
                        EntityId = member.Id,
                        EntityType = "Member",
                        Type = ContactType.Email,
                        Value = dto.LoginInfo.Email,
                        IsPrimary = true,
                        IsActive = true,
                        CreatedAtUtc = DateTime.UtcNow,
                        UpdatedAtUtc = DateTime.UtcNow
                    };

                    contactsToCreate.Add(emailContact);
                }
            }

            // 4.3. Salvar todos os contatos
            foreach (var contact in contactsToCreate)
            {
                await _unitOfWork.Repository<Contact>().AddAsync(contact, cancellationToken);
            }

            // 5. Criar endereço se fornecido
            if (dto.AddressInfo != null)
            {
                var address = _mapper.Map<Domain.Entities.Address>(dto.AddressInfo);
                address.Id = Guid.NewGuid();
                address.EntityId = member.Id;
                address.EntityType = "Member";
                address.CreatedAtUtc = DateTime.UtcNow;
                address.UpdatedAtUtc = DateTime.UtcNow;

                await _unitOfWork.Repository<Domain.Entities.Address>().AddAsync(address, cancellationToken);
            }

            // 6. Criar prontuário médico se fornecido
            if (dto.MedicalInfo != null)
            {
                var medicalRecord = _mapper.Map<MedicalRecord>(dto.MedicalInfo);
                medicalRecord.Id = Guid.NewGuid();
                medicalRecord.MemberId = member.Id;
                medicalRecord.CreatedAtUtc = DateTime.UtcNow;
                medicalRecord.UpdatedAtUtc = DateTime.UtcNow;

                await _unitOfWork.Repository<MedicalRecord>().AddAsync(medicalRecord, cancellationToken);
            }

            // 7. Criar credenciais de usuário se fornecidas
            if (dto.LoginInfo != null)
            {
                var userCredential = new UserCredential
                {
                    Id = Guid.NewGuid(),
                    MemberId = member.Id,
                    Email = dto.LoginInfo.Email,
                    PasswordHash = HashPassword(dto.LoginInfo.Password),
                    Salt = "", // Será preenchido pelo HashPassword se necessário
                    IsActive = true,
                    IsEmailVerified = false,
                    IsLockedOut = false,
                    FailedLoginAttempts = 0,
                    CreatedAtUtc = DateTime.UtcNow,
                    UpdatedAtUtc = DateTime.UtcNow
                };

                await _unitOfWork.Repository<UserCredential>().AddAsync(userCredential, cancellationToken);
            }

            // 8. Salvar todas as alterações (com rollback automático em caso de erro)
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var resultDto = _mapper.Map<MemberDto>(member);
            return BaseResponse<MemberDto>.SuccessResult(resultDto, "Membro criado com sucesso com todas as informações");
        }
        catch (Exception ex)
        {
            // O rollback é automático devido ao UnitOfWork
            return BaseResponse<MemberDto>.ErrorResult($"Erro ao criar membro: {ex.Message}");
        }
    }



    /// <summary>
    /// Updates an existing Member.
    /// </summary>
    /// <param name="id">The unique identifier of the Member to update.</param>
    /// <param name="dto">The data transfer object containing the updated Member's details.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the updated MemberDto if successful, or an error.</returns>
    public async Task<BaseResponse<MemberDto>> UpdateMemberAsync(Guid id, UpdateMemberDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var member = await _unitOfWork.Repository<Member>().GetByIdAsync(id, cancellationToken);
            if (member == null)
            {
                return BaseResponse<MemberDto>.NotFoundResult("Membro não encontrado");
            }

            // Note: Email validation is now handled through contacts
            // This method is kept for backward compatibility but should not be used for new implementations

            // Validate CPF if provided (excluding current member)
            if (!string.IsNullOrEmpty(dto.Cpf))
            {
                var cpfAvailable = await IsCpfAvailableAsync(dto.Cpf, id, cancellationToken);
                if (!cpfAvailable.Data)
                {
                    return BaseResponse<MemberDto>.ErrorResult("CPF already exists");
                }
            }

            // Validate age (minimum 10 years old) if DateOfBirth is provided
            if (dto.DateOfBirth.HasValue)
            {
                var age = CalculateAge(dto.DateOfBirth.Value);
                if (age < 10)
                {
                    return BaseResponse<MemberDto>.ErrorResult("Member must be at least 10 years old");
                }
            }

            _mapper.Map(dto, member);
            member.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.Repository<Member>().UpdateAsync(member, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var resultDto = _mapper.Map<MemberDto>(member);
            return BaseResponse<MemberDto>.SuccessResult(resultDto, "Member updated successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<MemberDto>.ErrorResult($"Error updating member: {ex.Message}");
        }
    }

    /// <summary>
    /// Deletes a Member by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the Member to delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse indicating success or failure of the deletion.</returns>
    public async Task<BaseResponse<bool>> DeleteMemberAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if member has active memberships
            var hasActiveMemberships = await _unitOfWork.Repository<Domain.Entities.Membership>().ExistsAsync(m => m.MemberId == id && m.IsActive, cancellationToken);
            if (hasActiveMemberships)
            {
                return BaseResponse<bool>.ErrorResult("Cannot delete member with active memberships");
            }

            // Check if member has active assignments
            var hasActiveAssignments = await _unitOfWork.Repository<Assignment>().ExistsAsync(a => a.MemberId == id && a.IsActive, cancellationToken);
            if (hasActiveAssignments)
            {
                return BaseResponse<bool>.ErrorResult("Cannot delete member with active assignments");
            }

            var deleted = await _unitOfWork.Repository<Member>().DeleteAsync(id, cancellationToken);
            if (!deleted)
            {
                return BaseResponse<bool>.ErrorResult("Member not found");
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return BaseResponse<bool>.SuccessResult(true, "Member deleted successfully");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.ErrorResult($"Error deleting member: {ex.Message}");
        }
    }

    /// <summary>
    /// Hard deletes a Member and all related data permanently.
    /// </summary>
    /// <param name="id">The unique identifier of the Member to hard delete.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse indicating success or failure of the hard deletion.</returns>
    public async Task<BaseResponse<bool>> HardDeleteMemberAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            // Verificar se o membro existe
            var member = await _unitOfWork.Repository<Member>().GetByIdAsync(id, cancellationToken);
            if (member == null)
            {
                return BaseResponse<bool>.ErrorResult("Membro não encontrado");
            }

            // 1. Deletar assignments (papéis) do membro
            var assignments = await _unitOfWork.Repository<Assignment>()
                .GetAsync(a => a.MemberId == id, cancellationToken);
            foreach (var assignment in assignments)
            {
                await _unitOfWork.Repository<Assignment>().DeleteAsync(assignment.Id, cancellationToken);
            }

            // 2. Deletar credenciais de usuário do membro
            var userCredentials = await _unitOfWork.Repository<UserCredential>()
                .GetAsync(uc => uc.MemberId == id, cancellationToken);
            foreach (var credential in userCredentials)
            {
                await _unitOfWork.Repository<UserCredential>().DeleteAsync(credential.Id, cancellationToken);
            }

            // 3. Deletar contatos do membro
            var contacts = await _unitOfWork.Repository<Contact>()
                .GetAsync(c => c.EntityId == id && c.EntityType == "Member", cancellationToken);
            foreach (var contact in contacts)
            {
                await _unitOfWork.Repository<Contact>().DeleteAsync(contact.Id, cancellationToken);
            }

            // 4. Deletar endereços do membro
            var addresses = await _unitOfWork.Repository<Domain.Entities.Address>()
                .GetAsync(a => a.EntityId == id && a.EntityType == "Member", cancellationToken);
            foreach (var address in addresses)
            {
                await _unitOfWork.Repository<Domain.Entities.Address>().DeleteAsync(address.Id, cancellationToken);
            }

            // 5. Deletar membroships do membro
            var memberships = await _unitOfWork.Repository<Domain.Entities.Membership>()
                .GetAsync(m => m.MemberId == id, cancellationToken);
            foreach (var membership in memberships)
            {
                await _unitOfWork.Repository<Domain.Entities.Membership>().DeleteAsync(membership.Id, cancellationToken);
            }

            // 6. Deletar o membro
            await _unitOfWork.Repository<Member>().DeleteAsync(id, cancellationToken);

            // Salvar todas as mudanças
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return BaseResponse<bool>.SuccessResult(true, "Membro e todos os dados relacionados foram deletados permanentemente");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.ErrorResult($"Erro ao deletar membro permanentemente: {ex.Message}");
        }
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Calculates the age based on date of birth
    /// </summary>
    /// <param name="dateOfBirth">Date of birth</param>
    /// <returns>Age in years</returns>
    private static int CalculateAge(DateTime dateOfBirth)
    {
        var today = DateTime.Today;
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > today.AddYears(-age)) age--;
        return age;
    }

    /// <summary>
    /// Generates a JWT token for authentication
    /// </summary>
    /// <param name="member">Member entity</param>
    /// <returns>JWT token string</returns>
    private string GenerateJwtToken(Member member)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
        var issuer = jwtSettings["Issuer"] ?? "PmsBackend";
        var audience = jwtSettings["Audience"] ?? "PmsBackend";
        var expirationMinutes = int.Parse(jwtSettings["ExpirationMinutes"] ?? "60");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, member.Id.ToString()),
            new(ClaimTypes.Email, member.PrimaryEmail ?? ""),
            new(ClaimTypes.Name, member.DisplayName),
            new("sub", member.Id.ToString())
        };

        // Add roles from assignments
        var activeAssignments = member.Assignments?.Where(a => a.IsActive).ToList() ?? new List<Assignment>();
        foreach (var assignment in activeAssignments)
        {
            claims.Add(new Claim(ClaimTypes.Role, assignment.RoleCatalog.Name));
            // Add scope based on assignment scope type
            claims.Add(new Claim("scope", assignment.ScopeType.ToString()));
        }

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Hashes a password using BCrypt
    /// </summary>
    /// <param name="password">Plain text password</param>
    /// <returns>Hashed password</returns>
    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    /// <summary>
    /// Verifies a password against a hash
    /// </summary>
    /// <param name="password">Plain text password</param>
    /// <param name="hash">Hashed password</param>
    /// <returns>True if password matches hash</returns>
    private static bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }

    /// <summary>
    /// Generates a secure random token
    /// </summary>
    /// <param name="length">Token length in bytes</param>
    /// <returns>Base64 encoded token</returns>
    private static string GenerateSecureToken(int length = 32)
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[length];
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Validates member name fields
    /// </summary>
    /// <param name="firstName">First name</param>
    /// <param name="middleNames">Middle names</param>
    /// <param name="lastName">Last name</param>
    /// <returns>Validation result</returns>
    private static BaseResponse<bool> ValidateMemberName(string firstName, string? middleNames, string lastName)
    {
        var error = Domain.Helpers.NameHelper.GetFullNameValidationError(firstName, middleNames, lastName);
        if (error != null)
        {
            return BaseResponse<bool>.ErrorResult(error);
        }

        return BaseResponse<bool>.SuccessResult(true);
    }

    /// <summary>
    /// Validates member contacts
    /// </summary>
    /// <param name="contacts">List of contacts</param>
    /// <returns>Validation result</returns>
    private static BaseResponse<bool> ValidateMemberContacts(List<MemberContactDto> contacts)
    {
        if (contacts == null || !contacts.Any())
        {
            return BaseResponse<bool>.ErrorResult("Pelo menos um contato é obrigatório");
        }

        // Check for at least one email contact
        var hasEmail = contacts.Any(c => c.Type == ContactType.Email);
        if (!hasEmail)
        {
            return BaseResponse<bool>.ErrorResult("Pelo menos um contato de email é obrigatório");
        }

        // Check for duplicate contact types
        var duplicateTypes = contacts.GroupBy(c => c.Type)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key.ToString())
            .ToList();

        if (duplicateTypes.Any())
        {
            return BaseResponse<bool>.ErrorResult($"Tipos de contato duplicados encontrados: {string.Join(", ", duplicateTypes)}");
        }

        // Validate each contact
        foreach (var contact in contacts)
        {
            if (string.IsNullOrWhiteSpace(contact.Value))
            {
                return BaseResponse<bool>.ErrorResult($"Valor do contato {contact.Type} não pode estar vazio");
            }

            // Basic email validation
            if (contact.Type == ContactType.Email)
            {
                if (!contact.Value.Contains("@") || contact.Value.Length < 5)
                {
                    return BaseResponse<bool>.ErrorResult("Email inválido");
                }
            }

            // Basic phone validation
            if (contact.Type == ContactType.Mobile || contact.Type == ContactType.Landline)
            {
                var phoneDigits = new string(contact.Value.Where(char.IsDigit).ToArray());
                if (phoneDigits.Length < 10)
                {
                    return BaseResponse<bool>.ErrorResult("Telefone deve ter pelo menos 10 dígitos");
                }
            }
        }

        return BaseResponse<bool>.SuccessResult(true);
    }

    /// <summary>
    /// Validates create member contacts
    /// </summary>
    /// <param name="contacts">List of create member contacts</param>
    /// <returns>Validation result</returns>
    private static BaseResponse<bool> ValidateCreateMemberContacts(List<CreateMemberContactDto> contacts)
    {
        if (contacts == null || !contacts.Any())
        {
            return BaseResponse<bool>.ErrorResult("Pelo menos um contato é obrigatório");
        }

        // Check for at least one email contact
        var hasEmail = contacts.Any(c => c.Type == ContactType.Email);
        if (!hasEmail)
        {
            return BaseResponse<bool>.ErrorResult("Pelo menos um contato de email é obrigatório");
        }

        // Check for duplicate contact types
        var duplicateTypes = contacts.GroupBy(c => c.Type)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key.ToString())
            .ToList();

        if (duplicateTypes.Any())
        {
            return BaseResponse<bool>.ErrorResult($"Tipos de contato duplicados encontrados: {string.Join(", ", duplicateTypes)}");
        }

        // Validate each contact
        foreach (var contact in contacts)
        {
            if (string.IsNullOrWhiteSpace(contact.Value))
            {
                return BaseResponse<bool>.ErrorResult($"Valor do contato {contact.Type} não pode estar vazio");
            }

            // Basic email validation
            if (contact.Type == ContactType.Email)
            {
                if (!contact.Value.Contains("@") || contact.Value.Length < 5)
                {
                    return BaseResponse<bool>.ErrorResult("Email inválido");
                }
            }

            // Basic phone validation
            if (contact.Type == ContactType.Mobile || contact.Type == ContactType.Landline)
            {
                var phoneDigits = new string(contact.Value.Where(char.IsDigit).ToArray());
                if (phoneDigits.Length < 10)
                {
                    return BaseResponse<bool>.ErrorResult("Telefone deve ter pelo menos 10 dígitos");
                }
            }
        }

        return BaseResponse<bool>.SuccessResult(true);
    }

    /// <summary>
    /// Completes a member's pending information (for /me/complete endpoint)
    /// </summary>
    /// <param name="id">The unique identifier of the Member to complete.</param>
    /// <param name="dto">The complete member information data.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A BaseResponse containing the updated MemberDto if successful, or an error.</returns>
    public async Task<BaseResponse<MemberDto>> CompleteMemberAsync(Guid id, CreateMemberCompleteDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            // Buscar o membro existente
            var member = await _unitOfWork.Repository<Member>().GetByIdAsync(id, cancellationToken);
            if (member == null)
            {
                return BaseResponse<MemberDto>.NotFoundResult("Membro não encontrado");
            }

            // Verificar se o membro está em status Pending
            if (member.Status != Domain.Entities.MemberStatus.Pending)
            {
                return BaseResponse<MemberDto>.UnprocessableEntityResult("Membro não está em status Pending");
            }

            // Atualizar informações básicas do membro
            member.FirstName = dto.FirstName;
            member.LastName = dto.LastName;
            member.MiddleNames = dto.MiddleNames;
            member.SocialName = dto.SocialName;
            member.DateOfBirth = dto.DateOfBirth;
            member.Gender = dto.Gender;
            member.Cpf = dto.Cpf;
            member.Rg = dto.Rg;

            // Atualizar endereço se fornecido
            if (dto.AddressInfo != null)
            {
                // Buscar endereço existente ou criar novo
                var existingAddress = await _unitOfWork.Repository<Domain.Entities.Address>()
                    .GetFirstOrDefaultAsync(a => a.EntityId == id && a.EntityType == "Member", cancellationToken);

                if (existingAddress != null)
                {
                    existingAddress.Street = dto.AddressInfo.Street;
                    existingAddress.Number = dto.AddressInfo.Number;
                    existingAddress.Complement = dto.AddressInfo.Complement;
                    existingAddress.Neighborhood = dto.AddressInfo.Neighborhood;
                    existingAddress.City = dto.AddressInfo.City;
                    existingAddress.State = dto.AddressInfo.State;
                    existingAddress.Cep = dto.AddressInfo.PostalCode;
                    existingAddress.Country = dto.AddressInfo.Country;
                    existingAddress.IsPrimary = dto.AddressInfo.IsPrimary;
                }
                else
                {
                    var newAddress = new Domain.Entities.Address
                    {
                        EntityId = id,
                        EntityType = "Member",
                        Street = dto.AddressInfo.Street,
                        Number = dto.AddressInfo.Number,
                        Complement = dto.AddressInfo.Complement,
                        Neighborhood = dto.AddressInfo.Neighborhood,
                        City = dto.AddressInfo.City,
                        State = dto.AddressInfo.State,
                        Cep = dto.AddressInfo.PostalCode,
                        Country = dto.AddressInfo.Country,
                        IsPrimary = dto.AddressInfo.IsPrimary
                    };
                    await _unitOfWork.Repository<Domain.Entities.Address>().AddAsync(newAddress, cancellationToken);
                }
            }

            // Atualizar contatos se fornecidos
            if (dto.Contacts != null && dto.Contacts.Any())
            {
                // Remover contatos existentes
                var existingContacts = await _unitOfWork.Repository<Contact>()
                    .GetAsync(c => c.EntityId == id && c.EntityType == "Member", cancellationToken);

                foreach (var contact in existingContacts)
                {
                    await _unitOfWork.Repository<Contact>().DeleteAsync(contact.Id, cancellationToken);
                }

                // Adicionar novos contatos
                foreach (var contactDto in dto.Contacts)
                {
                    var newContact = new Contact
                    {
                        EntityId = id,
                        EntityType = "Member",
                        Type = contactDto.Type,
                        Value = contactDto.Value,
                        IsPrimary = contactDto.IsPrimary,
                        IsActive = true
                    };
                    await _unitOfWork.Repository<Contact>().AddAsync(newContact, cancellationToken);
                }
            }

            // Atualizar informações de batismo se fornecidas
            if (dto.BaptismInfo != null)
            {
                member.Baptized = true;
                member.BaptizedAt = dto.BaptismInfo.BaptismDate;
                member.BaptizedPlace = dto.BaptismInfo.BaptismChurch;
                member.BaptismPastor = dto.BaptismInfo.BaptismPastor;
                member.BaptismChurch = dto.BaptismInfo.BaptismChurch;
                member.BaptismDate = dto.BaptismInfo.BaptismDate;
            }

            // Atualizar informações médicas se fornecidas
            if (dto.MedicalInfo != null)
            {
                member.Allergies = dto.MedicalInfo.Allergies;
                member.Medications = dto.MedicalInfo.Medications;
                member.MedicalInfo = dto.MedicalInfo.MedicalInfo;
                // As propriedades de contato de emergência não estão no DTO médico
                // Elas devem ser preenchidas através de outros meios se necessário
            }

            // Atualizar investidura se fornecida
            if (dto.InitialScarfInvestiture != null)
            {
                member.ScarfInvested = true;
                member.ScarfInvestedAt = dto.InitialScarfInvestiture.Date;
                member.ScarfPastor = dto.InitialScarfInvestiture.Witnesses.FirstOrDefault()?.NameText ?? "Pastor";
                member.ScarfChurch = dto.InitialScarfInvestiture.Place;
                member.ScarfDate = dto.InitialScarfInvestiture.Date;
            }

            // Verificar se deve ativar o membro
            var activationChecklist = new ActivationChecklistDto
            {
                HasCompleteAddress = !string.IsNullOrEmpty(dto.AddressInfo?.Street) &&
                                   !string.IsNullOrEmpty(dto.AddressInfo?.Number) &&
                                   !string.IsNullOrEmpty(dto.AddressInfo?.City) &&
                                   !string.IsNullOrEmpty(dto.AddressInfo?.State) &&
                                   !string.IsNullOrEmpty(dto.AddressInfo?.PostalCode),
                HasContactEmail = dto.Contacts?.Any(c => c.Type == ContactType.Email) ?? false,
                HasContactMobile = dto.Contacts?.Any(c => c.Type == ContactType.Mobile) ?? false
            };

            if (activationChecklist.IsActivationComplete)
            {
                member.Status = Domain.Entities.MemberStatus.Active;
            }

            // Salvar alterações
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Mapear para DTO de resposta
            var memberDto = _mapper.Map<MemberDto>(member);
            return BaseResponse<MemberDto>.SuccessResult(memberDto, "Informações do membro completadas com sucesso");
        }
        catch (Exception ex)
        {
            return BaseResponse<MemberDto>.InternalServerErrorResult($"Erro ao completar informações do membro: {ex.Message}", true);
        }
    }

    #endregion
}
