using Libreria1.Application.DTOs;
using Libreria1.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Libreria1.Controllers
{
    /// <summary>
    /// Controlador de autenticación.
    /// 
    /// Responsabilidad única (SRP): sólo maneja el ciclo de vida
    /// de la petición HTTP. La lógica de negocio vive en los servicios:
    ///   - IUsuarios      → valida credenciales
    ///   - IServicioToken → genera el JWT
    /// 
    /// El controlador no sabe CÓMO se genera el token,
    /// sólo sabe A QUIÉN pedírselo.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarios      _servicioUsuario;
        private readonly IServicioToken _servicioToken;

        // IConfiguration ya NO se inyecta aquí:
        // la capa de presentación no tiene que saber de configuración de JWT.
        public AuthController(IUsuarios servicioUsuario, IServicioToken servicioToken)
        {
            _servicioUsuario = servicioUsuario;
            _servicioToken   = servicioToken;
        }

        /// <summary>
        /// Autentica al usuario y devuelve un token JWT.
        /// </summary>
        /// <param name="dto">Email y contraseña del usuario.</param>
        /// <returns>Token JWT si las credenciales son válidas; 401 en caso contrario.</returns>
        // POST api/auth/login
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            // 1. Buscar el usuario por email
            var usuario = _servicioUsuario.BuscarPorEmail(dto.Email!);

            // 2. Verificar existencia y contraseña
            if (usuario is null || !usuario.VerificarPassword(dto.Password!))
            {
                return Unauthorized(new { mensaje = "Email o contraseña incorrectos." });
            }

            // 3. Delegar la generación del token al servicio especializado
            var token = _servicioToken.GenerarToken(usuario);

            return Ok(new { token });
        }
    }
}