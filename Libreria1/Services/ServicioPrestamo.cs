using System.Data.Common;

public class ServicioPrestamo
{
        private ICatalogo<Libro> catalogo;
        private List<Prestamo> prestamos;

    public ServicioPrestamo(ICatalogo<Libro> catalogo)
    {
        this.catalogo = catalogo;
        prestamos = new List<Prestamo>();
    }

    public Prestamo PrestarLibro(Guid idUsuario, Guid isbnLibro)
    {
        var libro = catalogo.BuscarLibro(isbnLibro);
        if (libro == null)
        {
            throw new Exception("Libro no encontrado.");
        }

        if(!libro.estaDisponible)
        {
            throw new Exception("El libro fue prestado.");
        }

        var nuevoPrestamo = new Prestamo
        {
            IdUuid = idUsuario,
            Libro = isbnLibro.ToString(),
            FechaPrestamo = DateTime.Now,
            FechaDevolucion  = DateTime.Now.AddDays(14), // Se pone los dias en los que debera devolverlo
            activo = true
        };

        libro.MarcarPrestado();
        prestamos.Add(nuevoPrestamo);
        return nuevoPrestamo;
    }

    public void DevolverLibro(Guid userid, string isbnLibro)
    {
        var prestamo = prestamos.FirstOrDefault(p => p.IdUuid == userid && p.Libro == isbnLibro && p.activo);
        if (prestamo == null)
        {
            throw new Exception("Préstamo no encontrado.");
        }

        prestamo.activo = false;
        
        var libro = catalogo.BuscarLibro(Guid.Parse(isbnLibro));
        if (libro != null)
        {
            libro.MarcarDevuelto();
        }
    }
}