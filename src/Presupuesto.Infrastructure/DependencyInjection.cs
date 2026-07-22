using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Presupuesto.Application.Abstracciones;
using Presupuesto.Domain.Transacciones;
using Presupuesto.Domain.Usuarios;
using Presupuesto.Infrastructure.Autenticacion;
using Presupuesto.Infrastructure.Database;
using Presupuesto.Infrastructure.Repositorios;

namespace Presupuesto.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OpcionesJwt>(configuration.GetSection(OpcionesJwt.SectionName));
        var jwtOptions = configuration.GetSection(OpcionesJwt.SectionName).Get<OpcionesJwt>() ?? new OpcionesJwt();

        services.AddDbContext<PresupuestoDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("presupuesto"));
        });

        services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
        services.AddScoped<IRepositorioTransaccion, RepositorioTransaccion>();
        services.AddScoped<IUnidadDeTrabajo>(provider => provider.GetRequiredService<PresupuestoDbContext>());
        services.AddScoped<IServicioHashContrasena, ServicioHashContrasena>();
        services.AddScoped<IServicioTokens, ServicioTokensJwt>();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                };
            });

        services.AddAuthorization();

        return services;
    }
}
