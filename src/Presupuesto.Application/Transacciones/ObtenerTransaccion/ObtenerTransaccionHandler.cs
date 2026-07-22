using Presupuesto.Application.Abstracciones;
using Presupuesto.Application.Dtos;
using Presupuesto.Domain.Abstracciones;
using Presupuesto.Domain.Transacciones;

namespace Presupuesto.Application.Transacciones.ObtenerTransaccion;

public sealed class ObtenerTransaccionHandler(IRepositorioTransaccion transacciones)
    : IManejadorConsulta<ObtenerTransaccionQuery, TransaccionDto>
{
    public async Task<Resultado<TransaccionDto>> Handle(ObtenerTransaccionQuery query, CancellationToken cancellationToken = default)
    {
        var transaccion = await transacciones.ObtenerPorIdYUsuarioAsync(query.TransaccionId, query.UsuarioId, cancellationToken);
        return transaccion is null
            ? Resultado.Fallo<TransaccionDto>(ErroresTransaccion.NoEncontrada)
            : Resultado.Exito(TransaccionDto.From(transaccion));
    }
}
