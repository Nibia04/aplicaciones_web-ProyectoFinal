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
        var usuarios = new UsuarioRepositoryFake();
        var unitOfWork = new UnitOfWorkFake();
        var hasher = new PasswordHasherFake();
        var tokenService = new TokenServiceFake();

        var registrar = new RegistrarUsuarioHandler(usuarios, hasher, unitOfWork);
        var usuario = await registrar.Handle(new RegistrarUsuarioCommand("Ana", "ana@example.com", "secreto123"));

        var login = new LoginUsuarioHandler(usuarios, hasher, tokenService);
        var token = await login.Handle(new LoginUsuarioCommand("ana@example.com", "secreto123"));

        Assert.True(usuario.IsSuccess);
        Assert.True(token.IsSuccess);
        Assert.StartsWith("token:", token.Value!.AccessToken);
        Assert.Equal(1, unitOfWork.Saves);
    }

    [Fact]
    public async Task RegistrarUsuario_RechazaEmailDuplicado()
    {
        var usuarios = new UsuarioRepositoryFake();
        var handler = new RegistrarUsuarioHandler(usuarios, new PasswordHasherFake(), new UnitOfWorkFake());
        await handler.Handle(new RegistrarUsuarioCommand("Ana", "ana@example.com", "secreto123"));

        var duplicado = await handler.Handle(new RegistrarUsuarioCommand("Ana", "ana@example.com", "secreto123"));

        Assert.True(duplicado.IsFailure);
        Assert.Equal("usuario.email_duplicado", duplicado.Error.Code);
    }

    [Fact]
    public async Task CrearTransaccionYResumen_FlujoCorrecto()
    {
        var usuarioId = Guid.NewGuid();
        var transacciones = new TransaccionRepositoryFake();
        var unitOfWork = new UnitOfWorkFake();
        var crear = new CrearTransaccionHandler(transacciones, unitOfWork);

        await crear.Handle(new CrearTransaccionCommand(usuarioId, 100, "Pago", "Trabajo", new DateOnly(2026, 7, 14), TipoTransaccion.Ingreso));
        await crear.Handle(new CrearTransaccionCommand(usuarioId, 25, "Almuerzo", "Comida", new DateOnly(2026, 7, 14), TipoTransaccion.Gasto));

        var resumen = await new ObtenerResumenPresupuestoHandler(transacciones)
            .Handle(new ObtenerResumenPresupuestoQuery(usuarioId));

        Assert.True(resumen.IsSuccess);
        Assert.Equal(75, resumen.Value!.SaldoActual);
        Assert.Equal(2, resumen.Value.ResumenDiario[0].CantidadTransacciones);
    }

    [Fact]
    public async Task ObtenerTransaccion_NoPermiteLeerTransaccionDeOtroUsuario()
    {
        var transacciones = new TransaccionRepositoryFake();
        var crear = new CrearTransaccionHandler(transacciones, new UnitOfWorkFake());
        var creada = await crear.Handle(new CrearTransaccionCommand(Guid.NewGuid(), 25, "Almuerzo", "Comida", new DateOnly(2026, 7, 14), TipoTransaccion.Gasto));

        var resultado = await new ObtenerTransaccionHandler(transacciones)
            .Handle(new ObtenerTransaccionQuery(Guid.NewGuid(), creada.Value!.Id));

        Assert.True(resultado.IsFailure);
        Assert.Equal("transaccion.no_encontrada", resultado.Error.Code);
    }
}
