using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Libreria1.Domain.Entities;
using Libreria1.Interfaces;

namespace Libreria1.Repositories;

public class RepositorioPrestamosEF : IRepositorio<Prestamo, Guid>
{
    private readonly LibreriaContext _context;

        public RepositorioPrestamosEF(LibreriaContext context)
        {
            _context = context;
        }

        public void Agregar(Prestamo prestamo)
        {
            // El Libro y el Usuario llegan desconectados (AsNoTracking) desde ServicioPrestamo.
            // Sin adjuntarlos explícitamente, EF Core los trata como entidades nuevas al hacer
            // Prestamos.Add(...) e intenta re-insertarlos, violando la PK ya existente.
            _context.Entry(prestamo.LibroPrestado).State = EntityState.Modified; // EstaDisponible cambió
            _context.Entry(prestamo.UsuarioAsignado).State = EntityState.Unchanged;
            _context.Prestamos.Add(prestamo);
            _context.SaveChanges();
        }

        public Prestamo? ObtenerPorId(Guid id)
        {
            return _context.Prestamos
                .AsNoTracking()
                .Include(p => p.LibroPrestado)
                .Include(p => p.UsuarioAsignado)
                .FirstOrDefault(p => p.Id == id);
        }

        public void Actualizar(Prestamo prestamo)
        {
            _context.Prestamos.Update(prestamo);
            _context.SaveChanges();
        }

        public void Eliminar(Guid id)
        {
            var prestamo = ObtenerPorId(id);
            if (prestamo != null)
            {
                _context.Prestamos.Remove(prestamo);
                _context.SaveChanges(); // Esto genera el DELETE
            }
        }

    public IEnumerable<Prestamo> ObtenerTodos()
    {
        return _context.Prestamos
                .AsNoTracking()
                .Include(p => p.LibroPrestado)
                .Include(p => p.UsuarioAsignado)
                .ToList();
    }

    public IEnumerable<Libro> ObtenerLibrosMasPrestados()
    {
        return _context.Prestamos
            .GroupBy(p => p.LibroPrestado)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .ToList();
    }

        public IEnumerable<Usuario> UsuariosConPrestamosActivos()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Multa> ObtenerMultasAgrupadasPorUsuario()
    {
        throw new NotImplementedException();
    }
}