namespace Presupuesto.Application.Dtos;

public sealed record ResumenDiarioDto(
    DateOnly Fecha,
    decimal Ingresos,
    decimal Gastos,
    decimal Saldo,
    int CantidadTransacciones);

public sealed record PresupuestoDto(
    decimal SaldoActual,
    decimal TotalIngresos,
    decimal TotalGastos,
    IReadOnlyList<ResumenDiarioDto> ResumenDiario);
