using app_movie_mvc.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace app_movie_mvc.Data
{
    public class MovieDbContext : IdentityDbContext<Usuario>
    // IdentityDbContext con la clase Usuario personalizada para la autenticación y autorización
    // Hereda de IdentityDbContext para incluir las tablas de identidad en la base de datos
    // IdentityDbContext<Usuario> permite usar la clase Usuario personalizada en lugar de la clase IdentityUser predeterminada
    // Esto es útil para agregar propiedades adicionales al usuario, como Nombre, Apellido, etc.
    {
        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options) { }

        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Plataforma> Plataformas { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Favorito> Favoritos { get; set; }

    }
}
