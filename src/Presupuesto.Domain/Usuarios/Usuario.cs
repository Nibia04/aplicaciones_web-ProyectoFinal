using Presupuesto.Domain.Abstractions;

namespace Presupuesto.Domain.Usuarios;

public sealed class Usuario : Entity
{
    private readonly List<Transacciones.Transaccion> _transacciones = [];

    private Usuario() { }

    private Usuario(Nombre nombre, Email email, string passwordHash, DateTime creadoEnUtc)
    {
        Nombre = nombre;
        Email = email;
        PasswordHash = passwordHash;
        CreadoEnUtc = creadoEnUtc;
    }

    public Nombre Nombre { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = string.Empty;
    public DateTime CreadoEnUtc { get; private set; }
    public IReadOnlyCollection<Transacciones.Transaccion> Transacciones => _transacciones.AsReadOnly();

    public static Result<Usuario> Registrar(Nombre nombre, Email email, string passwordHash, DateTime utcNow)
    {
        var usuario = new Usuario(nombre, email, passwordHash, utcNow);
        usuario.RaiseDomainEvent(new UsuarioRegistradoDomainEvent(usuario.Id, utcNow));
        return Result.Success(usuario);
    }
}
