using System.ComponentModel.DataAnnotations;

namespace PruebaUsuarios.Domain.Entities;

public class Usuario
{
    public int Id { get; set; }

    [Required]
    [MinLength(2)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [MinLength(2)]
    public string Apellidos { get; set; } = string.Empty;

    [Required]
    public string Cedula { get; set; } = string.Empty;

    [Required]
    public string Clave { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string CorreoElectronico { get; set; } = string.Empty;

    public DateTime FechaUltimoAcceso { get; set; }

    public int Puntaje { get; set; }
}
