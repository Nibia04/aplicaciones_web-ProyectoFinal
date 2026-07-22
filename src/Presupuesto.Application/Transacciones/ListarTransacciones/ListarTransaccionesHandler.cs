using Presupuesto.Application.Abstracciones;
using Presupuesto.Application.Dtos;
using Presupuesto.Domain.Abstracciones;
using Presupuesto.Domain.Transacciones;

namespace Presupuesto.Application.Transacciones.ListarTransacciones;

public sealed class ListarTransaccionesHandler(IRepositorioTransaccion transacciones)
    : IManejadorConsulta<ListarTransaccionesQuery, IReadOnlyList<TransaccionDto>>
{
    public async Task<Resultado<IReadOnlyList<TransaccionDto>>> Handle(ListarTransaccionesQuery query, CancellationToken cancellationToken = default)
    {
        var lista = await transacciones.ListarPorUsuarioAsync(query.UsuarioId, cancellationToken);
        return Resultado.Exito<IReadOnlyList<TransaccionDto>>(lista.Select(TransaccionDto.From).ToList());
    }
}
