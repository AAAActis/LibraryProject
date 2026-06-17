using Libreria1.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class LibreriaContext : DbContext
{
    // El constructor recibe las opciones (cadena de conexión, logging) desde Program.cs
    public LibreriaContext(DbContextOptions<LibreriaContext> options) : base(options)
    {
    }

    // Estas propiedades representan tus tablas en PostgreSQL
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }
        public DbSet<Multa> Multas { get; set; }
    
}