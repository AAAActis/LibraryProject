using System;
using System.Collections.Generic;
using System.Linq;
using Libreria1.Interfaces;
using Libreria1.Application.Interfaces;
using Libreria1.Domain.Entities;

namespace Libreria1.Repositories
{
    public class RepositorioEnMemoria<T, TId> : IRepositorio<T, TId> where T : IEntidad<TId>
    {
        // Acá centralizamos el almacenamiento temporal
        private readonly List<T> _datos = new List<T>();

        public void Agregar(T entidad)
        {
            _datos.Add(entidad);
        }

        public void Actualizar(T entidad)
        {
            var existente = ObtenerPorId(entidad.Id);
            if (existente != null)
            {
                _datos.Remove(existente);
            }
            _datos.Add(entidad);
        }

        public T? ObtenerPorId(TId id)
        {
            // EqualityComparer es a prueba de fallos para tipos genéricos
            return _datos.FirstOrDefault(e => EqualityComparer<TId>.Default.Equals(e.Id, id));
        }

        public IEnumerable<T> ObtenerTodos()
        {
            return _datos.ToList();
        }

        public void Eliminar(TId id)
        {
            var entidad = ObtenerPorId(id);
            if (entidad != null)
            {
                _datos.Remove(entidad);
            }
        }

        public IEnumerable<Libro> ObtenerLibrosMasPrestados()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Usuario> UsuariosConPrestamosActivos()
        {
            throw new NotImplementedException();
        }

        object ObtenerMultasAgrupadasPorUsuario()
        {
            return ObtenerMultasAgrupadasPorUsuario();
        }


        IEnumerable<Multa> IRepositorio<T, TId>.ObtenerMultasAgrupadasPorUsuario()
        {
            throw new NotImplementedException();
        }
    }
}