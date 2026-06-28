using Libreria1.Domain.Entities;

/// <summary>
/// Interfaz para el servicio de generación de tokens JWT. 
/// </summary>
namespace Libreria1.Application.Interfaces
{
    public interface IServicioToken
    {
        /// <summary>
        /// Genera un token JWT para el usuario especificado.
        /// </summary>
        /// <param name="usuario">El usuario para el cual se generará el token.</param>
        /// <returns>El token JWT generado como una cadena.</returns>
        string GenerarToken(Usuario usuario);
    }
}