public class LibroNoEncontradoException : Exception
{
    public LibroNoEncontradoException() : base("El libro no fue encontrado.")
    {
    }

    public LibroNoEncontradoException(string message) : base(message)
    {
    }

    public LibroNoEncontradoException(string message, Exception innerException) : base(message, innerException)
    {
    }
}