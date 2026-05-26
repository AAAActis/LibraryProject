using System.Linq;
using Libreria1.Interfaces;

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
                .FirstOrDefault(l => l.isbn == isbn);

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
                                .Where(l => l.titulo
                                .Contains(termino, StringComparison.OrdinalIgnoreCase))
                                .ToList();

            return libros;
        }

        public List<Libro> BuscarLibrosPorAutor(string autor)
        {
            string termino = autor ?? string.Empty;
            List<Libro> libros = _repositorio.ObtenerTodos()
                                .Where(l => l.autor
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
            var libro = _repositorio.ObtenerTodos().FirstOrDefault(l => l.isbn == isbn);
            if (libro == null)
            {
                throw new LibroNoEncontradoException();
            }

            _repositorio.Eliminar(libro.id);
        }

    }
    
}