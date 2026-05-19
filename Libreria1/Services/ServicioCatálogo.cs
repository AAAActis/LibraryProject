using Libreria1.Interfaces;

namespace Libreria1.Services
{
    public class ServicioCatalogo : ICatalogo
    {
        private List<Libro> _libros;

        public ServicioCatalogo()
        {
            _libros = new List<Libro>();
        }

        public void AgregarLibro(Libro libro)
        {
            _libros.Add(libro);
        }

        public Libro? BuscarLibroPorIsbn(Guid isbn)
        {
            return _libros.FirstOrDefault(l => l.isbn == isbn);
        }

        public List<Libro> BuscarLibrosPorTitulo(string titulo)
        {
            return _libros.Where(l => l.titulo.Contains(titulo, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<Libro> BuscarLibrosPorAutor(string autor)
        {
            return _libros.Where(l => l.autor.Contains(autor, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<Libro> ListarTodos()
        {
            return _libros.ToList();
        }
    }
    
}