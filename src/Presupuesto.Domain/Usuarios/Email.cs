using Presupuesto.Domain.Abstracciones;

namespace Presupuesto.Domain.Usuarios;

public sealed record Email
{
    private Email(string value) => Value = value;

    public string Value { get; }

    public static Resultado<Email> Crear(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.Contains('@'))
        {
            return Resultado.Fallo<Email>(ErroresUsuario.EmailInvalido);
        }

        return Resultado.Exito(new Email(value.Trim().ToLowerInvariant()));
    }

    public override string ToString() => Value;
}
