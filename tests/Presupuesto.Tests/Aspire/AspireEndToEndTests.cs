using System.Net.Http.Json;
using System.Net;
using Aspire.Hosting.Testing;

namespace Presupuesto.Tests.Aspire;

public sealed class AspireEndToEndTests
{
    [Fact]
    public async Task Api_FlujoPrincipal_ConAspire()
    {
        var builder = await DistributedApplicationTestingBuilder.CreateAsync<Projects.Presupuesto_AppHost>();
        await using var app = await builder.BuildAsync();
        await app.StartAsync();

        using var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(2));
        await app.ResourceNotifications.WaitForResourceHealthyAsync("api", timeout.Token);

        var client = app.CreateHttpClient("api");
        var sinAutorizacion = await client.GetAsync("/transacciones", timeout.Token);
        Assert.Equal(HttpStatusCode.Unauthorized, sinAutorizacion.StatusCode);

        var email = $"aspire-{Guid.NewGuid():N}@example.com";

        var registro = await client.PostAsJsonAsync("/auth/registro", new
        {
            nombre = "Ana",
            email,
            contrasena = "secreto123"
        }, timeout.Token);
        registro.EnsureSuccessStatusCode();

        var login = await client.PostAsJsonAsync("/auth/login", new
        {
            email,
            contrasena = "secreto123"
        }, timeout.Token);
        login.EnsureSuccessStatusCode();

        var token = (await login.Content.ReadFromJsonAsync<TokenResponse>())!.TokenAcceso;
        client.DefaultRequestHeaders.Authorization = new("Bearer", token);

        var crearTransaccion = await client.PostAsJsonAsync("/transacciones", new
        {
            monto = 10.5m,
            descripcion = "Almuerzo",
            categoria = "Comida",
            fecha = "2026-07-14",
            tipo = "Gasto"
        }, timeout.Token);
        crearTransaccion.EnsureSuccessStatusCode();
        var transaccion = (await crearTransaccion.Content
            .ReadFromJsonAsync<TransaccionResponse>(timeout.Token))!;

        var actualizar = await client.PutAsJsonAsync($"/transacciones/{transaccion.Id}", new
        {
            monto = 12m,
            descripcion = "Almuerzo actualizado"
        }, timeout.Token);
        actualizar.EnsureSuccessStatusCode();

        var obtenida = await client.GetFromJsonAsync<TransaccionResponse>(
            $"/transacciones/{transaccion.Id}", timeout.Token);
        Assert.Equal(12m, obtenida!.Monto);
        Assert.Equal("Almuerzo actualizado", obtenida.Descripcion);

        var listado = await client.GetFromJsonAsync<List<TransaccionResponse>>(
            "/transacciones", timeout.Token);
        Assert.Contains(listado!, item => item.Id == transaccion.Id);

        var resumen = await client.GetFromJsonAsync<ResumenResponse>(
            "/presupuesto/resumen", timeout.Token);
        Assert.NotNull(resumen);
        Assert.Equal(-12m, resumen!.SaldoActual);

        var eliminar = await client.DeleteAsync(
            $"/transacciones/{transaccion.Id}", timeout.Token);
        Assert.Equal(HttpStatusCode.NoContent, eliminar.StatusCode);

        var eliminada = await client.GetAsync(
            $"/transacciones/{transaccion.Id}", timeout.Token);
        Assert.Equal(HttpStatusCode.NotFound, eliminada.StatusCode);
    }

    private sealed record TokenResponse(string TokenAcceso, string TipoToken);
    private sealed record TransaccionResponse(
        Guid Id,
        decimal Monto,
        string Descripcion,
        string Categoria,
        DateOnly Fecha,
        string Tipo);
    private sealed record ResumenResponse(decimal SaldoActual, decimal TotalIngresos, decimal TotalGastos);
}
