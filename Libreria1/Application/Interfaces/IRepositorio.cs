namespace Libreria1.Interfaces
{
    public interface IRepositorio<T> where T : IEntidad
    {
        void Agregar(T entidad);
        T? ObtenerPorId(Guid id);
        IEnumerable<T> ObtenerTodos();
        void Eliminar(Guid id);
    }
}