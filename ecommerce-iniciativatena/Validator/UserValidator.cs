using ecommerce_iniciativatena.Model;
using FluentValidation;

namespace ecommerce_iniciativatena.Validator
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator() 
        {
            RuleFor(u => u.Nome)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(u => u.Usuario)
                .NotEmpty()
                .EmailAddress();

            RuleFor(u => u.Senha)
                .NotEmpty()
                .MaximumLength(8);

            RuleFor(u => u.Foto)
                .MaximumLength(5000);
        }
    }
}
