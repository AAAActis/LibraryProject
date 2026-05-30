//Responsabilidad unica: Representar un usuario en el sistema de biblioteca.
public class Usuario : IEntidad<Guid>
{
    private static int _contador = 1;
    public Guid Id { get; private set; } = Guid.NewGuid();
    public int NroSocio {get; private set;}
    public string Nombre {get; private set;}
    public string Email {get; private set;}
    public DateTime FechaRegistro {get; private set;}


    public Usuario(string nombre, string correoElectronico)
    {
        Id = Guid.NewGuid();
        NroSocio = _contador++;
        Nombre = nombre;
        Email = correoElectronico;
        FechaRegistro = DateTime.Now;
    }
}