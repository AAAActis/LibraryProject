using System;
using System.Linq;
using System.Collections.Generic;
using Libreria1.Interfaces;
using Libreria1.Domain.Entities;
using Libreria1.Application.Interfaces;

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

    public Prestamo PrestarLibro(int nroSocio, string isbnLibro)
    {
        // validar que el usuario exista
        var usuario = servicioUsuarios.BuscarPorNumeroSocio(nroSocio);
        if (usuario == null)
        {
            throw new UsuarioNoEncontradoException($"Usuario con número de socio '{nroSocio}' no encontrado.");
        }
        var libro = catalogo.BuscarLibroPorIsbn(isbnLibro);
        if (libro == null)
        {
            throw new LibroNoEncontradoException(isbnLibro);
        }
        // validar que el libro esté disponible
        ValidarDisponibilidadLibro(isbnLibro);
    
        // validar que el usuario no tenga más de 3 libros prestados
        ValidarLimitePrestamos(nroSocio);

        // marcar el libro como prestado
        libro.MarcarPrestado();

        // crear el préstamo 
        var Usuario = servicioUsuarios.BuscarPorNumeroSocio(nroSocio);
        var nuevoPrestamo = new Prestamo(libro, usuario);

        repositorioPrestamos.Agregar(nuevoPrestamo);
        return nuevoPrestamo;
    }
            

    public Prestamo DevolverLibro(int nroSocio, string isbnLibro)
    {
        // validar que el prestamo pertenece al usuario

        var prestamo = repositorioPrestamos.ObtenerTodos()
            .FirstOrDefault(p => p.UsuarioAsignado.NroSocio == nroSocio
            && p.LibroPrestado.Isbn == isbnLibro 
            && p.Activo);
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
        return prestamo;
    }

    public Prestamo ObtenerPrestamoEspecifico(int nroSocio, string isbnLibro)
    {
        var prestamo = repositorioPrestamos.ObtenerTodos()
            .FirstOrDefault(p => p.UsuarioAsignado.NroSocio == nroSocio
            && p.LibroPrestado.Isbn == isbnLibro);
        if (prestamo == null)
        {
            throw new PrestamoNoEncontradoException();
        }
        return prestamo;
    }
    public List<Prestamo> ListarPrestamosActivos()
    {
        return repositorioPrestamos.ObtenerTodos()
        .Where(p => p.Activo)
        .ToList();
    }

    public List<Prestamo> ListarPrestamosPorUsuario(int nroSocio)
    {
        return repositorioPrestamos.ObtenerTodos()
        .Where(p => p.UsuarioAsignado.NroSocio == nroSocio)
        .ToList();  
    }

    // validar que el usuario no tenga más de 3 libros prestados
    public void ValidarLimitePrestamos(int nroSocio)
    {
        var prestamosActivos = ListarPrestamosPorUsuario(nroSocio)
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