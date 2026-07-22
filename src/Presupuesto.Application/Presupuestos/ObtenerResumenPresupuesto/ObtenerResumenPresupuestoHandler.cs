using Presupuesto.Application.Abstracciones;
using Presupuesto.Application.Dtos;
using Presupuesto.Domain.Abstracciones;
using Presupuesto.Domain.Transacciones;

namespace Presupuesto.Application.Presupuestos.ObtenerResumenPresupuesto;

public sealed class ObtenerResumenPresupuestoHandler(IRepositorioTransaccion transacciones)
    : IManejadorConsulta<ObtenerResumenPresupuestoQuery, PresupuestoDto>
{
    public async Task<Resultado<PresupuestoDto>> Handle(ObtenerResumenPresupuestoQuery query, CancellationToken cancellationToken = default)
    {
        var lista = await transacciones.ListarPorUsuarioFechaAscAsync(query.UsuarioId, cancellationToken);
        var totalIngresos = lista.Where(t => t.Tipo == TipoTransaccion.Ingreso).Sum(t => t.Monto.Monto);
        var totalGastos = lista.Where(t => t.Tipo == TipoTransaccion.Gasto).Sum(t => t.Monto.Monto);

        var resumenDiario = lista
            .GroupBy(t => t.Fecha)
            .Select(g =>
            {
                var ingresos = g.Where(t => t.Tipo == TipoTransaccion.Ingreso).Sum(t => t.Monto.Monto);
                var gastos = g.Where(t => t.Tipo == TipoTransaccion.Gasto).Sum(t => t.Monto.Monto);
                return new ResumenDiarioDto(g.Key, ingresos, gastos, ingresos - gastos, g.Count());
            })
            .ToList();

        return Resultado.Exito(new PresupuestoDto(totalIngresos - totalGastos, totalIngresos, totalGastos, resumenDiario));
    }
}
