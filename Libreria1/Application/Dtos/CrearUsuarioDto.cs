using System.ComponentModel.DataAnnotations;

namespace Libreria1.Application.DTOs;

/// <summary>
/// DTO para crear un nuevo usuario. Contiene las propiedades necesarias para la creación de un usuario en el sistema.
/// </summary>
public class CrearUsuarioDto
{
    /// <summary>
    /// Nombre del usuario.
    /// </summary>
    /// <example>Juan</example>
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string? Nombre { get; set; }
    /// <summary>
    /// Apellido del usuario.
    /// </summary>
    /// <example>Pérez</example>
    [Required(ErrorMessage = "El apellido es obligatorio.")]
    public string? Apellido { get; set; }
    /// <summary>
    /// Correo electrónico del usuario.
    /// </summary>
    /// <example>juan.perez@example.com</example>
    [EmailAddress(ErrorMessage = "El correo electrónico es inválido.")]
    public string? Email { get; set; }
    /// <summary>
    /// Número de teléfono del usuario.
    /// </summary>
    /// <example>351-123456</example>
    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    public string? Telefono { get; set; }
    /// <summary>
    /// Número de documento del usuario.
    /// </summary>
    /// <example>99123456</example>
    [Required(ErrorMessage = "El documento es obligatorio.")]
    public string? Documento { get; set; }
}