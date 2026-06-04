/// <summary>
/// DTO para representar la información de una multa en el sistema.
/// </summary>
public class MultaDto
{
    public Guid PrestamoId { get; set; }
    public int DiasRetraso { get; set; }
    public decimal MontoTotal { get; set; }
    public DateTime FechaGenerada { get; set; }
}