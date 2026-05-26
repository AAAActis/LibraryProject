//Responsabilidad unica: Representar un usuario en el sistema de biblioteca.
public class Usuario : IEntidad
{
    private Guid _id {get;}
    private string _nombre {get;}
    private string _email {get;}
    private DateTime _fechaRegistro {get;}

    public Guid id => _id;
    public string nombre => _nombre;
    public string email => _email;
    public DateTime fechaRegistro => _fechaRegistro;

    public Usuario(string nombre, string correoElectronico)
    {
        _id = Guid.NewGuid();
        _nombre = nombre;
        _email = correoElectronico;
        _fechaRegistro = DateTime.Now;
    }
}