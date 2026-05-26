public class Prestamo : IEntidad
{
    private Guid _id {get;}
    private Libro _libroPrestado {get;}
    private Usuario _usuarioAsignado {get;}
    private DateTime _fechaPrestamo {get;}
    private DateTime _fechaDevolucion {get;}
    private bool _activo {get; set;}

    public Guid id => _id;
    public Libro libroPrestado => _libroPrestado;
    public Usuario usuarioAsignado => _usuarioAsignado;
    public DateTime fechaPrestamo => _fechaPrestamo;
    public DateTime fechaDevolucion => _fechaDevolucion;
    
    public bool Activo => _activo;    
    public string titulo => _libroPrestado.titulo;
    public string autor => _libroPrestado.autor;
    public bool estaDisponible => _libroPrestado.estaDisponible;

    public Prestamo(Libro libro, Usuario usuario)
    {
        _id = Guid.NewGuid();
        _libroPrestado = libro;
        _usuarioAsignado = usuario;
        _fechaPrestamo = DateTime.Now;
        _fechaDevolucion = _fechaPrestamo.AddDays(14); // Plazo de 14 días para la devolución
        _activo = true;
        _libroPrestado.MarcarPrestado();
    }

    public void CambiarEstado()
    {
        if (_activo)
        {
            _activo = false;
            _libroPrestado.MarcarDevuelto();
        }
        else
        {
            _activo = true;
            _libroPrestado.MarcarPrestado();
        }
    }
}