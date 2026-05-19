public class Prestamo
{
    private Libro libro1;
    private Guid Id { get; set; }
    private Libro LibroPrestado { get; set; }
    private string NombreUsuario { get; set; }
    private DateTime FechaPrestamo { get; set; }
    private DateTime FechaDevolucion { get; set; }
    private bool Activo { get; set; }

    public Prestamo(Libro libro, string nombreUsuario)
    {
        Id = Guid.NewGuid();
        LibroPrestado = libro;
        NombreUsuario = nombreUsuario;
        FechaPrestamo = DateTime.Now;
        FechaDevolucion = FechaPrestamo.AddDays(14); // Plazo de 14 días para la devolución
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