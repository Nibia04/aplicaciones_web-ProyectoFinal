using Presupuesto.Application.Abstracciones;
using Presupuesto.Application.Dtos;
using Presupuesto.Domain.Abstracciones;
using Presupuesto.Domain.Usuarios;

namespace Presupuesto.Application.Usuarios.RegistrarUsuario;

public sealed class RegistrarUsuarioHandler(
    IRepositorioUsuario usuarios,
    IServicioHashContrasena hashContrasenaer,
    IUnidadDeTrabajo unidadDeTrabajo) : IManejadorComando<RegistrarUsuarioCommand, UsuarioDto>
{
    public async Task<Resultado<UsuarioDto>> Handle(RegistrarUsuarioCommand command, CancellationToken cancellationToken = default)
    {
        var nombre = Nombre.Crear(command.Nombre);
        if (nombre.EsFallo) return Resultado.Fallo<UsuarioDto>(nombre.Error);

        var email = Email.Crear(command.Email);
        if (email.EsFallo) return Resultado.Fallo<UsuarioDto>(email.Error);

        var contrasena = Contrasena.Crear(command.Contrasena);
        if (contrasena.EsFallo) return Resultado.Fallo<UsuarioDto>(contrasena.Error);

        if (await usuarios.ObtenerPorEmailAsync(email.Value!, cancellationToken) is not null)
        {
            return Resultado.Fallo<UsuarioDto>(ErroresUsuario.EmailDuplicado);
        }

        var usuario = Usuario.Registrar(nombre.Value!, email.Value!, hashContrasenaer.GenerarHash(contrasena.Value!.Value), DateTime.UtcNow);
        if (usuario.EsFallo) return Resultado.Fallo<UsuarioDto>(usuario.Error);

        usuarios.Agregar(usuario.Value!);
        await unidadDeTrabajo.GuardarCambiosAsync(cancellationToken);

        return Resultado.Exito(new UsuarioDto(usuario.Value!.Id, usuario.Value.Nombre.Value, usuario.Value.Email.Value));
    }
}
