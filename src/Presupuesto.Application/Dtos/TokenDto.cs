namespace Presupuesto.Application.Dtos;

public sealed record TokenDto(string TokenAcceso, string TipoToken = "bearer");
