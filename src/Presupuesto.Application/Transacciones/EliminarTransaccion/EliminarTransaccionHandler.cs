using Presupuesto.Application.Abstractions;
using Presupuesto.Domain.Abstractions;
using Presupuesto.Domain.Transacciones;

namespace Presupuesto.Application.Transacciones.EliminarTransaccion;

public sealed class EliminarTransaccionHandler(
    ITransaccionRepository transacciones,
    IUnitOfWork unitOfWork) : ICommandHandler<EliminarTransaccionCommand>
{
    public async Task<Result> Handle(EliminarTransaccionCommand command, CancellationToken cancellationToken = default)
    {
        var transaccion = await transacciones.GetByIdAndUsuarioAsync(command.TransaccionId, command.UsuarioId, cancellationToken);
        if (transaccion is null) return Result.Failure(TransaccionErrors.NoEncontrada);

        transacciones.Remove(transaccion);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
