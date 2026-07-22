using Presupuesto.Application.Abstracciones;
using Presupuesto.Domain.Abstracciones;

namespace Presupuesto.Infrastructure.Eventos;

public sealed class DespachadorEventosDominio(
    IEnumerable<IManejadorEventoDominio> manejadores) : IDespachadorEventosDominio
{
    public async Task DespacharAsync(
        IReadOnlyCollection<IEventoDominio> eventos,
        CancellationToken cancellationToken = default)
    {
        foreach (var evento in eventos)
        {
            var tareas = manejadores
                .Where(manejador => manejador.TipoEvento.IsInstanceOfType(evento))
                .Select(manejador => manejador.ManejarAsync(evento, cancellationToken));

            await Task.WhenAll(tareas);
        }
    }
}
