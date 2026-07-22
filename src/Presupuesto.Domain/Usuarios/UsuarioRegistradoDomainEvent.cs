using Presupuesto.Domain.Abstractions;

namespace Presupuesto.Domain.Usuarios;

public sealed record UsuarioRegistradoDomainEvent(Guid UsuarioId, DateTime OccurredOnUtc) : IDomainEvent;
