using Presupuesto.Domain.Transacciones;
using Presupuesto.Domain.Usuarios;

namespace Presupuesto.Tests.Domain;

public sealed class DominioTests
{
    [Fact]
    public void RegistrarUsuario_CreaEventoDominio()
    {
        var nombre = Nombre.Crear("Ana").Value!;
        var email = Email.Crear("ana@example.com").Value!;

        var usuario = Usuario.Registrar(nombre, email, "hash", DateTime.UtcNow).Value!;

        Assert.Single(usuario.EventosDominio);
        Assert.IsType<UsuarioRegistradoEventoDominio>(usuario.EventosDominio.First());
        Assert.Equal("ana@example.com", usuario.Email.Value);
    }

    [Fact]
    public void CrearTransaccion_ValidaMontoYRegistraEvento()
    {
        var monto = Dinero.Crear(25.5m).Value!;
        var descripcion = Descripcion.Crear("Almuerzo").Value!;
        var categoria = Categoria.Crear("Comida").Value!;

        var transaccion = Transaccion.Crear(
            Guid.NewGuid(),
            monto,
            descripcion,
            categoria,
            new DateOnly(2026, 7, 14),
            TipoTransaccion.Gasto,
            DateTime.UtcNow).Value!;

        Assert.Equal(25.5m, transaccion.Monto.Monto);
        Assert.Single(transaccion.EventosDominio);
        Assert.IsType<TransaccionCreadaEventoDominio>(transaccion.EventosDominio.First());
    }

    [Fact]
    public void Dinero_RechazaMontoNoPositivo()
    {
        var Resultado = Dinero.Crear(0);

        Assert.True(Resultado.EsFallo);
        Assert.Equal("transaccion.monto_invalido", Resultado.Error.Codigo);
    }
}
