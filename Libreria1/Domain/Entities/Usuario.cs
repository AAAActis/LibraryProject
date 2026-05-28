//Responsabilidad unica: Representar un usuario en el sistema de biblioteca.
public class Usuario : IEntidad
{
    public Guid id {get; private set;} = Guid.NewGuid();
    public string Nombre {get; private set;}
    public string Email {get; private set;}
    public DateTime FechaRegistro {get; private set;}


    public Usuario(string nombre, string correoElectronico)
    {
        id = Guid.NewGuid();
        Nombre = nombre;
        Email = correoElectronico;
        FechaRegistro = DateTime.Now;
    }
}