using System.ComponentModel.DataAnnotations;


namespace Libreria1.Application.DTOs;
/// <summary>
/// DTO para crear un nuevo préstamo. 
/// </summary>
public class CrearPrestamoDto
{
    [Required(ErrorMessage = "El ISBN del libro es obligatorio.")]
    public string LibroIsbn { get; set; }

    [Required(ErrorMessage = "El ID del usuario es obligatorio.")]
    public int UsuarioId { get; set; }
}