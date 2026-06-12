using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using Libreria1.Interfaces;
using Libreria1.Domain.Entities;
using Libreria1.Application.DTOs;

namespace Libreria1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibrosController : ControllerBase
    {
        private readonly ICatalogo<Libro> _catalogo;

        // Inyección de dependencias por constructor (DIP)
        public LibrosController(ICatalogo<Libro> catalogo)
        {
            _catalogo = catalogo;
        }

        // GET /api/libros
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<LibroDto>), StatusCodes.Status200OK)]
        public IActionResult ObtenerTodos()
        {
            
                var libros = _catalogo.ListarTodos();

                // Mapeo de Entidades de Dominio a DTOs de lectura
                var librosDto = libros.Select(l => new LibroDto
                {
                    Isbn = l.Isbn,
                    Titulo = l.Titulo,
                    Autor = l.Autor,
                    EstaDisponible = l.EstaDisponible
                }).ToList();

                return Ok(librosDto);
        
        }

        // GET /api/libros/{isbn}
        [HttpGet("{isbn}")]
        [ProducesResponseType(typeof(LibroDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ObtenerPorIsbn(string isbn)
        {

                var libro = _catalogo.BuscarLibroPorIsbn(isbn);

                if (libro == null)
                {
                    return NotFound(new { mensaje = $"No se encontró un libro con ISBN {isbn}." });
                }

                var libroDto = new LibroDto
                {
                    Isbn = libro.Isbn,
                    Titulo = libro.Titulo,
                    Autor = libro.Autor,
                    EstaDisponible = libro.EstaDisponible
                };

                return Ok(libroDto);
            

        }

        // POST /api/libros
        [HttpPost]
        [ProducesResponseType(typeof(LibroDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult CrearLibro([FromBody] CrearLibroDto dto)
        {
            //Intentamos buscar si ya existe
            try
            {
                var libroExistente = _catalogo.BuscarLibroPorIsbn(dto.ISBN);
                
                // Si la línea de arriba no lanzó excepción, significa que el libro SÍ EXISTE.
                // Como es un POST (Crear), esto es un conflicto.
                return Conflict(new { mensaje = $"El libro con ISBN {dto.ISBN} ya existe en el sistema." });
            }
            catch (LibroNoEncontradoException)
            {
                // Si lanzó la excepción, significa que NO EXISTE. 
                
                // Instancia del Dominio
                var nuevoLibro = new Libro(dto.ISBN, dto.Titulo, dto.Autor);
                _catalogo.AgregarLibro(nuevoLibro);

                // DTO de respuesta
                var libroDto = new LibroDto
                {
                    Isbn = nuevoLibro.Isbn,
                    Titulo = nuevoLibro.Titulo,
                    Autor = nuevoLibro.Autor,
                    EstaDisponible = nuevoLibro.EstaDisponible
                };

                // Retorna 201 Created con Location Header apuntando al GET por ISBN
                return CreatedAtAction(nameof(ObtenerPorIsbn), new { isbn = libroDto.Isbn }, libroDto);
            }
        }

        // DELETE /api/libros/{isbn}
        [HttpDelete("{isbn}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult EliminarLibro(string isbn)
        {

                var libro = _catalogo.BuscarLibroPorIsbn(isbn);
                if (libro == null)
                {
                    return NotFound(new { mensaje = $"No se encontró un libro con ISBN {isbn} para eliminar." });
                }

                _catalogo.EliminarLibro(isbn);
                
                // 204 No Content para eliminaciones exitosas sin cuerpo de respuesta
                return NoContent();

        }

        // Metodo privado para centralizar y mapear excepciones a respuestas HTTP concretas
        private IActionResult MapearExcepcion(Exception ex)
        {
            switch (ex)
            {
                case LibroNoEncontradoException e:
                    return NotFound(new { mensaje = e.Message }); // 404

                case LibroNoDisponibleException e:
                    return Conflict(new { mensaje = e.Message }); // 409
                default:
                    return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Ocurrió un error inesperado en el servidor." }); // 500
            }       
        }
    }
}
