using Presupuesto.Application.Abstractions;
using Presupuesto.Application.Dtos;
using Presupuesto.Domain.Abstractions;
using Presupuesto.Domain.Transacciones;

namespace Presupuesto.Application.Transacciones.CrearTransaccion;

public sealed class CrearTransaccionHandler(
    ITransaccionRepository transacciones,
    IUnitOfWork unitOfWork) : ICommandHandler<CrearTransaccionCommand, TransaccionDto>
{
    public async Task<Result<TransaccionDto>> Handle(CrearTransaccionCommand command, CancellationToken cancellationToken = default)
    {
        var monto = Dinero.Create(command.Monto);
        if (monto.IsFailure) return Result.Failure<TransaccionDto>(monto.Error);

        var descripcion = Descripcion.Create(command.Descripcion);
        if (descripcion.IsFailure) return Result.Failure<TransaccionDto>(descripcion.Error);

        var categoria = Categoria.Create(command.Categoria);
        if (categoria.IsFailure) return Result.Failure<TransaccionDto>(categoria.Error);

        var transaccion = Transaccion.Crear(command.UsuarioId, monto.Value!, descripcion.Value!, categoria.Value!, command.Fecha, command.Tipo, DateTime.UtcNow);
        if (transaccion.IsFailure) return Result.Failure<TransaccionDto>(transaccion.Error);

        transacciones.Add(transaccion.Value!);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(TransaccionDto.From(transaccion.Value!));
    }
}
