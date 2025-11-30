using APIUsuario.Application.Interfaces;
using APIUsuario.Domain.Entities;
using APIUsuario.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace APIUsuario.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Usuario usuario, CancellationToken ct)
    {
        await _context.Usuarios.AddAsync(usuario, ct);

    }

    public async Task<IEnumerable<Usuario>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Usuarios.AsNoTracking().ToListAsync(ct);
    }

    public async Task<Usuario?> GetByEmailAsync(string email, CancellationToken ct)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email, ct);
    }

    public async Task<Usuario?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _context.Usuarios.FindAsync(new object[] {id}, ct);
    }

    public async Task RemoveAsync(Usuario usuario, CancellationToken ct)
    {
        usuario.Ativo = false;
        usuario.DataAtualizacao = DateTime.Now;

        _context.Usuarios.Update(usuario);

        await Task.CompletedTask;
    }

    public async Task UpdateAsync(Usuario usuario, CancellationToken ct)
    {
        usuario.DataAtualizacao = DateTime.Now;
        _context.Usuarios.Update(usuario);
        await Task.CompletedTask;
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken ct)
    {
        return await _context.Usuarios.AnyAsync(u => u.Email == email, ct);
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct)
    {
        return await _context.SaveChangesAsync(ct);
    }
}