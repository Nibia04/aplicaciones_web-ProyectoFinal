using Presupuesto.Domain.Abstracciones;

namespace Presupuesto.Domain.Transacciones;

public sealed record Categoria
{
    private Categoria(string value) => Value = value;

    public string Value { get; }

    public static Resultado<Categoria> Crear(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 100)
        {
            return Resultado.Fallo<Categoria>(ErroresTransaccion.CategoriaInvalida);
        }

        return Resultado.Exito(new Categoria(value.Trim()));
    }

    public override string ToString() => Value;
}
