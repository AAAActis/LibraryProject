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
        IRepositorio<Prestamo> repositorioPrestamos = new RepositorioEnMemoria<Prestamo>();

        var servicioUsuario = new ServicioUsuario(repositorioUsuario);
        var servicioCatalogo = new ServicioCatalogo(repositorio);
        var servicioPrestamo = new ServicioPrestamo(servicioCatalogo, servicioUsuario, repositorioPrestamos);


        var libroMenu = new LibroMenu(servicioCatalogo);
        var usuarioMenu = new UsuarioMenu(servicioUsuario);
        var prestamoMenu = new PrestamoMenu(servicioPrestamo, servicioCatalogo, servicioUsuario);
        var menuPrincipal = new MenuPrincipal(libroMenu, usuarioMenu, prestamoMenu);

        menuPrincipal.Ejecutar();
    }
}
