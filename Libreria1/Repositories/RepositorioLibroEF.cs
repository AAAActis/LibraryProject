using Libreria1.Application;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Libreria1.Domain.Entities;
using Libreria1.Interfaces;

namespace Libreria1.Repositories;
public class RepositorioLibroEF : IRepositorio<Libro, string>
{
    private readonly LibreriaContext _context;
    private readonly Prestamo prestamo;

    //inyeccion de dependiencias para el contexto de EF Core
    public RepositorioLibroEF(LibreriaContext context)
    {
        _context = context;
    }
    public IEnumerable<Libro> ObtenerLibrosMasPrestados()
    {
    // C# hace la magia del JOIN y el COUNT por ti
    return _context.Libros
        .Include(l => l.Prestamos)
        .OrderByDescending(l => l.Prestamos.Count)
        .ToList();
    }
    public IEnumerable<Libro> ObtenerTodos()
    {
        return _context.Libros
                .AsNoTracking()
                .ToList();
    }

    public Libro? ObtenerPorId(string isbn)
    {
        return _context.Libros
                .AsNoTracking()
                .FirstOrDefault(l => l.Isbn == isbn);
    }

    public void Agregar(Libro libro)
    {
        _context.Libros.Add(libro);
            
        _context.SaveChanges();
    }

    public void Actualizar(Libro libro)
    {
        _context.Libros.Update(libro);
        _context.SaveChanges();
    }

    public void Eliminar(string isbn)
    {
        var libro = ObtenerPorId(isbn);
        if (libro != null)
        {
            _context.Libros.Remove(libro);
            _context.SaveChanges();
        }
    }

    public IEnumerable<Usuario> UsuariosConPrestamosActivos()
    {
        throw new NotImplementedException();
    }

    public object ObtenerMultasAgrupadasPorUsuario()
    {
        throw new NotImplementedException();
    }
}
