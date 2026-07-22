namespace Presupuesto.Application.Abstracciones;

public interface IUnidadDeTrabajo
{
    Task<int> GuardarCambiosAsync(CancellationToken cancellationToken = default);
}
