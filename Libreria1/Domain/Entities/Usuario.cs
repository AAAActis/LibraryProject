//Responsabilidad unica: Representar un usuario en el sistema de biblioteca.
using Libreria1.Application.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BCrypt.Net;

namespace Libreria1.Domain.Entities;
public class Usuario : IEntidad<Guid>
{
    public int NroSocio {get; private set;}

    public Guid Id {get; private set;}
    public string Nombre {get; private set;}
    public string Apellido {get; private set;}
    public string Email {get; private set;}
    public string PasswordHash {get; private set;} 
    public DateTime FechaRegistro {get; private set;}
    public enum Rol { Administrador, Socio }

    public Rol TipoRol { get; private set; }

    public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();

    protected Usuario() { } // Constructor protegido para EF Core
    
    public Usuario(string nombre, string apellido, string email, string password)
    {
    Id = Guid.NewGuid(); // Se genera un ID nuevo
    Nombre = nombre;
    Apellido = apellido;
    Email = email;
    PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
    FechaRegistro = DateTime.UtcNow; // Fecha actual
    }   

    internal Usuario(Guid id, string nombre, int nroSocio, string apellido, string email, DateTime fechaRegistro)
    {
        Id = id;
        Nombre = nombre;
        NroSocio = nroSocio;
        Apellido = apellido;
        Email = email;
        FechaRegistro = fechaRegistro;
    }

    public bool VerificarPassword(string password)
    => BCrypt.Net.BCrypt.Verify(password, PasswordHash);
}