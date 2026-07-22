using Presupuesto.Application.Abstractions;
using Presupuesto.Domain.Transacciones;
using Presupuesto.Domain.Usuarios;

namespace Presupuesto.Tests.Application;

internal sealed class UsuarioRepositoryFake : IUsuarioRepository
{
    private readonly List<Usuario> _usuarios = [];

    public Task<Usuario?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_usuarios.FirstOrDefault(usuario => usuario.Email.Value == email.Value));
    }

    public Task<Usuario?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_usuarios.FirstOrDefault(usuario => usuario.Id == id));
    }

    public void Add(Usuario usuario) => _usuarios.Add(usuario);
}

internal sealed class TransaccionRepositoryFake : ITransaccionRepository
{
    private readonly List<Transaccion> _transacciones = [];

    public Task<IReadOnlyList<Transaccion>> ListByUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyList<Transaccion>>(
            _transacciones
                .Where(t => t.UsuarioId == usuarioId)
                .OrderByDescending(t => t.Fecha)
                .ThenByDescending(t => t.Id)
                .ToList());
    }

    public Task<IReadOnlyList<Transaccion>> ListByUsuarioFechaAscAsync(Guid usuarioId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyList<Transaccion>>(
            _transacciones
                .Where(t => t.UsuarioId == usuarioId)
                .OrderBy(t => t.Fecha)
                .ToList());
    }

    public Task<Transaccion?> GetByIdAndUsuarioAsync(Guid id, Guid usuarioId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_transacciones.FirstOrDefault(t => t.Id == id && t.UsuarioId == usuarioId));
    }

    public void Add(Transaccion transaccion) => _transacciones.Add(transaccion);

    public void Remove(Transaccion transaccion) => _transacciones.Remove(transaccion);
}

internal sealed class UnitOfWorkFake : IUnitOfWork
{
    public int Saves { get; private set; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        Saves++;
        return Task.FromResult(Saves);
    }
}

internal sealed class PasswordHasherFake : IPasswordHasher
{
    public string Hash(string password) => $"hashed:{password}";
    public bool Verify(string password, string passwordHash) => passwordHash == Hash(password);
}

internal sealed class TokenServiceFake : ITokenService
{
    public string CreateAccessToken(Usuario usuario) => $"token:{usuario.Id}";
}
