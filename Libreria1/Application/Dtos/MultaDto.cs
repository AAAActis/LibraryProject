/// <summary>
/// DTO para representar la información de una multa en el sistema.
/// </summary>
public class MultaDto
{
    /// <summary>
    /// Identificador del préstamo asociado a la multa.
    /// </summary>
    public Guid PrestamoId { get; set; }
    /// <summary>
    /// Número de días de retraso.
    /// </summary>
    public int DiasRetraso { get; set; }
    /// <summary>
    /// Monto de la multa.
    /// </summary>
    public decimal MontoTotal { get; set; }
    /// <summary>
    /// Fecha en la que se generó la multa.
    /// </summary>
    public DateTime FechaGenerada { get; set; }
}