using Presupuesto.Domain.Abstracciones;

namespace Presupuesto.Domain.Usuarios;

public sealed class Usuario : Entidad
{
    private readonly List<Transacciones.Transaccion> _transacciones = [];

    private Usuario() { }

    private Usuario(Nombre nombre, Email email, string hashContrasena, DateTime creadoEnUtc)
    {
        Nombre = nombre;
        Email = email;
        HashContrasena = hashContrasena;
        CreadoEnUtc = creadoEnUtc;
    }

    public Nombre Nombre { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public string HashContrasena { get; private set; } = string.Empty;
    public DateTime CreadoEnUtc { get; private set; }
    public IReadOnlyCollection<Transacciones.Transaccion> Transacciones => _transacciones.AsReadOnly();

    public static Resultado<Usuario> Registrar(Nombre nombre, Email email, string hashContrasena, DateTime utcNow)
    {
        var usuario = new Usuario(nombre, email, hashContrasena, utcNow);
        usuario.RegistrarEventoDominio(new UsuarioRegistradoEventoDominio(usuario.Id, utcNow));
        return Resultado.Exito(usuario);
    }
}
