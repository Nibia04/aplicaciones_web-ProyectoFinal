using Presupuesto.Domain.Abstractions;

namespace Presupuesto.Domain.Transacciones;

public sealed record TransaccionCreadaDomainEvent(Guid TransaccionId, Guid UsuarioId, DateTime OccurredOnUtc) : IDomainEvent;
