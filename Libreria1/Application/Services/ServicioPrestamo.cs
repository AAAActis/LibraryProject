using System;
using System.Linq;
using System.Collections.Generic;
using Libreria1.Interfaces;

public class ServicioPrestamo
{
    private readonly ICatalogo<Libro> catalogo;
    private readonly IUsuarios servicioUsuarios;
    private readonly IRepositorio<Prestamo> repositorioPrestamos;
    private readonly IServicioMulta servicioMultas;
    public ServicioPrestamo(ICatalogo<Libro> catalogo, IUsuarios servicioUsuarios, IRepositorio<Prestamo> repositorioPrestamos, IServicioMulta servicioMultas)
    {

        this.catalogo = catalogo;
        this.servicioUsuarios = servicioUsuarios;
        this.repositorioPrestamos = repositorioPrestamos;
        this.servicioMultas = servicioMultas;
    }

    public Prestamo PrestarLibro(Guid idUsuario, string isbnLibro)
    {
        var libro = catalogo.BuscarLibroPorIsbn(isbnLibro);
        if (libro == null)
        {
            throw new LibroNoEncontradoException(isbnLibro);
        }
        // validar que el libro esté disponible
        ValidarDisponibilidadLibro(isbnLibro);
    
        // validar que el usuario no tenga más de 3 libros prestados
        ValidarLimitePrestamos(idUsuario);

        // marcar el libro como prestado
        libro.MarcarPrestado();

        // crear el préstamo 
        var usuario = servicioUsuarios.BuscarUsuario(idUsuario);
        var nuevoPrestamo = new Prestamo(libro, usuario);

        repositorioPrestamos.Agregar(nuevoPrestamo);
        return nuevoPrestamo;
    }
            

    public void DevolverLibro(Guid userid, string isbnLibro)
    {
        var prestamo = repositorioPrestamos.ObtenerTodos()
            .FirstOrDefault(p => p.UsuarioAsignado.id == userid && p.LibroPrestado.Isbn == isbnLibro && p.Activo);
        if (prestamo == null)
        {
            throw new PrestamoNoEncontradoException();
        }
        servicioMultas.CalcularMulta(prestamo);

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

    public List<Prestamo> ListarPrestamosPorUsuario(Guid idUsuario)
    {
        return repositorioPrestamos.ObtenerTodos()
        .Where(p => p.UsuarioAsignado.id == idUsuario)
        .ToList();  
    }

    // validar que el usuario no tenga más de 3 libros prestados
    public void ValidarLimitePrestamos(Guid idUsuario)
    {
        var prestamosActivos = ListarPrestamosPorUsuario(idUsuario)
            .Where(p => p.Activo)
            .Count();

        if (prestamosActivos >= 3)
        {
            throw new LimitePrestamosAlcanzadoException();
        }
    }

    // validar no prestar un libro que ya esté prestado
    public void ValidarDisponibilidadLibro(string isbnLibro)
    {
        var libro = catalogo.BuscarLibroPorIsbn(isbnLibro);
        if (!libro.EstaDisponible)
        {
            throw new LibroNoDisponibleException();
        }
    }
}