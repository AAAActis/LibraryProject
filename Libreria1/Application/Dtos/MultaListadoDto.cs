/// <summary>
/// DTO plano para listar todas las multas, pensado para vistas administrativas
/// (tabla de multas del frontend).
/// </summary>
public class MultaListadoDto
{
    public Guid Id { get; set; }
    public string LibroTitulo { get; set; }
    public string UsuarioNombre { get; set; }
    public int DiasRetraso { get; set; }
    public decimal MontoTotal { get; set; }
    public DateTime FechaGenerada { get; set; }
}
