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

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow; // Inicializa con la fecha y hora actual en formato UTC

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
        [Display(Name = "Contraseña")]
        public string Clave { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Debes confirmar la clave.")]
        [Compare("Clave", ErrorMessage = "Las claves no coinciden.")]
        [Display(Name = "Confirmar Contraseña")]
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
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50)]
        public string Nombre { get; set; }
        
        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(50)]
        public string Apellido { get; set; }
        
        public string? Email { get; set; }
        
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime? FechaNacimiento { get; set; }
        
        [Phone]
        [Display(Name = "Número de Teléfono")]
        public string? PhoneNumber { get; set; }
        
        public IFormFile? ImagenPerfil { get; set; }
        public string? ImagenUrlPerfil { get; set; }
    }

    public class CambiarContraseñaViewModel
    {
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña actual")]
        [Required(ErrorMessage = "La contraseña actual es obligatoria.")]
        public string ContraseñaActual { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nueva contraseña")]
        [Required(ErrorMessage = "La nueva contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres.")]
        public string ContraseñaNueva { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar nueva contraseña")]
        [Required(ErrorMessage = "Debes confirmar la nueva contraseña.")]
        [Compare("ContraseñaNueva", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmarContraseña { get; set; }
    }

    public class AdministrarCuentaViewModel
    {
        public string Email { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool EstaActiva { get; set; }
        public int TotalReviews { get; set; }
        public int TotalFavoritos { get; set; }
    }
}
