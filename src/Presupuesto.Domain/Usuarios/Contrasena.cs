using Presupuesto.Domain.Abstracciones;

namespace Presupuesto.Domain.Usuarios;

public sealed record Contrasena
{
    private Contrasena(string value) => Value = value;

    public string Value { get; }

    public static Resultado<Contrasena> Crear(string value)
    {
        return string.IsNullOrWhiteSpace(value) || value.Length < 8
            ? Resultado.Fallo<Contrasena>(ErroresUsuario.ContrasenaInvalida)
            : Resultado.Exito(new Contrasena(value));
    }
}
