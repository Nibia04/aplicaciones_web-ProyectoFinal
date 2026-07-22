using Presupuesto.Application.Abstractions;
using Presupuesto.Application.Dtos;
using Presupuesto.Domain.Abstractions;
using Presupuesto.Domain.Usuarios;

namespace Presupuesto.Application.Usuarios.RegistrarUsuario;

public sealed class RegistrarUsuarioHandler(
    IUsuarioRepository usuarios,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork) : ICommandHandler<RegistrarUsuarioCommand, UsuarioDto>
{
    public async Task<Result<UsuarioDto>> Handle(RegistrarUsuarioCommand command, CancellationToken cancellationToken = default)
    {
        var nombre = Nombre.Create(command.Nombre);
        if (nombre.IsFailure) return Result.Failure<UsuarioDto>(nombre.Error);

        var email = Email.Create(command.Email);
        if (email.IsFailure) return Result.Failure<UsuarioDto>(email.Error);

        if (await usuarios.GetByEmailAsync(email.Value!, cancellationToken) is not null)
        {
            return Result.Failure<UsuarioDto>(UsuarioErrors.EmailDuplicado);
        }

        var usuario = Usuario.Registrar(nombre.Value!, email.Value!, passwordHasher.Hash(command.Password), DateTime.UtcNow);
        if (usuario.IsFailure) return Result.Failure<UsuarioDto>(usuario.Error);

        usuarios.Add(usuario.Value!);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new UsuarioDto(usuario.Value!.Id, usuario.Value.Nombre.Value, usuario.Value.Email.Value));
    }
}
