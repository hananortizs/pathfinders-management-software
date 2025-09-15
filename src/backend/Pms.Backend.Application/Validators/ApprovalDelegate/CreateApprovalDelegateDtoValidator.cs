using FluentValidation;
using Pms.Backend.Application.DTOs.Assignments;

namespace Pms.Backend.Application.Validators.ApprovalDelegate;

/// <summary>
/// Validador para CreateApprovalDelegateDto
/// </summary>
public class CreateApprovalDelegateDtoValidator : AbstractValidator<CreateApprovalDelegateDto>
{
    /// <summary>
    /// Inicializa uma nova instância do CreateApprovalDelegateDtoValidator
    /// </summary>
    public CreateApprovalDelegateDtoValidator()
    {
        RuleFor(x => x.DelegatedFromAssignmentId)
            .NotEmpty()
            .WithMessage("ID da atribuição do delegante é obrigatório");

        RuleFor(x => x.DelegatedToAssignmentId)
            .NotEmpty()
            .WithMessage("ID da atribuição do delegado é obrigatório")
            .NotEqual(x => x.DelegatedFromAssignmentId)
            .WithMessage("O delegado não pode ser o mesmo que o delegante");

        RuleFor(x => x.ScopeType)
            .IsInEnum()
            .WithMessage("Tipo de escopo inválido");

        RuleFor(x => x.ScopeId)
            .NotEmpty()
            .WithMessage("ID do escopo é obrigatório");

        RuleFor(x => x.Role)
            .NotEmpty()
            .WithMessage("Função é obrigatória")
            .MaximumLength(100)
            .WithMessage("Função deve ter no máximo 100 caracteres");

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Data de início é obrigatória")
            .Must(BeInTheFuture)
            .WithMessage("Data de início deve ser no futuro");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithMessage("Data de fim é obrigatória")
            .GreaterThan(x => x.StartDate)
            .WithMessage("Data de fim deve ser posterior à data de início");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Motivo é obrigatório")
            .MaximumLength(500)
            .WithMessage("Motivo deve ter no máximo 500 caracteres");

        RuleFor(x => x.Notes)
            .MaximumLength(1000)
            .WithMessage("Observações devem ter no máximo 1000 caracteres");
    }

    /// <summary>
    /// Valida se a data está no futuro
    /// </summary>
    /// <param name="date">Data a ser validada</param>
    /// <returns>True se a data estiver no futuro</returns>
    private static bool BeInTheFuture(DateTime date)
    {
        return date > DateTime.UtcNow;
    }
}
