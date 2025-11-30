using FluentValidation;
using APIUsuario.Application.DTOs;

namespace APIUsuario.Application.Validators;

public class UsuarioUpdateDtoValidator : AbstractValidator<UsuarioUpdateDto>
{
    public UsuarioUpdateDtoValidator()
    {
        // 1. Regras para o Nome (Mesma do Create)
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .Length(3, 100).WithMessage("Nome deve ter entre 3 e 100 caracteres");

        // 2. Regras para o Email (Formato apenas)
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório")
            .EmailAddress().WithMessage("Formato de email inválido");

        // 3. Regras para Data de Nascimento
        RuleFor(x => x.DataNascimento)
            .NotEmpty().WithMessage("Data de nascimento obrigatória");
        
        // NOTA: Não validamos a SENHA aqui, porque na atualização
        // geralmente não obrigamos o usuário a trocar de senha toda vez.
    }
}