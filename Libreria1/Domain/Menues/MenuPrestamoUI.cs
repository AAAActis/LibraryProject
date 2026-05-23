using Libreria1.Services;

public class MenuPrestamoUI
{
    private readonly ServicioPrestamo _servicioPrestamo;
    private readonly ServicioUsuario _servicioUsuario;
    private readonly Presenter _presenter = new Presenter();

    public MenuPrestamoUI(ServicioPrestamo servicioPrestamo, ServicioUsuario servicioUsuario)
    {
        _servicioPrestamo = servicioPrestamo;
        _servicioUsuario = servicioUsuario;
    }

    public void MostrarMenu()
    {
        bool volver = false;
        while (!volver)
        {
            Console.Clear();
            _presenter.MostrarTituloPrestamo();
            string? opcion = Console.ReadLine();
            switch (opcion)
            {
                case "1":
                    EjecutarPrestarLibro();
                    break;
                case "2":
                    EjecutarDevolverLibro();
                    break;
                case "3":
                    EjecutarListarPrestamosActivos();
                    break;
                case "0":
                    volver = true;
                    break;
                default:
                    Console.WriteLine("Opción no válida.");
                    break;
            }

            if (!volver)
            {
                Console.WriteLine("Presione Enter para continuar...");
                Console.ReadLine();
            }
        }
    }

    private void EjecutarPrestarLibro()
    {
        Console.WriteLine("Ingrese el ID del usuario que va a prestar el libro:");
        string? idUsuarioPrestamo = Console.ReadLine();
        Console.WriteLine("Ingrese el ISBN del libro a prestar:");
        string? isbnPrestamo = Console.ReadLine();
        try
        {
            var usuario = _servicioUsuario.BuscarUsuario(Guid.Parse(idUsuarioPrestamo));
            var prestamo = _servicioPrestamo.PrestarLibro(usuario.Id, Guid.Parse(isbnPrestamo));
            Console.WriteLine("Libro prestado exitosamente.");
        }
        catch (UsuarioNoEncontradoException)
        {
            Console.WriteLine("Usuario no encontrado.");
        }
        catch (LibroNoEncontradoException)
        {
            Console.WriteLine("Libro no encontrado.");
        }
        catch (LibroNoDisponibleException)
        {
            Console.WriteLine("Libro no disponible.");
        }
    }

    private void EjecutarDevolverLibro()
    {
        Console.WriteLine("Ingrese el ID del usuario que va a devolver el libro:");
        string? idUsuarioDevolucion = Console.ReadLine();
        Console.WriteLine("Ingrese el ISBN del libro a devolver:");
        string? isbnDevolucion = Console.ReadLine();
        try
        {
            _servicioPrestamo.DevolverLibro(Guid.Parse(idUsuarioDevolucion), isbnDevolucion);
            Console.WriteLine("Libro devuelto exitosamente.");
        }
        catch (PrestamoNoEncontradoException)
        {
            Console.WriteLine("Préstamo no encontrado.");
        }
        catch (LibroNoEncontradoException)
        {
            Console.WriteLine("Libro no encontrado.");
        }
    }

    private void EjecutarListarPrestamosActivos()
    {
        var prestamos = _servicioPrestamo.ListarPrestamosActivos();
        if (prestamos.Count == 0)
        {
            Console.WriteLine("No hay préstamos activos.");
            return;
        }
        Console.WriteLine("Préstamos activos:");
        foreach (var p in prestamos)
        {
            Console.WriteLine($"Usuario: {p.usuarioAsignado.nombre}, Libro: {p.libroPrestado.titulo}");
        }
    }
}
