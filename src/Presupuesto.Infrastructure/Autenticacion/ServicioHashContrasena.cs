using System.Security.Cryptography;
using Presupuesto.Application.Abstracciones;

namespace Presupuesto.Infrastructure.Autenticacion;

public sealed class ServicioHashContrasena : IServicioHashContrasena
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 100_000;

    public string GenerarHash(string contrasena)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(contrasena, salt, Iterations, HashAlgorithmName.SHA256, HashSize);
        return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    public bool Verificar(string contrasena, string hashContrasena)
    {
        var parts = hashContrasena.Split('.');
        if (parts.Length != 3 || !int.TryParse(parts[0], out var iterations))
        {
            return false;
        }

        var salt = Convert.FromBase64String(parts[1]);
        var hash = Convert.FromBase64String(parts[2]);
        var candidate = Rfc2898DeriveBytes.Pbkdf2(contrasena, salt, iterations, HashAlgorithmName.SHA256, hash.Length);
        return CryptographicOperations.FixedTimeEquals(candidate, hash);
    }
}
