//Responsabilidad unica: Representar un usuario en el sistema de biblioteca.
using Libreria1.Application.Interfaces;
using System.ComponentModel.DataAnnotations;
namespace Libreria1.Domain.Entities;
public class Usuario : IEntidad<Guid>
{
    private static int _contador = 1;
    [Key]
    public Guid Id { get; private set; } = Guid.NewGuid();
    public int NroSocio {get; private set;}
    public string Nombre {get; private set;}
    public string Apellido {get; private set;}
    public string Email {get; private set;}
    public DateTime FechaRegistro {get; private set;}

    protected Usuario() { } // Constructor protegido para EF Core
    
    public Usuario(string nombre, string apellido, string email)
    {
    Id = Guid.NewGuid(); // Se genera un ID nuevo
    Nombre = nombre;
    Apellido = apellido;
    Email = email;
    FechaRegistro = DateTime.Now; // Fecha actual
    
    // Acá le asignás el número de socio según la lógica que uses 
    // (puede ser un Random, autoincremental en la BD, o empezar en 0)
    NroSocio = 0; 
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
}