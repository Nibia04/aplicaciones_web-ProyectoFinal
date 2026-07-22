using Microsoft.EntityFrameworkCore;
using Presupuesto.Domain.Usuarios;
using Presupuesto.Infrastructure.Database;

namespace Presupuesto.Infrastructure.Repositories;

public sealed class UsuarioRepository(PresupuestoDbContext dbContext) : IUsuarioRepository
{
    public Task<Usuario?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        return dbContext.Usuarios
            .FirstOrDefaultAsync(usuario => usuario.Email.Value == email.Value, cancellationToken);
    }

    public Task<Usuario?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return dbContext.Usuarios.FirstOrDefaultAsync(usuario => usuario.Id == id, cancellationToken);
    }

    public void Add(Usuario usuario) => dbContext.Usuarios.Add(usuario);
}
