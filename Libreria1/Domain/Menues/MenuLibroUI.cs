using Libreria1.Interfaces;
using Libreria1.Services;

public class MenuLibroUI
{
    private readonly ServicioCatalogo _servicioCatalogo;
    private readonly Presenter _presenter = new Presenter();

    // Inyectar el servicio de catálogo a través del constructor
    public MenuLibroUI(ServicioCatalogo servicioCatalogo)
    {
        _servicioCatalogo = servicioCatalogo;
    }

    public void EjecutarAgregarLibro()
    {
        string? isbnInput;
        string? titulo;
        string? autor;

        do {
            Console.WriteLine("Ingrese el ISBN del libro (13 caracteres):");
            isbnInput = Console.ReadLine();
            if (isbnInput != null && isbnInput.Length == 13)
            {
                Console.WriteLine("Ingrese el título del libro:");
                titulo = Console.ReadLine();

                Console.WriteLine("Ingrese el autor del libro:");
                autor = Console.ReadLine();

                _servicioCatalogo.AgregarLibro(new Libro(isbnInput, titulo, autor));
                Console.WriteLine("Libro agregado exitosamente.");
                break;
            }
            else
            {
                Console.WriteLine("ISBN debe tener 13 caracteres. Intente nuevamente.");
            }
        } while (true);
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                

        _servicioCatalogo.AgregarLibro(new Libro(isbnInput, titulo, autor));
        Console.WriteLine("Libro agregado exitosamente.");
    }

    public void EjecutarBuscarLibroPorIsbn()
    {
        Console.WriteLine("Ingrese el ISBN del libro a buscar:");
        string? isbnBuscar = Console.ReadLine();
        try
        {
            Libro libroEncontrado = _servicioCatalogo.BuscarLibroPorIsbn(Guid.Parse(isbnBuscar));
            Console.WriteLine($"Libro encontrado: {libroEncontrado.titulo} por {libroEncontrado.autor}");
        }
        catch (LibroNoEncontradoException)
        {
            Console.WriteLine("Libro no encontrado.");
        }
    }
    public void EjecutarBuscarLibroPorTitulo()
    {
    Console.WriteLine("Ingrese el título del libro a buscar:");
                    string? tituloBuscar = Console.ReadLine();
                    try
                    {
                        List<Libro> librosEncontrados = _servicioCatalogo.BuscarLibrosPorTitulo(tituloBuscar);
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
    }

    public void EjecutarListarTodos()
    {
        var lista = _servicioCatalogo.ListarTodos();
        if (lista.Count == 0)
        {
            Console.WriteLine("No hay libros cargados.");
            return;
        }
        Console.WriteLine("Libros:");
        foreach (var libro in lista)
        {
            Console.WriteLine($"{libro.id} - {libro.titulo} por {libro.autor}");
        }
    }

                    public void EjecutarEliminarLibro(){
                     Console.WriteLine("Ingrese el ISBN del libro a eliminar:");
                    string? isbnEliminar = Console.ReadLine();
                    try
                    {
                        Libro libroAEliminar = _servicioCatalogo.BuscarLibroPorIsbn(Guid.Parse(isbnEliminar));
                        _servicioCatalogo.EliminarLibro(libroAEliminar.id);
                        Console.WriteLine("Libro eliminado exitosamente.");
                    }
                    catch (LibroNoEncontradoException)
                    {
                        Console.WriteLine("Libro no encontrado.");
                    }
                    }

                    public void MostrarMenu()
                    {
                        bool volver = false;
                        while (!volver)
                        {
                            Console.Clear();
                            _presenter.MostrarTituloLibro();
                            string? opcion = Console.ReadLine();
                            switch (opcion)
                            {
                                case "1":
                                    EjecutarAgregarLibro();
                                    break;
                                case "2":
                                    EjecutarBuscarLibroPorIsbn();
                                    break;
                                case "3":
                                    EjecutarListarTodos();
                                    break;
                                case "4":
                                    EjecutarEliminarLibro();
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
}

