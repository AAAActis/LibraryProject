using System.Collections.Generic;

namespace Libreria1.Interfaces
{
    public interface ICatalogo<Libro>
    {
        void AgregarLibro(Libro libro);
        Libro BuscarLibroPorIsbn(string isbn);
        List<Libro> BuscarLibrosPorTitulo(string titulo);
        List<Libro> BuscarLibrosPorAutor(string autor);
        List<Libro> ListarTodos();
    }
}
