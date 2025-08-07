using EMSI_Corporation.Data;
using EMSI_Corporation.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = LicenseType.Community;

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configurar conexión a PostgreSQL
var envVar = Environment.GetEnvironmentVariable("DATABASE_URL");
string connectionString;

if (!string.IsNullOrEmpty(envVar))
{
    try
    {
        // Manejar formato Railway (postgresql://)
        if (envVar.StartsWith("postgresql://"))
        {
            envVar = "postgres://" + envVar.Substring("postgresql://".Length);
        }

        var uri = new Uri(envVar);
        var host = uri.Host;
        var port = uri.Port;
        var database = uri.AbsolutePath.TrimStart('/');
        var user = uri.UserInfo.Split(':')[0];
        var password = uri.UserInfo.Split(':')[1];

        // Construir connection string para entorno Railway
        connectionString = $"Host={host};Port={port};Database={database};" +
                          $"Username={user};Password={password};" +
                          "SSL Mode=Require;" +
                          "Trust Server Certificate=true;" +
                          "Pooling=true;" +
                          "Timeout=30;";

        // Log para depuración (solo en desarrollo)
        if (builder.Environment.IsDevelopment())
        {
            Console.WriteLine($"Database config: Host={host}, Port={port}, DB={database}, User={user}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error parsing DATABASE_URL: {ex.Message}");
        throw;
    }
}
else
{
    connectionString = builder.Configuration.GetConnectionString("CadenaSQL");
}

builder.Services.AddDbContext<AppDBContext>(option =>
{
    option.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorCodesToAdd: null
        );
    });
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Acceso/Login";
        options.AccessDeniedPath = "/Acceso/Denegado";
    });

var app = builder.Build();

// Aplicar migraciones después de configurar la app
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDBContext>();
        var logger = services.GetRequiredService<ILogger<Program>>();

        // Obtener y mostrar detalles de conexión
        var dbUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
        logger.LogInformation($"DATABASE_URL: {dbUrl ?? "No encontrada"}");

        if (!string.IsNullOrEmpty(dbUrl))
        {
            try
            {
                // Parsear y mostrar detalles de conexión
                var uri = new Uri(dbUrl);
                logger.LogInformation($"Database host: {uri.Host}, port: {uri.Port}, database: {uri.AbsolutePath.TrimStart('/')}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error parsing DATABASE_URL");
            }
        }

        // Aplicar migraciones solo si es producción
        if (!app.Environment.IsDevelopment())
        {
            logger.LogInformation("Applying database migrations...");
            context.Database.Migrate();
            logger.LogInformation("Migrations applied successfully");
        }
        else
        {
            logger.LogInformation("Development environment - ensuring database exists");
            context.Database.EnsureCreated();
        }

        // Crear roles fijos si no existen
        var rolesFijos = new List<Rol>
        {
            new Rol { nomRol = "Administrador", Descripcion = "Acceso total" },
            new Rol { nomRol = "Supervisor", Descripcion = "Mismas funciones que administrador" },
            new Rol { nomRol = "Atencion cliente", Descripcion = "Acceso a ventas, proveedores, calendario, productos y clientes" },
            new Rol { nomRol = "Almacenero", Descripcion = "Acceso a operaciones, registro de mantenimiento y calendario" }
        };

        foreach (var rol in rolesFijos)
        {
            if (!context.Roles.Any(r => r.nomRol == rol.nomRol))
            {
                context.Roles.Add(rol);
            }
        }
        context.SaveChanges();

        // Crear solo el usuario admin si no hay usuarios
        if (!context.usuarios.Any())
        {
            var hasher = new PasswordHasher<Usuario>();

            var adminEmpleado = new Empleado
            {
                nomEmpleado = "Admin",
                apeEmpleado = "Sistema",
                dni = "12345678",
                gmail = "admin@correo.com",
                numCelular = "999999999"
            };

            context.empleados.Add(adminEmpleado);
            context.SaveChanges();

            var usuarioAdmin = new Usuario
            {
                usuario = "admin",
                password = hasher.HashPassword(null, "Admin123"),
                IdEmpleado = adminEmpleado.IdEmpleado
            };

            context.usuarios.Add(usuarioAdmin);
            context.SaveChanges();

            var rolAdmin = context.Roles.First(r => r.nomRol == "Administrador");

            context.UserRoles.Add(new User_Rol
            {
                IdUsuario = usuarioAdmin.IdUsuario,
                IdRol = rolAdmin.IdRol
            });

            context.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during database initialization");
        throw;
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Acceso}/{action=Login}/{id?}");

app.Run();
