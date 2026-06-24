using Libreria1.Interfaces;
using Libreria1.Domain.Entities;
using Npgsql;

namespace Libreria1 {


    public class RepositorioPrestamosPostgres : IRepositorio<Prestamo, Guid>
    {
    private readonly string _connectionString;

    public RepositorioPrestamosPostgres(string connectionString)
    {
        _connectionString = connectionString;
    }

    private Prestamo MapearPrestamo(NpgsqlDataReader reader)
        {

            // Mapeamos el libro
            var libro = new Libro(
                reader.GetString(reader.GetOrdinal("libro_isbn")),
                reader.GetString(reader.GetOrdinal("libro_titulo")),
                reader.GetString(reader.GetOrdinal("libro_autor"))
            );

            // Mapeamos el usuario
            var usuario = new Usuario(
                reader.GetGuid(reader.GetOrdinal("usuario_id")),
                reader.GetString(reader.GetOrdinal("usuario_nombre")),
                reader.GetInt32(reader.GetOrdinal("usuario_nro_socio")),
                reader.GetString(reader.GetOrdinal("usuario_apellido")),
                reader.GetString(reader.GetOrdinal("usuario_email")),
                reader.GetDateTime(reader.GetOrdinal("usuario_fecha_registro"))
            );

            DateTime? fechaDev = reader.IsDBNull(reader.GetOrdinal("prestamo_devolucion")) 
                ? null 
                : reader.GetDateTime(reader.GetOrdinal("prestamo_devolucion"));

            return new Prestamo(
                reader.GetGuid(reader.GetOrdinal("prestamo_id")),
                libro,
                usuario,
                reader.GetDateTime(reader.GetOrdinal("prestamo_fecha")),
                fechaDev,
                reader.GetBoolean(reader.GetOrdinal("prestamo_activo"))
            );
        }
        public IEnumerable<Prestamo> ObtenerTodos()
        {
            var prestamos = new List<Prestamo>();
            const string query = @"
                SELECT 
                p.id as prestamo_id,
                p.fecha_prestamo as prestamo_fecha,
                p.fecha_devolucion as prestamo_devolucion,
                p.esta_activo as prestamo_activo, 
                u.id as usuario_id, 
                u.nombre as usuario_nombre,
                u.nro_socio as usuario_nro_socio,
                u.apellido as usuario_apellido,
                u.email as usuario_email,
                u.fecha_registro as usuario_fecha_registro,
                l.isbn as libro_isbn,
                l.titulo as libro_titulo,
                l.autor as libro_autor
                FROM prestamos p
                JOIN usuarios u ON p.usuario_id = u.id
                JOIN libros l ON p.libro_isbn = l.isbn";

            using var connection = new NpgsqlConnection(_connectionString);
            using var command = new NpgsqlCommand(query, connection);

            connection.Open();
            using var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
            prestamos.Add(MapearPrestamo(reader));
            }
            return prestamos;
        }
        public Prestamo? ObtenerPorId(Guid id)
        {
            const string query = @"
                SELECT 
                p.id as prestamo_id,
                p.fecha_prestamo as prestamo_fecha,
                p.fecha_devolucion as prestamo_devolucion,
                p.esta_activo as prestamo_activo, 
                u.id as usuario_id, 
                u.nombre as usuario_nombre,
                u.nro_socio as usuario_nro_socio,
                u.apellido as usuario_apellido,
                u.email as usuario_email,
                u.fecha_registro as usuario_fecha_registro,
                l.isbn as libro_isbn,
                l.titulo as libro_titulo,
                l.autor as libro_autor
                FROM prestamos p
                JOIN usuarios u ON p.usuario_id = u.id
                JOIN libros l ON p.libro_isbn = l.isbn
                WHERE p.id = @id";

            using var connection = new NpgsqlConnection(_connectionString);
            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            connection.Open();
            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return MapearPrestamo(reader);
            }

