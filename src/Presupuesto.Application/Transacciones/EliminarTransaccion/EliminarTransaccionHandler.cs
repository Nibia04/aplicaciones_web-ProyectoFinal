using Presupuesto.Application.Abstracciones;
using Presupuesto.Domain.Abstracciones;
using Presupuesto.Domain.Transacciones;

namespace Presupuesto.Application.Transacciones.EliminarTransaccion;

public sealed class EliminarTransaccionHandler(
    IRepositorioTransaccion transacciones,
    IUnidadDeTrabajo unidadDeTrabajo) : IManejadorComando<EliminarTransaccionCommand>
{
    public async Task<Resultado> Handle(EliminarTransaccionCommand command, CancellationToken cancellationToken = default)
    {
        var transaccion = await transacciones.ObtenerPorIdYUsuarioAsync(command.TransaccionId, command.UsuarioId, cancellationToken);
        if (transaccion is null) return Resultado.Fallo(ErroresTransaccion.NoEncontrada);

        transacciones.Remover(transaccion);
        await unidadDeTrabajo.GuardarCambiosAsync(cancellationToken);

        return Resultado.Exito();
    }
}
