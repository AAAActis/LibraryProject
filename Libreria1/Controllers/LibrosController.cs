using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Libreria1.Domain.Entities;
using Libreria1.Application.DTOs;
using Libreria1.Application.Interfaces;
using Libreria1.Interfaces;

namespace Libreria1.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibrosController : ControllerBase
    {
        private readonly ICatalogo<Libro> _catalogo;

        public LibrosController(ICatalogo<Libro> catalogo)
        {
            _catalogo = catalogo;
        }

        [HttpGet]
        public ActionResult<List<LibroDto>> ObtenerTodos()
        {
            var libros = _catalogo.ListarTodos();
            var librosDto = libros.Select(l => new LibroDto
            {
                Isbn = l.Isbn,
                Titulo = l.Titulo,
                Autor = l.Autor,
                EstaDisponible = l.EstaDisponible
            }).ToList();

            return Ok(librosDto); // Retorna HTTP 200 con la lista
        }

        // GET /api/libros/{isbn}
        [HttpGet("{isbn}")]
        public ActionResult<LibroDto> ObtenerPorIsbn(string isbn)
        {
            try
            {
                var libro = _catalogo.BuscarLibroPorIsbn(isbn);

                // Por si el catálogo devuelve null en lugar de tirar la excepción
                if (libro == null)
                {
                    return NotFound(new { mensaje = $"No se encontró un libro con ISBN {isbn}." });
                }

                // Mapeamos la entidad encontrada al DTO
                var libroDto = new LibroDto
                {
                    Isbn = libro.Isbn,
                    Titulo = libro.Titulo,
                    Autor = libro.Autor,
                    EstaDisponible = libro.EstaDisponible
                };

                return Ok(libroDto); // Retorna HTTP 200 con el DTO
            }
            catch (LibroNoEncontradoException ex)
            {
                // Mapeo explícito de la excepción del dominio a un error HTTP 404
                return NotFound(new { mensaje = ex.Message });
            }
        }
    }
}