using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Presupuesto.Application.Abstracciones;
using Presupuesto.Domain.Usuarios;

namespace Presupuesto.Infrastructure.Autenticacion;

public sealed class ServicioTokensJwt(IOptions<OpcionesJwt> options) : IServicioTokens
{
    private readonly OpcionesJwt _options = options.Value;

    public string CrearTokenAcceso(Usuario usuario)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email.Value),
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_options.ExpirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
