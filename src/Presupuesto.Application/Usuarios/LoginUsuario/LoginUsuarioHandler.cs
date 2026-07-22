using Presupuesto.Application.Abstractions;
using Presupuesto.Application.Dtos;
using Presupuesto.Domain.Abstractions;
using Presupuesto.Domain.Usuarios;

namespace Presupuesto.Application.Usuarios.LoginUsuario;

public sealed class LoginUsuarioHandler(
    IUsuarioRepository usuarios,
    IPasswordHasher passwordHasher,
    ITokenService tokenService) : ICommandHandler<LoginUsuarioCommand, TokenDto>
{
    public async Task<Result<TokenDto>> Handle(LoginUsuarioCommand command, CancellationToken cancellationToken = default)
    {
        var email = Email.Create(command.Email);
        if (email.IsFailure) return Result.Failure<TokenDto>(UsuarioErrors.CredencialesInvalidas);

        var usuario = await usuarios.GetByEmailAsync(email.Value!, cancellationToken);
        if (usuario is null || !passwordHasher.Verify(command.Password, usuario.PasswordHash))
        {
            return Result.Failure<TokenDto>(UsuarioErrors.CredencialesInvalidas);
        }

        return Result.Success(new TokenDto(tokenService.CreateAccessToken(usuario)));
    }
}
