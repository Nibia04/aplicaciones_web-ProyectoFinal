using Presupuesto.Domain.Abstracciones;

namespace Presupuesto.Application.Abstracciones;

public interface IDespachadorEventosDominio
{
    Task DespacharAsync(
        IReadOnlyCollection<IEventoDominio> eventos,
        CancellationToken cancellationToken = default);
}

public interface IManejadorEventoDominio
{
    Type TipoEvento { get; }

    Task ManejarAsync(
        IEventoDominio eventoDominio,
        CancellationToken cancellationToken = default);
}

public abstract class ManejadorEventoDominio<TEvento> : IManejadorEventoDominio
    where TEvento : IEventoDominio
{
    public Type TipoEvento => typeof(TEvento);

    public Task ManejarAsync(
        IEventoDominio eventoDominio,
        CancellationToken cancellationToken = default)
    {
        return ManejarAsync((TEvento)eventoDominio, cancellationToken);
    }

    protected abstract Task ManejarAsync(
        TEvento eventoDominio,
        CancellationToken cancellationToken);
}
