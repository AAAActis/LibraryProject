using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Libreria1.Domain.Entities;
using Libreria1.Application.Interfaces;
using Libreria1.Interfaces;

namespace Libreria1.Repositories
{
    public class RepositorioMultasEF : IRepositorio<Multa, Guid>
    {
        private readonly LibreriaContext _context;

        public RepositorioMultasEF(LibreriaContext context)
        {
            _context = context;
        }

        public object ObtenerMultasAgrupadasPorUsuario()
        {
            var multasPorUsuario = _context.Set<Multa>()
                .Include(m => m.PrestamoAsignado) // Cambiado a PrestamoAsignado
                    .ThenInclude(p => p.UsuarioAsignado) // Cambiado a UsuarioAsignado
                .GroupBy(m => m.PrestamoAsignado.UsuarioAsignado.NroSocio) // Cambiado a la ruta correcta
                .Select(grupo => new 
                {
                    NumeroSocio = grupo.Key,               // La llave por la que agrupamos
                    CantidadDeMultas = grupo.Count(),      // Contamos cuántas multas tiene
                    TotalDiasRetraso = grupo.Sum(m => m.DiasRetraso) 
                })
                .ToList();

            return multasPorUsuario;
        }

        public void Agregar(Multa entidad)
        {
            // El Prestamo (y su Libro/Usuario) llegan desconectados desde ServicioMultas
            // (AsNoTracking en los repos de origen). Sin adjuntarlos, EF Core los trata
            // como entidades nuevas al hacer Add(...) e intenta re-insertarlos, violando
            // la PK ya existente en la fila real.
            if (entidad.PrestamoAsignado != null)
            {
                _context.Entry(entidad.PrestamoAsignado).State = EntityState.Unchanged;
                _context.Entry(entidad.PrestamoAsignado.LibroPrestado).State = EntityState.Unchanged;
                _context.Entry(entidad.PrestamoAsignado.UsuarioAsignado).State = EntityState.Unchanged;
            }
            _context.Set<Multa>().Add(entidad);
            _context.SaveChanges();
        }

        public void Actualizar(Multa entidad)
        {
            _context.Set<Multa>().Update(entidad);
            _context.SaveChanges();
        }

        public Multa? ObtenerPorId(Guid id)
        {
           return _context.Set<Multa>()
                .Include(m => m.PrestamoAsignado)
                    .ThenInclude(p => p.LibroPrestado)
                .Include(m => m.PrestamoAsignado)
                    .ThenInclude(p => p.UsuarioAsignado)
                .FirstOrDefault(m => m.Id == id);
        }

        public IEnumerable<Multa> ObtenerTodos()
        {
            return _context.Set<Multa>()
                .Include(m => m.PrestamoAsignado)
                    .ThenInclude(p => p.LibroPrestado)
                .Include(m => m.PrestamoAsignado)
                    .ThenInclude(p => p.UsuarioAsignado)
                .ToList();
        }

        public void Eliminar(Guid id)
        {
            var multa = _context.Set<Multa>().Find(id);
            if (multa != null)
            {
                _context.Set<Multa>().Remove(multa);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Multa> ObtenerMultasPorPrestamo(Guid prestamoId)
        {
            return _context.Set<Multa>()
                .Include(m => m.PrestamoAsignado)
                .Where(m => m.PrestamoId == prestamoId)
                .ToList();
        }

        public IEnumerable<Multa> ObtenerMultasVencidas()
        {
            // La fecha se evalúa del lado del servidor de base de datos
            var limite = DateTime.Now.AddDays(-30); 
            return _context.Set<Multa>()
                .Include(m => m.PrestamoAsignado)
                .Where(m => m.FechaGenerada < limite)
                .ToList();
        }

        public IEnumerable<Libro> ObtenerLibrosMasPrestados()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Usuario> UsuariosConPrestamosActivos()
        {
            throw new NotImplementedException();
        }
    }
}