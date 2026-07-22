using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Presupuesto.Application.Abstractions;
using Presupuesto.Domain.Transacciones;
using Presupuesto.Domain.Usuarios;
using Presupuesto.Infrastructure.Authentication;
using Presupuesto.Infrastructure.Database;
using Presupuesto.Infrastructure.Repositories;

namespace Presupuesto.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>() ?? new JwtOptions();

        services.AddDbContext<PresupuestoDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("presupuesto"));
        });

        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<ITransaccionRepository, TransaccionRepository>();
        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<PresupuestoDbContext>());
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ITokenService, JwtTokenService>();

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
