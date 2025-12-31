using app_movie_mvc.Data;
using app_movie_mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace app_movie_mvc.Controllers
{
    [Authorize]
    public class FavoritosController : Controller
    {
        private readonly MovieDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public FavoritosController(MovieDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Favoritos
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var favoritos = await _context.Favoritos
                .Include(f => f.Pelicula)
                .ThenInclude(p => p.Genero)
                .Include(f => f.Pelicula)
                .ThenInclude(p => p.ListaReviews)
                .Include(f => f.Pelicula)
                .ThenInclude(p => p.UusariosFavorito)
                .Where(f => f.UsuarioId == userId)
                .ToListAsync();

            var peliculas = favoritos.Select(f => f.Pelicula).ToList();
            return View(peliculas);
        }

        // POST: Favoritos/Toggle/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Toggle(int id, string? returnUrl = null)
        {
            var userId = _userManager.GetUserId(User);

            var existente = await _context.Favoritos
                .FirstOrDefaultAsync(f => f.PeliculaId == id && f.UsuarioId == userId);

            if (existente == null)
            {
                var nuevo = new Favorito
                {
                    PeliculaId = id,
                    UsuarioId = userId,
                    FechaAgregado = DateTime.UtcNow
                };
                _context.Favoritos.Add(nuevo);
            }
            else
            {
                _context.Favoritos.Remove(existente);
            }

            await _context.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        // GET: Favoritos/IsFavorite/5
        [HttpGet]
        public async Task<IActionResult> IsFavorite(int id)
        {
            var userId = _userManager.GetUserId(User);
            var isFav = await _context.Favoritos.AnyAsync(f => f.PeliculaId == id && f.UsuarioId == userId);
            return Json(new { success = true, isFavorite = isFav });
        }
    }
}