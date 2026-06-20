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
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Esto es buena practica dejenlo
            base.OnModelCreating(modelBuilder);

            // Le decimos: "Un Préstamo tiene un LibroPrestado, un Libro tiene muchos Préstamos, y la columna real es LibroId"
            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.LibroPrestado)
                .WithMany(l => l.Prestamos)
                .HasForeignKey(p => p.LibroId);

            // Hacemos lo mismo para el Usuario
            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.UsuarioAsignado)
                .WithMany(u => u.Prestamos) 
                .HasForeignKey(p => p.UsuarioId);
        }
}