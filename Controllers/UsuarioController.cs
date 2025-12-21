using app_movie_mvc.Models;
using app_movie_mvc.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace app_movie_mvc.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly ImagenStorage _imagenStorage;
        private readonly IEmailService _emailService;

        public UsuarioController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, ImagenStorage imagenStorage, IEmailService emailService)
        {
            _signInManager = signInManager; // Sirve para manejar la autenticacion de usuarios, cookies, etc
            _userManager = userManager; // Sirve para manejar los usuarios, roles y claims
            _imagenStorage = imagenStorage;
            _emailService = emailService;
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
                    ImagenUrlPerfil = "/images/default-avatar.png" // ruta de la carpeta images - root // Asignar una imagen de perfil por defecto
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
                        if (!string.IsNullOrWhiteSpace(usuarioActual.ImagenUrlPerfil))
                            await _imagenStorage.DeleteAsync(usuarioActual.ImagenUrlPerfil);

                        var nuevaRuta = await _imagenStorage.SaveAsync(usuarioActual.Id, usuarioVM.ImagenPerfil);
                        usuarioActual.ImagenUrlPerfil = nuevaRuta;
                        usuarioVM.ImagenUrlPerfil = nuevaRuta;
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return View(usuarioVM);
                }

                usuarioActual.Nombre = usuarioVM.Nombre;
                usuarioActual.Apellido = usuarioVM.Apellido;

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

    }
}
