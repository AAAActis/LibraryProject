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
    // Relación Prestamo → Libro (muchos préstamos pueden tener un libro)
    modelBuilder.Entity<Prestamo>()
        .HasOne(p => p.LibroPrestado)
        .WithMany()
        .HasForeignKey(p => p.LibroId);

    // Relación Prestamo → Usuario (muchos préstamos pueden tener un usuario)
    modelBuilder.Entity<Prestamo>()
        .HasOne(p => p.UsuarioAsignado)
        .WithMany()
        .HasForeignKey(p => p.UsuarioId);
    }
}