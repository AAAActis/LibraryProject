public class UsuarioNoEncontradoException : Exception
{
    public UsuarioNoEncontradoException(Guid idUsuario) : base($"Usuario con ID '{{idUsuario}}' no encontrado.")
    {
    }

    public UsuarioNoEncontradoException(string message) : base(message)
    {
    }

    public UsuarioNoEncontradoException(string message, Exception innerException) : base(message, innerException)
    {
    }
}