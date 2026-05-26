public class Prestamo : IEntidad
{
    private Guid _id;
    private Libro _libroPrestado;
    private Usuario _usuarioAsignado;
    private DateTime _fechaPrestamo;
    private DateTime _fechaDevolucion;
    private bool _activo;

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