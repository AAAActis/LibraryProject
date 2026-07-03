using System;
using System.Collections.Generic;
using System.Linq;
using Libreria1.Domain.Entities;
using Libreria1.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Libreria1.Controllers
{
    /// <summary>
    /// Lista las multas registradas en el sistema para vistas administrativas.
    /// Reutiliza el repositorio de multas ya inyectado; no calcula ni aplica multas
    /// (eso es responsabilidad de IServicioMulta, usado desde PrestamosController).
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MultasController : ControllerBase
    {
        private readonly IRepositorio<Multa, Guid> _repositorioMultas;

        public MultasController(IRepositorio<Multa, Guid> repositorioMultas)
        {
            _repositorioMultas = repositorioMultas;
        }

        // GET /api/multas
        [HttpGet]
        [ProducesResponseType(typeof(List<MultaListadoDto>), StatusCodes.Status200OK)]
        public ActionResult<List<MultaListadoDto>> ListarTodas()
        {
            var multas = _repositorioMultas.ObtenerTodos()
                .Select(m => new MultaListadoDto
                {
                    Id = m.Id,
                    LibroTitulo = m.PrestamoAsignado.LibroPrestado.Titulo,
                    UsuarioNombre = $"{m.PrestamoAsignado.UsuarioAsignado.Nombre} {m.PrestamoAsignado.UsuarioAsignado.Apellido}",
                    DiasRetraso = m.DiasRetraso,
                    MontoTotal = m.MontoMulta,
                    FechaGenerada = m.FechaGenerada
                })
                .ToList();

            return Ok(multas);
        }
    }
}
