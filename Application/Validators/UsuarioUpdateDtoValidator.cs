using FluentValidation;
using APIUsuario.Application.DTOs;

namespace APIUsuario.Application.Validators;

public class UsuarioUpdateDtoValidator : AbstractValidator<UsuarioUpdateDto>
{
    public UsuarioUpdateDtoValidator()
    {
        
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .Length(3, 100).WithMessage("Nome deve ter entre 3 e 100 caracteres");

        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório")
            .EmailAddress().WithMessage("Formato de email inválido");

        
        RuleFor(x => x.DataNascimento)
            .NotEmpty().WithMessage("Data de nascimento obrigatória");
        
        
    }
}