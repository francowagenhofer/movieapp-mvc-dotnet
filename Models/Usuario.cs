using Microsoft.AspNetCore.Identity;

namespace app_movie_mvc.Models
{
    public class Usuario: IdentityUser // Sirve para extender la clase IdentityUser y agregar propiedades personalizadas
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        public DateTime FechaNacimiento { get; set; }
        public string ImagenUrlPerfil { get; set; }

        public List<Favorito>? PeliculasFavoritas { get; set; } // Lista de peliculas favoritas del usuario
        public List<Review>? ReviewsUsuario { get; set; } // Lista de reviews hechas por el usuario

    }
}
