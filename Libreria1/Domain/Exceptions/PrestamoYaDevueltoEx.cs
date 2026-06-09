public class PrestamoYaDevueltoEx : Exception
{
    public PrestamoYaDevueltoEx(int idPrestamo) : base($"Prestamo con id {idPrestamo} ya fue devuelto.")
    {
        
    }
}