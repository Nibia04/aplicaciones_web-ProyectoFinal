using Microsoft.EntityFrameworkCore;
using Presupuesto.Application.Abstracciones;
using Presupuesto.Domain.Transacciones;
using Presupuesto.Domain.Usuarios;

namespace Presupuesto.Infrastructure.Database;

public sealed class PresupuestoDbContext(
    DbContextOptions<PresupuestoDbContext> options,
    IDespachadorEventosDominio despachadorEventos)
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

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entidadesConEventos = ChangeTracker
            .Entries<Domain.Abstracciones.Entidad>()
            .Select(entry => entry.Entity)
            .Where(entidad => entidad.EventosDominio.Count > 0)
            .ToList();

        var eventos = entidadesConEventos
            .SelectMany(entidad => entidad.EventosDominio)
            .ToList();

        var cambios = await base.SaveChangesAsync(cancellationToken);

        if (eventos.Count > 0)
        {
            await despachadorEventos.DespacharAsync(eventos, cancellationToken);
            entidadesConEventos.ForEach(entidad => entidad.LimpiarEventosDominio());
        }

        return cambios;
    }
}
