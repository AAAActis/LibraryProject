using System.Linq;
using Libreria1.Interfaces;
using Libreria1.Domain.Entities;
using Libreria1.Repositories;
using Libreria1.Application.Interfaces;

namespace Libreria1.Services
{
    public class ServicioCatalogo : ICatalogo<Libro>
    {
        private readonly IRepositorio<Libro> _repositorio;

        public ServicioCatalogo(IRepositorio<Libro> repositorio)
        {
            _repositorio = repositorio;
        }

        public void AgregarLibro(Libro entidad)
        {
            _repositorio.Agregar(entidad);
        }

        public Libro BuscarLibroPorIsbn(string isbn)
        {
            var libro = _repositorio.ObtenerTodos()
                .FirstOrDefault(l => l.Isbn == isbn);

            if (libro == null)
            {
                throw new LibroNoEncontradoException();
            }

            return libro;
        }

        public List<Libro> BuscarLibrosPorTitulo(string titulo)
        {
            string termino = titulo ?? string.Empty;
            List<Libro> libros = _repositorio.ObtenerTodos()
                                .Where(l => l.Titulo
                                .Contains(termino, StringComparison.OrdinalIgnoreCase))
                                .ToList();

            return libros;
        }

        public List<Libro> BuscarLibrosPorAutor(string autor)
        {
            string termino = autor ?? string.Empty;
            List<Libro> libros = _repositorio.ObtenerTodos()
                                .Where(l => l.Autor
                                .Contains(termino, StringComparison.OrdinalIgnoreCase))
                                .ToList();

            return libros;
        }

        public List<Libro> ListarTodos()
        {
            return _repositorio.ObtenerTodos().ToList();
        }

        public void EliminarLibro(string isbn)
        {
            var libro = _repositorio.ObtenerTodos().FirstOrDefault(l => l.Isbn == isbn);
            if (libro == null)
            {
                throw new LibroNoEncontradoException();
            }

            _repositorio.Eliminar(libro.Id);
        }

    }
    
}