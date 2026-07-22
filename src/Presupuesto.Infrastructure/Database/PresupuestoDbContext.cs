using Microsoft.EntityFrameworkCore;
using Presupuesto.Application.Abstracciones;
using Presupuesto.Domain.Transacciones;
using Presupuesto.Domain.Usuarios;

namespace Presupuesto.Infrastructure.Database;

public sealed class PresupuestoDbContext(DbContextOptions<PresupuestoDbContext> options)
    : DbContext(options), IUnidadDeTrabajo
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Transaccion> Transacciones => Set<Transaccion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PresupuestoDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public Task<int> GuardarCambiosAsync(CancellationToken cancellationToken = default)
    {
        return SaveChangesAsync(cancellationToken);
    }
}
