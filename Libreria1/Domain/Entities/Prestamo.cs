public class Prestamo : IEntidad
{
    public Guid id {get; private set;} = Guid.NewGuid();
    public Libro LibroPrestado {get; private set;}
    public Usuario UsuarioAsignado {get; private set;}
    public DateTime FechaPrestamo {get; private set;}
    public DateTime FechaDevolucion {get; private set;}
    public bool Activo {get; private set;}


    public string titulo => LibroPrestado.Titulo;
    public string autor => LibroPrestado.Autor;
    public bool estaDisponible => LibroPrestado.EstaDisponible;

    public Prestamo(Libro libro, Usuario usuario)
    {
        id = Guid.NewGuid();
        LibroPrestado = libro;
        UsuarioAsignado = usuario;
        FechaPrestamo = DateTime.Now;
        FechaDevolucion = FechaPrestamo.AddDays(14); // Plazo de 14 días para la devolución
        Activo = true;
        LibroPrestado.MarcarPrestado();
    }

    public void CambiarEstado()
    {
        if (Activo)
        {
            Activo = false;
            LibroPrestado.MarcarDevuelto();
        }
        else
        {
            Activo = true;
            LibroPrestado.MarcarPrestado();
        }
    }
}