using System.Data.Common;
using Libreria1.Interfaces;

public class ServicioPrestamo
{
        private ICatalogo<Libro> catalogo;
        private List<Prestamo> prestamos;
        private IUsuarios servicioUsuarios;

    public ServicioPrestamo(ICatalogo<Libro> catalogo, IUsuarios servicioUsuarios)
    {
        this.catalogo = catalogo;
        this.servicioUsuarios = servicioUsuarios;
        prestamos = new List<Prestamo>();
    }

    public Prestamo PrestarLibro(Guid idUsuario, Guid isbnLibro)
    {
        var libro = catalogo.BuscarLibroPorIsbn(isbnLibro);
        if (libro == null)
        {
            throw new Exception("Libro no encontrado.");
        }

        if(!libro.estaDisponible)
        {
            throw new Exception("El libro fue prestado.");
        }

        var usuario = servicioUsuarios.BuscarUsuario(idUsuario);
        var nuevoPrestamo = new Prestamo(libro, usuario);

        prestamos.Add(nuevoPrestamo);
        return nuevoPrestamo;
    }
            

    public void DevolverLibro(Guid userid, string isbnLibro)
    {
        var prestamo = prestamos.FirstOrDefault(p => p.id == userid && p.libroPrestado.isbn == Guid.Parse(isbnLibro) && p.Activo);
        if (prestamo == null)
        {
            throw new Exception("Préstamo no encontrado.");
        }

        if(prestamo.Activo == true)
        {
            prestamo.CambiarEstado();
        }

        var libro = catalogo.BuscarLibroPorIsbn(Guid.Parse(isbnLibro));
        if (libro != null)
        {
            libro.MarcarDevuelto();
        }
    }
}