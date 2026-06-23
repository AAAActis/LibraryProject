
using System.Collections.Generic;
using System.Linq;
using Libreria1.Application.DTOs;
using Libreria1.Application.Interfaces;
using Libreria1.Domain.Entities;
using Libreria1.Interfaces;
using Libreria1.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Libreria1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrestamosController : ControllerBase
    {
        private readonly ServicioPrestamo _servicioPrestamo;
        private readonly IServicioMulta _servicioMulta;
        private readonly IRepositorio<Multa, Guid> _repositorioMultas; // Para el endpoint de multas agrupadas

        public PrestamosController(ServicioPrestamo servicioPrestamo, IServicioMulta servicioMulta, IRepositorio<Multa, Guid> repositorioMultas)
        {
            _servicioPrestamo = servicioPrestamo;
            _servicioMulta = servicioMulta;
            _repositorioMultas = repositorioMultas;
        }

        [HttpGet("reportes/agrupadas-por-usuario")]
        public IActionResult ObtenerMultasAgrupadas()
        {
            try
            {
                // Llama a tu método con el GroupBy en RepositorioMultasEF
                var multasAgrupadas = _repositorioMultas.ObtenerMultasAgrupadasPorUsuario();
                return Ok(multasAgrupadas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener las multas: {ex.Message}");
            }
        }

        // GET: api/prestamos/usuario/{nroSocio}
        [HttpGet("usuario/{nroSocio}")]
        [ProducesResponseType(typeof(List<PrestamoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<PrestamoDto>> ListarPorSocio(int nroSocio)
        {
            // buscar los prestamos del socio
            var prestamos = _servicioPrestamo.ListarPrestamosPorUsuario(nroSocio);

            if (!prestamos.Any())
            {
                return NotFound(new { mensaje = $"No se encontraron préstamos para el socio {nroSocio}." });
            }

            var prestamosDto = prestamos.Select(p => new PrestamoDto
            {
                LibroIsbn = p.LibroPrestado.Isbn,
                NroSocio = p.UsuarioAsignado.NroSocio,
                FechaPrestamo = p.FechaPrestamo,
                FechaDevolucion = p.FechaDevolucion.GetValueOrDefault(), // Si es null, devuelve DateTime.MinValue
                EstaActivo = p.Activo
            }).ToList();

            return Ok(prestamosDto); // http 200 con la lista limpia
        }

        // POST: api/prestamos
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(PrestamoDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult Crear([FromBody] PrestamoDto dto)
        {

                _servicioPrestamo.PrestarLibro(dto.NroSocio, dto.LibroIsbn); 

                // devolvemos un 201 Created y redirigimos a la ruta del socio
                return CreatedAtAction(nameof(ListarPorSocio), new { nroSocio = dto.NroSocio }, new { mensaje = "Préstamo registrado exitosamente." }); 

        }

        [HttpPut("devolver")]
        [ProducesResponseType(typeof(PrestamoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Devolver([FromBody] CrearPrestamoDto dto)
        {

                var prestamoActualizado = _servicioPrestamo.DevolverLibro(dto.UsuarioId, dto.LibroIsbn);

                var prestamoDto = new PrestamoDto
                {
                    LibroIsbn = prestamoActualizado.LibroPrestado.Isbn,
                    NroSocio = prestamoActualizado.UsuarioAsignado.NroSocio,
                    FechaPrestamo = prestamoActualizado.FechaPrestamo,
                    FechaDevolucion = prestamoActualizado.FechaDevolucion.GetValueOrDefault(), // Si es null, devuelve DateTime.MinValue
                    EstaActivo = prestamoActualizado.Activo
                }; 

                // 3. Revisamos si el servicio generó una multa para este préstamo
        var multa = _servicioMulta.ObtenerMultaPorPrestamo(prestamoActualizado.Id); 

        if (multa != null)
        {
            var multaDto = new MultaDto
            {
                DiasRetraso = multa.DiasRetraso,
                MontoTotal = multa.MontoMulta
            };
            
            // Requerimiento: "si hay retraso, retornar la multa en el body" junto con el préstamo
            return Ok(new { prestamo = prestamoDto, multa = multaDto });
        }

        // Si no hay multa, devolvemos solo el préstamo actualizado
        return Ok(new { prestamo = prestamoDto }); 
    
}
                

        [HttpGet("usuario/{nroSocio}/libro/{isbnLibro}/multa")]
        [ProducesResponseType(typeof(MultaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
public ActionResult<MultaDto> ConsultarMulta(int nroSocio, string isbnLibro)
{

        var prestamo = _servicioPrestamo.ObtenerPrestamoEspecifico(nroSocio, isbnLibro);
        //VALIDACIÓN SALVAVIDAS: Si no hay préstamo, devolvemos un 404 limpio
        if (prestamo == null)
        {
            return NotFound(new { mensaje = $"No se encontró un préstamo activo del libro {isbnLibro} para el socio {nroSocio}." });
        }
                
        var multa = _servicioMulta.CalcularMulta(prestamo); // Suponiendo que esto lanza excepción si no está vencido

        if (multa == null)
        {
            return Ok(new { mensaje = "El préstamo está al día, no hay multas." });
        }

        var multaDto = new MultaDto
        {
            DiasRetraso = multa.DiasRetraso,
            MontoTotal = multa.MontoMulta,
            FechaGenerada = multa.FechaGenerada
        };

        // 200 con MultaDto
        return Ok(multaDto); 

}

    }
}