using System.ComponentModel.DataAnnotations;

namespace app_movie_mvc.Models
{
    public class Favorito
    {
        public int Id { get; set; }

        [Required]
        public string UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        [Required]
        public int PeliculaId { get; set; }
        public Pelicula Pelicula { get; set; }

        public DateTime FechaAgregado { get; set; } = DateTime.UtcNow;
    }
}
    