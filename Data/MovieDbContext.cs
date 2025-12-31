using app_movie_mvc.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace app_movie_mvc.Data
{
    public class MovieDbContext : IdentityDbContext<Usuario>
    {
        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options) { }

        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Plataforma> Plataformas { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Favorito> Favoritos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Favorito>()
                .HasIndex(f => new { f.UsuarioId, f.PeliculaId })
                .IsUnique();

            builder.Entity<Favorito>()
                .HasOne(f => f.Usuario)
                .WithMany(u => u.PeliculasFavoritas)
                .HasForeignKey(f => f.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Favorito>()
                .HasOne(f => f.Pelicula)
                .WithMany(p => p.UusariosFavorito)
                .HasForeignKey(f => f.PeliculaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}


