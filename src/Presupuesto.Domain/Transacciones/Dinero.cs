using Presupuesto.Domain.Abstractions;

namespace Presupuesto.Domain.Transacciones;

public sealed record Dinero
{
    private Dinero(decimal monto) => Monto = monto;

    public decimal Monto { get; }

    public static Result<Dinero> Create(decimal monto)
    {
        if (monto <= 0)
        {
            return Result.Failure<Dinero>(TransaccionErrors.MontoInvalido);
        }

        return Result.Success(new Dinero(monto));
    }
}
