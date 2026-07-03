namespace Libreria1.Application.DTOs;
/// <summary>
/// DTO para representar un libro.
/// </summary>
public class LibroDto
{
    public string? Isbn { get; set; }
    public string? Titulo { get; set; }
    public string? Autor { get; set; }
    public int AnioPublicacion { get; set; }
    public int CantPaginas { get; set; }
    public bool EstaDisponible { get; set; }
}