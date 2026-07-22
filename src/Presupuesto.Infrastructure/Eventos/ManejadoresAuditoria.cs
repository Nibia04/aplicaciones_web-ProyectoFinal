using Microsoft.Extensions.Logging;
using Presupuesto.Application.Abstracciones;
using Presupuesto.Domain.Transacciones;
using Presupuesto.Domain.Usuarios;

namespace Presupuesto.Infrastructure.Eventos;

public sealed class UsuarioRegistradoManejador(
    ILogger<UsuarioRegistradoManejador> logger)
    : ManejadorEventoDominio<UsuarioRegistradoEventoDominio>
{
    protected override Task ManejarAsync(
        UsuarioRegistradoEventoDominio eventoDominio,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Evento de dominio procesado: usuario {UsuarioId} registrado en {OcurrioEnUtc}",
            eventoDominio.UsuarioId,
            eventoDominio.OcurrioEnUtc);

        return Task.CompletedTask;
    }
}

public sealed class TransaccionCreadaManejador(
    ILogger<TransaccionCreadaManejador> logger)
    : ManejadorEventoDominio<TransaccionCreadaEventoDominio>
{
    protected override Task ManejarAsync(
        TransaccionCreadaEventoDominio eventoDominio,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Evento de dominio procesado: transaccion {TransaccionId} creada para usuario {UsuarioId}",
            eventoDominio.TransaccionId,
            eventoDominio.UsuarioId);

        return Task.CompletedTask;
    }
}
