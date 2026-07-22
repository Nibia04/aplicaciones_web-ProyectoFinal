using Presupuesto.Domain.Abstracciones;

namespace Presupuesto.Application.Abstracciones;

public interface IManejadorConsulta<TQuery, TResponse>
{
    Task<Resultado<TResponse>> Handle(TQuery query, CancellationToken cancellationToken = default);
}
