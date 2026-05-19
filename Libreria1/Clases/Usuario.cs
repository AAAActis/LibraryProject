public class Usuario
{
    private Guid Id { get; set; }
    private string Nombre { get; set; }
    private string email { get; set; }
    private DateTime fechaRegistro { get; set; }

    public Usuario(string nombre, string correoElectronico)
    {
        Id = Guid.NewGuid();
        Nombre = nombre;
        email = correoElectronico;
        fechaRegistro = DateTime.Now;
    }
}