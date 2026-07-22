using Presupuesto.Application.Abstracciones;
using Presupuesto.Domain.Transacciones;
using Presupuesto.Domain.Usuarios;

namespace Presupuesto.Tests.Application;

internal sealed class RepositorioUsuarioFake : IRepositorioUsuario
{
    private readonly List<Usuario> _usuarios = [];

    public Task<Usuario?> ObtenerPorEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_usuarios.FirstOrDefault(usuario => usuario.Email.Value == email.Value));
    }

    public Task<Usuario?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_usuarios.FirstOrDefault(usuario => usuario.Id == id));
    }

    public void Agregar(Usuario usuario) => _usuarios.Add(usuario);
}

internal sealed class RepositorioTransaccionFake : IRepositorioTransaccion
{
    private readonly List<Transaccion> _transacciones = [];

    public Task<IReadOnlyList<Transaccion>> ListarPorUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyList<Transaccion>>(
            _transacciones
                .Where(t => t.UsuarioId == usuarioId)
                .OrderByDescending(t => t.Fecha)
                .ThenByDescending(t => t.Id)
                .ToList());
    }

    public Task<IReadOnlyList<Transaccion>> ListarPorUsuarioFechaAscAsync(Guid usuarioId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyList<Transaccion>>(
            _transacciones
                .Where(t => t.UsuarioId == usuarioId)
                .OrderBy(t => t.Fecha)
                .ToList());
    }

    public Task<Transaccion?> ObtenerPorIdYUsuarioAsync(Guid id, Guid usuarioId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_transacciones.FirstOrDefault(t => t.Id == id && t.UsuarioId == usuarioId));
    }

    public void Agregar(Transaccion transaccion) => _transacciones.Add(transaccion);

    public void Remover(Transaccion transaccion) => _transacciones.Remove(transaccion);
}

internal sealed class UnidadDeTrabajoFake : IUnidadDeTrabajo
{
    public int Guardados { get; private set; }

    public Task<int> GuardarCambiosAsync(CancellationToken cancellationToken = default)
    {
        Guardados++;
        return Task.FromResult(Guardados);
    }
}

internal sealed class ServicioHashContrasenaFake : IServicioHashContrasena
{
    public string GenerarHash(string contrasena) => $"hashed:{contrasena}";
    public bool Verificar(string contrasena, string hashContrasena) => hashContrasena == GenerarHash(contrasena);
}

internal sealed class ServicioTokensFake : IServicioTokens
{
    public string CrearTokenAcceso(Usuario usuario) => $"token:{usuario.Id}";
}
