namespace Presupuesto.Domain.Usuarios;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);
    Task<Usuario?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(Usuario usuario);
}
