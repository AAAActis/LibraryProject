using Libreria1.Application.Interfaces;
namespace Libreria1.Domain.Entities;
public class Prestamo : IEntidad<Guid>
{
    public Guid Id {get; private set;} = Guid.NewGuid();
    public Libro LibroPrestado {get; private set;}
    public Usuario UsuarioAsignado {get; private set;}
    public DateTime FechaPrestamo {get; private set;}
    public DateTime? FechaDevolucion {get; private set;}
    public bool Activo {get; private set;}


    public string titulo => LibroPrestado.Titulo;
    public string autor => LibroPrestado.Autor;
    public bool estaDisponible => LibroPrestado.EstaDisponible;


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