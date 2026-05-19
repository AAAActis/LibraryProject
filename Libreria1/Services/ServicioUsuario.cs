public class ServicioUsuario
{
    private List<Usuario> usuarios;
    public ServicioUsuario()
    {
        usuarios = new List<Usuario>();
    }
    public Usuario RegistrarUsuario(string nombre, string email)
    {
        var nuevoUsuario = new Usuario(nombre, email);
        usuarios.Add(nuevoUsuario);
        Console.WriteLine($"Usuario '{nombre}' registrado exitosamente.");
        return nuevoUsuario;
    }
    

    public Usuario BuscarUsuario(Guid idUsuario)
    {
        var usuario = usuarios.FirstOrDefault(u => u.IdUuid == idUsuario);
        if (usuario == null)
        {
            throw new Exception($"Usuario con ID '{idUsuario}' no encontrado.");
        }
        return usuario;
    }
    public void EliminarUsuario(Guid idUsuario)
    {
        var usuario = BuscarUsuario(idUsuario);
        usuarios.Remove(usuario);
        Console.WriteLine($"Usuario con ID '{idUsuario}' eliminado exitosamente.");
    }
}       