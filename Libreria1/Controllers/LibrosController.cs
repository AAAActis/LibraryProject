using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using Libreria1.Interfaces;
using Libreria1.Domain.Entities;
using Libreria1.Application.DTOs;
using Libreria1.Repositories;

namespace Libreria1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibrosController : ControllerBase
    {
        private readonly ICatalogo<Libro> _catalogo;
        private readonly IRepositorio<Libro, string> _repositorioLibros;

        // Solo inyectamos la interfaz (DIP)
        public LibrosController(ICatalogo<Libro> catalogo, IRepositorio<Libro, string> repositorioLibros)
        {
            _catalogo = catalogo;
            _repositorioLibros = repositorioLibros;
        }

        [HttpGet("reportes/mas-prestados")]
        public IActionResult ObtenerLibrosMasPrestados()
        {
            try
            {
                // Ahora usamos el repositorio inyectado correctamente
                var libros = _repositorioLibros.ObtenerLibrosMasPrestados();
                return Ok(libros);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener los libros: {ex.Message}");
            }
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

                Libro? libroExistente = null;
                try
                {
                    // Intentamos buscarlo. Si el servicio tira la excepción, pasamos al catch.
                    libroExistente = _catalogo.BuscarLibroPorIsbn(dto.ISBN);
                }
                catch (LibroNoEncontradoException)
                {
                }

                if (libroExistente != null)
                {
                    return Conflict(new { mensaje = $"El libro con ISBN {dto.ISBN} ya existe en el sistema." });
                }
                // Instancia del Dominio
                var nuevoLibro = new Libro(dto.ISBN, dto.Titulo, dto.Autor, int.Parse(dto.AñoPublicacion), dto.CantPaginas);
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
