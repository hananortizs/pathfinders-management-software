using Pms.Backend.Application.DTOs;
using Pms.Backend.Application.DTOs.Members;
using Pms.Backend.Application.Interfaces;
using Pms.Backend.Domain.Entities;
using Pms.Backend.Domain.Enums;
using Pms.Backend.Domain.Helpers;

namespace Pms.Backend.Application.Services.Members;

/// <summary>
/// Serviço responsável pela validação e ativação de membros.
/// Implementa as regras de negócio para determinar se um membro pode ser ativado.
/// </summary>
public class MemberActivationService : IMemberActivationService
{
    private readonly IRepository<Member> _memberRepository;
    private readonly IRepository<Domain.Entities.Address> _addressRepository;
    private readonly IRepository<Contact> _contactRepository;
    private readonly IRepository<MedicalRecord> _medicalRecordRepository;
    private readonly IRepository<BaptismRecord> _baptismRecordRepository;
    private readonly ILoggingService _loggingService;

    /// <summary>
    /// Inicializa uma nova instância do serviço de ativação de membros.
    /// </summary>
    /// <param name="memberRepository">Repositório de membros.</param>
    /// <param name="addressRepository">Repositório de endereços.</param>
    /// <param name="contactRepository">Repositório de contatos.</param>
    /// <param name="medicalRecordRepository">Repositório de registros médicos.</param>
    /// <param name="baptismRecordRepository">Repositório de registros de batismo.</param>
    /// <param name="loggingService">Serviço de logging.</param>
    public MemberActivationService(
        IRepository<Member> memberRepository,
        IRepository<Domain.Entities.Address> addressRepository,
        IRepository<Contact> contactRepository,
        IRepository<MedicalRecord> medicalRecordRepository,
        IRepository<BaptismRecord> baptismRecordRepository,
        ILoggingService loggingService)
    {
        _memberRepository = memberRepository;
        _addressRepository = addressRepository;
        _contactRepository = contactRepository;
        _medicalRecordRepository = medicalRecordRepository;
        _baptismRecordRepository = baptismRecordRepository;
        _loggingService = loggingService;
    }

