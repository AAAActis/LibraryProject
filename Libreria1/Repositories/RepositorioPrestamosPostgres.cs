using Libreria1.Interfaces;
using Libreria1.Domain.Entities;
using Npgsql;

namespace Libreria1 {


    public class RepositorioPrestamosPostgres : IRepositorio<Prestamo>
    {
    private readonly string _connectionString;

    public RepositorioPrestamosPostgres(string connectionString)
    {
        _connectionString = connectionString;
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
                // Mapeamos los datos de las columnas de la BD al objeto del Dominio
                var libro = new Libro(
                    reader.GetString(reader.GetOrdinal("libro_isbn")),
                    reader.GetString(reader.GetOrdinal("libro_titulo")),
                    reader.GetString(reader.GetOrdinal("libro_autor"))
                );
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

                var prestamo = new Prestamo(
                    reader.GetGuid(reader.GetOrdinal("prestamo_id")),
                    libro,
                    usuario,
                    reader.GetDateTime(reader.GetOrdinal("prestamo_fecha")),
                    fechaDev, 
                    reader.GetBoolean(reader.GetOrdinal("prestamo_activo"))

                );
                prestamos.Add(prestamo);
            }

            return prestamos;
        }
        public Prestamo? ObtenerPorId(Guid id)
        {
            throw new NotImplementedException();
        }
        public void Agregar(Prestamo entidad)
        {

            throw new NotImplementedException();
        }

        public void Eliminar(Guid id)
        {
            throw new NotImplementedException();
        }

        

        
    }
}