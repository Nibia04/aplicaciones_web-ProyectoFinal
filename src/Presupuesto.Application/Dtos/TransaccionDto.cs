using Presupuesto.Domain.Transacciones;

namespace Presupuesto.Application.Dtos;

public sealed record TransaccionDto(
    Guid Id,
    decimal Monto,
    string Descripcion,
    string Categoria,
    DateOnly Fecha,
    TipoTransaccion Tipo)
{
    public static TransaccionDto From(Transaccion transaccion) =>
        new(
            transaccion.Id,
            transaccion.Monto.Monto,
            transaccion.Descripcion.Value,
            transaccion.Categoria.Value,
            transaccion.Fecha,
            transaccion.Tipo);
}
