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

        Presenter presenter = new Presenter();

        MenuLibroUI menuL = new MenuLibroUI(servicioCatalogo);
        MenuUsuarioUI menuU = new MenuUsuarioUI((ServicioUsuario)servicioUsuario);
        MenuPrestamoUI menuP = new MenuPrestamoUI(servicioPrestamo, (ServicioUsuario)servicioUsuario);

        bool salir = false;

        while (!salir)
        {
            Console.Clear();
            presenter.MostrarMensaje("Seleccione una opción:");
            presenter.MostrarMenuPrincipal();

            string? opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    menuL.MostrarMenu();
                    break;
                case "2":
                    menuU.MostrarMenu();
                    break;
                case "3":
                    menuP.MostrarMenu();
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
