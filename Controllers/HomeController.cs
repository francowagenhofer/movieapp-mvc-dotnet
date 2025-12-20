using app_movie_mvc.Data;
using app_movie_mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace app_movie_mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MovieDbContext _context;
        private const int PageSize = 8;
        //private readonly LlmService _llmService; // Servicio de LLM inyectado // Sirve para futuras funcionalidades relacionadas con LLM (large language models)

        public HomeController(ILogger<HomeController> logger, MovieDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Vista principal con paginación, búsqueda y filtro por género
        public async Task<IActionResult> Index(int pagina = 1, string txtBusqueda = "", int generoId = 0)
        {
            var consulta = _context.Peliculas.AsQueryable();
            if (!string.IsNullOrEmpty(txtBusqueda))
            {
                consulta = consulta.Where(p => p.Titulo.Contains(txtBusqueda));
            }

            if (generoId > 0)
            {
                consulta = consulta.Where(p => p.GeneroId == generoId);
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

            var generos = await _context.Generos.OrderBy(g => g.Descripcion).ToListAsync(); // Sirve para obtener la lista de géneros desde la base de datos
            generos.Insert(0, new Genero { Id = 0, Descripcion = "Género" });
            ViewBag.GeneroId = new SelectList(
                generos,
                "Id",
                "Descripcion",
                generoId
            );

            return View(peliculas);
        }

        // Vista de detalles de una película
        public async Task<IActionResult> Details(int Id)
        {
            var pelicula = await _context.Peliculas
                .Include(p => p.Genero)
                .FirstOrDefaultAsync(p => p.Id == Id);

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
    }
}
