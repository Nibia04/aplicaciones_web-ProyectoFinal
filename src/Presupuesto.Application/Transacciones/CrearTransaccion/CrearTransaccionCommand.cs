using Presupuesto.Domain.Transacciones;

namespace Presupuesto.Application.Transacciones.CrearTransaccion;

public sealed record CrearTransaccionCommand(
    Guid UsuarioId,
    decimal Monto,
    string Descripcion,
    string Categoria,
    DateOnly Fecha,
    TipoTransaccion Tipo);
