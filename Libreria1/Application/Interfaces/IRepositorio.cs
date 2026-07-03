using System;
using System.Collections.Generic;
using Libreria1.Domain.Entities;  
using Libreria1.Application.Interfaces;  
using System.Collections.Generic;


namespace Libreria1.Interfaces
{
    public interface IRepositorio<T, TId> where T : IEntidad<TId>
    {
        void Agregar(T entidad);
        void Actualizar(T entidad);
        T? ObtenerPorId(TId id);
        IEnumerable<T> ObtenerTodos();
        void Eliminar(TId id);

        IEnumerable<Libro> ObtenerLibrosMasPrestados();
        IEnumerable<Usuario> UsuariosConPrestamosActivos();
        IEnumerable<Multa> ObtenerMultasAgrupadasPorUsuario();
    }
}