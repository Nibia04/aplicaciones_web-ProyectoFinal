using Presupuesto.Domain.Abstracciones;

namespace Presupuesto.Domain.Usuarios;

public sealed record Nombre
{
    private Nombre(string value) => Value = value;

    public string Value { get; }

    public static Resultado<Nombre> Crear(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Trim().Length < 2)
        {
            return Resultado.Fallo<Nombre>(ErroresUsuario.NombreInvalido);
        }

        return Resultado.Exito(new Nombre(value.Trim()));
    }

    public override string ToString() => Value;
}
