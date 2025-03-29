using Microsoft.EntityFrameworkCore;
using PruebaUsuarios.Application.Interfaces;
using PruebaUsuarios.Domain.Entities;
using PruebaUsuarios.Infrastructure.Data;

namespace PruebaUsuarios.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Usuario>> GetAllAsync() => await _context.Usuarios.ToListAsync();

    public async Task<Usuario?> GetByIdAsync(int id) => await _context.Usuarios.FindAsync(id);

    public async Task<Usuario> CreateAsync(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task UpdateAsync(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var usuario = await GetByIdAsync(id);
        if (usuario != null)
        {
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
        }
    }
}