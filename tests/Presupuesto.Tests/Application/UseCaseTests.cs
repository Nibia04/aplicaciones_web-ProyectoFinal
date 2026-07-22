using Presupuesto.Application.Presupuestos.ObtenerResumenPresupuesto;
using Presupuesto.Application.Transacciones.CrearTransaccion;
using Presupuesto.Application.Transacciones.ObtenerTransaccion;
using Presupuesto.Application.Usuarios.LoginUsuario;
using Presupuesto.Application.Usuarios.RegistrarUsuario;
using Presupuesto.Domain.Transacciones;

namespace Presupuesto.Tests.Application;

public sealed class UseCaseTests
{
    [Fact]
    public async Task RegistrarYLoginUsuario_FlujoCorrecto()
    {
        var usuarios = new RepositorioUsuarioFake();
        var unidadDeTrabajo = new UnidadDeTrabajoFake();
        var hasher = new ServicioHashContrasenaFake();
        var servicioTokens = new ServicioTokensFake();

        var registrar = new RegistrarUsuarioHandler(usuarios, hasher, unidadDeTrabajo);
        var usuario = await registrar.Handle(new RegistrarUsuarioCommand("Ana", "ana@example.com", "secreto123"));

        var login = new LoginUsuarioHandler(usuarios, hasher, servicioTokens);
        var token = await login.Handle(new LoginUsuarioCommand("ana@example.com", "secreto123"));

        Assert.True(usuario.EsExitoso);
        Assert.True(token.EsExitoso);
        Assert.StartsWith("token:", token.Value!.TokenAcceso);
        Assert.Equal(1, unidadDeTrabajo.Guardados);
    }

    [Fact]
    public async Task RegistrarUsuario_RechazaEmailDuplicado()
    {
        var usuarios = new RepositorioUsuarioFake();
        var handler = new RegistrarUsuarioHandler(usuarios, new ServicioHashContrasenaFake(), new UnidadDeTrabajoFake());
        await handler.Handle(new RegistrarUsuarioCommand("Ana", "ana@example.com", "secreto123"));

        var duplicado = await handler.Handle(new RegistrarUsuarioCommand("Ana", "ana@example.com", "secreto123"));

        Assert.True(duplicado.EsFallo);
        Assert.Equal("usuario.email_duplicado", duplicado.Error.Codigo);
    }

    [Fact]
    public async Task RegistrarUsuario_RechazaContrasenaCortaSinGuardar()
    {
        var unidadDeTrabajo = new UnidadDeTrabajoFake();
        var handler = new RegistrarUsuarioHandler(new RepositorioUsuarioFake(), new ServicioHashContrasenaFake(), unidadDeTrabajo);

        var resultado = await handler.Handle(new RegistrarUsuarioCommand("Ana", "ana@example.com", "corta"));

        Assert.Equal("usuario.contrasena_invalida", resultado.Error.Codigo);
        Assert.Equal(0, unidadDeTrabajo.Guardados);
    }

    [Fact]
    public async Task CrearTransaccionYResumen_FlujoCorrecto()
    {
        var usuarioId = Guid.NewGuid();
        var transacciones = new RepositorioTransaccionFake();
        var unidadDeTrabajo = new UnidadDeTrabajoFake();
        var crear = new CrearTransaccionHandler(transacciones, unidadDeTrabajo);

        await crear.Handle(new CrearTransaccionCommand(usuarioId, 100, "Pago", "Trabajo", new DateOnly(2026, 7, 14), TipoTransaccion.Ingreso));
        await crear.Handle(new CrearTransaccionCommand(usuarioId, 25, "Almuerzo", "Comida", new DateOnly(2026, 7, 14), TipoTransaccion.Gasto));

        var resumen = await new ObtenerResumenPresupuestoHandler(transacciones)
            .Handle(new ObtenerResumenPresupuestoQuery(usuarioId));

        Assert.True(resumen.EsExitoso);
        Assert.Equal(75, resumen.Value!.SaldoActual);
        Assert.Equal(2, resumen.Value.ResumenDiario[0].CantidadTransacciones);
    }

    [Fact]
    public async Task ObtenerTransaccion_NoPermiteLeerTransaccionDeOtroUsuario()
    {
        var transacciones = new RepositorioTransaccionFake();
        var crear = new CrearTransaccionHandler(transacciones, new UnidadDeTrabajoFake());
        var creada = await crear.Handle(new CrearTransaccionCommand(Guid.NewGuid(), 25, "Almuerzo", "Comida", new DateOnly(2026, 7, 14), TipoTransaccion.Gasto));

        var resultado = await new ObtenerTransaccionHandler(transacciones)
            .Handle(new ObtenerTransaccionQuery(Guid.NewGuid(), creada.Value!.Id));

        Assert.True(resultado.EsFallo);
        Assert.Equal("transaccion.no_encontrada", resultado.Error.Codigo);
    }
}
