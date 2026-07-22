using Presupuesto.Application.Abstractions;
using Presupuesto.Application.Dtos;
using Presupuesto.Domain.Abstractions;
using Presupuesto.Domain.Transacciones;

namespace Presupuesto.Application.Transacciones.ListarTransacciones;

public sealed class ListarTransaccionesHandler(ITransaccionRepository transacciones)
    : IQueryHandler<ListarTransaccionesQuery, IReadOnlyList<TransaccionDto>>
{
    public async Task<Result<IReadOnlyList<TransaccionDto>>> Handle(ListarTransaccionesQuery query, CancellationToken cancellationToken = default)
    {
        var lista = await transacciones.ListByUsuarioAsync(query.UsuarioId, cancellationToken);
        return Result.Success<IReadOnlyList<TransaccionDto>>(lista.Select(TransaccionDto.From).ToList());
    }
}
