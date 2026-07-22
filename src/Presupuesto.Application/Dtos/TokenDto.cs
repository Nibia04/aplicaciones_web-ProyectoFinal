namespace Presupuesto.Application.Dtos;

public sealed record TokenDto(string AccessToken, string TokenType = "bearer");
