using Microsoft.EntityFrameworkCore;
using Presupuesto.Application.Abstracciones;
using Presupuesto.Domain.Abstracciones;
using Presupuesto.Domain.Usuarios;
using Presupuesto.Infrastructure.Database;

namespace Presupuesto.Tests.Infrastructure;

public sealed class EventosDominioTests
{
    [Fact]
    public async Task GuardarCambios_DespachaYLimpiaEventosDeDominio()
    {
        var despachador = new DespachadorFake();
        var opciones = new DbContextOptionsBuilder<PresupuestoDbContext>()
            .UseInMemoryDatabase($"eventos-{Guid.NewGuid()}")
            .Options;

        await using var dbContext = new PresupuestoDbContext(opciones, despachador);
        var usuario = Usuario.Registrar(
            Nombre.Crear("Ana").Value!,
            Email.Crear("eventos@example.com").Value!,
            "hash",
            DateTime.UtcNow).Value!;

        dbContext.Usuarios.Add(usuario);
        await dbContext.GuardarCambiosAsync();

        Assert.Single(despachador.Eventos);
        Assert.IsType<UsuarioRegistradoEventoDominio>(despachador.Eventos[0]);
        Assert.Empty(usuario.EventosDominio);
    }

    private sealed class DespachadorFake : IDespachadorEventosDominio
    {
        public List<IEventoDominio> Eventos { get; } = [];

        public Task DespacharAsync(
            IReadOnlyCollection<IEventoDominio> eventos,
            CancellationToken cancellationToken = default)
        {
            Eventos.AddRange(eventos);
            return Task.CompletedTask;
        }
    }
}
