using System.ComponentModel;
using APIUsuario.Application.DTOs;

namespace APIUsuario.Application.Interfaces;

public interface IUsuarioService
{
    Task<IEnumerable<UsuarioReadDto>> ListarAsync(CancellationToken ct);
    Task<UsuarioReadDto?> ObterAsync(int id, CategoryAttribute ct);
    Task<UsuarioReadDto> CriarAsync(UsuarioCreateDto dto, CancellationToken ct);
    Task<UsuarioReadDto> AtualizarAsync(int id, UsuarioUpdateDto dto, CancellationToken ct);
    Task<bool> RemoverAsync (int id, CancellationToken ct);
    Task<object?> ObterAsync(int id, CancellationToken ct);
}