namespace Presupuesto.Domain.Abstracciones;

public abstract class Entidad
{
    private readonly List<IEventoDominio> _eventosDominio = [];

    public Guid Id { get; protected init; } = Guid.NewGuid();

    public IReadOnlyCollection<IEventoDominio> EventosDominio => _eventosDominio.AsReadOnly();

    public void LimpiarEventosDominio() => _eventosDominio.Clear();

    protected void RegistrarEventoDominio(IEventoDominio eventoDominio) => _eventosDominio.Add(eventoDominio);
}
