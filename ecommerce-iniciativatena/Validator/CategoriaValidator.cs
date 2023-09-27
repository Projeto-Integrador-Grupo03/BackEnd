using ecommerce_iniciativatena.Model;
using FluentValidation;

namespace ecommerce_iniciativatena.Validator
{
    public class CategoriaValidator : AbstractValidator<Categoria>
    {
        public CategoriaValidator() 
        {
            RuleFor(n => n.Nome)
              .NotEmpty()
              .MinimumLength(5)
              .MaximumLength(255);

            RuleFor(d => d.Descricao)
                .NotEmpty()
                .MinimumLength(10)
                .MaximumLength(255);
        }
    }
}
