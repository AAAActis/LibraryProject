using System.Linq;
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

    public Prestamo PrestarLibro(Guid idUsuario, string isbnLibro)
    {   
        
        var libro = catalogo.BuscarLibroPorIsbn(isbnLibro);

        if (!libro.estaDisponible)
        {
            throw new LibroNoDisponibleException();
        }

        var usuario = servicioUsuarios.BuscarUsuario(idUsuario);
        var nuevoPrestamo = new Prestamo(libro, usuario);

        prestamos.Add(nuevoPrestamo);
        return nuevoPrestamo;
    }
            

    public void DevolverLibro(Guid userid, string isbnLibro)
    {
        var prestamo = prestamos.FirstOrDefault(p => p.id == userid && p.libroPrestado.isbn == isbnLibro && p.Activo);
        if (prestamo == null)
        {
            throw new PrestamoNoEncontradoException();
        }

        if (prestamo.Activo)
        {
            prestamo.CambiarEstado();
        }

        var libro = catalogo.BuscarLibroPorIsbn(isbnLibro);
        libro.MarcarDevuelto();
    }

    public List<Prestamo> ListarPrestamosActivos()
    {
        return prestamos.Where(p => p.Activo).ToList();
    }
}