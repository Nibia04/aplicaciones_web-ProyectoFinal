using Presupuesto.Domain.Abstractions;

namespace Presupuesto.Application.Abstractions;

public interface IQueryHandler<TQuery, TResponse>
{
    Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken = default);
}
