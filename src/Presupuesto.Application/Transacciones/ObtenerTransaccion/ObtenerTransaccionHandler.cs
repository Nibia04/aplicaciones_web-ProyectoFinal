using Presupuesto.Application.Abstractions;
using Presupuesto.Application.Dtos;
using Presupuesto.Domain.Abstractions;
using Presupuesto.Domain.Transacciones;

namespace Presupuesto.Application.Transacciones.ObtenerTransaccion;

public sealed class ObtenerTransaccionHandler(ITransaccionRepository transacciones)
    : IQueryHandler<ObtenerTransaccionQuery, TransaccionDto>
{
    public async Task<Result<TransaccionDto>> Handle(ObtenerTransaccionQuery query, CancellationToken cancellationToken = default)
    {
        var transaccion = await transacciones.GetByIdAndUsuarioAsync(query.TransaccionId, query.UsuarioId, cancellationToken);
        return transaccion is null
            ? Result.Failure<TransaccionDto>(TransaccionErrors.NoEncontrada)
            : Result.Success(TransaccionDto.From(transaccion));
    }
}
