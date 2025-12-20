namespace app_movie_mvc.Models
{
    public class Usuario
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        public DateTime FechaNacimiento { get; set; }
        public string ImagenUrlPerfil { get; set; }

        public List<Favorito>? PeliculasFavoritas { get; set; } // Lista de peliculas favoritas del usuario


        //sas
    }
}
