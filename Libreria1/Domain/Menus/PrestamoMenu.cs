using System;
using Libreria1.Interfaces;
using Libreria1.Services;

namespace Libreria1.Domain.Menus
{
    public class PrestamoMenu : MenuBase
    {
        private readonly ServicioPrestamo _servicioPrestamo;
        private readonly ServicioCatalogo _servicioCatalogo;
        private readonly IUsuarios _servicioUsuario;

        public PrestamoMenu(ServicioPrestamo servicioPrestamo, ServicioCatalogo servicioCatalogo, IUsuarios servicioUsuario)
        {
            _servicioPrestamo = servicioPrestamo;
            _servicioCatalogo = servicioCatalogo;
            _servicioUsuario = servicioUsuario;
        }

        public override void Ejecutar()
        {
            bool volver = false;

            while (!volver)
            {
                Console.Clear();
                MostrarTitulo("Menú de Préstamos");
                Console.WriteLine("1. Prestar libro");
                Console.WriteLine("2. Devolver libro");
                Console.WriteLine("3. Listar préstamos activos");
                Console.WriteLine("0. Volver al menú principal");
                Console.Write("Opción: ");

                string? opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        PrestarLibro();
                        break;
                    case "2":
                        DevolverLibro();
                        break;
                    case "3":
                        ListarPrestamosActivos();
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

        private void PrestarLibro()
        {
            Console.Write("Ingrese el numero de socio del usuario que va a prestar el libro: ");
            string? nroSocioPrestamo = Console.ReadLine();

            if (!int.TryParse(nroSocioPrestamo, out int nroSocio))
            {
                MostrarMensaje("Número de socio inválido.");
                return;
            }

            Console.Write("Ingrese el ISBN del libro a prestar: ");
            string? isbnPrestamo = Console.ReadLine();

            if (!ValidarIsbn13(isbnPrestamo, out string isbn))
            {
                MostrarMensaje("ISBN inválido. Debe contener exactamente 13 dígitos numéricos.");
                return;
            }

            try
            {
                _servicioPrestamo.PrestarLibro(nroSocio, isbn);
                MostrarMensaje("Libro prestado exitosamente.");
            }
            catch (UsuarioNoEncontradoException)
            {
                MostrarMensaje("Usuario no encontrado.");
            }
            catch (LibroNoEncontradoException)
            {
                MostrarMensaje("Libro no encontrado.");
            }
            catch (LibroNoDisponibleException)
            {
                MostrarMensaje("El libro no está disponible.");
            }
        }

        private void DevolverLibro()
        {
            Console.Write("Ingrese el número de socio del usuario que va a devolver el libro: ");
            string? nroSocioDevolucion = Console.ReadLine();

            if (!int.TryParse(nroSocioDevolucion, out int nroSocio))
            {
                MostrarMensaje("Número de socio inválido.");
                return;
            }

            Console.Write("Ingrese el ISBN del libro a devolver: ");
            string? isbnDevolucion = Console.ReadLine();

            if (!ValidarIsbn13(isbnDevolucion, out string isbn))
            {
                MostrarMensaje("ISBN inválido. Debe contener exactamente 13 dígitos numéricos.");
                return;
            }

            try
            {
                _servicioPrestamo.DevolverLibro(nroSocio, isbn);
                MostrarMensaje("Libro devuelto exitosamente.");
            }
            catch (PrestamoNoEncontradoException)
            {
                MostrarMensaje("Préstamo no encontrado.");
            }
            catch (LibroNoEncontradoException)
            {
                MostrarMensaje("Libro no encontrado.");
            }
        }

        private void ListarPrestamosActivos()
        {
            var prestamosActivos = _servicioPrestamo.ListarPrestamosActivos();

            if (prestamosActivos.Count > 0)
            {
                Console.WriteLine("Préstamos activos:");
                foreach (var prestamo in prestamosActivos)
                {
                    Console.WriteLine($"Usuario: {prestamo.UsuarioAsignado.Nombre}, Libro: {prestamo.LibroPrestado.Titulo}");
                }
            }
            else
            {
                MostrarMensaje("No hay préstamos activos.");
            }
        }
    }
}
