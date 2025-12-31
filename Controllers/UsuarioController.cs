using app_movie_mvc.Models;
using app_movie_mvc.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using app_movie_mvc.Data;

namespace app_movie_mvc.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly ImagenStorage _imagenStorage;
        private readonly IEmailService _emailService;
        private readonly MovieDbContext _context;

        public UsuarioController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, ImagenStorage imagenStorage, IEmailService emailService, MovieDbContext context)
        {
            _signInManager = signInManager; // Sirve para manejar la autenticacion de usuarios, cookies, etc
            _userManager = userManager; // Sirve para manejar los usuarios, roles y claims
            _imagenStorage = imagenStorage;
            _emailService = emailService;
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel usuario)
        {
            if(ModelState.IsValid)
            {
                // Intentar iniciar sesion con el email y clave proporcionados
                var resultado = await _signInManager.PasswordSignInAsync(usuario.Email, usuario.Clave, usuario.Recordarme, lockoutOnFailure: false);
                
                if (resultado.Succeeded) // Si el inicio de sesion fue exitoso, redirigir a la pagina principal
                {
                    return RedirectToAction("Index", "Home");
                }
                else // Si hubo un error, mostrar mensaje en la vista
                {
                    ModelState.AddModelError(string.Empty, resultado.ToString());
                }
            }

            return View(usuario);
        }

        public IActionResult Registro()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Protege contra ataques CSRF, asegurando que el formulario provenga del mismo sitio.
        public async Task<IActionResult> Registro(RegistroViewModel usuario)
        {
            if(ModelState.IsValid)
            {
                var nuevoUsuario = new Usuario
                {
                    UserName = usuario.Email,
                    Email = usuario.Email,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    ImagenUrlPerfil = "/images/default-avatar.png",
                    FechaRegistro = DateTime.UtcNow // Establecer la fecha de registro al crear un nuevo usuario
                };
                var resultado = await _userManager.CreateAsync(nuevoUsuario, usuario.Clave);
                // Crear el usuario en la base de datos

                if (resultado.Succeeded) // Si la creacion del usuario fue exitosa, redirigir al login
                {
                    await _signInManager.SignInAsync(nuevoUsuario, isPersistent: false); // Iniciar sesion automaticamente despues del registro
                    await _emailService.SendAsync(nuevoUsuario.Email, "Bienvenido a App Movie", "<h1>Gracias por registrarte en App Movie!</h1><p>Esperamos que disfrutes de nuestra plataforma.</p>");
                    return RedirectToAction("Index", "Home"); // Redirigir a la pagina principal despues del registro
                }
                else // Si hubo errores, mostrarlos en la vista
                {
                    foreach(var error in resultado.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> MiPerfil()
        {
            var usuarioActual = await _userManager.GetUserAsync(User);

            var usuarioVM = new MiPerfilViewModel
            {
                Nombre = usuarioActual.Nombre,
                Apellido = usuarioActual.Apellido,
                Email = usuarioActual.Email,
                FechaNacimiento = usuarioActual.FechaNacimiento == DateTime.MinValue ? null : usuarioActual.FechaNacimiento,
                PhoneNumber = usuarioActual.PhoneNumber,
                ImagenUrlPerfil = usuarioActual.ImagenUrlPerfil
            };

            return View(usuarioVM);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MiPerfil(MiPerfilViewModel usuarioVM)
        {
            if (ModelState.IsValid)
            {
                var usuarioActual = await _userManager.GetUserAsync(User);

                try
                {
                    if (usuarioVM.ImagenPerfil is not null && usuarioVM.ImagenPerfil.Length > 0)
                    {
                        // opcional: borrar la anterior (si no es placeholder)
                        if (!string.IsNullOrWhiteSpace(usuarioActual.ImagenUrlPerfil) && 
                            !usuarioActual.ImagenUrlPerfil.Contains("default-avatar"))
                            await _imagenStorage.DeleteAsync(usuarioActual.ImagenUrlPerfil);

                        var nuevaRuta = await _imagenStorage.SaveAsync(usuarioActual.Id, usuarioVM.ImagenPerfil);
                        usuarioActual.ImagenUrlPerfil = nuevaRuta;
                        usuarioVM.ImagenUrlPerfil = nuevaRuta;
                    }
                    else
                    {
                        // Mantener la imagen actual si no se subió una nueva
                        usuarioVM.ImagenUrlPerfil = usuarioActual.ImagenUrlPerfil;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    usuarioVM.ImagenUrlPerfil = usuarioActual.ImagenUrlPerfil;
                    return View(usuarioVM);
                }

                usuarioActual.Nombre = usuarioVM.Nombre;
                usuarioActual.Apellido = usuarioVM.Apellido;
                usuarioActual.FechaNacimiento = usuarioVM.FechaNacimiento ?? DateTime.MinValue;
                usuarioActual.PhoneNumber = usuarioVM.PhoneNumber;

                var resultado = await _userManager.UpdateAsync(usuarioActual);

                if (resultado.Succeeded)
                {
                    ViewBag.Mensaje = "Perfil actualizado con éxito.";
                    return View(usuarioVM);
                }
                else
                {
                    foreach (var error in resultado.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(usuarioVM);
        }

        [Authorize]
        public async Task<IActionResult> Cuenta()
        {
            var usuarioActual = await _userManager.GetUserAsync(User);
            
            var usuario = await _context.Users
                .Include(u => u.ReviewsUsuario)
                .Include(u => u.PeliculasFavoritas)
                .FirstOrDefaultAsync(u => u.Id == usuarioActual.Id);
            
            var cuentaVM = new AdministrarCuentaViewModel
            {
                Email = usuarioActual.Email,
                Nombre = usuarioActual.Nombre,
                Apellido = usuarioActual.Apellido,
                FechaRegistro = usuario?.FechaRegistro ?? DateTime.UtcNow,
                EstaActiva = !usuarioActual.LockoutEnabled || usuarioActual.LockoutEnd == null || usuarioActual.LockoutEnd < DateTime.UtcNow,
                TotalReviews = usuario?.ReviewsUsuario?.Count ?? 0,
                TotalFavoritos = usuario?.PeliculasFavoritas?.Count ?? 0
            };

            return View(cuentaVM);
        }

        [Authorize]
        public IActionResult CambiarContraseña()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarContraseña(CambiarContraseñaViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuarioActual = await _userManager.GetUserAsync(User);
            if (usuarioActual == null)
                return RedirectToAction("Login");

            var resultado = await _userManager.ChangePasswordAsync(usuarioActual, model.ContraseñaActual, model.ContraseñaNueva);

            if (resultado.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(usuarioActual);
                ViewBag.Mensaje = "Contraseña cambiada con éxito.";
                return View();
            }
            else
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SuspenderCuenta()
        {
            var usuarioActual = await _userManager.GetUserAsync(User);
            if (usuarioActual == null)
                return RedirectToAction("Login");

            await _userManager.SetLockoutEnabledAsync(usuarioActual, true);
            await _userManager.SetLockoutEndDateAsync(usuarioActual, DateTimeOffset.MaxValue);

            await _signInManager.SignOutAsync();

            TempData["Mensaje"] = "Tu cuenta ha sido suspendida exitosamente. No podrás iniciar sesión nuevamente sin contactar al equipo de soporte.";
            return RedirectToAction("Index", "Home");
        }
    }
}
