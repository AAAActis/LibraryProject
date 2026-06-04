using System;
using System.Collections.Generic;
using System.Linq;
using Libreria1.Interfaces;
using Libreria1.Application.Interfaces;

namespace Libreria1.Repositories
{
    public class RepositorioEnMemoria<T> : IRepositorio<T> where T : IEntidad<Guid>
    {
        // Acá centralizamos el almacenamiento temporal
        private readonly List<T> _datos = new List<T>();

        public void Agregar(T entidad)
        {
            _datos.Add(entidad);
        }

        public T? ObtenerPorId(Guid id)
        {
            return _datos.FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<T> ObtenerTodos()
        {
            return _datos.ToList();
        }

        public void Eliminar(Guid id)
        {
            var entidad = ObtenerPorId(id);
            if (entidad != null)
            {
                _datos.Remove(entidad);
            }
        }
    }
}