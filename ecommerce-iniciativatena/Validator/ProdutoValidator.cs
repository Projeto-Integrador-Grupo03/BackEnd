using ecommerce_iniciativatena.Model;
using FluentValidation;

namespace ecommerce_iniciativatena.Validator
{
    public class ProdutoValidator : AbstractValidator<Produto>
    {
        public ProdutoValidator()
        {
            RuleFor(p => p.Nome)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(255);

            RuleFor(p => p.Duracao)
                .NotEmpty()
                .MinimumLength(2)
                .MaximumLength(255);

        }

    }
}
