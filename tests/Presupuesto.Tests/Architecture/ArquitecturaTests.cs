using Presupuesto.Domain.Abstracciones;

namespace Presupuesto.Tests.Architecture;

public sealed class ArquitecturaTests
{
    [Fact]
    public void Dominio_NoDependeDeApplicationInfrastructureNiApi()
    {
        var referencias = typeof(Entidad).Assembly
            .GetReferencedAssemblies()
            .Select(assembly => assembly.Name)
            .Where(nombre => nombre is not null)
            .ToList();

        Assert.DoesNotContain("Presupuesto.Application", referencias);
        Assert.DoesNotContain("Presupuesto.Infrastructure", referencias);
        Assert.DoesNotContain("Presupuesto.Api", referencias);
        Assert.DoesNotContain(referencias, nombre =>
            nombre!.StartsWith("Microsoft.EntityFrameworkCore", StringComparison.Ordinal));
    }
}
