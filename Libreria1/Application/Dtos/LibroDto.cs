namespace Libreria1.Application.DTOs;
public class LibroDto
{
    public string? Isbn { get; set; }
    public string? Titulo { get; set; }
    public string? Autor { get; set; }
    public bool EstaDisponible { get; set; }
}