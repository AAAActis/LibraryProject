using System.ComponentModel.DataAnnotations;

namespace Libreria1.Application.DTOs
{
    /// <summary>
    /// DTO para el login de un usuario.
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// El email del usuario que intenta iniciar sesión.
        /// </summary>
        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El email no es válido.")]
        public string Email { get; set; }
    
        /// <summary>
        /// La contraseña del usuario que intenta iniciar sesión.
        /// </summary>
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Password { get; set; }
    }
}