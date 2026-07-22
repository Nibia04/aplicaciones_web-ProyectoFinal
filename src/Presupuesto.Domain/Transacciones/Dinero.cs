using Presupuesto.Domain.Abstracciones;

namespace Presupuesto.Domain.Transacciones;

public sealed record Dinero
{
    private Dinero(decimal monto) => Monto = monto;

    public decimal Monto { get; }

    public static Resultado<Dinero> Crear(decimal monto)
    {
        if (monto <= 0)
        {
            return Resultado.Fallo<Dinero>(ErroresTransaccion.MontoInvalido);
        }

        return Resultado.Exito(new Dinero(monto));
    }
}
