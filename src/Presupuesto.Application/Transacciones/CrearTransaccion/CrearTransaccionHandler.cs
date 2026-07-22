using Presupuesto.Application.Abstracciones;
using Presupuesto.Application.Dtos;
using Presupuesto.Domain.Abstracciones;
using Presupuesto.Domain.Transacciones;

namespace Presupuesto.Application.Transacciones.CrearTransaccion;

public sealed class CrearTransaccionHandler(
    IRepositorioTransaccion transacciones,
    IUnidadDeTrabajo unidadDeTrabajo) : IManejadorComando<CrearTransaccionCommand, TransaccionDto>
{
    public async Task<Resultado<TransaccionDto>> Handle(CrearTransaccionCommand command, CancellationToken cancellationToken = default)
    {
        var monto = Dinero.Crear(command.Monto);
        if (monto.EsFallo) return Resultado.Fallo<TransaccionDto>(monto.Error);

        var descripcion = Descripcion.Crear(command.Descripcion);
        if (descripcion.EsFallo) return Resultado.Fallo<TransaccionDto>(descripcion.Error);

        var categoria = Categoria.Crear(command.Categoria);
        if (categoria.EsFallo) return Resultado.Fallo<TransaccionDto>(categoria.Error);

        var transaccion = Transaccion.Crear(command.UsuarioId, monto.Value!, descripcion.Value!, categoria.Value!, command.Fecha, command.Tipo, DateTime.UtcNow);
        if (transaccion.EsFallo) return Resultado.Fallo<TransaccionDto>(transaccion.Error);

        transacciones.Agregar(transaccion.Value!);
        await unidadDeTrabajo.GuardarCambiosAsync(cancellationToken);

        return Resultado.Exito(TransaccionDto.From(transaccion.Value!));
    }
}
