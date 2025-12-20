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
}
