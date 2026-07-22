using Microsoft.EntityFrameworkCore;
using Presupuesto.Domain.Transacciones;
using Presupuesto.Infrastructure.Database;

namespace Presupuesto.Infrastructure.Repositorios;

public sealed class RepositorioTransaccion(PresupuestoDbContext dbContext) : IRepositorioTransaccion
{
    public async Task<IReadOnlyList<Transaccion>> ListarPorUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Transacciones
            .Where(transaccion => transaccion.UsuarioId == usuarioId)
            .OrderByDescending(transaccion => transaccion.Fecha)
            .ThenByDescending(transaccion => transaccion.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Transaccion>> ListarPorUsuarioFechaAscAsync(Guid usuarioId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Transacciones
            .Where(transaccion => transaccion.UsuarioId == usuarioId)
            .OrderBy(transaccion => transaccion.Fecha)
            .ToListAsync(cancellationToken);
    }

    public Task<Transaccion?> ObtenerPorIdYUsuarioAsync(Guid id, Guid usuarioId, CancellationToken cancellationToken = default)
    {
        return dbContext.Transacciones
            .FirstOrDefaultAsync(transaccion => transaccion.Id == id && transaccion.UsuarioId == usuarioId, cancellationToken);
    }

    public void Agregar(Transaccion transaccion) => dbContext.Transacciones.Add(transaccion);

    public void Remover(Transaccion transaccion) => dbContext.Transacciones.Remove(transaccion);
}