            return null;
        }
        public void Agregar(Prestamo prestamo)
        {

            const string query = @"
        INSERT INTO prestamos (id, libro_isbn, usuario_id, fecha_prestamo, fecha_devolucion, esta_activo)
        VALUES (@id, @libro_isbn, @usuario_id, @fecha_prestamo, @fecha_devolucion, @esta_activo)";

    using var connection = new NpgsqlConnection(_connectionString);
    using var command = new NpgsqlCommand(query, connection);

    command.Parameters.AddWithValue("@id", prestamo.Id);
    command.Parameters.AddWithValue("@libro_isbn", prestamo.LibroPrestado.Isbn);
    command.Parameters.AddWithValue("@usuario_id", prestamo.UsuarioAsignado.Id);
    command.Parameters.AddWithValue("@fecha_prestamo", prestamo.FechaPrestamo);

    command.Parameters.AddWithValue("@fecha_devolucion", prestamo.FechaDevolucion ?? (object)DBNull.Value);
    command.Parameters.AddWithValue("@esta_activo", prestamo.Activo);

    connection.Open();
    command.ExecuteNonQuery();
        }

        public void Actualizar(Prestamo prestamo)
{
    const string query = @"
        UPDATE prestamos 
        SET fecha_devolucion = @fecha_devolucion, 
            esta_activo = @esta_activo 
        WHERE id = @id";

    using var connection = new NpgsqlConnection(_connectionString);
    using var command = new NpgsqlCommand(query, connection);

    command.Parameters.AddWithValue("@id", prestamo.Id);
    command.Parameters.AddWithValue("@fecha_devolucion", prestamo.FechaDevolucion ?? (object)DBNull.Value);
    command.Parameters.AddWithValue("@esta_activo", prestamo.Activo);

    connection.Open();
    command.ExecuteNonQuery();
}

public IEnumerable<Prestamo> ObtenerPorUsuario(Guid usuarioId)
{
    var prestamos = new List<Prestamo>();

    const string query = @"
        SELECT 
        p.id as prestamo_id, p.fecha_prestamo as prestamo_fecha, p.fecha_devolucion as prestamo_devolucion, p.esta_activo as prestamo_activo, 
        u.id as usuario_id, u.nombre as usuario_nombre, u.nro_socio as usuario_nro_socio, u.apellido as usuario_apellido, u.email as usuario_email, u.fecha_registro as usuario_fecha_registro,
        l.isbn as libro_isbn, l.titulo as libro_titulo, l.autor as libro_autor
        FROM prestamos p
        JOIN usuarios u ON p.usuario_id = u.id
        JOIN libros l ON p.libro_isbn = l.isbn
        WHERE p.usuario_id = @usuario_id";

    using var connection = new NpgsqlConnection(_connectionString);
    using var command = new NpgsqlCommand(query, connection);
    
    command.Parameters.AddWithValue("@usuario_id", usuarioId);

    connection.Open();
    using var reader = command.ExecuteReader();

    // Como puede haber historial, iteramos
    while (reader.Read())
    {
        prestamos.Add(MapearPrestamo(reader));
    }

    return prestamos;
}

        public void Eliminar(Guid id)
{
    const string query = "DELETE FROM prestamos WHERE id = @id";

    using var connection = new NpgsqlConnection(_connectionString);
    using var command = new NpgsqlCommand(query, connection);

    command.Parameters.AddWithValue("@id", id);

    connection.Open();
    command.ExecuteNonQuery();
}

        public IEnumerable<Libro> ObtenerLibrosMasPrestados()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Usuario> UsuariosConPrestamosActivos()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Multa> ObtenerMultasAgrupadasPorUsuario()
        {
            throw new NotImplementedException();
        }

        IEnumerable<Multa> IRepositorio<Prestamo, Guid>.ObtenerMultasAgrupadasPorUsuario()
        {
            return ObtenerMultasAgrupadasPorUsuario();
        }

        IEnumerable<Usuario> IRepositorio<Prestamo, Guid>.UsuariosConPrestamosActivos()
        {
            return UsuariosConPrestamosActivos();
        }
    }
}