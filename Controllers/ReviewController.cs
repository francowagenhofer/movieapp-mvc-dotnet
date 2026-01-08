using app_movie_mvc.Data;
using app_movie_mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace app_movie_mvc.Controllers
{
    public class ReviewController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly MovieDbContext _context;
        public ReviewController(UserManager<Usuario> userManager, MovieDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        // GET: ReviewController
        public async Task<ActionResult> Index()//Mis Reviews
        {
            var userId = _userManager.GetUserId(User);
            var reviews = await _context.Reviews
                .Include(r => r.Pelicula)
                .Where(r => r.UsuarioId == userId)
                .ToListAsync();

            return View(reviews);
        }

        // GET: ReviewController/Details/5
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            var review = await _context.Reviews
                .Include(r => r.Pelicula)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (review == null)
                return NotFound();

            var vm = new ReviewCreateViewModel
            {
                Id = review.Id,
                PeliculaId = review.PeliculaId,
                UsuarioId = review.UsuarioId,
                Rating = review.Rating,
                Comentario = review.Comentario,
                PeliculaTitulo = review.Pelicula?.Titulo ?? "Desconocido"
            };

            return View(vm);
        }

        // GET: ReviewController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReviewController/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReviewCreateViewModel review)
        {
            try
            {
                review.UsuarioId = _userManager.GetUserId(User);

                //Validación de si ya existe una review del mismo usuario.
                var reviewExiste = _context.Reviews
                    .FirstOrDefault(r => r.PeliculaId == review.PeliculaId && r.UsuarioId == review.UsuarioId);
                if (reviewExiste != null)
                {
                    TempData["ReviewExiste"] = "Ya has realizado una reseña para esta película.";
                    return RedirectToAction("Details", "Home", new { id = review.PeliculaId });
                }
                //Fin validación.

                if (ModelState.IsValid)
                {
                    var nuevaReview = new Review
                    {
                        PeliculaId = review.PeliculaId,
                        UsuarioId = review.UsuarioId,
                        Rating = review.Rating,
                        Comentario = review.Comentario,
                        FechaReview = DateTime.Now
                    };
                    _context.Reviews.Add(nuevaReview);
                    _context.SaveChanges();
                    return RedirectToAction("Details", "Home", new { id = review.PeliculaId });
                }


                return View(review);
            }
            catch
            {
                return View(review);
            }
        }

        // GET: ReviewController/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(int id)
        {

            var review = _context.Reviews
                .Include(r => r.Pelicula)
                .FirstOrDefault(r => r.Id == id);
            if (review == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (review.UsuarioId != user.Id && !_userManager.IsInRoleAsync(user, "Admin").Result)
                return Forbid();

            var reviewViewModel = new ReviewCreateViewModel
            {
                Id = review.Id,
                PeliculaId = review.PeliculaId,
                UsuarioId = review.UsuarioId,
                Rating = review.Rating,
                Comentario = review.Comentario,
                PeliculaTitulo = review.Pelicula?.Titulo
            };

            return View(reviewViewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ReviewCreateViewModel review, string returnUrl = null)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(review);

                var reviewExistente = await _context.Reviews
                    .FirstOrDefaultAsync(r => r.Id == review.Id);

                if (reviewExistente == null)
                    return NotFound();

                var user = await _userManager.GetUserAsync(User);
                var esAdmin = await _userManager.IsInRoleAsync(user!, "Admin");

                // 🔐 Validación correcta: dueño real o admin
                if (reviewExistente.UsuarioId != user!.Id && !esAdmin)
                    return Forbid();

                reviewExistente.Rating = review.Rating;
                reviewExistente.Comentario = review.Comentario;

                _context.Reviews.Update(reviewExistente);
                await _context.SaveChangesAsync();

                // Si hay returnUrl, redirigir allí (admin desde Details)
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                // Si es admin y hay PeliculaId, redirigir a Details
                if (esAdmin && review.PeliculaId > 0)
                {
                    return RedirectToAction("Details", "Home", new { id = review.PeliculaId });
                }

                // Por defecto, redirigir al Index de Reviews
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(review);
            }
        }




        // GET: ReviewController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReviewController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
