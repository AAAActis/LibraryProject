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
            modelBuilder.Entity<Prestamo>(entity =>
{
    entity.HasKey(p => p.Id);



    entity.Ignore(p => p.titulo);
    entity.Ignore(p => p.autor);
    entity.Ignore(p => p.estaDisponible);

    // 3. Relación con Libro
    entity.HasOne(p => p.LibroPrestado)
          .WithMany(l => l.Prestamos)
          .HasForeignKey(p => p.LibroId);

    // 4. Relación con Usuario
    entity.HasOne(p => p.UsuarioAsignado)
          .WithMany(u => u.Prestamos) 
          .HasForeignKey(p => p.UsuarioId);
});

            modelBuilder.Entity<Multa>(entity =>
            {
            entity.HasKey(m => m.Id);
            entity.HasOne(m => m.PrestamoAsignado)
            .WithMany() 
            .HasForeignKey(m => m.PrestamoId);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
            entity.HasKey(u => u.Id); // Reemplaza al [Key]
            entity.Property(u => u.NroSocio)
            .ValueGeneratedOnAdd();
            entity.Property(u => u.TipoRol)
            .HasConversion<string>(); // Fundamental para la Issue #3
            });
            
            modelBuilder.Entity<Libro>(entity =>
            {
            entity.HasKey(l => l.Isbn); // Define la PK
            entity.Ignore(l => l.Id);   // Reemplaza al [NotMapped]
            });
        }
}