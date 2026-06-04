public class PrestamoNoVencidoEx : Exception
{
    public PrestamoNoVencidoEx() : base("El préstamo no está vencido, no se puede generar una multa.")
    {
    }
}