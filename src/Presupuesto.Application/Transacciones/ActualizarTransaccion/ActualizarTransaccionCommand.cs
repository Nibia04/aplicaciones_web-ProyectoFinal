using Presupuesto.Domain.Transacciones;

namespace Presupuesto.Application.Transacciones.ActualizarTransaccion;

public sealed record ActualizarTransaccionCommand(
    Guid UsuarioId,
    Guid TransaccionId,
    decimal? Monto,
    string? Descripcion,
    string? Categoria,
    DateOnly? Fecha,
    TipoTransaccion? Tipo);
