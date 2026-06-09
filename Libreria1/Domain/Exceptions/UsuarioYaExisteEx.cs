namespace Libreria1.Domain.Exceptions
{
    public class UsuarioYaExisteException : Exception
    {
        public UsuarioYaExisteException(string mensaje) : base(mensaje) { }
    }
}