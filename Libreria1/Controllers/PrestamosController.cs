
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
        private readonly ServicioMultas _servicioMulta;

        public PrestamosController(ServicioPrestamo servicioPrestamo, ServicioMultas servicioMulta)
        {
            _servicioPrestamo = servicioPrestamo;
            _servicioMulta = servicioMulta;
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

                _servicioPrestamo.PrestarLibro(dto.NroSocio, dto.LibroIsbn); 

                // devolvemos un 201 Created y redirigimos a la ruta del socio
                return CreatedAtAction(nameof(ListarPorSocio), new { nroSocio = dto.NroSocio }, new { mensaje = "Préstamo registrado exitosamente." }); 

        }

        [HttpPut("devolver")]
        public ActionResult Devolver([FromBody] CrearPrestamoDto dto)
        {

                var prestamoActualizado = _servicioPrestamo.DevolverLibro(dto.UsuarioId, dto.LibroIsbn);

                var prestamoDto = new PrestamoDto
                {
                    LibroIsbn = prestamoActualizado.LibroPrestado.Isbn,
                    NroSocio = prestamoActualizado.UsuarioAsignado.NroSocio,
                    FechaPrestamo = prestamoActualizado.FechaPrestamo,
                    FechaDevolucion = prestamoActualizado.FechaDevolucion,
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
public ActionResult<MultaDto> ConsultarMulta(int nroSocio, string isbnLibro)
{

        var prestamo = _servicioPrestamo.ObtenerPrestamoEspecifico(nroSocio, isbnLibro);
        
        var multa = _servicioMulta.CalcularMulta(prestamo); // Suponiendo que esto lanza excepción si no está vencido

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