using System.ComponentModel.DataAnnotations;

namespace Libreria1.Application.DTOs;

/// <summary>
/// DTO para crear un nuevo libro.
/// </summary>
public class CrearLibroDto
{
    [Required(ErrorMessage = "El ISBN es obligatorio.")]
    public string? ISBN { get; set; }
    [Required]
    [StringLength(100, ErrorMessage = "El título no puede exceder los 100 caracteres.")]
    public string? Titulo { get; set; }
    [Required(ErrorMessage = "El autor es obligatorio.")]
    public string? Autor { get; set; }
    [Required(ErrorMessage = "El año de publicación es obligatorio.")]
    public string? AñoPublicacion { get; set; }
    [Required(ErrorMessage = "La cantidad de páginas es obligatoria.")]
    public int CantPaginas { get; set; }
}