using System;
using Libreria1.Services;
using Libreria1.Interfaces;
using Libreria1.Repositories;
class Program
{
    static void Main(string[] args)
    {

        
        IRepositorio<Libro> repositorio = new RepositorioEnMemoria<Libro>();
        IRepositorio<Usuario> repositorioUsuario = new RepositorioEnMemoria<Usuario>();


        IUsuarios servicioUsuario = new ServicioUsuario(repositorioUsuario);
        ICatalogo<Libro> catalogo = new ServicioCatalogo(new RepositorioEnMemoria<Libro>());
        ServicioCatalogo servicioCatalogo = new ServicioCatalogo(repositorio);
        ServicioPrestamo servicioPrestamo = new ServicioPrestamo(catalogo, servicioUsuario);


        bool salir = false;

        while (!salir)
        {
            Console.Clear();
            Console.WriteLine("==Sistema de Gestión de Biblioteca==");
            Console.WriteLine("Seleccione una opción:");
            Console.WriteLine("1. Agregar libro");
            Console.WriteLine("2. Buscar libro por ISBN");
            Console.WriteLine("3. Listar todos los libros");
            Console.WriteLine("4. Eliminar libro");
            Console.WriteLine("5. Registrar usuario");
            Console.WriteLine("6. Buscar usuario por ID");
            Console.WriteLine("7. Prestar libro");
            Console.WriteLine("8. Devolver libro");
            Console.WriteLine("9. Listar préstamos activos");
            Console.WriteLine("0. Salir");

            string? opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    Console.WriteLine("Ingrese el ISBN del libro:");
                    //string? isbnInput = Console.ReadLine();
                    Guid isbn = Guid.NewGuid(); // Generar un ISBN único
                    Console.WriteLine("Ingrese el título del libro:");
                    string? titulo = Console.ReadLine();
                    Console.WriteLine("Ingrese el autor del libro:");
                    string? autor = Console.ReadLine();
                    servicioCatalogo.AgregarLibro(new Libro(isbn, titulo, autor));
                    break;
                case "2":
                    // Lógica para buscar libro por ISBN
                    Console.WriteLine("Ingrese el ISBN del libro a buscar:");
                    string? isbnBuscar = Console.ReadLine();
                    try
                    {
                        Libro libroEncontrado = servicioCatalogo.BuscarLibroPorIsbn(Guid.Parse(isbnBuscar));
                        Console.WriteLine($"Libro encontrado: {libroEncontrado.titulo} por {libroEncontrado.autor}");
                    }
                    catch (LibroNoEncontradoException)
                    {
                        Console.WriteLine("Libro no encontrado.");
                    }
                    break;
                case "3":
                    // Lógica para buscar libros por título
                    Console.WriteLine("Ingrese el título del libro a buscar:");
                    string? tituloBuscar = Console.ReadLine();
                    try
                    {
                        List<Libro> librosEncontrados = servicioCatalogo.BuscarLibrosPorTitulo(tituloBuscar);
                        if (librosEncontrados.Count > 0)
                        {
                            Console.WriteLine("Libros encontrados:");
                            foreach (var libro in librosEncontrados)
                            {
                                Console.WriteLine($"{libro.titulo} por {libro.autor}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No se encontraron libros con ese título.");
                        }
                    }
                    catch (LibroNoEncontradoException)
                    {
                        Console.WriteLine("Libro no encontrado.");
                    }
                    break;
                case "4":
                    // Lógica para eliminar libro
                     Console.WriteLine("Ingrese el ISBN del libro a eliminar:");
                    string? isbnEliminar = Console.ReadLine();
                    try
                    {
                        Libro libroAEliminar = servicioCatalogo.BuscarLibroPorIsbn(Guid.Parse(isbnEliminar));
                        servicioCatalogo.EliminarLibro(libroAEliminar.id);
                        Console.WriteLine("Libro eliminado exitosamente.");
                    }
                    catch (LibroNoEncontradoException)
                    {
                        Console.WriteLine("Libro no encontrado.");
                    }
                    break;
                case "5":
                    // Lógica para Registrar usuario
                     Console.WriteLine("Ingrese el nombre del usuario:");
                    string? nombreUsuario = Console.ReadLine();
                    Console.WriteLine("Ingrese el email del usuario:");
                    string? emailUsuario = Console.ReadLine();
                    servicioUsuario.AgregarUsuario(new Usuario(nombreUsuario, emailUsuario));
                    break;
                case "6":
                    // Lógica para Buscar usuario por ID
                    Console.WriteLine("Ingrese el ID del usuario a buscar:");
                    string? idUsuario = Console.ReadLine();
                    try
                    {
                        Usuario usuarioEncontrado = servicioUsuario.BuscarUsuario(Guid.Parse(idUsuario));
                        Console.WriteLine($"Usuario encontrado: {usuarioEncontrado.nombre} ({usuarioEncontrado.email})");
                    }
                    catch (UsuarioNoEncontradoException)
                    {
                        Console.WriteLine("Usuario no encontrado.");
                    }
                    break;
                case "7":
                    // Lógica para Prestar libro
                     Console.WriteLine("Ingrese el ID del usuario que va a prestar el libro:");
                    string? idUsuarioPrestamo = Console.ReadLine();
                    Console.WriteLine("Ingrese el ISBN del libro a prestar:");
                    string? isbnPrestamo = Console.ReadLine();
                    try
                    {
                        Usuario usuarioPrestamo = servicioUsuario.BuscarUsuario(Guid.Parse(idUsuarioPrestamo));
                        Libro libroPrestamo = servicioCatalogo.BuscarLibroPorIsbn(Guid.Parse(isbnPrestamo));
                        servicioPrestamo.PrestarLibro(usuarioPrestamo.Id, libroPrestamo.Id);
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
                    break;
                case "8":
                    // Lógica para Devolver libro
                    Console.WriteLine("Ingrese el ID del usuario que va a devolver el libro:");
                    string? idUsuarioDevolucion = Console.ReadLine();
                    Console.WriteLine("Ingrese el ISBN del libro a devolver:");
                    string? isbnDevolucion = Console.ReadLine();
                    try
                    {
                        servicioPrestamo.DevolverLibro(Guid.Parse(idUsuarioDevolucion), isbnDevolucion);
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
                    break;
                case "9":
                    // Lógica para Listar préstamos activos
                    Console.WriteLine("Préstamos activos:");
                    var prestamosActivos = servicioPrestamo.ListarPrestamosActivos();
                    foreach (var prestamo in prestamosActivos)
                    {
                        Console.WriteLine($"Usuario: {prestamo.usuarioAsignado.nombre}, Libro: {prestamo.libroPrestado.titulo}");
                    }
                    break;
                case "0":
                    salir = true;
                    break;
                default:
                    Console.WriteLine("Opción no válida. Por favor, seleccione una opción del menú.");
                    break;
            }
        }
    }
}
