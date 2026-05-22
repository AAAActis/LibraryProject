using Libreria1.Interfaces;

public class ServicioUsuario : IUsuarios
{
    private readonly IRepositorio<Usuario> _repositorio;
    public ServicioUsuario(IRepositorio<Usuario> repositorio)
    {
        _repositorio = repositorio;
    }

    // Implementación de los métodos de la interfaz IUsuarios
    public void AgregarUsuario(Usuario usuario)
    {
        _repositorio.Agregar(usuario);
    }
    public Usuario BuscarUsuario(Guid idUsuario)
    {
        var usuario = _repositorio.ObtenerPorId(idUsuario);
        if (usuario == null)
        {
            throw new UsuarioNoEncontradoException(idUsuario);
        }
        return usuario;
    }
    public List<Usuario> ListarUsuarios()
    {
        return _repositorio.ObtenerTodos().ToList();
    }

    // Métodos adicionales para la gestión de usuarios
    public Usuario RegistrarUsuario(string nombre, string email)
    {
        var nuevoUsuario = new Usuario(nombre, email);
        _repositorio.Agregar(nuevoUsuario);
        Console.WriteLine($"Usuario '{nombre}' registrado exitosamente.");
        return nuevoUsuario;
    }
    
    public void EliminarUsuario(Guid idUsuario)
    {
        var usuario = BuscarUsuario(idUsuario);

        if (usuario == null)
        {
            throw new UsuarioNoEncontradoException(idUsuario);
        }
        else {
        _repositorio.Eliminar(idUsuario);
        Console.WriteLine($"Usuario con ID '{idUsuario}' eliminado exitosamente.");
        }
    }
}       