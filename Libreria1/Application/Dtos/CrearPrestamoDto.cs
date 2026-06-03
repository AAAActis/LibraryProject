/// <summary>
/// DTO para crear un nuevo préstamo. 
/// </summary>
public class CrearPrestamoDto
{
    /// <summary>
    /// ISBN del libro a prestar.
    /// </summary>
    /// <example>978-3-16-148410-0</example>
    public string? LibroIsbn { get; set; }
    /// <summary>
    /// Identificador del usuario que realiza el préstamo.
    /// </summary>
    /// <example>e7b8c9d2-c4f7-4</example>
    public string? UsuarioId { get; set; }
}