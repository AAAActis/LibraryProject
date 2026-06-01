namespace Libreria1.Interfaces
{
    public interface IUsuarios
    {
        // Registra un usuario y devuelve la entidad creada (con Id Guid y NroSocio asignado)
        Usuario RegistrarUsuario(string nombre, string email);

        // Busca por número de socio (1,2,3...) y devuelve la entidad o lanza excepción si no existe
        Usuario BuscarPorNumeroSocio(int nroSocio);

        // Busca por Id (Guid)
        Usuario? BuscarPorId(Guid id);

        // Lista todos los usuarios
        List<Usuario> ListarUsuarios();

        // Elimina un usuario por Id y devuelve true si se eliminó
        bool EliminarUsuario(Guid id);
    }
}