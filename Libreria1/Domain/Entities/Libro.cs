//Responsabilidad unica: Representar un libro en el sistema de biblioteca.
public class Libro : IEntidad
{
    private Guid _id {get;}
    private string _isbn {get;}
    private string _titulo {get;}
    private string _autor {get;}
    private bool _estaDisponible {get; set;} 

    public Guid id => _id;
    public string isbn => _isbn;
    public string titulo => _titulo;
    public string autor => _autor;
    public bool estaDisponible => _estaDisponible;

    public Libro(string isbn, string titulo, string autor)
    {
        _id = Guid.NewGuid();
        _isbn = isbn;
        _titulo = titulo;
        _autor = autor;
        _estaDisponible = true;
    }

    public void MarcarPrestado()
    {
        _estaDisponible = false;
    }

    public void MarcarDevuelto()
    {
        _estaDisponible = true;
    }
}