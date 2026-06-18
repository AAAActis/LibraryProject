using Libreria1.Application.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Libreria1.Domain.Entities;
public class Prestamo : IEntidad<Guid>
{
    
    [Key]
    public Guid Id { get; private set; } = Guid.NewGuid();

    // Claves foráneas explícitas
    public string LibroId { get; private set; }
    public Guid UsuarioId { get; private set; }

    // Propiedades de navegación atadas a las FK
    [ForeignKey("LibroId")]
    public Libro LibroPrestado { get; private set; }

    [ForeignKey("UsuarioId")]
    public Usuario UsuarioAsignado { get; private set; }
    
    public DateTime FechaPrestamo { get; private set; }
    public DateTime? FechaDevolucion { get; private set; }
    public bool Activo { get; private set; }

    // [NotMapped] evita que EF Core intente crear columnas "titulo" o "autor" en PostgreSQL
    [NotMapped] public string titulo => LibroPrestado?.Titulo;
    [NotMapped] public string autor => LibroPrestado?.Autor;
    [NotMapped] public bool estaDisponible => LibroPrestado?.EstaDisponible ?? false;

    protected Prestamo() { } // Constructor protegido para EF Core
    public Prestamo(Libro libro, Usuario usuario)
    {
        Id = Guid.NewGuid();
        LibroPrestado = libro;
        UsuarioAsignado = usuario;
        FechaPrestamo = DateTime.Now;
        FechaDevolucion = FechaPrestamo.AddDays(14); // Plazo de 14 días para la devolución
        Activo = true;
        LibroPrestado.MarcarPrestado();
    }
    

    internal Prestamo(Guid id, Libro libro, Usuario usuario, DateTime fechaPrestamo, DateTime? fechaDevolucion, bool activo)
{
    Id = id;
    LibroPrestado = libro;
    UsuarioAsignado = usuario;
    FechaPrestamo = fechaPrestamo;
    FechaDevolucion = fechaDevolucion;
    Activo = activo;
}

    public void CambiarEstado()
    {
        if (Activo)
        {
            Activo = false;
            LibroPrestado.MarcarDevuelto();
        }
        else
        {
            Activo = true;
            LibroPrestado.MarcarPrestado();
        }
    }
}