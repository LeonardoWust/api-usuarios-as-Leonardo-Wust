using APIUsuario.Application.DTOs;
using APIUsuario.Application.Interfaces;
using APIUsuario.Domain.Entities;
using FluentValidation;

namespace APIUsuario.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _repository;
    private readonly IValidator<UsuarioCreateDto> _createValidator;
    private readonly IValidator<UsuarioUpdateDto> _updateValidator;

    public UsuarioService(
        IUsuarioRepository repository, 
        IValidator<UsuarioCreateDto> createValidator,
        IValidator<UsuarioUpdateDto> updateValidator)
    {
        _repository = repository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<IEnumerable<UsuarioReadDto>> ListarAsync(CancellationToken ct)
    {
        var usuarios = await _repository.GetAllAsync(ct);
        return usuarios.Select(u => MapToReadDto(u));
    }

    // Este método agora bate EXATAMENTE com a interface
    public async Task<UsuarioReadDto?> ObterAsync(int id, CancellationToken ct)
    {
        var usuario = await _repository.GetByIdAsync(id, ct);
        if (usuario == null) return null;
        return MapToReadDto(usuario);
    }

    public async Task<UsuarioReadDto> CriarAsync(UsuarioCreateDto dto, CancellationToken ct)
    {
        var validacao = await _createValidator.ValidateAsync(dto, ct);
        if (!validacao.IsValid) throw new ValidationException(validacao.Errors);

        if (await _repository.EmailExistsAsync(dto.Email, ct))
            throw new Exception("Email já cadastrado.");

        var idade = DateTime.Now.Year - dto.DataNascimento.Year;
        if (dto.DataNascimento.Date > DateTime.Now.AddYears(-idade)) idade--;
        
        if (idade < 18) throw new Exception("Usuário deve ter pelo menos 18 anos.");

        var usuario = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email.ToLower(),
            Senha = dto.Senha,
            DataNascimento = dto.DataNascimento,
            Telefone = dto.Telefone,
            Ativo = true,
            DataCriacao = DateTime.Now
        };

        await _repository.AddAsync(usuario, ct);
        await _repository.SaveChangesAsync(ct);

        return MapToReadDto(usuario);
    }

    public async Task<UsuarioReadDto> AtualizarAsync(int id, UsuarioUpdateDto dto, CancellationToken ct)
    {
        var validacao = await _updateValidator.ValidateAsync(dto, ct);
        if (!validacao.IsValid) throw new ValidationException(validacao.Errors);

        var usuario = await _repository.GetByIdAsync(id, ct);
        if (usuario == null) throw new Exception("Usuário não encontrado");

        if (dto.Email.ToLower() != usuario.Email)
        {
            if (await _repository.EmailExistsAsync(dto.Email, ct))
                throw new Exception("Este email já está em uso por outro usuário.");
        }

        usuario.Nome = dto.Nome;
        usuario.Email = dto.Email.ToLower();
        usuario.DataNascimento = dto.DataNascimento;
        usuario.Telefone = dto.Telefone;
        usuario.Ativo = dto.Ativo;
        
        await _repository.UpdateAsync(usuario, ct);
        await _repository.SaveChangesAsync(ct);

        return MapToReadDto(usuario);
    }

    public async Task<bool> RemoverAsync(int id, CancellationToken ct)
    {
        var usuario = await _repository.GetByIdAsync(id, ct);
        if (usuario == null) return false;

        await _repository.RemoveAsync(usuario, ct);
        await _repository.SaveChangesAsync(ct);
        return true;
    }

    private static UsuarioReadDto MapToReadDto(Usuario u)
    {
        return new UsuarioReadDto(u.Id, u.Nome, u.Email, u.DataNascimento, u.Telefone, u.Ativo, u.DataCriacao);
    }
}