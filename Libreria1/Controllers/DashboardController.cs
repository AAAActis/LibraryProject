using System;
using System.Linq;
using Libreria1.Application.DTOs;
using Libreria1.Application.Interfaces;
using Libreria1.Application.Services;
using Libreria1.Domain.Entities;
using Libreria1.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Libreria1.Controllers
{
    /// <summary>
    /// Expone las métricas agregadas que consume el dashboard del frontend.
    /// No contiene lógica de negocio propia: sólo agrega datos ya calculados
    /// por el catálogo, el servicio de préstamos y el repositorio de multas.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly ICatalogo<Libro> _catalogo;
        private readonly ServicioPrestamo _servicioPrestamo;
        private readonly IRepositorio<Multa, Guid> _repositorioMultas;

        public DashboardController(
            ICatalogo<Libro> catalogo,
            ServicioPrestamo servicioPrestamo,
            IRepositorio<Multa, Guid> repositorioMultas)
        {
            _catalogo = catalogo;
            _servicioPrestamo = servicioPrestamo;
            _repositorioMultas = repositorioMultas;
        }

        // GET /api/dashboard/stats
        [HttpGet("stats")]
        [ProducesResponseType(typeof(DashboardStatsDto), StatusCodes.Status200OK)]
        public ActionResult<DashboardStatsDto> ObtenerEstadisticas()
        {
            var libros = _catalogo.ListarTodos();

            var stats = new DashboardStatsDto
            {
                TotalLibros = libros.Count,
                LibrosDisponibles = libros.Count(l => l.EstaDisponible),
                PrestamosActivos = _servicioPrestamo.ListarPrestamosActivos().Count,
                MultasPendientes = _repositorioMultas.ObtenerTodos().Count()
            };

            return Ok(stats);
        }
    }
}
