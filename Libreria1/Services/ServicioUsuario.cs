public class ServicioUsuario : IUsuarios
{
    private List<Usuario> usuarios;
    public ServicioUsuario()
    {
        usuarios = new List<Usuario>();
    }

    // Implementación de los métodos de la interfaz IUsuarios
    public void AgregarUsuario(Usuario usuario)
    {
        usuarios.Add(usuario);
    }
    public Usuario BuscarUsuario(Guid idUsuario)
    {
        var usuario = usuarios.FirstOrDefault(u => u.Id == idUsuario);
        if (usuario == null)
        {
            throw new Exception($"Usuario con ID '{idUsuario}' no encontrado.");
        }
        return usuario;
    }
    public List<Usuario> ListarUsuarios()
    {
        return usuarios;
    }

    // Métodos adicionales para la gestión de usuarios
    public Usuario RegistrarUsuario(string nombre, string email)
    {
        var nuevoUsuario = new Usuario(nombre, email);
        usuarios.Add(nuevoUsuario);
        Console.WriteLine($"Usuario '{nombre}' registrado exitosamente.");
        return nuevoUsuario;
    }
    
    public void EliminarUsuario(Guid idUsuario)
    {
        var usuario = BuscarUsuario(idUsuario);
        usuarios.Remove(usuario);
        Console.WriteLine($"Usuario con ID '{idUsuario}' eliminado exitosamente.");
    }
}       