/// <summary>
/// DTO para crear un nuevo usuario. Contiene las propiedades necesarias para la creación de un usuario en el sistema.
/// </summary>
public class CrearUsuarioDto
{
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
    public string? Correo { get; set; }
    /// <summary>
    /// Número de teléfono del usuario.
    /// </summary>
    /// <example>351-123456</example>
    public string? Telefono { get; set; }
    /// <summary>
    /// Número de documento del usuario.
    /// </summary>
    /// <example>99123456</example>
    public string? Documento { get; set; }
}