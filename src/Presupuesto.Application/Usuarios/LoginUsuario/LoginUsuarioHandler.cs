using Presupuesto.Application.Abstracciones;
using Presupuesto.Application.Dtos;
using Presupuesto.Domain.Abstracciones;
using Presupuesto.Domain.Usuarios;

namespace Presupuesto.Application.Usuarios.LoginUsuario;

public sealed class LoginUsuarioHandler(
    IRepositorioUsuario usuarios,
    IServicioHashContrasena servicioHashContrasena,
    IServicioTokens servicioTokens) : IManejadorComando<LoginUsuarioCommand, TokenDto>
{
    public async Task<Resultado<TokenDto>> Handle(LoginUsuarioCommand command, CancellationToken cancellationToken = default)
    {
        var email = Email.Crear(command.Email);
        if (email.EsFallo) return Resultado.Fallo<TokenDto>(ErroresUsuario.CredencialesInvalidas);

        var usuario = await usuarios.ObtenerPorEmailAsync(email.Value!, cancellationToken);
        if (usuario is null || !servicioHashContrasena.Verificar(command.Contrasena, usuario.HashContrasena))
        {
            return Resultado.Fallo<TokenDto>(ErroresUsuario.CredencialesInvalidas);
        }

        return Resultado.Exito(new TokenDto(servicioTokens.CrearTokenAcceso(usuario)));
    }
}
