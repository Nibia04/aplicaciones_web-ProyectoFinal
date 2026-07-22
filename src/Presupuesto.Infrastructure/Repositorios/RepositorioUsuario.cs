using Microsoft.EntityFrameworkCore;
using Presupuesto.Domain.Usuarios;
using Presupuesto.Infrastructure.Database;

namespace Presupuesto.Infrastructure.Repositorios;

public sealed class RepositorioUsuario(PresupuestoDbContext dbContext) : IRepositorioUsuario
{
    public Task<Usuario?> ObtenerPorEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        return dbContext.Usuarios
            .FirstOrDefaultAsync(usuario => usuario.Email.Value == email.Value, cancellationToken);
    }

    public Task<Usuario?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return dbContext.Usuarios.FirstOrDefaultAsync(usuario => usuario.Id == id, cancellationToken);
    }

    public void Agregar(Usuario usuario) => dbContext.Usuarios.Add(usuario);
}
