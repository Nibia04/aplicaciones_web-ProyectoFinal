namespace Presupuesto.Domain.Transacciones;

public interface ITransaccionRepository
{
    Task<IReadOnlyList<Transaccion>> ListByUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Transaccion>> ListByUsuarioFechaAscAsync(Guid usuarioId, CancellationToken cancellationToken = default);
    Task<Transaccion?> GetByIdAndUsuarioAsync(Guid id, Guid usuarioId, CancellationToken cancellationToken = default);
    void Add(Transaccion transaccion);
    void Remove(Transaccion transaccion);
}
