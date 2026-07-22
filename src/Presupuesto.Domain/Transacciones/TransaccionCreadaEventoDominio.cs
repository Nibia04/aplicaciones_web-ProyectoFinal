using Presupuesto.Domain.Abstracciones;

namespace Presupuesto.Domain.Transacciones;

public sealed record TransaccionCreadaEventoDominio(Guid TransaccionId, Guid UsuarioId, DateTime OcurrioEnUtc) : IEventoDominio;
