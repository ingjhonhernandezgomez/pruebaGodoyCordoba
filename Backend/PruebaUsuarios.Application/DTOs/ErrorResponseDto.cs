namespace PruebaUsuarios.Application.DTOs;

public class ErrorResponseDto
{
    public string Mensaje { get; set; } = "Ocurrió un error inesperado.";
    public string? Detalle { get; set; }
    public string? Codigo { get; set; }
}
