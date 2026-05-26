public class Libro : IEntidad
{
    private Guid _id;
    private string _isbn;
    private string _titulo;
    private string _autor;
    private bool _estaDisponible;

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