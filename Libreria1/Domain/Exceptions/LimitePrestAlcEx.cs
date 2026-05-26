public class LimitePrestamosAlcanzadoException : Exception
{
    public LimitePrestamosAlcanzadoException()
        : base("El usuario ha alcanzado el límite de préstamos permitidos.")
    {
    }
}