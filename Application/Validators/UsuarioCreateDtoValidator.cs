using FluentValidation;
using APIUsuario.Application.DTOs;

namespace APIUsuarios.Application.Validators;

public class UsuarioCreateDtoValidator : AbstractValidator<UsuarioCreateDto>
{
    public UsuarioCreateDtoValidator()
    {
        RuleFor(x => x.Nome)
        .NotEmpty().WithMessage("Nome é obrigatório")
        .Length(3, 100).WithMessage("Nome deve ter entre 3 e 100 caracteres");

        RuleFor(x => x.Email)
        .NotEmpty().WithMessage("Email é obrigatório")
        .EmailAddress().WithMessage("Formato de email inválido");

        RuleFor(x => x.Senha)
        .NotEmpty().WithMessage("Senha é obrigatória")
        .MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres");

        RuleFor(x => x.DataNascimento)
        .NotEmpty().WithMessage("Data de nascimento obrigatória");

    }
}