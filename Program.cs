using app_movie_mvc.Data;
using app_movie_mvc.Models;
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
    o.SignIn.RequireConfirmedAccount = true;
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequiredLength = 4;
    o.Password.RequireUppercase = false;
})
    .AddRoles<IdentityRole>() // Sirve para manejar los roles de los usuarios
    .AddEntityFrameworkStores<MovieDbContext>() // Sirve para manejar el almacenamiento de los usuarios en la base de datos
    .AddSignInManager(); // Sirve para manejar la autenticacion de usuarios

builder.Services.AddAuthentication(o =>
{
    o.DefaultSignInScheme = IdentityConstants.ApplicationScheme; // Sirve para manejar la autenticacion externa (google, facebook, etc)
})
.AddIdentityCookies();

builder.Services.ConfigureApplicationCookie(o =>
{
    o.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Tiempo de expiracion de la cookie
    o.SlidingExpiration = true; // Renovar la cookie si el usuario esta activo
    o.LoginPath = "/Usuario/Login"; // Redirigir a esta ruta si el usuario no esta autenticado
    o.AccessDeniedPath = "/Usuario/AccessDenied"; // Redirigir a esta ruta si el usuario no tiene permisos
});


var app = builder.Build();

// Invocacion de la ejecucion de la siembra de datos en la base de datos
using (var scope = app.Services.CreateScope())
{
    try
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<MovieDbContext>();
        DbSeeder.Seed(context);
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
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
