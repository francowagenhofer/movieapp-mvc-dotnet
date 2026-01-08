using app_movie_mvc.Models;
using Microsoft.AspNetCore.Identity;

namespace app_movie_mvc.Data
{
    public class DbSeeder
    {

        public static async Task Seed(MovieDbContext context, UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
        {
            context.Database.EnsureCreated();

            // Crear rol Admin si no existe
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Crear usuario admin si no existe
            var adminUser = await userManager.FindByEmailAsync("admin@admin.com");
            if (adminUser == null)
            {
                adminUser = new Usuario
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    Nombre = "Admin",
                    Apellido = "Sistema",
                    //ImagenUrlPerfil = "https://ui-avatars.com/api/?name=Admin+Sistema&background=667eea&color=fff&size=200"
                    ImagenUrlPerfil = "/image/admin.png"
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            if (context.Peliculas.Any() || context.Plataformas.Any() || context.Generos.Any())
                return;

            // 1) Plataformas
            var plataformas = new List<Plataforma>
                {
                    new Plataforma {
                        Nombre = "Netflix",
                        Url = "https://www.netflix.com",
                        LogoUrl = "https://upload.wikimedia.org/wikipedia/commons/0/08/Netflix_2015_logo.svg"
                    },
                    new Plataforma {
                        Nombre = "Prime Video",
                        Url = "https://www.primevideo.com",
                        LogoUrl = "https://upload.wikimedia.org/wikipedia/commons/f/f1/Prime_Video.png"
                    },
                    new Plataforma {
                        Nombre = "Disney+",
                        Url = "https://www.disneyplus.com",
                        LogoUrl = "https://upload.wikimedia.org/wikipedia/commons/3/3e/Disney%2B_logo.svg"
                    },
                    new Plataforma {
                        Nombre = "Max",
                        Url = "https://www.max.com",
                        LogoUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSYFMc246jbEr7NfK-8ayHXVi61n_D25KSY6g&s"
                    },
                    new Plataforma {
                        Nombre = "Apple TV",
                        Url = "https://www.apple.com/tv",
                        LogoUrl = "https://upload.wikimedia.org/wikipedia/commons/2/28/Apple_TV_Plus_Logo.svg"
                    }
                };

            // 2) Géneros
            var generos = new List<Genero>
                {
                    new Genero { Descripcion = "Acción" },
                    new Genero { Descripcion = "Drama" },
                    new Genero { Descripcion = "Comedia" },
                    new Genero { Descripcion = "Ciencia Ficción" },
                    new Genero { Descripcion = "Animación" },
                    new Genero { Descripcion = "Thriller" },
                    new Genero { Descripcion = "Suspenso" },
                    new Genero { Descripcion = "Romance" },
                    new Genero { Descripcion = "Terror" },
                    new Genero { Descripcion = "Aventura" }
                };

            context.Plataformas.AddRange(plataformas);
            context.Generos.AddRange(generos);
            context.SaveChanges();

            var p = plataformas.ToDictionary(x => x.Nombre);
            var g = generos.ToDictionary(x => x.Descripcion);

            // 3) Películas
            var peliculas = new List<Pelicula>
                    {
                    // --- Netflix ---
                    new Pelicula {
                        Titulo = "El irlandés",
                        Sinopsis = "Frank Sheeran recuerda su vida en el crimen organizado.",
                        FechaLanzamiento = new DateTime(2019, 11, 27),
                        MinutosDuracion = 209,
                        PosterUrlPortada = "https://m.media-amazon.com/images/M/MV5BYjRiMzYyNjItZmMwMy00ZGIwLWE2NDktMjcxYTM3MTE2ODhhXkEyXkFqcGc@._V1_.jpg",
                        Genero = g["Drama"], Plataforma = p["Netflix"]
                    },
                    new Pelicula {
                        Titulo = "Bird Box: A ciegas",
                        Sinopsis = "Una madre protege a sus hijos de una entidad mortal.",
                        FechaLanzamiento = new DateTime(2018, 12, 21),
                        MinutosDuracion = 124,
                        PosterUrlPortada = "https://m.media-amazon.com/images/M/MV5BMjAzMTI1MjMyN15BMl5BanBnXkFtZTgwNzU5MTE2NjM@._V1_FMjpg_UX1000_.jpg",
                        Genero = g["Ciencia Ficción"], Plataforma = p["Netflix"]
                    },
                    new Pelicula {
                        Titulo = "El hombre gris",
                        Sinopsis = "Un agente de la CIA huye de un ex colega psicópata.",
                        FechaLanzamiento = new DateTime(2022, 7, 22),
                        MinutosDuracion = 129,
                        PosterUrlPortada = "https://image.tmdb.org/t/p/w500/8cXbitsS6dWQ5gfMTZdorpAAzEH.jpg",
                        Genero = g["Acción"], Plataforma = p["Netflix"]
                    },
                    new Pelicula {
                        Titulo = "El proyecto Adam",
                        Sinopsis = "Un piloto viaja en el tiempo y conoce a su yo de 12 años.",
                        FechaLanzamiento = new DateTime(2022, 3, 11),
                        MinutosDuracion = 106,
                        PosterUrlPortada = "https://image.tmdb.org/t/p/w500/wFjboE0aFZNbVOF05fzrka9Fqyx.jpg",
                        Genero = g["Ciencia Ficción"], Plataforma = p["Netflix"]
                    },
                    new Pelicula {
                        Titulo = "Pinocho de Guillermo del Toro",
                        Sinopsis = "Reinvención en stop-motion del clásico cuento.",
                        FechaLanzamiento = new DateTime(2022, 12, 9),
                        MinutosDuracion = 117,
                        PosterUrlPortada = "https://image.tmdb.org/t/p/w500/vx1u0uwxdlhV2MUzj4VlcMB0N6m.jpg",
                        Genero = g["Animación"], Plataforma = p["Netflix"]
                    },
                    new Pelicula {
                        Titulo = "Barbie",
                        Sinopsis = "Barbie sale de su mundo de fantasía al mundo real.",
                        FechaLanzamiento = new DateTime(2023, 7, 21),
                        MinutosDuracion = 114,
                        PosterUrlPortada = "https://image.tmdb.org/t/p/w500/iuFNMS8U5cb6xfzi51Dbkovj7vM.jpg",
                        Genero = g["Comedia"], Plataforma = p["Netflix"]
                    },
                    new Pelicula {
                        Titulo = "The Creator",
                        Sinopsis = "Un soldado debe matar a la creadora de una IA que causa la guerra.",
                        FechaLanzamiento = new DateTime(2023, 9, 29),
                        MinutosDuracion = 134,
                        PosterUrlPortada = "https://image.tmdb.org/t/p/w500/vBZ0qvaRxqEhZwl6LWmruJqWE8Z.jpg",
                        Genero = g["Ciencia Ficción"], Plataforma = p["Netflix"]
                    },
                    new Pelicula {
                        Titulo = "Gladiador",
                        Sinopsis = "Un general romano se convierte en gladiador para vengar su familia.",
                        FechaLanzamiento = new DateTime(2000, 5, 5),
                        MinutosDuracion = 155,
                        PosterUrlPortada = "https://image.tmdb.org/t/p/w500/ty8TGRuvJLPUmAR1H1nRIsgwvim.jpg",
                        Genero = g["Acción"], Plataforma = p["Netflix"]
                    },

                    // --- Prime Video ---
                    new Pelicula {
                        Titulo = "La guerra del mañana",
                        Sinopsis = "Soldados del futuro reclutan gente del presente.",
                        FechaLanzamiento = new DateTime(2021, 7, 2),
                        MinutosDuracion = 138,
                        PosterUrlPortada = "https://image.tmdb.org/t/p/w500/xipF6XqfSYV8DxLqfLN6aWlwuRp.jpg",
                        Genero = g["Ciencia Ficción"], Plataforma = p["Prime Video"]
                    },
                    new Pelicula {
                        Titulo = "Sound of Metal",
                        Sinopsis = "Un baterista de metal pierde la audición.",
                        FechaLanzamiento = new DateTime(2020, 11, 20),
                        MinutosDuracion = 120,
                        PosterUrlPortada = "https://image.tmdb.org/t/p/w500/y89kFMNYXNKMdlZjR2yg7nQtcQH.jpg",
                        Genero = g["Drama"], Plataforma = p["Prime Video"]
                    },
                    new Pelicula {
                        Titulo = "One Night in Miami...",
                        Sinopsis = "Encuentro ficticio entre Ali, Malcolm X, Sam Cooke y Jim Brown.",
                        FechaLanzamiento = new DateTime(2020, 12, 25),
                        MinutosDuracion = 114,
                        PosterUrlPortada = "https://image.tmdb.org/t/p/original/1DLUb9PTDqXMSgsD7RmiJs7ZJIx.jpg",  // no funciona
                        Genero = g["Drama"], Plataforma = p["Prime Video"]
                    },
                    new Pelicula {
                        Titulo = "Air",
                        Sinopsis = "Cómo Nike fichó a Michael Jordan y nació Air Jordan.",
                        FechaLanzamiento = new DateTime(2023, 4, 5),
                        MinutosDuracion = 112,
                        PosterUrlPortada = "https://i.ebayimg.com/images/g/b-AAAOSwfOBkEoXc/s-l1200.jpg", // esta mal el poster
                        Genero = g["Drama"], Plataforma = p["Prime Video"]
                    },
                    new Pelicula {
                        Titulo = "Manchester by the Sea",
                        Sinopsis = "Un hombre vuelve a su pueblo tras una tragedia familiar.",
                        FechaLanzamiento = new DateTime(2016, 12, 16),
                        MinutosDuracion = 137,
                        PosterUrlPortada = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcR-LUBCbMxYcP5veZx5LOmMKSOQdLsxR6NrWw&s", // no funciona
                        Genero = g["Drama"], Plataforma = p["Prime Video"]
                    },
                    new Pelicula {
                        Titulo = "Oppenheimer",
                        Sinopsis = "La historia del científico que desarrolló la bomba atómica.",
                        FechaLanzamiento = new DateTime(2023, 7, 21),
                        MinutosDuracion = 180,
                        PosterUrlPortada = "https://image.tmdb.org/t/p/w500/ptpr0kGAckfQkJeJIt8st5dglvd.jpg",
                        Genero = g["Drama"], Plataforma = p["Prime Video"]
                    },
                    new Pelicula {
                        Titulo = "El Aro",
                        Sinopsis = "Una mujer descubre una cinta de video maldita.",
                        FechaLanzamiento = new DateTime(2002, 10, 18),
                        MinutosDuracion = 115,
                        PosterUrlPortada = "https://m.media-amazon.com/images/M/MV5BNDA2NTg2NjE4Ml5BMl5BanBnXkFtZTYwMjYxMDg5._V1_.jpg",  // no funciona
                        Genero = g["Terror"], Plataforma = p["Prime Video"]
                    },

                    // --- Disney+ ---
                    new Pelicula {
                        Titulo = "Soul",
                        Sinopsis = "Un profesor de música busca su propósito tras una experiencia extracorpórea.",
                        FechaLanzamiento = new DateTime(2020, 12, 25),
                        MinutosDuracion = 101,
                        PosterUrlPortada = "https://image.tmdb.org/t/p/w500/hm58Jw4Lw8OIeECIq5qyPYhAeRJ.jpg",
                        Genero = g["Animación"], Plataforma = p["Disney+"]
                    },
                    new Pelicula {
                        Titulo = "Luca",
                        Sinopsis = "Dos amigos monstruos marinos viven un verano inolvidable en la Riviera italiana.",
                        FechaLanzamiento = new DateTime(2021, 6, 18),
                        MinutosDuracion = 95,
                        PosterUrlPortada = "https://image.tmdb.org/t/p/w500/jTswp6KyDYKtvC52GbHagrZbGvD.jpg",
                        Genero = g["Animación"], Plataforma = p["Disney+"]
                    },
                    new Pelicula {
                        Titulo = "Turning Red",
                        Sinopsis = "Una chica de 13 años se transforma en un panda rojo gigante.",
                        FechaLanzamiento = new DateTime(2022, 3, 11),
                        MinutosDuracion = 100,
                        PosterUrlPortada = "https://image.tmdb.org/t/p/w500/qsdjk9oAKSQMWs0Vt5Pyfh6O4GZ.jpg",
                        Genero = g["Animación"], Plataforma = p["Disney+"]
                    },
                    new Pelicula {
                        Titulo = "Abracadabra 2 (Hocus Pocus 2)",
                        Sinopsis = "Regresan las hermanas Sanderson a causar caos en Salem.",
                        FechaLanzamiento = new DateTime(2022, 9, 30),
                        MinutosDuracion = 104,
                        PosterUrlPortada = "https://image.tmdb.org/t/p/w500/7ze7YNmUaX81ufctGqt0AgHxRtL.jpg",
                        Genero = g["Comedia"], Plataforma = p["Disney+"]
                    },
                    new Pelicula {
                        Titulo = "Desencantada (Disenchanted)",
                        Sinopsis = "Giselle desea una vida de cuento… y el hechizo se complica.",
                        FechaLanzamiento = new DateTime(2022, 11, 18),
                        MinutosDuracion = 119,
                        PosterUrlPortada = "https://m.media-amazon.com/images/I/712x0sdoR5L._AC_UF894,1000_QL80_.jpg",  // no funciona 
                        Genero = g["Comedia"], Plataforma = p["Disney+"]
                    },
                    new Pelicula {
                        Titulo = "Encanto",
                        Sinopsis = "Una familia mágica vive en una casa encantada en Colombia.",
                        FechaLanzamiento = new DateTime(2021, 11, 24),
                        MinutosDuracion = 102,
                        PosterUrlPortada = "https://image.tmdb.org/t/p/w500/4j0PNHkMr5ax3IA8tjtxcmPU3QT.jpg",
                        Genero = g["Animación"], Plataforma = p["Disney+"]
                    },
                    new Pelicula {
                        Titulo = "Espíritu Indomable",
                        Sinopsis = "La historia de un caballo salvaje y una chica.",
                        FechaLanzamiento = new DateTime(2021, 6, 4),
                        MinutosDuracion = 93,
                        PosterUrlPortada = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSlyd1sNGJo1XXmSLPTIfKjAzOsAMs2uSUpZQ&s",  // no funciona
                        Genero = g["Aventura"], Plataforma = p["Disney+"]
                    },

                    // --- Max ---
                    new Pelicula {
                        Titulo = "Inception",
                        Sinopsis = "Un ladrón de sueños infiltra su mente en una corporación.",
                        FechaLanzamiento = new DateTime(2010, 7, 16),
                        MinutosDuracion = 148,
                        PosterUrlPortada = "https://image.tmdb.org/t/p/w500/edv5CZvWj09upOsy2Y6IwDhK8bt.jpg",
                        Genero = g["Ciencia Ficción"], Plataforma = p["Max"]
                    },
                    new Pelicula {
                        Titulo = "The Dark Knight",
                        Sinopsis = "Batman se enfrenta a su mayor enemigo: el Joker.",
                        FechaLanzamiento = new DateTime(2008, 7, 18),
                        MinutosDuracion = 152,
                        PosterUrlPortada = "https://image.tmdb.org/t/p/w500/qJ2tW6WMUDux911r6m7haRef0WH.jpg",
                        Genero = g["Acción"], Plataforma = p["Max"]
                    },
                    new Pelicula {
                        Titulo = "Interstellar",
                        Sinopsis = "Un grupo de astronautas viaja a través de un agujero de gusano.",
                        FechaLanzamiento = new DateTime(2014, 11, 7),
                        MinutosDuracion = 169,
                        PosterUrlPortada = "https://image.tmdb.org/t/p/w500/rAiYTfKGqDCRIIqo664sY9XZIvQ.jpg",
                        Genero = g["Ciencia Ficción"], Plataforma = p["Max"]
                    },
                    new Pelicula {
                        Titulo = "Dune",
                        Sinopsis = "Un joven debe cumplir su destino en un planeta desértico.",
                        FechaLanzamiento = new DateTime(2021, 10, 22),
                        MinutosDuracion = 156,
                        PosterUrlPortada = "https://image.tmdb.org/t/p/w500/d5NXSklXo0qyIYkgV94XAgMIckC.jpg",
                        Genero = g["Aventura"], Plataforma = p["Max"]
                    },
                  
                    // --- Apple TV ---
                    new Pelicula {
                        Titulo = "Forrest Gump",
                        Sinopsis = "La vida de un hombre simple pero influyente en la historia.",
                        FechaLanzamiento = new DateTime(1994, 7, 6),
                        MinutosDuracion = 142,
                        PosterUrlPortada = "https://image.tmdb.org/t/p/w500/saHP97rTPS5eLmrLQEcANmKrsFl.jpg",
                        Genero = g["Drama"], Plataforma = p["Apple TV"]
                    },
                    new Pelicula {
                        Titulo = "Killers of the Flower Moon",
                        Sinopsis = "Históricos asesinatos de nativos americanos por dinero del petróleo.",
                        FechaLanzamiento = new DateTime(2023, 10, 20),
                        MinutosDuracion = 206,
                        PosterUrlPortada = "https://image.tmdb.org/t/p/w500/dB6Krk806zeqd0YNp2ngQ9zXteH.jpg",
                        Genero = g["Drama"], Plataforma = p["Apple TV"]
                    }
                };

            context.Peliculas.AddRange(peliculas);
            context.SaveChanges();

            // 4) Crear 10 usuarios regulares con reviews y favoritos
            var usuarios = new List<Usuario>();
            var usuariosEmails = new[]
            {
                    "maria.garcia@email.com",
                    "juan.lopez@email.com",
                    "ana.martinez@email.com",
                    "carlos.diaz@email.com",
                    "lucia.fernandez@email.com",
                    "sergio.gomez@email.com",
                    "paula.rodriguez@email.com",
                    "manuel.sanchez@email.com",
                    "elena.torres@email.com",
                    "diego.ruiz@email.com"
                };

            var usuariosData = new[]
            {
                    ("María", "García", new DateTime(1990, 5, 15)),
                    ("Juan", "López", new DateTime(1985, 8, 22)),
                    ("Ana", "Martínez", new DateTime(1995, 3, 10)),
                    ("Carlos", "Díaz", new DateTime(1988, 11, 30)),
                    ("Lucía", "Fernández", new DateTime(1992, 7, 25)),
                    ("Sergio", "Gómez", new DateTime(1987, 2, 14)),
                    ("Paula", "Rodríguez", new DateTime(1993, 9, 5)),
                    ("Manuel", "Sánchez", new DateTime(1986, 6, 18)),
                    ("Elena", "Torres", new DateTime(1994, 4, 12)),
                    ("Diego", "Ruiz", new DateTime(1989, 12, 3))
                };

            // Avatares que funcionan correctamente (ui-avatars.com)
            var avatarUrls = new[]
            {
                    "https://ui-avatars.com/api/?name=Maria+Garcia&background=667eea&color=fff&size=200",
                    "https://ui-avatars.com/api/?name=Juan+Lopez&background=764ba2&color=fff&size=200",
                    "https://ui-avatars.com/api/?name=Ana+Martinez&background=f093fb&color=fff&size=200",
                    "https://ui-avatars.com/api/?name=Carlos+Diaz&background=4facfe&color=fff&size=200",
                    "https://ui-avatars.com/api/?name=Lucia+Fernandez&background=00f2fe&color=fff&size=200",
                    "https://ui-avatars.com/api/?name=Sergio+Gomez&background=43e97b&color=fff&size=200",
                    "https://ui-avatars.com/api/?name=Paula+Rodriguez&background=fa709a&color=fff&size=200",
                    "https://ui-avatars.com/api/?name=Manuel+Sanchez&background=fee140&color=000&size=200",
                    "https://ui-avatars.com/api/?name=Elena+Torres&background=30cfd0&color=fff&size=200",
                    "https://ui-avatars.com/api/?name=Diego+Ruiz&background=a8edea&color=000&size=200"
                };

            for (int i = 0; i < usuariosEmails.Length; i++)
            {
                var usuario = await userManager.FindByEmailAsync(usuariosEmails[i]);
                if (usuario == null)
                {
                    usuario = new Usuario
                    {
                        UserName = usuariosEmails[i],
                        Email = usuariosEmails[i],
                        Nombre = usuariosData[i].Item1,
                        Apellido = usuariosData[i].Item2,
                        FechaNacimiento = usuariosData[i].Item3,
                        ImagenUrlPerfil = avatarUrls[i],
                        FechaRegistro = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 365))
                    };

                    await userManager.CreateAsync(usuario, "Password123!");
                    usuarios.Add(usuario);
                }
            }

            // Cargar las películas para asociarlas
            var peliculasDb = context.Peliculas.ToList();

            // 5) Crear reviews para cada usuario
            var reviews = new List<Review>();
            var comentarios = new[]
            {
                    "Excelente película, muy recomendada.",
                    "Fue una experiencia cinematográfica memorable.",
                    "No me convenció mucho, esperaba más.",
                    "¡Magnífica! Una obra maestra del cine.",
                    "Interesante argumento pero la ejecución fue floja.",
                    "Definitivamente una de mis favoritas.",
                    "Decepcionante, no vivió a la altura de las expectativas.",
                    "Impresionante de principio a fin.",
                    "Regular, nada del otro mundo.",
                    "Sorprendentemente buena, la disfruté mucho."
                };

            foreach (var usuario in usuarios)
            {
                // Cada usuario hace 7-10 reviews
                var reviewCount = Random.Shared.Next(7, 11);
                var peliculasParaReview = peliculasDb
                    .OrderBy(x => Random.Shared.Next())
                    .Take(reviewCount)
                    .ToList();

                foreach (var pelicula in peliculasParaReview)
                {
                    var review = new Review
                    {
                        UsuarioId = usuario.Id,
                        Usuario = usuario,
                        PeliculaId = pelicula.Id,
                        Pelicula = pelicula,
                        Rating = Random.Shared.Next(1, 6),
                        Comentario = comentarios[Random.Shared.Next(comentarios.Length)],
                        FechaReview = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 180))
                    };

                    reviews.Add(review);
                }
            }

            context.Reviews.AddRange(reviews);
            context.SaveChanges();

            // 6) Crear favoritos para cada usuario
            var favoritos = new List<Favorito>();

            foreach (var usuario in usuarios)
            {
                // Cada usuario marca 10-15 películas como favoritas
                var favCount = Random.Shared.Next(10, 16);
                var peliculasFav = peliculasDb
                    .OrderBy(x => Random.Shared.Next())
                    .Take(favCount)
                    .ToList();

                foreach (var pelicula in peliculasFav)
                {
                    var favorito = new Favorito
                    {
                        UsuarioId = usuario.Id,
                        Usuario = usuario,
                        PeliculaId = pelicula.Id,
                        Pelicula = pelicula,
                        FechaAgregado = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 180))
                    };

                    favoritos.Add(favorito);
                }
            }

            context.Favoritos.AddRange(favoritos);
            context.SaveChanges();
        }
    }
}