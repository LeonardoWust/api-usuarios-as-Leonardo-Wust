namespace APIUsuario.Application.DTOs;

public record UsuarioCreateDto(
    string Nome,
    string Email,
    string Senha,
    DateTime DataNascimento,
    string? Telefone
);