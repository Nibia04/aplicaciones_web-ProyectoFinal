namespace Presupuesto.Application.Transacciones.EliminarTransaccion;

public sealed record EliminarTransaccionCommand(Guid UsuarioId, Guid TransaccionId);
