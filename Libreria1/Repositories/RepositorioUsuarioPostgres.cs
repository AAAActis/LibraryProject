using Npgsql;
using Libreria1.Domain.Entities;
using Libreria1.Interfaces;
 
namespace Libreria1.Repositories
{
    public class RepositorioUsuariosPostgres : IRepositorio<Usuario, Guid>
    {
        private readonly string _connectionString;
 
        public RepositorioUsuariosPostgres(string connectionString)
        {
            _connectionString = connectionString;
        }
 
        // SELECT * FROM usuarios ORDER BY nro_socio
        public IEnumerable<Usuario> ObtenerTodos()
        {
            var usuarios = new List<Usuario>();
            const string query = @"
                SELECT id, nro_socio, nombre, apellido, email, fecha_registro
                FROM usuarios
                ORDER BY nro_socio";
 
            using var connection = new NpgsqlConnection(_connectionString);
            using var command = new NpgsqlCommand(query, connection);
 
            connection.Open();
            using var reader = command.ExecuteReader();
 
            while (reader.Read())
                usuarios.Add(MapearUsuario(reader));
 
            return usuarios;
        }
 
        // SELECT * FROM usuarios WHERE id = @id
        public Usuario? ObtenerPorId(Guid id)
        {
            const string query = @"
                SELECT id, nro_socio, nombre, apellido, email, fecha_registro
                FROM usuarios
                WHERE id = @id";
 
            using var connection = new NpgsqlConnection(_connectionString);
            using var command = new NpgsqlCommand(query, connection);
 
            command.Parameters.AddWithValue("@id", id);
 
            connection.Open();
            using var reader = command.ExecuteReader();
 
            return reader.Read() ? MapearUsuario(reader) : null;
        }
 
        // INSERT INTO usuarios (...)
        public void Agregar(Usuario usuario)
        {
            const string query = @"
                INSERT INTO usuarios (id, nro_socio, nombre, apellido, email, fecha_registro)
                VALUES (@id, @nro_socio, @nombre, @apellido, @email, @fecha_registro)";
 
            using var connection = new NpgsqlConnection(_connectionString);
            using var command = new NpgsqlCommand(query, connection);
 
            command.Parameters.AddWithValue("@id",             usuario.Id);
            command.Parameters.AddWithValue("@nro_socio",      usuario.NroSocio);
            command.Parameters.AddWithValue("@nombre",         usuario.Nombre);
            command.Parameters.AddWithValue("@apellido",       usuario.Apellido);
            command.Parameters.AddWithValue("@email",          usuario.Email);
            command.Parameters.AddWithValue("@fecha_registro", usuario.FechaRegistro);
 
            connection.Open();
            command.ExecuteNonQuery();
        }
 
        // SELECT * FROM usuarios WHERE email = @email (para validar duplicados)
        public Usuario? BuscarPorEmail(string email)
        {
            const string query = @"
                SELECT id, nro_socio, nombre, apellido, email, fecha_registro
                FROM usuarios
                WHERE email = @email";
 
            using var connection = new NpgsqlConnection(_connectionString);
            using var command = new NpgsqlCommand(query, connection);
 
            command.Parameters.AddWithValue("@email", email);
 
            connection.Open();
            using var reader = command.ExecuteReader();
 
            return reader.Read() ? MapearUsuario(reader) : null;
        }
 
        // UPDATE usuarios SET ... WHERE id = @id
        public void Actualizar(Usuario usuario)
        {
            const string query = @"
                UPDATE usuarios
                SET nombre = @nombre, apellido = @apellido, email = @email
                WHERE id = @id";

            using var connection = new NpgsqlConnection(_connectionString);
            using var command = new NpgsqlCommand(query, connection);

            command.Parameters.AddWithValue("@id", usuario.Id);
            command.Parameters.AddWithValue("@nombre", usuario.Nombre);
            command.Parameters.AddWithValue("@apellido", usuario.Apellido);
            command.Parameters.AddWithValue("@email", usuario.Email);

            connection.Open();
            command.ExecuteNonQuery();
        }

        // DELETE FROM usuarios WHERE id = @id
        public void Eliminar(Guid id)
        {
            const string query = "DELETE FROM usuarios WHERE id = @id";
 
            using var connection = new NpgsqlConnection(_connectionString);
            using var command = new NpgsqlCommand(query, connection);
 
            command.Parameters.AddWithValue("@id", id);
 
            connection.Open();
            command.ExecuteNonQuery();
        }
 
        // Mapeo centralizado — orden de parámetros igual al constructor internal de Usuario
        private static Usuario MapearUsuario(NpgsqlDataReader reader)
        {
            return new Usuario(
                reader.GetGuid    (reader.GetOrdinal("id")),
                reader.GetString  (reader.GetOrdinal("nombre")),
                reader.GetInt32   (reader.GetOrdinal("nro_socio")),
                reader.GetString  (reader.GetOrdinal("apellido")),
                reader.GetString  (reader.GetOrdinal("email")),
                reader.GetDateTime(reader.GetOrdinal("fecha_registro"))
            );
        }

        public IEnumerable<Libro> ObtenerLibrosMasPrestados()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Usuario> UsuariosConPrestamosActivos()
        {
            throw new NotImplementedException();
        }

        public object ObtenerMultasAgrupadasPorUsuario()
        {
            throw new NotImplementedException();
        }
    }
}