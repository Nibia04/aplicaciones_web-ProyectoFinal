using Microsoft.Extensions.DependencyInjection;
using Presupuesto.Application.Abstracciones;
using Presupuesto.Application.Presupuestos.ObtenerResumenPresupuesto;
using Presupuesto.Application.Transacciones.ActualizarTransaccion;
using Presupuesto.Application.Transacciones.CrearTransaccion;
using Presupuesto.Application.Transacciones.EliminarTransaccion;
using Presupuesto.Application.Transacciones.ListarTransacciones;
using Presupuesto.Application.Transacciones.ObtenerTransaccion;
using Presupuesto.Application.Usuarios.LoginUsuario;
using Presupuesto.Application.Usuarios.RegistrarUsuario;

namespace Presupuesto.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<RegistrarUsuarioHandler>();
        services.AddScoped<LoginUsuarioHandler>();
        services.AddScoped<CrearTransaccionHandler>();
        services.AddScoped<ListarTransaccionesHandler>();
        services.AddScoped<ObtenerTransaccionHandler>();
        services.AddScoped<ActualizarTransaccionHandler>();
        services.AddScoped<EliminarTransaccionHandler>();
        services.AddScoped<ObtenerResumenPresupuestoHandler>();
        return services;
    }
}
