using Libreria1.Services;
using Libreria1.Domain;
using Libreria1.Application;

public class MenuUsuarioUI
{
    private readonly ServicioUsuario _servicioUsuario;
    private readonly Presenter _presenter = new Presenter();

    public MenuUsuarioUI(ServicioUsuario servicioUsuario)
    {
        _servicioUsuario = servicioUsuario;
    }

    public void MostrarMenu()
    {
        bool volver = false;
        while (!volver)
        {
            Console.Clear();
            _presenter.MostrarTituloUsuario();
            string? opcion = Console.ReadLine();
            switch (opcion)
            {
                case "1":
                    EjecutarRegistrarUsuario();
                    break;
                case "2":
                    EjecutarBuscarUsuarioPorId();
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

    private void EjecutarRegistrarUsuario()
    {
        Console.WriteLine("Ingrese el nombre del usuario:");
        string? nombre = Console.ReadLine();
        Console.WriteLine("Ingrese el email del usuario:");
        string? email = Console.ReadLine();
        _servicioUsuario.RegistrarUsuario(nombre, email);
    }

    private void EjecutarBuscarUsuarioPorId()
    {
        Console.WriteLine("Ingrese el ID del usuario a buscar:");
        string? idUsuario = Console.ReadLine();
        try
        {
            Usuario usuario = _servicioUsuario.BuscarUsuario(Guid.Parse(idUsuario));
            Console.WriteLine($"Usuario encontrado: {usuario.nombre} ({usuario.email})");
        }
        catch (UsuarioNoEncontradoException)
        {
            Console.WriteLine("Usuario no encontrado.");
        }
    }
}
