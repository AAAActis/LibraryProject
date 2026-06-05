using Microsoft.AspNetCore.Mvc;
using Libreria1.Domain.Entities;
using Libreria1.Application.DTOs;
using Libreria1.Application.Interfaces;
using Libreria1.Domain.Exceptions;

namespace Libreria1.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarios _servicioUsuario;

        public UsuariosController(IUsuarios servicioUsuario)
        {
            _servicioUsuario = servicioUsuario;
        }

        // GET api/usuarios
        [HttpGet]
        [ProducesResponseType(typeof(List<UsuarioDto>), StatusCodes.Status200OK)]
        public ActionResult<List<UsuarioDto>> ObtenerTodos()
        {
            var usuarios = _servicioUsuario.ListarUsuarios();
            var dtos = usuarios.Select(u => new UsuarioDto
            {
                Id = u.Id,           // ← propiedad directa, no método
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                Email = u.Email      // ← propiedad directa, no método
            }).ToList();
            return Ok(dtos);
        }

        // GET api/usuarios/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UsuarioDto> ObtenerPorId(Guid id)
        {
            var usuario = _servicioUsuario.BuscarPorId(id);
            if (usuario == null)
                return NotFound($"Usuario con Id {id} no encontrado.");

            var dto = new UsuarioDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email
            };
            return Ok(dto);
        }

        // POST api/usuarios
        [HttpPost]
        [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<UsuarioDto> Crear([FromBody] CrearUsuarioDto dto)
        {

                var usuario = _servicioUsuario.RegistrarUsuario(
                    dto.Nombre, dto.Apellido, dto.Email
                );
                var usuarioDto = new UsuarioDto
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    Email = usuario.Email
                };
                return CreatedAtAction(nameof(ObtenerPorId), new { id = usuario.Id }, usuarioDto);
        }
    }
}