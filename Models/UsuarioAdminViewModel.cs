using System.ComponentModel.DataAnnotations;

namespace app_movie_mvc.Models
{
    public class UsuarioAdminViewModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string ImagenUrlPerfil { get; set; }
        public bool EstaActivo { get; set; }
        public int TotalReviews { get; set; }
        public int TotalFavoritos { get; set; }
        public string Roles { get; set; }
    }

    public class UsuarioDetalleViewModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string ImagenUrlPerfil { get; set; }
        public bool EstaActivo { get; set; }
        public List<string> Roles { get; set; }
        public List<Review> Reviews { get; set; }
        public List<Pelicula> PeliculasFavoritas { get; set; }
    }

    public class ContactarUsuarioViewModel
    {
        public string UsuarioId { get; set; }
        public string UsuarioEmail { get; set; }
        public string UsuarioNombre { get; set; }
        
        [Required(ErrorMessage = "El asunto es obligatorio")]
        [StringLength(200)]
        public string Asunto { get; set; }
        
        [Required(ErrorMessage = "El mensaje es obligatorio")]
        [StringLength(2000)]
        public string Mensaje { get; set; }
    }
}