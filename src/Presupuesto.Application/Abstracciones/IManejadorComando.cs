using Presupuesto.Domain.Abstracciones;

namespace Presupuesto.Application.Abstracciones;

public interface IManejadorComando<TCommand, TResponse>
{
    Task<Resultado<TResponse>> Handle(TCommand command, CancellationToken cancellationToken = default);
}

public interface IManejadorComando<TCommand>
{
    Task<Resultado> Handle(TCommand command, CancellationToken cancellationToken = default);
}
