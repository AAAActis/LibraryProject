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
        private readonly DbContext _context;

        public RepositorioMultasEF(LibreriaContext context)
        {
            _context = context;
        }

        public void Agregar(Multa entidad)
        {
            _context.Set<Multa>().Add(entidad);
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

    }
}