using System.Linq;
using Libreria1.Interfaces;

public class ServicioPrestamo
{
    private readonly ICatalogo<Libro> catalogo;
    private readonly IUsuarios servicioUsuarios;
    private readonly IRepositorio<Prestamo> repositorioPrestamos;
    public ServicioPrestamo(ICatalogo<Libro> catalogo, IUsuarios servicioUsuarios, IRepositorio<Prestamo> repositorioPrestamos)
    {

        this.catalogo = catalogo;
        this.servicioUsuarios = servicioUsuarios;
        this.repositorioPrestamos = repositorioPrestamos;
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

        repositorioPrestamos.Agregar(nuevoPrestamo);
        return nuevoPrestamo;
    }
            

    public void DevolverLibro(Guid userid, string isbnLibro)
    {
        var prestamo = repositorioPrestamos.ObtenerTodos()
            .FirstOrDefault(p => p.usuarioAsignado.id == userid && p.libroPrestado.isbn == isbnLibro && p.Activo);
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
        return repositorioPrestamos.ObtenerTodos()
        .Where(p => p.Activo)
        .ToList();
    }
}