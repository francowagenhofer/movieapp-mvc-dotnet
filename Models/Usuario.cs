using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace app_movie_mvc.Models
{
    public class Usuario: IdentityUser // Sirve para extender la clase IdentityUser y agregar propiedades personalizadas
    {
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50)]
        public string Apellido { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        public string ImagenUrlPerfil { get; set; }

        public List<Favorito>? PeliculasFavoritas { get; set; } // Lista de peliculas favoritas del usuario
        public List<Review>? ReviewsUsuario { get; set; } // Lista de reviews hechas por el usuario
    }

    public class RegistroViewModel
    {
        [Required(ErrorMessage = "Debes ingresar un nombre")]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Debes ingresar un apellido")]
        [StringLength(50)]
        public string Apellido { get; set; }

        [EmailAddress(ErrorMessage = "Ingresa un email válido.")]
        [Required(ErrorMessage = "El email es obligatorio.")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "La clave es obligatoria.")]
        public string Clave { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Debes confirmar la clave.")]
        [Compare("Clave", ErrorMessage = "Las claves no coinciden.")]
        public string ConfirmarClave { get; set; }
    }

    public class LoginViewModel
    {
        [EmailAddress(ErrorMessage = "Ingresa un email válido.")]
        [Required(ErrorMessage = "El email es obligatorio.")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "La clave es obligatoria.")]
        public string Clave { get; set; }
        public bool Recordarme { get; set; }
    }
    public class MiPerfilViewModel
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string? Email { get; set; }
        public IFormFile? ImagenPerfil { get; set; }
        public string? ImagenUrlPerfil { get; set; }
    }
}
