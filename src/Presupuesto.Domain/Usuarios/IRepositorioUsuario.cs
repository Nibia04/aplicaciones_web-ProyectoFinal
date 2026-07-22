namespace Presupuesto.Domain.Usuarios;

public interface IRepositorioUsuario
{
    Task<Usuario?> ObtenerPorEmailAsync(Email email, CancellationToken cancellationToken = default);
    Task<Usuario?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Agregar(Usuario usuario);
}
