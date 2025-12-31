using app_movie_mvc.Data;
using app_movie_mvc.Models;
using app_movie_mvc.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace app_movie_mvc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUsuariosController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly MovieDbContext _context;
        private readonly IEmailService _emailService;

        public AdminUsuariosController(UserManager<Usuario> userManager, MovieDbContext context, IEmailService emailService)
        {
            _userManager = userManager;
            _context = context;
            _emailService = emailService;
        }

        // GET: AdminUsuarios
        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.Users
                .Include(u => u.ReviewsUsuario)
                .Include(u => u.PeliculasFavoritas)
                .ToListAsync();

            var usuariosVM = new List<UsuarioAdminViewModel>();

            foreach (var usuario in usuarios)
            {
                var roles = await _userManager.GetRolesAsync(usuario);
                var estaActivo = !usuario.LockoutEnabled || usuario.LockoutEnd == null || usuario.LockoutEnd < DateTimeOffset.UtcNow;

                usuariosVM.Add(new UsuarioAdminViewModel
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    Email = usuario.Email,
                    FechaRegistro = usuario.FechaRegistro,
                    ImagenUrlPerfil = usuario.ImagenUrlPerfil,
                    EstaActivo = estaActivo,
                    TotalReviews = usuario.ReviewsUsuario?.Count ?? 0,
                    TotalFavoritos = usuario.PeliculasFavoritas?.Count ?? 0,
                    Roles = string.Join(", ", roles)
                });
            }

            return View(usuariosVM);
        }

        // GET: AdminUsuarios/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
                return NotFound();

            var usuario = await _context.Users
                .Include(u => u.ReviewsUsuario)
                    .ThenInclude(r => r.Pelicula)
                .Include(u => u.PeliculasFavoritas)
                    .ThenInclude(f => f.Pelicula)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(usuario);
            var estaActivo = !usuario.LockoutEnabled || usuario.LockoutEnd == null || usuario.LockoutEnd < DateTimeOffset.UtcNow;

            var usuarioVM = new UsuarioDetalleViewModel
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                PhoneNumber = usuario.PhoneNumber,
                FechaNacimiento = usuario.FechaNacimiento,
                FechaRegistro = usuario.FechaRegistro,
                ImagenUrlPerfil = usuario.ImagenUrlPerfil,
                EstaActivo = estaActivo,
                Roles = roles.ToList(),
                Reviews = usuario.ReviewsUsuario?.OrderByDescending(r => r.FechaReview).ToList() ?? new List<Review>(),
                PeliculasFavoritas = usuario.PeliculasFavoritas?.Select(f => f.Pelicula).ToList() ?? new List<Pelicula>()
            };

            return View(usuarioVM);
        }

        // POST: AdminUsuarios/ToggleSuspension
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleSuspension(string id)
        {
            // FUNCIONALIDAD DESHABILITADA - No implementada
            //var usuario = await _userManager.FindByIdAsync(id);
            //if (usuario == null)
            //    return NotFound();

            //var estaActivo = !usuario.LockoutEnabled || usuario.LockoutEnd == null || usuario.LockoutEnd < DateTimeOffset.UtcNow;

            //if (estaActivo)
            //{
            //    // Suspender
            //    await _userManager.SetLockoutEnabledAsync(usuario, true);
            //    await _userManager.SetLockoutEndDateAsync(usuario, DateTimeOffset.MaxValue);
            //    TempData["Mensaje"] = $"Usuario {usuario.Email} suspendido correctamente.";
            //}
            //else
            //{
            //    // Activar
            //    await _userManager.SetLockoutEnabledAsync(usuario, false);
            //    await _userManager.SetLockoutEndDateAsync(usuario, null);
            //    TempData["Mensaje"] = $"Usuario {usuario.Email} activado correctamente.";
            //}

            TempData["Mensaje"] = "Funcionalidad deshabilitada temporalmente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: AdminUsuarios/Contactar/5
        public async Task<IActionResult> Contactar(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
                return NotFound();

            var contactoVM = new ContactarUsuarioViewModel
            {
                UsuarioId = usuario.Id,
                UsuarioEmail = usuario.Email,
                UsuarioNombre = $"{usuario.Nombre} {usuario.Apellido}"
            };

            return View(contactoVM);
        }

        // POST: AdminUsuarios/Contactar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contactar(ContactarUsuarioViewModel model)
        {
            // FUNCIONALIDAD DESHABILITADA - No implementada
            //if (!ModelState.IsValid)
            //    return View(model);

            //try
            //{
            //    await _emailService.SendAsync(model.UsuarioEmail, model.Asunto, model.Mensaje);
            //    TempData["Mensaje"] = $"Email enviado correctamente a {model.UsuarioEmail}";
            //    return RedirectToAction(nameof(Details), new { id = model.UsuarioId });
            //}
            //catch (Exception ex)
            //{
            //    ModelState.AddModelError(string.Empty, $"Error al enviar email: {ex.Message}");
            //    return View(model);
            //}

            TempData["Mensaje"] = "Funcionalidad de envío de email deshabilitada temporalmente.";
            return RedirectToAction(nameof(Details), new { id = model.UsuarioId });
        }
    }
}   