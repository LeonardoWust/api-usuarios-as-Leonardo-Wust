// Importações necessárias para o código funcionar
using System.ComponentModel;
using APIUsuario.Application.DTOs;       
using APIUsuario.Application.Interfaces; 
using APIUsuario.Domain.Entities;        
using FluentValidation;                  

namespace APIUsuario.Application.Services;


public class UsuarioService : IUsuarioService
{
    
    private readonly IUsuarioRepository _repository;          
    private readonly IValidator<UsuarioCreateDto> _validator; 

    
    public UsuarioService(IUsuarioRepository repository, IValidator<UsuarioCreateDto> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    
    public async Task<UsuarioReadDto> CriarAsync(UsuarioCreateDto dto, CancellationToken ct)
    {
        
        var validacao = await _validator.ValidateAsync(dto, ct);
        
        
        if (!validacao.IsValid)
        {
        
            throw new ValidationException(validacao.Errors);
        }

        
        if (await _repository.EmailExistsAsync(dto.Email, ct))
        {
            throw new Exception("Email já cadastrado.");
        }

        
        var idade = DateTime.Now.Year - dto.DataNascimento.Year;
        
        
        if (dto.DataNascimento.Date > DateTime.Now.AddYears(-idade)) idade--;
        
        
        if (idade < 18)
        {
            throw new Exception("Usuário deve ter pelo menos 18 anos.");
        }

        
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

        
        return new UsuarioReadDto(
            usuario.Id, 
            usuario.Nome, 
            usuario.Email, 
            usuario.DataNascimento, 
            usuario.Telefone, 
            usuario.Ativo, 
            usuario.DataCriacao
        );
    }

    
    public async Task<IEnumerable<UsuarioReadDto>> ListarAsync(CancellationToken ct)
    {
        
        var usuarios = await _repository.GetAllAsync(ct);
        
        
        return usuarios.Select(u => new UsuarioReadDto(
            u.Id, u.Nome, u.Email, u.DataNascimento, u.Telefone, u.Ativo, u.DataCriacao
        ));
    }

    
    public async Task<UsuarioReadDto?> ObterAsync(int id, CancellationToken ct)
    {
        
        var usuario = await _repository.GetByIdAsync(id, ct);
        
        
        if (usuario == null) return null;

        
        return new UsuarioReadDto(
            usuario.Id, usuario.Nome, usuario.Email, usuario.DataNascimento, usuario.Telefone, usuario.Ativo, usuario.DataCriacao
        );
    }

    
    public async Task<bool> RemoverAsync(int id, CancellationToken ct)
    {
        
        var usuario = await _repository.GetByIdAsync(id, ct);
        if (usuario == null) return false; 

        
        await _repository.RemoveAsync(usuario, ct);
        
        
        await _repository.SaveChangesAsync(ct);
        
        return true; 
    }

    
    public async Task<UsuarioReadDto> AtualizarAsync(int id, UsuarioUpdateDto dto, CancellationToken ct)
    {
        
        var usuario = await _repository.GetByIdAsync(id, ct);
        
        
        if (usuario == null) throw new Exception("Usuário não encontrado");

        
        usuario.Nome = dto.Nome;
        usuario.Email = dto.Email.ToLower(); 
        usuario.DataNascimento = dto.DataNascimento;
        usuario.Telefone = dto.Telefone;
        usuario.Ativo = dto.Ativo;
        
        
        await _repository.UpdateAsync(usuario, ct);
        
        
        await _repository.SaveChangesAsync(ct);

        
        return new UsuarioReadDto(
            usuario.Id, usuario.Nome, usuario.Email, usuario.DataNascimento, usuario.Telefone, usuario.Ativo, usuario.DataCriacao
        );
    }

    public Task<UsuarioReadDto?> ObterAsync(int id, CategoryAttribute ct)
    {
        throw new NotImplementedException();
    }
}