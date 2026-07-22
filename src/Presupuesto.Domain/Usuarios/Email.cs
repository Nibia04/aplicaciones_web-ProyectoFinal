using Presupuesto.Domain.Abstractions;

namespace Presupuesto.Domain.Usuarios;

public sealed record Email
{
    private Email(string value) => Value = value;

    public string Value { get; }

    public static Result<Email> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.Contains('@'))
        {
            return Result.Failure<Email>(UsuarioErrors.EmailInvalido);
        }

        return Result.Success(new Email(value.Trim().ToLowerInvariant()));
    }

    public override string ToString() => Value;
}
