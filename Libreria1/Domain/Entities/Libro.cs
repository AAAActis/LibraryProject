//Responsabilidad unica: Representar un libro en el sistema de biblioteca.
public class Libro : IEntidad
{
    public Guid id {get; private set;} = Guid.NewGuid();
    public string Isbn {get; private set;}
    public string Titulo {get; private set;}
    public string Autor {get; private set;}
    public bool EstaDisponible {get; private set;} 


    public Libro(string isbn, string titulo, string autor)
    {
        id = Guid.NewGuid();
        Isbn = isbn;
        Titulo = titulo;
        Autor = autor;
        EstaDisponible = true;
    }

    public void MarcarPrestado()
    {
        EstaDisponible = false;
    }

    public void MarcarDevuelto()
    {
        EstaDisponible = true;
    }
}