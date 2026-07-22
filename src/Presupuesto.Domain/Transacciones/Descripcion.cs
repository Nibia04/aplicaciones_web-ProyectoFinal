using Presupuesto.Domain.Abstractions;

namespace Presupuesto.Domain.Transacciones;

public sealed record Descripcion
{
    private Descripcion(string value) => Value = value;

    public string Value { get; }

    public static Result<Descripcion> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 255)
        {
            return Result.Failure<Descripcion>(TransaccionErrors.DescripcionInvalida);
        }

        return Result.Success(new Descripcion(value.Trim()));
    }

    public override string ToString() => Value;
}
