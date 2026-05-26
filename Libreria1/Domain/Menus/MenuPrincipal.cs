using Libreria1.Application;

namespace Libreria1.Domain.Menus
{
    public class MenuPrincipal : MenuBase
    {
        private readonly LibroMenu _libroMenu;
        private readonly UsuarioMenu _usuarioMenu;
        private readonly PrestamoMenu _prestamoMenu;

        public MenuPrincipal(LibroMenu libroMenu, UsuarioMenu usuarioMenu, PrestamoMenu prestamoMenu, IPresenter presenter)
            : base(presenter)
        {
            _libroMenu = libroMenu;
            _usuarioMenu = usuarioMenu;
            _prestamoMenu = prestamoMenu;
        }

        public override void Ejecutar()
        {
            bool salir = false;

            while (!salir)
            {
                Console.Clear();
                MostrarTitulo("Sistema de Gestión de Biblioteca");
                Console.WriteLine("1. Menú Libros");
                Console.WriteLine("2. Menú Usuarios");
                Console.WriteLine("3. Menú Préstamos");
                Console.WriteLine("0. Salir");
                Console.Write("Opción: ");

                string? opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        _libroMenu.Ejecutar();
                        break;
                    case "2":
                        _usuarioMenu.Ejecutar();
                        break;
                    case "3":
                        _prestamoMenu.Ejecutar();
                        break;
                    case "0":
                        salir = true;
                        break;
                    default:
                        MostrarMensaje("Opción no válida. Por favor, seleccione una opción del menú.");
                        break;
                }
            }
        }
    }
}
