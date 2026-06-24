using Libreria1.Domain.Entities;
namespace Libreria1.Application.Interfaces
{
    public interface IUsuarios
    {
        // Registra un usuario y devuelve la entidad creada (con Id Guid y NroSocio asignado)
        Usuario RegistrarUsuario(string nombre, string apellido, string email);

        // Busca por número de socio (1,2,3...) y devuelve la entidad o lanza excepción si no existe
        Usuario BuscarPorNumeroSocio(int nroSocio);
        // Busca por email y devuelve la entidad o lanza excepción si no existe
        Usuario BuscarPorEmail(string email);

        // Busca por Id (Guid)
        Usuario? BuscarPorId(Guid id);

        // Lista todos los usuarios
        List<Usuario> ListarUsuarios();

        // Elimina un usuario por Id y devuelve true si se eliminó
        bool EliminarUsuario(Guid id);
    }
}