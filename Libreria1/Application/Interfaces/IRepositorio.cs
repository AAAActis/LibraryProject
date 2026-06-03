using System;
using Libreria1.Domain.Entities;
using System.Collections.Generic;
using Libreria1.Application.Interfaces;

namespace Libreria1.Interfaces
{
    public interface IRepositorio<T> where T : IEntidad<Guid>
    {
        void Agregar(T entidad);
        T? ObtenerPorId(Guid id);
        IEnumerable<T> ObtenerTodos();
        void Eliminar(Guid id);
    }
}