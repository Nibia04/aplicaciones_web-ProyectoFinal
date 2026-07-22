using Presupuesto.Domain.Usuarios;

namespace Presupuesto.Application.Abstracciones;

public interface IServicioTokens
{
    string CrearTokenAcceso(Usuario usuario);
}
