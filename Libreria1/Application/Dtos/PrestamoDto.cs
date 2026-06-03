/// <summary>
/// DTO para representar la información de un préstamo en el sistema. 
/// </summary>
public class PrestamoDto
{
    /// <summary>
    /// Identificador único del préstamo.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// ISBN del libro prestado.
    /// </summary>
    public string? LibroIsbn { get; set; }
    /// <summary>
    /// Identificador del usuario que realiza el préstamo.
    /// </summary>
    public string? UsuarioId { get; set; }
    /// <summary>
    /// Fecha en la que se realiza el préstamo.
    /// </summary>
    public DateTime FechaPrestamo { get; set; }
    /// <summary>
    /// Indica si el préstamo está activo.
    /// </summary>
    public bool EstaActivo { get; set; }
}