using Libreria1.Interfaces;

namespace Libreria1.Services
{
    public class ServicioCatalogo : ICatalogo<Libro>
    {
        
        private readonly IRepositorio<Libro> _repositorio;
       
        public ServicioCatalogo(IRepositorio<Libro> repositorio)
        {
            _repositorio = repositorio  ;
        }

        public void AgregarLibro(Libro entidad)
        {
            _repositorio.Agregar(entidad);
        }

        public Libro BuscarLibroPorIsbn(Guid isbn)
        {
            Libro libro = _repositorio.ObtenerPorId(isbn);
            if (libro == null)
            {
                throw new LibroNoEncontradoException();
            }
            else { return libro; }
        }

        public List<Libro> BuscarLibrosPorTitulo(string titulo)
        {
           List<Libro> libros = _repositorio.ObtenerTodos()
                                .Where(l => l.titulo
                                .Contains(titulo, StringComparison.OrdinalIgnoreCase))
                                .ToList();
            if (libros == null)
            {
                throw new LibroNoEncontradoException();
            }
            else { return libros; }
        }
        public List<Libro> BuscarLibrosPorAutor(string autor)
        {
            List<Libro> libros = _repositorio.ObtenerTodos()
                                .Where(l => l.autor
                                .Contains(autor, StringComparison.OrdinalIgnoreCase))
                                .ToList();

            if (_repositorio == null)
            {
                throw new LibroNoEncontradoException();
            } else {
            return libros;
            }
        }

        public List<Libro> ListarTodos()
        {
            return _repositorio.ObtenerTodos().ToList();
        }

    }
    
}