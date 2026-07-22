using Presupuesto.Domain.Abstracciones;

namespace Presupuesto.Domain.Transacciones;

public sealed record Descripcion
{
    private Descripcion(string value) => Value = value;

    public string Value { get; }

    public static Resultado<Descripcion> Crear(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 255)
        {
            return Resultado.Fallo<Descripcion>(ErroresTransaccion.DescripcionInvalida);
        }

        return Resultado.Exito(new Descripcion(value.Trim()));
    }

    public override string ToString() => Value;
}
