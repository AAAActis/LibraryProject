using System.Collections.Generic;

namespace Libreria1.Interfaces
{
	public interface ICatalogo
    {
        void AgregarLibro(Libro libro);
        Libro?  BuscarLibroPorIsbn(Guid isbn);  
        List<Libro> BuscarLibrosPorTitulo(string titulo);
        List<Libro> BuscarLibrosPorAutor(string autor);
        List<Libro> ListarTodos();
    }
}
