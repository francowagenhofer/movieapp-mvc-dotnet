using app_movie_mvc.Data;
using app_movie_mvc.Models;
using app_movie_mvc.Service;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Incluir dbContext
builder.Services.AddDbContext<MovieDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MovieDbContext")));

// Configuracion Identity 
builder.Services.AddIdentityCore<Usuario>(o =>
{
    o.SignIn.RequireConfirmedAccount = false;
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequiredLength = 4;
    o.Password.RequireUppercase = false;
})
    .AddRoles<IdentityRole>() // Sirve para manejar los roles de los usuarios
    .AddEntityFrameworkStores<MovieDbContext>() // Sirve para manejar el almacenamiento de los usuarios en la base de datos
    .AddSignInManager(); // Sirve para manejar la autenticacion de usuarios

builder.Services.AddAuthentication(o =>
{
    o.DefaultScheme = IdentityConstants.ApplicationScheme; // Sirve para manejar la autenticacion de usuarios con cookies
})
.AddIdentityCookies();

builder.Services.ConfigureApplicationCookie(o =>
{
    o.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Tiempo de expiracion de la cookie
    o.SlidingExpiration = true; // Renovar la cookie si el usuario esta activo
    o.LoginPath = "/Usuario/Login"; // Redirigir a esta ruta si el usuario no esta autenticado
    o.AccessDeniedPath = "/Usuario/AccessDenied"; // Redirigir a esta ruta si el usuario no tiene permisos
});

//Servicios de archivos
builder.Services.AddScoped<ImagenStorage>();
builder.Services.Configure<FormOptions>(o => { o.MultipartBodyLengthLimit = 2 * 1024 * 1024; }); // Sirve para limitar el tamaño de los archivos subidos a 2 MB

//Servicios de email
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddScoped<IEmailService, SmtpEmailService>();

//Servicio LLM - no esta habilitado.
//builder.Services.AddScoped<LlmService>();

var app = builder.Build();

// Invocacion de la ejecucion de la siembra de datos en la base de datos
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<MovieDbContext>();
        var userManager = services.GetRequiredService<UserManager<Usuario>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await DbSeeder.Seed(context, userManager, roleManager);
    }
    catch (Exception ex)
    {
        // Log errors or handle them as needed
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
