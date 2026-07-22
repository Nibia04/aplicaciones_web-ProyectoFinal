using Presupuesto.Domain.Abstractions;

namespace Presupuesto.Application.Abstractions;

public interface ICommandHandler<TCommand, TResponse>
{
    Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken = default);
}

public interface ICommandHandler<TCommand>
{
    Task<Result> Handle(TCommand command, CancellationToken cancellationToken = default);
}
