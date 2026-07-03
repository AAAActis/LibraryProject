/// <summary>
/// DTO para listar préstamos con datos de libro y usuario ya resueltos,
/// pensado para vistas administrativas (tabla de préstamos del frontend).
/// </summary>
public class PrestamoListadoDto
{
    public Guid Id { get; set; }
    public string LibroIsbn { get; set; }
    public string LibroTitulo { get; set; }
    public int NroSocio { get; set; }
    public string UsuarioNombre { get; set; }
    public DateTime FechaPrestamo { get; set; }
    public DateTime? FechaDevolucion { get; set; }
    public bool Activo { get; set; }
}
