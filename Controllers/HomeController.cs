using app_movie_mvc.Data;
using app_movie_mvc.Models;
using app_movie_mvc.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace app_movie_mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MovieDbContext _context;
        private const int PageSize = 8;
        //private readonly LlmService _llmService;

        //public HomeController(ILogger<HomeController> logger, MovieDbContext context, LlmService llmService)
        public HomeController(ILogger<HomeController> logger, MovieDbContext context)
        {
            _logger = logger;
            _context = context;
            //_llmService = llmService;
        }

        public async Task<IActionResult> Index(int pagina = 1, string txtBusqueda = "", int generoId = 0, int plataformaId = 0, int minRating = 0)
        {
            if (pagina < 1) pagina = 1;

            // Obtener películas destacadas (top 4 por rating)
            var peliculasDestacadas = await _context.Peliculas
                .Include(p => p.Genero)
                .Include(p => p.ListaReviews)
                .Include(p => p.UusariosFavorito)
                .Where(p => p.ListaReviews.Any())
                .OrderByDescending(p => p.ListaReviews.Average(r => r.Rating))
                .Take(4)
                .ToListAsync();

            ViewBag.PeliculasDestacadas = peliculasDestacadas;

            var consulta = _context.Peliculas
                .Include(p => p.ListaReviews)
                .Include(p => p.UusariosFavorito)
                .Include(p => p.Genero)
                .AsQueryable();
            if (!string.IsNullOrEmpty(txtBusqueda))
            {
                consulta = consulta.Where(p => p.Titulo.Contains(txtBusqueda));
            }

            if (generoId > 0)
            {
                consulta = consulta.Where(p => p.GeneroId == generoId);
            }

            if (plataformaId > 0)
            {
                consulta = consulta.Where(p => p.PlataformaId == plataformaId);
            }

            if (minRating > 0)
            {
                consulta = consulta.Where(p =>
                    _context.Reviews
                        .Where(r => r.PeliculaId == p.Id)
                        .Select(r => (double?)r.Rating)
                        .Average() >= minRating
                );
            }



            var totalPeliculas = await consulta.CountAsync();
            var totalPaginas = (int)Math.Ceiling(totalPeliculas / (double)PageSize);

            if (pagina > totalPaginas && totalPaginas > 0) pagina = totalPaginas;

            var peliculas = await consulta
                .Skip((pagina - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = totalPaginas;
            ViewBag.TotalPeliculas = totalPeliculas;
            ViewBag.TxtBusqueda = txtBusqueda;
            ViewBag.PlataformaSeleccionada = plataformaId;
            ViewBag.MinRatingSeleccionado = minRating;

            var generos = await _context.Generos.OrderBy(g => g.Descripcion).ToListAsync();
            generos.Insert(0, new Genero { Id = 0, Descripcion = "Género" });
            ViewBag.GeneroId = new SelectList(
                generos,
                "Id",
                "Descripcion",
                generoId
            );

            var plataformas = await _context.Plataformas.OrderBy(p => p.Nombre).ToListAsync();
            plataformas.Insert(0, new Plataforma { Id = 0, Nombre = "Plataforma" });

            ViewBag.PlataformaId = new SelectList(plataformas, "Id", "Nombre", plataformaId);

            var ratings = new List<SelectListItem>
            {
                new("Calificación", "0"),
                new("5", "5"),
                new("4 o más", "4"),
                new("3 o más", "3"),
                new("2 o más", "2"),
                new("1 o más", "1"),
            };
            ViewBag.MinRating = new SelectList(ratings, "Value", "Text", minRating.ToString());



            return View(peliculas);
        }

        public async Task<IActionResult> Details(int Id)
        {
            var pelicula = await _context.Peliculas
                .Include(p => p.Genero)
                .Include(p => p.ListaReviews)
                .ThenInclude(r => r.Usuario)
                .Include(p => p.UusariosFavorito)
                .FirstOrDefaultAsync(p => p.Id == Id);

            if (pelicula == null)
            {
                return NotFound(); // o return View("NotFound"); según UX
            }

            ViewBag.UserReview = false;
            if (User?.Identity?.IsAuthenticated == true && pelicula.ListaReviews != null)
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                ViewBag.UserReview = pelicula.ListaReviews.Any(r => r.UsuarioId == userId);
            }

            return View(pelicula);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //[HttpGet]
        //public async Task<IActionResult> Spoiler(string titulo)
        //{
        //    try
        //    {
        //        var spoiler = await _llmService.ObtenerSpoilerAsync(titulo);
        //        return Json(new { success = true, data = spoiler });

        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = ex.Message });
        //    }
        //}

        //[HttpGet]
        //public async Task<IActionResult> Resumen(string titulo)
        //{
        //    try
        //    {
        //        var resumen = await _llmService.ObtenerResumenAsync(titulo);
        //        return Json(new { success = true, data = resumen });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = ex.Message });
        //    }
        //}
    }
}
