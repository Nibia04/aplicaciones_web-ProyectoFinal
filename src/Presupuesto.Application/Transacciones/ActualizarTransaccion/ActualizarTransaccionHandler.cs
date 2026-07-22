using Presupuesto.Application.Abstractions;
using Presupuesto.Application.Dtos;
using Presupuesto.Domain.Abstractions;
using Presupuesto.Domain.Transacciones;

namespace Presupuesto.Application.Transacciones.ActualizarTransaccion;

public sealed class ActualizarTransaccionHandler(
    ITransaccionRepository transacciones,
    IUnitOfWork unitOfWork) : ICommandHandler<ActualizarTransaccionCommand, TransaccionDto>
{
    public async Task<Result<TransaccionDto>> Handle(ActualizarTransaccionCommand command, CancellationToken cancellationToken = default)
    {
        var transaccion = await transacciones.GetByIdAndUsuarioAsync(command.TransaccionId, command.UsuarioId, cancellationToken);
        if (transaccion is null) return Result.Failure<TransaccionDto>(TransaccionErrors.NoEncontrada);

        Dinero? monto = null;
        if (command.Monto is not null)
        {
            var montoResult = Dinero.Create(command.Monto.Value);
            if (montoResult.IsFailure) return Result.Failure<TransaccionDto>(montoResult.Error);
            monto = montoResult.Value;
        }

        Descripcion? descripcion = null;
        if (command.Descripcion is not null)
        {
            var descripcionResult = Descripcion.Create(command.Descripcion);
            if (descripcionResult.IsFailure) return Result.Failure<TransaccionDto>(descripcionResult.Error);
            descripcion = descripcionResult.Value;
        }

        Categoria? categoria = null;
        if (command.Categoria is not null)
        {
            var categoriaResult = Categoria.Create(command.Categoria);
            if (categoriaResult.IsFailure) return Result.Failure<TransaccionDto>(categoriaResult.Error);
            categoria = categoriaResult.Value;
        }

        transaccion.Actualizar(monto, descripcion, categoria, command.Fecha, command.Tipo);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(TransaccionDto.From(transaccion));
    }
}
