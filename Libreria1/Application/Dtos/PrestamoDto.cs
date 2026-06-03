public class PrestamoDto
{
    public int NroSocio { get; set; }
    public string IsbnLibro { get; set; }
    public string TituloLibro { get; set; }
    public string NombreUsuario { get; set; }
    public DateTime FechaPrestamo { get; set; }
    public DateTime FechaDevolucion { get; set; }
    public bool Activo { get; set; }
}