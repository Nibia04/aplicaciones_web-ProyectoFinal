namespace Presupuesto.Application.Abstracciones;

public interface IServicioHashContrasena
{
    string GenerarHash(string contrasena);
    bool Verificar(string contrasena, string hashContrasena);
}
