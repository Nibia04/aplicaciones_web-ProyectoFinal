using Presupuesto.Domain.Abstractions;

namespace Presupuesto.Domain.Transacciones;

public sealed class Transaccion : Entity
{
    private Transaccion() { }

    private Transaccion(
        Guid usuarioId,
        Dinero monto,
        Descripcion descripcion,
        Categoria categoria,
        DateOnly fecha,
        TipoTransaccion tipo,
        DateTime creadoEnUtc)
    {
        UsuarioId = usuarioId;
        Monto = monto;
        Descripcion = descripcion;
        Categoria = categoria;
        Fecha = fecha;
        Tipo = tipo;
        CreadoEnUtc = creadoEnUtc;
    }

    public Guid UsuarioId { get; private set; }
    public Dinero Monto { get; private set; } = null!;
    public Descripcion Descripcion { get; private set; } = null!;
    public Categoria Categoria { get; private set; } = null!;
    public DateOnly Fecha { get; private set; }
    public TipoTransaccion Tipo { get; private set; }
    public DateTime CreadoEnUtc { get; private set; }

    public static Result<Transaccion> Crear(
        Guid usuarioId,
        Dinero monto,
        Descripcion descripcion,
        Categoria categoria,
        DateOnly fecha,
        TipoTransaccion tipo,
        DateTime utcNow)
    {
        var transaccion = new Transaccion(usuarioId, monto, descripcion, categoria, fecha, tipo, utcNow);
        transaccion.RaiseDomainEvent(new TransaccionCreadaDomainEvent(transaccion.Id, usuarioId, utcNow));
        return Result.Success(transaccion);
    }

    public void Actualizar(Dinero? monto, Descripcion? descripcion, Categoria? categoria, DateOnly? fecha, TipoTransaccion? tipo)
    {
        if (monto is not null) Monto = monto;
        if (descripcion is not null) Descripcion = descripcion;
        if (categoria is not null) Categoria = categoria;
        if (fecha is not null) Fecha = fecha.Value;
        if (tipo is not null) Tipo = tipo.Value;
    }
}
