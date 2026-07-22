namespace Presupuesto.Domain.Abstracciones;

public class Resultado
{
    protected Resultado(bool esExitoso, Error error)
    {
        EsExitoso = esExitoso;
        Error = error;
    }

    public bool EsExitoso { get; }
    public bool EsFallo => !EsExitoso;
    public Error Error { get; }

    public static Resultado Exito() => new(true, Error.Ninguno);
    public static Resultado Fallo(Error error) => new(false, error);
    public static Resultado<TValue> Exito<TValue>(TValue value) => new(value, true, Error.Ninguno);
    public static Resultado<TValue> Fallo<TValue>(Error error) => new(default, false, error);
}

public sealed class Resultado<TValue> : Resultado
{
    internal Resultado(TValue? value, bool esExitoso, Error error)
        : base(esExitoso, error)
    {
        Value = value;
    }

    public TValue? Value { get; }
}
