public class PrestamoNoEncontradoException : Exception
{
    public PrestamoNoEncontradoException() : base("El préstamo no fue encontrado.")
    {
    }

    public PrestamoNoEncontradoException(string message) : base(message)
    {
    }

    public PrestamoNoEncontradoException(string message, Exception innerException) : base(message, innerException)
    {
    }
}