using System;
using System.Collections.Generic;
using Npgsql;
using Libreria1.Domain.Entities;
using Libreria1.Application.Interfaces;
using Libreria1.Interfaces;

namespace RepositorioLibrosPostgres
{
    public class RepositorioLibrosPostgres : IRepositorio<Libro>
    {
        private readonly string _connectionString;

        public RepositorioLibrosPostgres(string connectionString)
        {
            _connectionString = connectionString;
        }

        //Get all libros: Select * from libros
        IEnumerable<Libro> IRepositorio<Libro>.ObtenerTodos()
        {
            var libros = new List<Libro>();
            const string query = "SELECT isbn, titulo, autor, estaDisponible FROM libros";
            using var connection = new NpgsqlConnection(_connectionString);
            using var command = new NpgsqlCommand(query, connection);

            connection.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                // Mapeamos los datos de las columnas de la BD al objeto del Dominio
                var libro = new Libro(
                    reader.GetString(0), // isbn
                    reader.GetString(1), // titulo
                    reader.GetString(2)  // autor
                )
                {
                    EstaDisponible = reader.GetBoolean(3)
                };
                libros.Add(libro);
            }

            return libros;
        }
        
        //Get by id: Select * from libros where isbn = @isbn
        public Libro? ObtenerPorId(Guid id)
        {
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
        }


        // INSERT: INSERT INTO libros VALUES (@isbn, @titulo, ...)
        public void Agregar(Libro libro)
        {
            const string query = @"
                INSERT INTO libros (isbn, titulo, autor, esta_disponible) 
                VALUES (@isbn, @titulo, @autor, @esta_disponible)";

            using var connection = new NpgsqlConnection(_connectionString);
            using var command = new NpgsqlCommand(query, connection);

            command.Parameters.AddWithValue("@isbn", libro.Isbn);
            command.Parameters.AddWithValue("@titulo", libro.Titulo);
            command.Parameters.AddWithValue("@autor", libro.Autor);
            command.Parameters.AddWithValue("@esta_disponible", libro.EstaDisponible);

            connection.Open();
            command.ExecuteNonQuery(); // Se usa NonQuery porque no esperamos filas de retorno
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