using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app_movie_mvc.Models
{
    public class Pelicula
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Titulo { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime FechaLanzamientp { get; set; }
        
        [Required]
        [Range(1, 400)]
        public int MinutosDuracion { get; set; }

        [Required]
        [StringLength(1000)]
        public string Sinopsis { get; set; }
        
        [Url]
        [Required]
        public string PosterUrlPortada { get; set; }

        public int GeneroId { get; set; }
        public Genero? Genero { get; set; }
        public int PlataformaId { get; set; }
        public Plataforma? Plataforma { get; set; }

        [NotMapped] // Para que no lo genere en la base de datos, porque es calculado
        public int PromedioRating { get; set; }

        public List<Review>? ListaReviews { get; set; } // Reviews de la pelicula
        public List<Favorito>? UusariosFavorito { get; set; } // Usuario que marcaron como favorito
    }
}
