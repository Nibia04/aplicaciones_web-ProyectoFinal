namespace Presupuesto.Domain.Transacciones;

public interface IRepositorioTransaccion
{
    Task<IReadOnlyList<Transaccion>> ListarPorUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Transaccion>> ListarPorUsuarioFechaAscAsync(Guid usuarioId, CancellationToken cancellationToken = default);
    Task<Transaccion?> ObtenerPorIdYUsuarioAsync(Guid id, Guid usuarioId, CancellationToken cancellationToken = default);
    void Agregar(Transaccion transaccion);
    void Remover(Transaccion transaccion);
}
