using Presupuesto.Domain.Usuarios;

namespace Presupuesto.Application.Abstractions;

public interface ITokenService
{
    string CreateAccessToken(Usuario usuario);
}
