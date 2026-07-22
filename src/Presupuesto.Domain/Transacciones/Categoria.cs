using Presupuesto.Domain.Abstractions;

namespace Presupuesto.Domain.Transacciones;

public sealed record Categoria
{
    private Categoria(string value) => Value = value;

    public string Value { get; }

    public static Result<Categoria> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 100)
        {
            return Result.Failure<Categoria>(TransaccionErrors.CategoriaInvalida);
        }

        return Result.Success(new Categoria(value.Trim()));
    }

    public override string ToString() => Value;
}
