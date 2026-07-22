using Presupuesto.Domain.Abstractions;

namespace Presupuesto.Domain.Usuarios;

public sealed record Nombre
{
    private Nombre(string value) => Value = value;

    public string Value { get; }

    public static Result<Nombre> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Trim().Length < 2)
        {
            return Result.Failure<Nombre>(UsuarioErrors.NombreInvalido);
        }

        return Result.Success(new Nombre(value.Trim()));
    }

    public override string ToString() => Value;
}