    /// <summary>
    /// Valida se um membro pode ser ativado
    /// </summary>
    public async Task<BaseResponse<MemberActivationValidationDto>> ValidateMemberActivationAsync(
        string memberId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _loggingService.LogInformation("Iniciando validação de ativação para membro {MemberId}", memberId);

            var member = await _memberRepository.GetByIdAsync(Guid.Parse(memberId), cancellationToken);
            if (member == null)
            {
                return BaseResponse<MemberActivationValidationDto>.ErrorResult("Membro não encontrado");
            }

            var validation = new MemberActivationValidationDto
            {
                MemberId = memberId,
                MemberName = member.DisplayName,
                CanBeActivated = true,
                ValidationDate = DateTime.UtcNow
            };

            // 1. Verificar se há endereço principal ativo
            var hasPrimaryAddress = await ValidatePrimaryAddressAsync(memberId, cancellationToken);
            validation.AddressValidation = hasPrimaryAddress;
            if (!hasPrimaryAddress.IsValid)
            {
                validation.CanBeActivated = false;
            }

            // 2. Verificar se há ficha médica completa
            var medicalValidation = await ValidateMedicalRecordAsync(memberId, cancellationToken);
            validation.MedicalValidation = medicalValidation;
            if (!medicalValidation.IsValid)
            {
                validation.CanBeActivated = false;
            }

            // 3. Verificar se há contatos cadastrados
            var contactValidation = await ValidateContactsAsync(memberId, member.DateOfBirth, cancellationToken);
            validation.ContactValidation = contactValidation;
            if (!contactValidation.IsValid)
            {
                validation.CanBeActivated = false;
            }

            // 4. Verificar batismo (apenas para maiores de 16 anos)
            var baptismValidation = await ValidateBaptismAsync(memberId, member.DateOfBirth, cancellationToken);
            validation.BaptismValidation = baptismValidation;
            if (!baptismValidation.IsValid)
            {
                validation.CanBeActivated = false;
            }

            _loggingService.LogInformation("Validação de ativação concluída para membro {MemberId}. Pode ser ativado: {CanBeActivated}", memberId, validation.CanBeActivated);

            return BaseResponse<MemberActivationValidationDto>.SuccessResult(validation);
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Erro ao validar ativação do membro {MemberId}", memberId);
            return BaseResponse<MemberActivationValidationDto>.ErrorResult("Erro interno ao validar ativação do membro");
        }
    }

    /// <summary>
    /// Ativa um membro se todas as validações passarem
    /// </summary>
    public async Task<BaseResponse<MemberDto>> ActivateMemberAsync(
        string memberId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _loggingService.LogInformation("Iniciando ativação do membro {MemberId}", memberId);

            // Validar se pode ser ativado
            var validation = await ValidateMemberActivationAsync(memberId, cancellationToken);
            if (!validation.IsSuccess || validation.Data?.CanBeActivated != true)
            {
                return BaseResponse<MemberDto>.ErrorResult("Membro não pode ser ativado. Verifique as validações necessárias.");
            }

            // Ativar o membro
            var member = await _memberRepository.GetByIdAsync(Guid.Parse(memberId), cancellationToken);
            if (member == null)
            {
                return BaseResponse<MemberDto>.ErrorResult("Membro não encontrado");
            }

            member.Status = MemberStatus.Active;
            member.UpdatedAtUtc = DateTime.UtcNow;

            await _memberRepository.UpdateAsync(member, cancellationToken);

            _loggingService.LogInformation("Membro {MemberId} ativado com sucesso", memberId);

            // Mapear para DTO (implementar mapeamento)
            var memberDto = new MemberDto(); // TODO: Implementar mapeamento
            return BaseResponse<MemberDto>.SuccessResult(memberDto);
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Erro ao ativar membro {MemberId}", memberId);
            return BaseResponse<MemberDto>.ErrorResult("Erro interno ao ativar membro");
        }
    }

    /// <summary>
    /// Valida se há endereço principal ativo
    /// </summary>
    private async Task<ValidationItemDto> ValidatePrimaryAddressAsync(string memberId, CancellationToken cancellationToken)
    {
        var addresses = await _addressRepository.GetAllAsync(
            a => a.EntityId == Guid.Parse(memberId) && a.EntityType == "Member" && !a.IsDeleted && a.IsPrimary,
            cancellationToken);

        return new ValidationItemDto
        {
            IsValid = addresses.Any(),
            Message = addresses.Any()
                ? "Endereço principal encontrado"
                : "Endereço principal é obrigatório para ativação"
        };
    }

    /// <summary>
    /// Valida se há ficha médica completa
    /// </summary>
    private async Task<ValidationItemDto> ValidateMedicalRecordAsync(string memberId, CancellationToken cancellationToken)
    {
        var memberGuid = Guid.Parse(memberId);
        var medicalRecord = await _medicalRecordRepository.GetFirstOrDefaultAsync(
            m => m.MemberId == memberGuid && !m.IsDeleted,
            cancellationToken);

        if (medicalRecord == null)
        {
            return new ValidationItemDto
            {
                IsValid = false,
                Message = "Ficha médica é obrigatória para ativação"
            };
        }

        var isComplete = medicalRecord.IsCompleteForActivation();
        return new ValidationItemDto
        {
            IsValid = isComplete,
            Message = isComplete
                ? "Ficha médica está completa"
                : "Ficha médica deve ser preenchida para ativação"
        };
    }

    /// <summary>
    /// Valida se há contatos cadastrados
    /// </summary>
    private async Task<ValidationItemDto> ValidateContactsAsync(string memberId, DateTime dateOfBirth, CancellationToken cancellationToken)
    {
        var contacts = await _contactRepository.GetAllAsync(
            c => c.EntityId == Guid.Parse(memberId) && c.EntityType == "Member" && !c.IsDeleted,
            cancellationToken);

        if (!contacts.Any())
        {
            return new ValidationItemDto
            {
                IsValid = false,
                Message = "Pelo menos um contato é obrigatório para ativação"
            };
        }

        // Para menores de idade, verificar se há contato do responsável
        var age = AgeHelper.CalculateAge(dateOfBirth);
        if (age < 18)
        {
            var hasLegalGuardian = contacts.Any(c => c.Category == Pms.Backend.Domain.Enums.ContactCategory.LegalGuardian);
            if (!hasLegalGuardian)
            {
                return new ValidationItemDto
                {
                    IsValid = false,
                    Message = "Contato do responsável é obrigatório para menores de idade"
                };
            }
        }

        return new ValidationItemDto
        {
            IsValid = true,
            Message = "Contatos validados com sucesso"
        };
    }

    /// <summary>
    /// Valida se há batismo (apenas para maiores de 16 anos)
    /// </summary>
    private async Task<ValidationItemDto> ValidateBaptismAsync(string memberId, DateTime dateOfBirth, CancellationToken cancellationToken)
    {
        var age = AgeHelper.CalculateAge(dateOfBirth);

        // Batismo é obrigatório apenas para maiores de 16 anos
        if (age < 16)
        {
            return new ValidationItemDto
            {
                IsValid = true,
                Message = "Batismo não é obrigatório para menores de 16 anos"
            };
        }

        var baptismRecords = await _baptismRecordRepository.GetAllAsync(
            b => b.MemberId == Guid.Parse(memberId) && !b.IsDeleted,
            cancellationToken);

        return new ValidationItemDto
        {
            IsValid = baptismRecords.Any(),
            Message = baptismRecords.Any()
                ? "Batismo encontrado"
                : "Batismo é obrigatório para maiores de 16 anos"
        };
    }
}

