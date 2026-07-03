using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Libreria1.Application.Interfaces;
using Libreria1.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Libreria1.Application.Services
{
    /// <summary>
    /// Servicio responsable exclusivamente de generar tokens JWT.
    /// </summary>
    public class ServicioToken : IServicioToken
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// La configuración se inyecta vía DI; el servicio no sabe
        /// de dónde vienen los valores (appsettings, secrets, env vars).
        /// </summary>
        public ServicioToken(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <inheritdoc />
        public string GenerarToken(Usuario usuario)
        {
            // 1. Leer la clave secreta — falla rápido si no está configurada
            var key = _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException(
                    "Falta la clave 'Jwt:Key' en la configuración. " +
                    "Agregala en appsettings.json o en User Secrets.");

            // 2. Construir las credenciales de firma con HMAC-SHA256
            var claves       = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credenciales = new SigningCredentials(claves, SecurityAlgorithms.HmacSha256);

            // 3. Definir los claims que viajan dentro del token
            //    - NameIdentifier : permite identificar al usuario en endpoints protegidos
            //    - Email           : dato de presentación / auditoría
            //    - Role            : habilita [Authorize(Roles = "...")] en los controladores
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email,          usuario.Email),
                new Claim(ClaimTypes.Role,           usuario.TipoRol.ToString())
            };

            // 4. Armar el token con expiración de 1 hora
            var token = new JwtSecurityToken(
                claims:             claims,
                expires:            DateTime.UtcNow.AddHours(1),
                signingCredentials: credenciales
            );

            // 5. Serializar a string y devolver
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}