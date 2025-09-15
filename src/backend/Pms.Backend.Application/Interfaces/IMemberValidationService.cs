using Pms.Backend.Application.DTOs.Members;
using Pms.Backend.Domain.Entities;

namespace Pms.Backend.Application.Interfaces;

/// <summary>
/// Interface para serviços de validação de membros
/// </summary>
public interface IMemberValidationService
{
    /// <summary>
    /// Valida se um membro possui todos os dados mínimos obrigatórios
    /// </summary>
    /// <param name="member">Membro a ser validado</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da validação com detalhes do que está pendente</returns>
    Task<MemberValidationResult> ValidateMemberDataAsync(Member member, CancellationToken cancellationToken = default);

    /// <summary>
    /// Valida se um DTO de criação de membro possui todos os dados mínimos obrigatórios
    /// </summary>
    /// <param name="dto">DTO de criação de membro</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Resultado da validação com detalhes do que está pendente</returns>
    Task<MemberValidationResult> ValidateCreateMemberDtoAsync(CreateMemberCompleteDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Calcula a idade do membro baseado no dia 1º de junho do ano vigente
    /// </summary>
    /// <param name="dateOfBirth">Data de nascimento do membro</param>
    /// <param name="referenceYear">Ano de referência (padrão: ano atual)</param>
    /// <returns>Idade do membro em 1º de junho do ano de referência</returns>
    int CalculateAgeForBaptismRequirement(DateTime dateOfBirth, int? referenceYear = null);

    /// <summary>
    /// Verifica se os dados de batismo são obrigatórios para o membro
    /// </summary>
    /// <param name="dateOfBirth">Data de nascimento do membro</param>
    /// <param name="referenceYear">Ano de referência (padrão: ano atual)</param>
    /// <returns>True se os dados de batismo são obrigatórios</returns>
    bool IsBaptismDataRequired(DateTime dateOfBirth, int? referenceYear = null);
}

/// <summary>
/// Resultado da validação de dados do membro
/// </summary>
public class MemberValidationResult
{
    /// <summary>
    /// Indica se o membro possui todos os dados obrigatórios
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// Indica se o membro pode ser ativado
    /// </summary>
    public bool CanBeActivated { get; set; }

    /// <summary>
    /// Motivo da inatividade (se aplicável)
    /// </summary>
    public string? InactivityReason { get; set; }

    /// <summary>
    /// Lista de campos pendentes
    /// </summary>
    public List<string> PendingFields { get; set; } = new();

    /// <summary>
    /// Lista de tipos de dados pendentes (para exibição no frontend)
    /// </summary>
    public List<string> PendingDataTypes { get; set; } = new();

    /// <summary>
    /// Mensagem de erro detalhada
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Indica se os dados de batismo são obrigatórios para este membro
    /// </summary>
    public bool BaptismDataRequired { get; set; }

    /// <summary>
    /// Indica se os dados de batismo estão completos (se obrigatórios)
    /// </summary>
    public bool BaptismDataComplete { get; set; }
}
