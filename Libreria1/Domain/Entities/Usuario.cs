//Responsabilidad unica: Representar un usuario en el sistema de biblioteca.
using Libreria1.Application.Interfaces;
namespace Libreria1.Domain.Entities;
public class Usuario : IEntidad<Guid>
{
    private static int _contador = 1;
    public Guid Id { get; private set; } = Guid.NewGuid();
    public int NroSocio {get; private set;}
    public string Nombre {get; private set;}
    public string Apellido {get; private set;}
    public string Email {get; private set;}
    public DateTime FechaRegistro {get; private set;}


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