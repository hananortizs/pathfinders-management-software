using Microsoft.Extensions.Logging;
using Pms.Backend.Application.DTOs.Members;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Domain.Enums;

namespace Pms.Backend.Application.Services.Members;

/// <summary>
/// Serviço para validação de dados mínimos obrigatórios de membros
/// </summary>
public class MemberValidationService : IMemberValidationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<MemberValidationService> _logger;

    /// <summary>
    /// Inicializa uma nova instância do MemberValidationService
    /// </summary>
    /// <param name="unitOfWork">Unit of Work para acesso aos dados</param>
    /// <param name="logger">Logger</param>
    public MemberValidationService(IUnitOfWork unitOfWork, ILogger<MemberValidationService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Valida se um membro possui todos os dados mínimos obrigatórios
    /// </summary>
    /// <param name="member">Membro a ser validado</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da validação com detalhes do que está pendente</returns>
    public async Task<MemberValidationResult> ValidateMemberDataAsync(Member member, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Iniciando validação de dados do membro {MemberId}", member.Id);

            var result = new MemberValidationResult();

            // Verificar se os dados de batismo são obrigatórios
            result.BaptismDataRequired = IsBaptismDataRequired(member.DateOfBirth);

            // Validar dados pessoais básicos
            ValidatePersonalData(member, result);

            // Validar dados de contato
            await ValidateContactDataAsync(member, result, cancellationToken);

            // Validar dados de endereço
            await ValidateAddressDataAsync(member, result, cancellationToken);

            // Validar dados médicos
            await ValidateMedicalDataAsync(member, result, cancellationToken);

            // Validar dados de batismo (se obrigatórios)
            if (result.BaptismDataRequired)
            {
                ValidateBaptismData(member, result);
            }

            // Determinar se o membro pode ser ativado
            result.CanBeActivated = result.PendingFields.Count == 0;
            result.IsValid = result.CanBeActivated;

            // Definir motivo da inatividade
            if (!result.CanBeActivated)
            {
                result.InactivityReason = "PendingData";
                result.ErrorMessage = $"Dados pendentes: {string.Join(", ", result.PendingDataTypes)}";
            }

            _logger.LogInformation("Validação concluída para membro {MemberId}. Válido: {IsValid}, Pode ser ativado: {CanBeActivated}",
                member.Id, result.IsValid, result.CanBeActivated);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao validar dados do membro {MemberId}", member.Id);
            return new MemberValidationResult
            {
                IsValid = false,
                CanBeActivated = false,
                ErrorMessage = "Erro interno durante a validação",
                InactivityReason = "ValidationError"
            };
        }
    }

    /// <summary>
    /// Valida se um DTO de criação de membro possui todos os dados mínimos obrigatórios
    /// </summary>
    /// <param name="dto">DTO de criação de membro</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da validação com detalhes do que está pendente</returns>
    public async Task<MemberValidationResult> ValidateCreateMemberDtoAsync(CreateMemberCompleteDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Iniciando validação de DTO de criação de membro");

            var result = new MemberValidationResult();

            // Verificar se os dados de batismo são obrigatórios
            result.BaptismDataRequired = IsBaptismDataRequired(dto.DateOfBirth);

            // Validar dados pessoais básicos
            ValidatePersonalDataDto(dto, result);

            // Validar dados de contato
            ValidateContactDataDto(dto, result);

            // Validar dados de endereço
            ValidateAddressDataDto(dto, result);

            // Validar dados médicos
            ValidateMedicalDataDto(dto, result);

            // Validar dados de batismo (se obrigatórios)
            if (result.BaptismDataRequired)
            {
                ValidateBaptismDataDto(dto, result);
            }

            // Determinar se o membro pode ser ativado
            // Para criação, sempre permitir, mas marcar como Pending se houver dados pendentes
            result.CanBeActivated = result.PendingFields.Count == 0;
            result.IsValid = true; // Sempre permitir criação, mesmo com dados pendentes

            // Definir motivo da inatividade
            if (!result.CanBeActivated)
            {
                result.InactivityReason = "PendingData";
                result.ErrorMessage = $"Dados pendentes: {string.Join(", ", result.PendingDataTypes)}";
            }

            _logger.LogInformation("Validação de DTO concluída. Válido: {IsValid}, Pode ser ativado: {CanBeActivated}",
                result.IsValid, result.CanBeActivated);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao validar DTO de criação de membro");
            return new MemberValidationResult
            {
                IsValid = false,
                CanBeActivated = false,
                ErrorMessage = "Erro interno durante a validação",
                InactivityReason = "ValidationError"
            };
        }
    }

    /// <summary>
    /// Calcula a idade do membro baseado no dia 1º de junho do ano vigente
    /// </summary>
    /// <param name="dateOfBirth">Data de nascimento do membro</param>
    /// <param name="referenceYear">Ano de referência (padrão: ano atual)</param>
    /// <returns>Idade do membro em 1º de junho do ano de referência</returns>
    public int CalculateAgeForBaptismRequirement(DateTime dateOfBirth, int? referenceYear = null)
    {
        var year = referenceYear ?? DateTime.UtcNow.Year;
        var juneFirst = new DateTime(year, 6, 1);

        var age = juneFirst.Year - dateOfBirth.Year;

        // Ajustar se o aniversário ainda não ocorreu em 1º de junho
        if (dateOfBirth.Date > juneFirst.AddYears(-age))
        {
            age--;
        }

        return age;
    }

    /// <summary>
    /// Verifica se os dados de batismo são obrigatórios para o membro
    /// </summary>
    /// <param name="dateOfBirth">Data de nascimento do membro</param>
    /// <param name="referenceYear">Ano de referência (padrão: ano atual)</param>
    /// <returns>True se os dados de batismo são obrigatórios</returns>
    public bool IsBaptismDataRequired(DateTime dateOfBirth, int? referenceYear = null)
    {
        var age = CalculateAgeForBaptismRequirement(dateOfBirth, referenceYear);
        return age >= 16;
    }

    #region Private Methods

    /// <summary>
    /// Valida dados pessoais básicos do membro
    /// </summary>
    private void ValidatePersonalData(Member member, MemberValidationResult result)
    {
        // Nome obrigatório
        if (string.IsNullOrWhiteSpace(member.FirstName))
        {
            result.PendingFields.Add("FirstName");
            result.PendingDataTypes.Add("Nome");
        }

        if (string.IsNullOrWhiteSpace(member.LastName))
        {
            result.PendingFields.Add("LastName");
            result.PendingDataTypes.Add("Sobrenome");
        }

        // Data de nascimento obrigatória
        if (member.DateOfBirth == default)
        {
            result.PendingFields.Add("DateOfBirth");
            result.PendingDataTypes.Add("Data de Nascimento");
        }

        // Gênero obrigatório - verificar se foi definido explicitamente
        // Como o enum não tem valor "NaoInformado", vamos assumir que se não foi definido, será Masculino por padrão
        // Mas vamos validar se o membro tem dados suficientes para determinar o gênero
        // Por enquanto, vamos pular esta validação já que o enum não suporta "não informado"

        // CPF obrigatório
        if (string.IsNullOrWhiteSpace(member.Cpf))
        {
            result.PendingFields.Add("Cpf");
            result.PendingDataTypes.Add("CPF");
        }
    }

    /// <summary>
    /// Valida dados pessoais básicos do DTO
    /// </summary>
    private void ValidatePersonalDataDto(CreateMemberCompleteDto dto, MemberValidationResult result)
    {
        // Nome obrigatório
        if (string.IsNullOrWhiteSpace(dto.FirstName))
        {
            result.PendingFields.Add("FirstName");
            result.PendingDataTypes.Add("Nome");
        }

        if (string.IsNullOrWhiteSpace(dto.LastName))
        {
            result.PendingFields.Add("LastName");
            result.PendingDataTypes.Add("Sobrenome");
        }

        // Data de nascimento obrigatória
        if (dto.DateOfBirth == default)
        {
            result.PendingFields.Add("DateOfBirth");
            result.PendingDataTypes.Add("Data de Nascimento");
        }

        // Gênero obrigatório - verificar se foi definido explicitamente
        // Como o enum não tem valor "NaoInformado", vamos assumir que se não foi definido, será Masculino por padrão
        // Mas vamos validar se o membro tem dados suficientes para determinar o gênero
        // Por enquanto, vamos pular esta validação já que o enum não suporta "não informado"

        // CPF obrigatório
        if (string.IsNullOrWhiteSpace(dto.Cpf))
        {
            result.PendingFields.Add("Cpf");
            result.PendingDataTypes.Add("CPF");
        }
    }

    /// <summary>
    /// Valida dados de contato do membro
    /// </summary>
    private async Task ValidateContactDataAsync(Member member, MemberValidationResult result, CancellationToken cancellationToken)
    {
        // Buscar contatos do membro
        var contacts = await _unitOfWork.Repository<Contact>()
            .GetAsync(c => c.EntityId == member.Id && c.EntityType == "Member", cancellationToken);

        var emailContacts = contacts.Where(c => c.Type == ContactType.Email).ToList();
        var phoneContacts = contacts.Where(c => c.Type == ContactType.Mobile || c.Type == ContactType.Landline || c.Type == ContactType.WhatsApp).ToList();

        // Pelo menos um email obrigatório
        if (!emailContacts.Any())
        {
            result.PendingFields.Add("Email");
            result.PendingDataTypes.Add("E-mail");
        }

        // Pelo menos um telefone obrigatório
        if (!phoneContacts.Any())
        {
            result.PendingFields.Add("Phone");
            result.PendingDataTypes.Add("Telefone");
        }
    }

    /// <summary>
    /// Valida dados de contato do DTO
    /// </summary>
    private void ValidateContactDataDto(CreateMemberCompleteDto dto, MemberValidationResult result)
    {
        var hasEmail = false;
        var hasPhone = false;

        // Verificar se há email no loginInfo
        if (!string.IsNullOrWhiteSpace(dto.LoginInfo?.Email))
        {
            hasEmail = true;
        }

        // Verificar se há email nos contatos
        if (dto.Contacts?.Any(c => c.Type == ContactType.Email && !string.IsNullOrWhiteSpace(c.Value)) == true)
        {
            hasEmail = true;
        }

        // Verificar se há telefone nos contatos
        if (dto.Contacts?.Any(c => (c.Type == ContactType.Mobile || c.Type == ContactType.Landline || c.Type == ContactType.WhatsApp) && !string.IsNullOrWhiteSpace(c.Value)) == true)
        {
            hasPhone = true;
        }

        // Pelo menos um email obrigatório
        if (!hasEmail)
        {
            result.PendingFields.Add("Email");
            result.PendingDataTypes.Add("E-mail");
        }

        // Telefone é opcional na criação - apenas marcar como pendente se não fornecido
        if (!hasPhone)
        {
            result.PendingFields.Add("Phone");
            result.PendingDataTypes.Add("Telefone");
        }
    }

    /// <summary>
    /// Valida dados de endereço do membro
    /// </summary>
    private async Task ValidateAddressDataAsync(Member member, MemberValidationResult result, CancellationToken cancellationToken)
    {
        // Buscar endereços do membro
        var addresses = await _unitOfWork.Repository<Domain.Entities.Address>()
            .GetAsync(a => a.EntityId == member.Id && a.EntityType == "Member", cancellationToken);

        // Pelo menos um endereço obrigatório
        if (!addresses.Any())
        {
            result.PendingFields.Add("Address");
            result.PendingDataTypes.Add("Address");
        }
        else
        {
            // Verificar se o endereço está completo
            var address = addresses.First();
            if (string.IsNullOrWhiteSpace(address.Street) ||
                string.IsNullOrWhiteSpace(address.City) ||
                string.IsNullOrWhiteSpace(address.State))
            {
                result.PendingFields.Add("AddressComplete");
                result.PendingDataTypes.Add("Address");
            }
        }
    }

    /// <summary>
    /// Valida dados de endereço do DTO
    /// </summary>
    private void ValidateAddressDataDto(CreateMemberCompleteDto dto, MemberValidationResult result)
    {
        // Endereço é opcional na criação - apenas marcar como pendente se não fornecido
        if (dto.AddressInfo == null)
        {
            result.PendingFields.Add("Address");
            result.PendingDataTypes.Add("Address");
        }
        else
        {
            // Verificar se o endereço está completo
            var address = dto.AddressInfo;
            if (string.IsNullOrWhiteSpace(address.Street) ||
                string.IsNullOrWhiteSpace(address.City) ||
                string.IsNullOrWhiteSpace(address.State))
            {
                result.PendingFields.Add("AddressComplete");
                result.PendingDataTypes.Add("Address");
            }
        }
    }

    /// <summary>
    /// Valida dados de batismo do membro
    /// </summary>
    private void ValidateBaptismData(Member member, MemberValidationResult result)
    {
        bool hasBaptismIssues = false;

        // Data de batismo obrigatória
        if (member.BaptismDate == null)
        {
            result.PendingFields.Add("BaptismDate");
            hasBaptismIssues = true;
        }

        // Local de batismo obrigatório
        if (string.IsNullOrWhiteSpace(member.BaptismChurch))
        {
            result.PendingFields.Add("BaptismChurch");
            hasBaptismIssues = true;
        }

        // Nome do pastor obrigatório
        if (string.IsNullOrWhiteSpace(member.BaptismPastor))
        {
            result.PendingFields.Add("BaptismPastor");
            hasBaptismIssues = true;
        }

        // Adicionar BaptismRecord apenas uma vez se houver problemas
        if (hasBaptismIssues)
        {
            result.PendingDataTypes.Add("BaptismRecord");
        }

        // Verificar se todos os dados de batismo estão completos
        result.BaptismDataComplete = member.BaptismDate != null &&
                                   !string.IsNullOrWhiteSpace(member.BaptismChurch) &&
                                   !string.IsNullOrWhiteSpace(member.BaptismPastor);
    }

    /// <summary>
    /// Valida dados de batismo do DTO
    /// </summary>
    private void ValidateBaptismDataDto(CreateMemberCompleteDto dto, MemberValidationResult result)
    {
        bool hasBaptismIssues = false;

        // Data de batismo obrigatória
        if (dto.BaptismInfo?.BaptismDate == null)
        {
            result.PendingFields.Add("BaptismDate");
            hasBaptismIssues = true;
        }

        // Local de batismo obrigatório
        if (string.IsNullOrWhiteSpace(dto.BaptismInfo?.BaptismChurch))
        {
            result.PendingFields.Add("BaptismChurch");
            hasBaptismIssues = true;
        }

        // Nome do pastor obrigatório
        if (string.IsNullOrWhiteSpace(dto.BaptismInfo?.BaptismPastor))
        {
            result.PendingFields.Add("BaptismPastor");
            hasBaptismIssues = true;
        }

        // Adicionar BaptismRecord apenas uma vez se houver problemas
        if (hasBaptismIssues)
        {
            result.PendingDataTypes.Add("BaptismRecord");
        }

        // Verificar se todos os dados de batismo estão completos
        result.BaptismDataComplete = dto.BaptismInfo?.BaptismDate != null &&
                                   !string.IsNullOrWhiteSpace(dto.BaptismInfo?.BaptismChurch) &&
                                   !string.IsNullOrWhiteSpace(dto.BaptismInfo?.BaptismPastor);
    }

    /// <summary>
    /// Valida dados médicos do membro
    /// </summary>
    private async Task ValidateMedicalDataAsync(Member member, MemberValidationResult result, CancellationToken cancellationToken)
    {
        // Buscar prontuário médico do membro
        var medicalRecords = await _unitOfWork.Repository<MedicalRecord>()
            .GetAsync(mr => mr.MemberId == member.Id && !mr.IsDeleted, cancellationToken);

        // Dados médicos são obrigatórios para todos os membros
        if (!medicalRecords.Any())
        {
            result.PendingFields.Add("MedicalRecord");
            result.PendingDataTypes.Add("MedicalRecord");
        }
    }

    /// <summary>
    /// Valida dados médicos do DTO
    /// </summary>
    private void ValidateMedicalDataDto(CreateMemberCompleteDto dto, MemberValidationResult result)
    {
        // Dados médicos são opcionais na criação - apenas marcar como pendente se não fornecido
        if (dto.MedicalInfo == null)
        {
            result.PendingFields.Add("MedicalRecord");
            result.PendingDataTypes.Add("MedicalRecord");
        }
    }

    #endregion
}
