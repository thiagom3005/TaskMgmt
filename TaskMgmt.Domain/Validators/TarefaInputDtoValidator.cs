using FluentValidation;
using TaskMgmt.Domain.Models;

namespace TaskMgmt.Domain.Validators
{
    public class TarefaInputDtoValidator : AbstractValidator<TarefaInputDto>
    {
        public TarefaInputDtoValidator()
        {
            RuleFor(x => x.Titulo)
                .NotEmpty().WithMessage("O título é obrigatório.")
                .MaximumLength(100).WithMessage("O título deve ter no máximo 100 caracteres.");

            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("A descrição é obrigatória.")
                .MaximumLength(500).WithMessage("A descrição deve ter no máximo 500 caracteres.");

            RuleFor(x => x.DataVencimento)
                .GreaterThan(DateTime.MinValue).WithMessage("A data de vencimento é obrigatória.");
        }
    }
}
