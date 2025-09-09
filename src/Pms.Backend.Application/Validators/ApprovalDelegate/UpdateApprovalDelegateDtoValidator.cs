using FluentValidation;
using Pms.Backend.Application.DTOs.Assignments;

namespace Pms.Backend.Application.Validators.ApprovalDelegate;

/// <summary>
/// Validador para UpdateApprovalDelegateDto
/// </summary>
public class UpdateApprovalDelegateDtoValidator : AbstractValidator<UpdateApprovalDelegateDto>
{
    /// <summary>
    /// Inicializa uma nova instância do UpdateApprovalDelegateDtoValidator
    /// </summary>
    public UpdateApprovalDelegateDtoValidator()
    {
        RuleFor(x => x.Role)
            .NotEmpty()
            .WithMessage("Função é obrigatória")
            .MaximumLength(100)
            .WithMessage("Função deve ter no máximo 100 caracteres");

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Data de início é obrigatória");

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
}
