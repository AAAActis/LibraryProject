using Libreria1.Application;
using Libreria1.Domain.Menus;
using Libreria1.Interfaces;
using Libreria1.Repositories;
using Libreria1.Services;

class Program
{
    static void Main(string[] args)
    {
        IRepositorio<Libro> repositorio = new RepositorioEnMemoria<Libro>();
        IRepositorio<Usuario> repositorioUsuario = new RepositorioEnMemoria<Usuario>();

        var servicioUsuario = new ServicioUsuario(repositorioUsuario);
        var servicioCatalogo = new ServicioCatalogo(repositorio);
        var servicioPrestamo = new ServicioPrestamo(servicioCatalogo, servicioUsuario);

        IPresenter presenter = new Presenter();
        var libroMenu = new LibroMenu(servicioCatalogo, presenter);
        var usuarioMenu = new UsuarioMenu(servicioUsuario, presenter);
        var prestamoMenu = new PrestamoMenu(servicioPrestamo, servicioCatalogo, servicioUsuario, presenter);
        var menuPrincipal = new MenuPrincipal(libroMenu, usuarioMenu, prestamoMenu, presenter);

        menuPrincipal.Ejecutar();
    }
}
