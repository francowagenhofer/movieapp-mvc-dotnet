using app_movie_mvc.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace app_movie_mvc.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        //private readonly ImagenStorage _imagenStorage;
        //private readonly IEmailService _emailService;
 
        //public UsuarioController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, ImagenStorage imagenStorage, IEmailService emailService)
        public UsuarioController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager) // 
        {
            _signInManager = signInManager; // Sirve para manejar la autenticacion de usuarios, cookies, etc
            _userManager = userManager; // Sirve para manejar los usuarios, roles y claims
            //_imagenStorage = imagenStorage;
            //_emailService = emailService;
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
                    ImagenUrlPerfil = "default-profile.png" // Asignar una imagen de perfil por defecto
                };
                var resultado = await _userManager.CreateAsync(nuevoUsuario, usuario.Clave);
                // Crear el usuario en la base de datos

                if (resultado.Succeeded) // Si la creacion del usuario fue exitosa, redirigir al login
                {
                    await _signInManager.SignInAsync(nuevoUsuario, isPersistent: false); // Iniciar sesion automaticamente despues del registro
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

        public IActionResult Logout()
        {
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
 
    }
}
