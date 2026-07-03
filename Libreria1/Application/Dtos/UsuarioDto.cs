namespace Libreria1.Application.DTOs;

/// <summary>
/// DTO para representar la información de un usuario en el sistema.
/// </summary>
public class UsuarioDto
{
    /// <summary>
    /// Identificador único del usuario. 
    /// </summary>
    /// <example>e7b8c9d2-3f4a-4b5c-9a1e-2f3d4e5f6a7b</example>
    public Guid Id { get; set; }

    /// <summary>
    /// Número de socio del usuario, usado para préstamos y multas.
    /// </summary>
    /// <example>1024</example>
    public int NroSocio { get; set; }

    /// <summary>
    /// Nombre del usuario.
    /// </summary>
    /// <example>Juan</example>
    public string? Nombre { get; set; }

    /// <summary>
    /// Apellido del usuario.
    /// </summary>
    /// <example>Pérez</example>
    public string? Apellido { get; set; }

    /// <summary>
    /// Correo electrónico del usuario. 
    /// </summary>
    /// <example>juan.perez@example.com</example>
    public string? Email { get; set; }
}