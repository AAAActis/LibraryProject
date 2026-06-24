using System;
using System.Text;
using System.Security.Claims;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Libreria1.Domain.Entities;
using Libreria1.Application.DTOs;
using Libreria1.Application.Interfaces;


namespace Libreria1.Controllers
{
        [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarios _servicioUsuario;
        private readonly IConfiguration _configuration;

        public AuthController(IUsuarios servicioUsuario, IConfiguration configuration)
        {
            _servicioUsuario = servicioUsuario;
            _configuration = configuration;
        }

        // POST api/auth/login
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            //Buscar el usuario por email
            var usuario = _servicioUsuario.BuscarPorEmail(dto.Email!);

            //Verificar si el usuario existe y si la contraseña es correcta
            if (usuario is null || !usuario.VerificarPassword(dto.Password!))
            {
                return Unauthorized(new { mensaje = "Email o contraseña incorrectos." });
            }

            // Generar el token y devolverlo
            var token = GenerarToken(usuario);
            return Ok(new { token });
        }

        private string GenerarToken(Usuario usuario)
        {
            var key = _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("Falta la clave Jwt:Key en la configuración.");

            var claves = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credenciales = new SigningCredentials(claves, SecurityAlgorithms.HmacSha256);

            // Claims requeridos por el criterio de aceptación: userId, email, rol
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email,          usuario.Email),
                new Claim(ClaimTypes.Role,           usuario.Rol)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), // expira en 1 hora
                signingCredentials: credenciales
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}