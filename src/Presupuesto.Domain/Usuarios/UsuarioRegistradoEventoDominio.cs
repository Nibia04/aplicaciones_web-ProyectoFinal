using Presupuesto.Domain.Abstracciones;

namespace Presupuesto.Domain.Usuarios;

public sealed record UsuarioRegistradoEventoDominio(Guid UsuarioId, DateTime OcurrioEnUtc) : IEventoDominio;
