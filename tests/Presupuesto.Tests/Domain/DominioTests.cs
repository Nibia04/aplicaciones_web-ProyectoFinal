using Presupuesto.Domain.Transacciones;
using Presupuesto.Domain.Usuarios;

namespace Presupuesto.Tests.Domain;

public sealed class DominioTests
{
    [Fact]
    public void RegistrarUsuario_CreaEventoDominio()
    {
        var nombre = Nombre.Create("Ana").Value!;
        var email = Email.Create("ana@example.com").Value!;

        var usuario = Usuario.Registrar(nombre, email, "hash", DateTime.UtcNow).Value!;

        Assert.Single(usuario.DomainEvents);
        Assert.IsType<UsuarioRegistradoDomainEvent>(usuario.DomainEvents.First());
        Assert.Equal("ana@example.com", usuario.Email.Value);
    }

    [Fact]
    public void CrearTransaccion_ValidaMontoYRegistraEvento()
    {
        var monto = Dinero.Create(25.5m).Value!;
        var descripcion = Descripcion.Create("Almuerzo").Value!;
        var categoria = Categoria.Create("Comida").Value!;

        var transaccion = Transaccion.Crear(
            Guid.NewGuid(),
            monto,
            descripcion,
            categoria,
            new DateOnly(2026, 7, 14),
            TipoTransaccion.Gasto,
            DateTime.UtcNow).Value!;

        Assert.Equal(25.5m, transaccion.Monto.Monto);
        Assert.Single(transaccion.DomainEvents);
        Assert.IsType<TransaccionCreadaDomainEvent>(transaccion.DomainEvents.First());
    }

    [Fact]
    public void Dinero_RechazaMontoNoPositivo()
    {
        var result = Dinero.Create(0);

        Assert.True(result.IsFailure);
        Assert.Equal("transaccion.monto_invalido", result.Error.Code);
    }
}
