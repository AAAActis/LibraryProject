public interface IUsuarios
{
    void AgregarUsuario(Usuario usuario);
    Usuario BuscarUsuario(Guid idUsuario);
    List<Usuario> ListarUsuarios();

    // se pueden agregar métodos adicionales para la gestión de usuarios, como eliminar o actualizar información de usuario
}