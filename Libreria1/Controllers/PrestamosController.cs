
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Libreria1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrestamosController : ControllerBase
    {
        private readonly ServicioPrestamo _servicioPrestamo;

        public PrestamosController(ServicioPrestamo servicioPrestamo)
        {
            _servicioPrestamo = servicioPrestamo;
        }

        // GET: api/prestamos/usuario/{nroSocio}
        [HttpGet("usuario/{nroSocio}")]
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
                FechaDevolucion = p.FechaDevolucion,
                EstaActivo = p.Activo
            }).ToList();

            return Ok(prestamosDto); // http 200 con la lista limpia
        }

        // POST: api/prestamos
        [HttpPost]
        public ActionResult Crear([FromBody] PrestamoDto dto)
        {
            try
            {
                _servicioPrestamo.PrestarLibro(dto.NroSocio, dto.LibroIsbn); 

                // devolvemos un 201 Created y redirigimos a la ruta del socio
                return CreatedAtAction(nameof(ListarPorSocio), new { nroSocio = dto.NroSocio }, new { mensaje = "Préstamo registrado exitosamente." }); 
            }
            catch (LibroNoDisponibleException ex)
            {
                return Conflict(new { mensaje = ex.Message }); // 409
            }
            catch (LimitePrestamosAlcanzadoException ex)
            {
                return Conflict(new { mensaje = ex.Message }); // 409
            }
            catch (LibroNoEncontradoException ex)
            {
                return NotFound(new { mensaje = ex.Message }); // 404
            }
            catch (UsuarioNoEncontradoException ex)
            {
                return NotFound(new { mensaje = ex.Message }); // 404
            }
        }
    }
}