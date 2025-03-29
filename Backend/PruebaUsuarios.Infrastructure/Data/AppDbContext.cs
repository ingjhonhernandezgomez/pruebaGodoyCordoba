using Microsoft.EntityFrameworkCore;
using PruebaUsuarios.Domain.Entities;

namespace PruebaUsuarios.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<Usuario> Usuarios => Set<Usuario>();

}