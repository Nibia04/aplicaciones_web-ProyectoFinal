using System.Net.Http.Json;
using Aspire.Hosting.Testing;

namespace Presupuesto.Tests.Aspire;

public sealed class AspireEndToEndTests
{
    [Fact]
    public async Task Api_FlujoPrincipal_ConAspire()
    {
        if (!DockerDisponible())
        {
            return;
        }

        var builder = await DistributedApplicationTestingBuilder.CreateAsync<Projects.Presupuesto_AppHost>();
        await using var app = await builder.BuildAsync();
        await app.StartAsync();

        var client = app.CreateHttpClient("api");

        var registro = await client.PostAsJsonAsync("/auth/registro", new
        {
            nombre = "Ana",
            email = "aspire@example.com",
            contrasena = "secreto123"
        });
        registro.EnsureSuccessStatusCode();

        var login = await client.PostAsJsonAsync("/auth/login", new
        {
            email = "aspire@example.com",
            contrasena = "secreto123"
        });
        login.EnsureSuccessStatusCode();

        var token = (await login.Content.ReadFromJsonAsync<TokenResponse>())!.TokenAcceso;
        client.DefaultRequestHeaders.Authorization = new("Bearer", token);

        var transaccion = await client.PostAsJsonAsync("/transacciones", new
        {
            monto = 10.5m,
            descripcion = "Almuerzo",
            categoria = "Comida",
            fecha = "2026-07-14",
            tipo = "Gasto"
        });
        transaccion.EnsureSuccessStatusCode();

        var resumen = await client.GetFromJsonAsync<ResumenResponse>("/presupuesto/resumen");
        Assert.NotNull(resumen);
        Assert.Equal(-10.5m, resumen!.SaldoActual);
    }

    private static bool DockerDisponible()
    {
        try
        {
            using var process = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "docker",
                Arguments = "info",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            });
            process!.WaitForExit(3000);
            return process.ExitCode == 0;
        }
        catch
        {
            return false;
        }
    }

    private sealed record TokenResponse(string TokenAcceso, string TipoToken);
    private sealed record ResumenResponse(decimal SaldoActual, decimal TotalIngresos, decimal TotalGastos);
}
