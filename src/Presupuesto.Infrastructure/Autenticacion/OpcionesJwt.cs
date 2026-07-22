namespace Presupuesto.Infrastructure.Autenticacion;

public sealed class OpcionesJwt
{
    public const string SectionName = "Jwt";

    public string SecretKey { get; init; } = "clave-de-desarrollo-cambiar-en-produccion-123456789";
    public string Issuer { get; init; } = "Presupuesto";
    public string Audience { get; init; } = "Presupuesto.Api";
    public int ExpirationMinutes { get; init; } = 60;
}
