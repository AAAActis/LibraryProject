using System;
using Libreria1.Application;
using Libreria1.Services;

namespace Libreria1.Domain.Menus
{
    public class LibroMenu : MenuBase
    {
        private readonly ServicioCatalogo _servicioCatalogo;

        public LibroMenu(ServicioCatalogo servicioCatalogo, IPresenter presenter)
            : base(presenter)
        {
            _servicioCatalogo = servicioCatalogo;
        }

        public override void Ejecutar()
        {
            bool volver = false;

            while (!volver)
            {
                Console.Clear();
                MostrarTitulo("Menú de Libros");
                Console.WriteLine("1. Agregar libro");
                Console.WriteLine("2. Buscar libro por ISBN");
                Console.WriteLine("3. Listar libros por título");
                Console.WriteLine("4. Eliminar libro");
                Console.WriteLine("0. Volver al menú principal");
                Console.Write("Opción: ");

                string? opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        AgregarLibro();
                        break;
                    case "2":
                        BuscarLibroPorIsbn();
                        break;
                    case "3":
                        ListarLibrosPorTitulo();
                        break;
                    case "4":
                        EliminarLibro();
                        break;
                    case "0":
                        volver = true;
                        break;
                    default:
                        MostrarMensaje("Opción no válida.");
                        break;
                }

                if (!volver)
                {
                    EsperarContinuar();
                }
            }
        }

        private void AgregarLibro()
        {
            Console.Write("Ingrese el ISBN del libro: ");
            string? isbnInput = Console.ReadLine();

            if (!ValidarIsbn13(isbnInput, out string isbn))
            {
                MostrarMensaje("ISBN inválido. Debe contener exactamente 13 dígitos numéricos.");
                return;
            }

            Console.Write("Ingrese el título del libro: ");
            string? titulo = Console.ReadLine();

            Console.Write("Ingrese el autor del libro: ");
            string? autor = Console.ReadLine();

            _servicioCatalogo.AgregarLibro(new Libro(isbn, titulo ?? string.Empty, autor ?? string.Empty));
            MostrarMensaje($"Libro agregado exitosamente con ISBN: {isbn}");
        }

        private void BuscarLibroPorIsbn()
        {
            Console.Write("Ingrese el ISBN del libro a buscar: ");
            string? isbnBuscar = Console.ReadLine();

            if (!ValidarIsbn13(isbnBuscar, out string isbn))
            {
                MostrarMensaje("ISBN inválido. Debe contener exactamente 13 dígitos numéricos.");
                return;
            }

            try
            {
                Libro libroEncontrado = _servicioCatalogo.BuscarLibroPorIsbn(isbn);
                MostrarMensaje($"Libro encontrado: {libroEncontrado.titulo} por {libroEncontrado.autor}");
            }
            catch (LibroNoEncontradoException)
            {
                MostrarMensaje("Libro no encontrado.");
            }
        }

        private void ListarLibrosPorTitulo()
        {
            Console.Write("Ingrese el título del libro a buscar: ");
            string? tituloBuscar = Console.ReadLine();

            var librosEncontrados = _servicioCatalogo.BuscarLibrosPorTitulo(tituloBuscar ?? string.Empty);

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
                MostrarMensaje("No se encontraron libros con ese título.");
            }
        }

        private void EliminarLibro()
        {
            Console.Write("Ingrese el ISBN del libro a eliminar: ");
            string? isbnEliminar = Console.ReadLine();

            if (!ValidarIsbn13(isbnEliminar, out string isbn))
            {
                MostrarMensaje("ISBN inválido. Debe contener exactamente 13 dígitos numéricos.");
                return;
            }

            try
            {
                _servicioCatalogo.EliminarLibro(isbn);
                MostrarMensaje("Libro eliminado exitosamente.");
            }
            catch (LibroNoEncontradoException)
            {
                MostrarMensaje("Libro no encontrado.");
            }
        }
    }
}
