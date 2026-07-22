using Presupuesto.Domain.Abstracciones;

namespace Presupuesto.Domain.Transacciones;

public static class ErroresTransaccion
{
    public static readonly Error MontoInvalido = new("transaccion.monto_invalido", "El monto debe ser mayor a cero.");
    public static readonly Error DescripcionInvalida = new("transaccion.descripcion_invalida", "La descripcion es obligatoria y no debe superar 255 caracteres.");
    public static readonly Error CategoriaInvalida = new("transaccion.categoria_invalida", "La categoria es obligatoria y no debe superar 100 caracteres.");
    public static readonly Error NoEncontrada = new("transaccion.no_encontrada", "Transaccion no encontrada.");
}
