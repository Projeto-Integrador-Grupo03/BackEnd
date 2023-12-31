﻿using ecommerce_iniciativatena.Model;
using ecommerce_iniciativatena.Service;
using FluentValidation;

namespace ecommerce_iniciativatena.Validator
{
    public class CategoriaValidator : AbstractValidator<Categoria>
    {
        //private readonly ICategoriaService _categoriaService;

        public CategoriaValidator() 
        {
            RuleFor(p => p.Nome)
              .NotEmpty()
              .MinimumLength(5)
              .MaximumLength(255);

            RuleFor(p => p.Descricao)
                .NotEmpty()
                .MinimumLength(10)
                .MaximumLength(255);
        }
    }
}
