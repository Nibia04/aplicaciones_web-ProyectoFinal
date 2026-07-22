using System.Security.Claims;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Presupuesto.Application;
using Presupuesto.Application.Presupuestos.ObtenerResumenPresupuesto;
using Presupuesto.Application.Transacciones.ActualizarTransaccion;
using Presupuesto.Application.Transacciones.CrearTransaccion;
using Presupuesto.Application.Transacciones.EliminarTransaccion;
using Presupuesto.Application.Transacciones.ListarTransacciones;
using Presupuesto.Application.Transacciones.ObtenerTransaccion;
using Presupuesto.Application.Usuarios.LoginUsuario;
using Presupuesto.Application.Usuarios.RegistrarUsuario;
using Presupuesto.Domain.Abstractions;
using Presupuesto.Domain.Transacciones;
using Presupuesto.Domain.Usuarios;
using Presupuesto.Infrastructure;
using Presupuesto.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PresupuestoDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultEndpoints();

app.MapGet("/", () => Results.Ok(new { mensaje = "API de presupuesto diario funcionando" }));

app.MapPost("/auth/registro", async (
    RegistrarUsuarioRequest request,
    RegistrarUsuarioHandler handler,
    CancellationToken cancellationToken) =>
{
    var result = await handler.Handle(new RegistrarUsuarioCommand(request.Nombre, request.Email, request.Password), cancellationToken);
    return result.ToHttpCreated("/auth/registro");
});

app.MapPost("/auth/login", async (
    LoginRequest request,
    LoginUsuarioHandler handler,
    CancellationToken cancellationToken) =>
{
    var result = await handler.Handle(new LoginUsuarioCommand(request.Email, request.Password), cancellationToken);
    return result.ToHttpResult();
});

var transacciones = app.MapGroup("/transacciones").RequireAuthorization();

transacciones.MapPost("/", async (
    CrearTransaccionRequest request,
    ClaimsPrincipal user,
    CrearTransaccionHandler handler,
    CancellationToken cancellationToken) =>
{
    var result = await handler.Handle(
        new CrearTransaccionCommand(user.GetUsuarioId(), request.Monto, request.Descripcion, request.Categoria, request.Fecha, request.Tipo),
        cancellationToken);
    return result.ToHttpCreated("/transacciones");
});

transacciones.MapGet("/", async (
    ClaimsPrincipal user,
    ListarTransaccionesHandler handler,
    CancellationToken cancellationToken) =>
{
    var result = await handler.Handle(new ListarTransaccionesQuery(user.GetUsuarioId()), cancellationToken);
    return result.ToHttpResult();
});

transacciones.MapGet("/{id:guid}", async (
    Guid id,
    ClaimsPrincipal user,
    ObtenerTransaccionHandler handler,
    CancellationToken cancellationToken) =>
{
    var result = await handler.Handle(new ObtenerTransaccionQuery(user.GetUsuarioId(), id), cancellationToken);
    return result.ToHttpResult();
});

transacciones.MapPut("/{id:guid}", async (
    Guid id,
    ActualizarTransaccionRequest request,
    ClaimsPrincipal user,
    ActualizarTransaccionHandler handler,
    CancellationToken cancellationToken) =>
{
    var result = await handler.Handle(
        new ActualizarTransaccionCommand(user.GetUsuarioId(), id, request.Monto, request.Descripcion, request.Categoria, request.Fecha, request.Tipo),
        cancellationToken);
    return result.ToHttpResult();
});

transacciones.MapDelete("/{id:guid}", async (
    Guid id,
    ClaimsPrincipal user,
    EliminarTransaccionHandler handler,
    CancellationToken cancellationToken) =>
{
    var result = await handler.Handle(new EliminarTransaccionCommand(user.GetUsuarioId(), id), cancellationToken);
    return result.IsSuccess ? Results.NoContent() : result.ToHttpResult();
});

app.MapGet("/presupuesto/resumen", async (
    ClaimsPrincipal user,
    ObtenerResumenPresupuestoHandler handler,
    CancellationToken cancellationToken) =>
{
    var result = await handler.Handle(new ObtenerResumenPresupuestoQuery(user.GetUsuarioId()), cancellationToken);
    return result.ToHttpResult();
}).RequireAuthorization();

app.Run();

public partial class Program;

public sealed record RegistrarUsuarioRequest(string Nombre, string Email, string Password);
public sealed record LoginRequest(string Email, string Password);
public sealed record CrearTransaccionRequest(decimal Monto, string Descripcion, string Categoria, DateOnly Fecha, TipoTransaccion Tipo);
public sealed record ActualizarTransaccionRequest(decimal? Monto, string? Descripcion, string? Categoria, DateOnly? Fecha, TipoTransaccion? Tipo);

internal static class ClaimsPrincipalExtensions
{
    public static Guid GetUsuarioId(this ClaimsPrincipal user)
    {
        var id = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue("sub");
        return Guid.Parse(id!);
    }
}

internal static class ResultExtensions
{
    public static IResult ToHttpResult(this Result result)
    {
        if (result.IsSuccess)
        {
            return Results.Ok();
        }

        return ToProblem(result.Error);
    }

    public static IResult ToHttpResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        return ToProblem(result.Error);
    }

    public static IResult ToHttpCreated<T>(this Result<T> result, string location)
    {
        if (result.IsSuccess)
        {
            return Results.Created(location, result.Value);
        }

        return ToProblem(result.Error);
    }

    private static IResult ToProblem(Error error)
    {
        var status = error.Code switch
        {
            "usuario.email_duplicado" => StatusCodes.Status409Conflict,
            "usuario.credenciales_invalidas" => StatusCodes.Status401Unauthorized,
            "usuario.no_encontrado" => StatusCodes.Status401Unauthorized,
            "transaccion.no_encontrada" => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status400BadRequest
        };

        return Results.Json(
            new { error = new { codigo = error.Code, mensaje = error.Message } },
            statusCode: status);
    }
}
