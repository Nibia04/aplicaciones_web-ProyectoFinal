using Presupuesto.Domain.Abstracciones;

namespace Presupuesto.Domain.Usuarios;

public static class ErroresUsuario
{
    public static readonly Error NombreInvalido = new("usuario.nombre_invalido", "El nombre debe tener al menos 2 caracteres.");
    public static readonly Error EmailInvalido = new("usuario.email_invalido", "El email no tiene un formato valido.");
    public static readonly Error EmailDuplicado = new("usuario.email_duplicado", "El email ya esta registrado.");
    public static readonly Error NoEncontrado = new("usuario.no_encontrado", "Usuario no encontrado.");
    public static readonly Error CredencialesInvalidas = new("usuario.credenciales_invalidas", "Credenciales invalidas.");
}
