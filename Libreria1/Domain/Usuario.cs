public class Usuario : IEntidad
{
    private Guid _id;
    private string _nombre;
    private string _email;
    private DateTime _fechaRegistro;

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