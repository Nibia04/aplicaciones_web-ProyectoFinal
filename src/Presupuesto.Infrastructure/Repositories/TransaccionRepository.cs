using Microsoft.EntityFrameworkCore;
using Presupuesto.Domain.Transacciones;
using Presupuesto.Infrastructure.Database;

namespace Presupuesto.Infrastructure.Repositories;

public sealed class TransaccionRepository(PresupuestoDbContext dbContext) : ITransaccionRepository
{
    public async Task<IReadOnlyList<Transaccion>> ListByUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Transacciones
            .Where(transaccion => transaccion.UsuarioId == usuarioId)
            .OrderByDescending(transaccion => transaccion.Fecha)
            .ThenByDescending(transaccion => transaccion.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Transaccion>> ListByUsuarioFechaAscAsync(Guid usuarioId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Transacciones
            .Where(transaccion => transaccion.UsuarioId == usuarioId)
            .OrderBy(transaccion => transaccion.Fecha)
            .ToListAsync(cancellationToken);
    }

    public Task<Transaccion?> GetByIdAndUsuarioAsync(Guid id, Guid usuarioId, CancellationToken cancellationToken = default)
    {
        return dbContext.Transacciones
            .FirstOrDefaultAsync(transaccion => transaccion.Id == id && transaccion.UsuarioId == usuarioId, cancellationToken);
    }

    public void Add(Transaccion transaccion) => dbContext.Transacciones.Add(transaccion);

    public void Remove(Transaccion transaccion) => dbContext.Transacciones.Remove(transaccion);
}
