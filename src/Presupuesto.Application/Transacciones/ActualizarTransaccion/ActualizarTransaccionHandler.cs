using Presupuesto.Application.Abstracciones;
using Presupuesto.Application.Dtos;
using Presupuesto.Domain.Abstracciones;
using Presupuesto.Domain.Transacciones;

namespace Presupuesto.Application.Transacciones.ActualizarTransaccion;

public sealed class ActualizarTransaccionHandler(
    IRepositorioTransaccion transacciones,
    IUnidadDeTrabajo unidadDeTrabajo) : IManejadorComando<ActualizarTransaccionCommand, TransaccionDto>
{
    public async Task<Resultado<TransaccionDto>> Handle(ActualizarTransaccionCommand command, CancellationToken cancellationToken = default)
    {
        var transaccion = await transacciones.ObtenerPorIdYUsuarioAsync(command.TransaccionId, command.UsuarioId, cancellationToken);
        if (transaccion is null) return Resultado.Fallo<TransaccionDto>(ErroresTransaccion.NoEncontrada);

        Dinero? monto = null;
        if (command.Monto is not null)
        {
            var montoResult = Dinero.Crear(command.Monto.Value);
            if (montoResult.EsFallo) return Resultado.Fallo<TransaccionDto>(montoResult.Error);
            monto = montoResult.Value;
        }

        Descripcion? descripcion = null;
        if (command.Descripcion is not null)
        {
            var descripcionResult = Descripcion.Crear(command.Descripcion);
            if (descripcionResult.EsFallo) return Resultado.Fallo<TransaccionDto>(descripcionResult.Error);
            descripcion = descripcionResult.Value;
        }

        Categoria? categoria = null;
        if (command.Categoria is not null)
        {
            var categoriaResult = Categoria.Crear(command.Categoria);
            if (categoriaResult.EsFallo) return Resultado.Fallo<TransaccionDto>(categoriaResult.Error);
            categoria = categoriaResult.Value;
        }

        transaccion.Actualizar(monto, descripcion, categoria, command.Fecha, command.Tipo);
        await unidadDeTrabajo.GuardarCambiosAsync(cancellationToken);

        return Resultado.Exito(TransaccionDto.From(transaccion));
    }
}
