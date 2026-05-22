public class LibroNoDisponibleException : Exception
{
    public LibroNoDisponibleException() : base("El libro no está disponible para préstamo.")
    {
    }

    public LibroNoDisponibleException(string message) : base(message)
    {
    }

    public LibroNoDisponibleException(string message, Exception innerException) : base(message, innerException)
    {
    }
}