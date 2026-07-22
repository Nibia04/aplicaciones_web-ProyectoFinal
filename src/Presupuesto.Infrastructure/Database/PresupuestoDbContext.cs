using Microsoft.EntityFrameworkCore;
using Presupuesto.Application.Abstractions;
using Presupuesto.Domain.Transacciones;
using Presupuesto.Domain.Usuarios;

namespace Presupuesto.Infrastructure.Database;

public sealed class PresupuestoDbContext(DbContextOptions<PresupuestoDbContext> options)
    : DbContext(options), IUnitOfWork
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Transaccion> Transacciones => Set<Transaccion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PresupuestoDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
