namespace Libreria1.Application.DTOs;
public class CrearLibroDto
{
    public string? ISBN { get; set; }
    public string? Titulo { get; set; }
    public string? Autor { get; set; }
    public string? AñoPublicacion { get; set; }
    public int CantPaginas { get; set; }
}