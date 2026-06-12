using System;
using System.Collections.Generic;
using Npgsql;
using Libreria1.Domain.Entities;
using Libreria1.Application.Interfaces;
using Libreria1.Interfaces;

namespace Libreria1.Repositories
{
    public class RepositorioLibrosPostgres : IRepositorio<Libro>
    {
        private readonly string _connectionString;

        public RepositorioLibrosPostgres(string connectionString)
        {
            _connectionString = connectionString;
        }

        //Get all libros: Select * from libros
        public IEnumerable<Libro> ObtenerTodos()
        {
            var libros = new List<Libro>();
            // ERROR 500 CORREGIDO: Se usa esta_disponible y se suman las columnas nuevas
            const string query = "SELECT isbn, titulo, autor, año_publicacion, cant_paginas, esta_disponible FROM libros";
            
            using var connection = new NpgsqlConnection(_connectionString);
            using var command = new NpgsqlCommand(query, connection);

            connection.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var libro = new Libro(
                    reader.GetString(reader.GetOrdinal("isbn")),
                    reader.GetString(reader.GetOrdinal("titulo")),
                    reader.GetString(reader.GetOrdinal("autor")),
                    // Manejo de nulos por si tenés libros viejos sin año/páginas
                    reader.IsDBNull(reader.GetOrdinal("año_publicacion")) ? 0 : reader.GetInt32(reader.GetOrdinal("año_publicacion")),
                    reader.IsDBNull(reader.GetOrdinal("cant_paginas")) ? 0 : reader.GetInt32(reader.GetOrdinal("cant_paginas"))
                )
                {
                    EstaDisponible = reader.GetBoolean(reader.GetOrdinal("esta_disponible"))
                };
                libros.Add(libro);
            }

            return libros;
        }
        
        //Get by id: Select * from libros where isbn = @isbn
        public Libro? ObtenerPorId(Guid id)
        {
            /*
            const string query = "SELECT isbn, titulo, autor, esta_disponible FROM libros WHERE isbn = @isbn";

            using var connection = new NpgsqlConnection(_connectionString);
            using var command = new NpgsqlCommand(query, connection);
            
            // Siempre usar parametros, nunca concatenar strings
            command.Parameters.AddWithValue("@isbn", id);

            connection.Open();
            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new Libro(
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2)
                )
                {
                    EstaDisponible = reader.GetBoolean(3)
                };
            }

            return null; // Si no lo encuentra, devuelve null
            */
            return null; // Por ahora lo dejamos así porque el ISBN no es un Guid, habría que cambiar la firma del método en la interfaz
        }


        // INSERT: INSERT INTO libros VALUES (@isbn, @titulo, ...)
        public void Agregar(Libro libro)
        {
            const string query = @"
                INSERT INTO libros (isbn, titulo, autor, año_publicacion, cant_paginas, esta_disponible) 
                VALUES (@isbn, @titulo, @autor, @ano, @pags, @esta_disponible)";

            using var connection = new NpgsqlConnection(_connectionString);
            using var command = new NpgsqlCommand(query, connection);

            command.Parameters.AddWithValue("@isbn", libro.Isbn);
            command.Parameters.AddWithValue("@titulo", libro.Titulo);
            command.Parameters.AddWithValue("@autor", libro.Autor);
            command.Parameters.AddWithValue("@ano", libro.AñoPublicacion);
            command.Parameters.AddWithValue("@pags", libro.CantPaginas);
            command.Parameters.AddWithValue("@esta_disponible", libro.EstaDisponible);

            connection.Open();
            command.ExecuteNonQuery();
        }

        // DELETE: DELETE FROM libros WHERE isbn = @isbn
        public void Eliminar(Guid id)   
        {
            const string query = "DELETE FROM libros WHERE isbn = @isbn";
            using var connection = new NpgsqlConnection(_connectionString);
            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.AddWithValue("@isbn", id);
            connection.Open();
            command.ExecuteNonQuery();
        }

    }

}