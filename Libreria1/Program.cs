using System;
using Libreria1.Interfaces;
using Libreria1.Repositories;
using Libreria1.Services; // Ajustá los namespaces si difieren
using Libreria1.Application.Services;
using Libreria1.Domain.Entities;
using Libreria1.Application.Interfaces;
using Microsoft.OpenApi;
using Libreria1.Application.DTOs;
using Libreria1.API.Controllers;
using Npgsql;
using Libreria1;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var cadena = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Falta la cadena de conexión en appsettings.json");

// 1. Habilitar controladores
builder.Services.AddControllers();

// Agregamos la cadena de conexión a tu Docker
//var connectionString = "Host=localhost;Port=5432;Database=libreria;Username=postgres;Password=1234;";
// 2. Inyección de Repositorios (Singleton: los datos viven mientras la API esté prendida)
builder.Services.AddScoped<IRepositorio<Libro, string>>(sp => new RepositorioLibrosPostgres(cadena));
builder.Services.AddScoped<IRepositorio<Usuario, Guid>>(sp => new RepositorioUsuariosPostgres(cadena));
builder.Services.AddScoped<IRepositorio<Prestamo, Guid>>(sp => new RepositorioPrestamosPostgres(cadena));
builder.Services.AddSingleton<IRepositorio<Multa, Guid>, RepositorioMultas>(); // Multas se guardan en memoria porque son temporales y no críticas

// 3. Inyección de Servicios (Scoped: nacen y mueren con cada petición HTTP)
builder.Services.AddScoped<ICatalogo<Libro>, ServicioCatalogo>();
builder.Services.AddScoped<IUsuarios, ServicioUsuario>();
builder.Services.AddScoped<IServicioMulta, ServicioMultas>();
builder.Services.AddScoped<ServicioPrestamo>();


builder.Services.AddSwaggerGen(opciones =>
{
    opciones.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Librería API",
        Description = "API para gestionar una librería, incluyendo libros, usuarios, préstamos y multas."
        
        
    });


    // 1. Calculamos el nombre del archivo XML que se generó
    var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";

    // 2. Armamos la ruta completa de dónde está guardado en tu PC
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);

    // 3. Le decimos a Swagger que lo incluya en la página web
    opciones.IncludeXmlComments(xmlPath);

}); // Agrega Swagger para documentación de la API

builder.Services.AddDbContext<LibreriaContext>(options =>
{
    //Usa PostgreSQL y lee la cadena de conexión de appsettings.json
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
           //Activa el log de SQL en la consola para ver qué hace EF Core por detrás
           .LogTo(Console.WriteLine, LogLevel.Information);
});

var app = builder.Build();


/*INICIO DEL TEST DE CONEXIÓN A POSTGRESQL(Se comenta porque se realizo para una prueba puntual y no es necesario que se ejecute cada vez que se inicia la API)
var connectionString = "Host=localhost;Port=5432;Database=libreria;Username=postgres;Password=1234;";

using (var connection = new NpgsqlConnection(connectionString))
{
    try
    {
        Console.WriteLine("Intentando conectar a la base de datos...");
        connection.Open(); // Si las credenciales o el puerto estan mal, esto lanza una excepción
        Console.WriteLine("La API se conectó a PostgreSQL correctamente.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERROR DE CONEXIÓN: {ex.Message}");
    }
}
*/


//se habilita el middleware de manejo de excepciones personalizado para toda la aplicación
//app.UseMiddleware<Libreria1.Presentation.Middleware.ExceptionHandlerMiddLeware>();

app.UseSwagger();
app.UseSwaggerUI();

// 4. Conectar las rutas URL con los controladores
app.MapControllers();

app.Run();



/*EX Consola
static void Main(string[] args)
{
    IRepositorio<Libro> repositorio = new RepositorioEnMemoria<Libro>();
    IRepositorio<Usuario> repositorioUsuario = new RepositorioEnMemoria<Usuario>();
    IRepositorio<Prestamo> repositorioPrestamos = new RepositorioEnMemoria<Prestamo>();
    IRepositorio<Multa> repositorioMultas = new RepositorioEnMemoria<Multa>();
    IServicioMulta servicioMultas = new ServicioMultas(repositorioMultas);

    var servicioUsuario = new ServicioUsuario(repositorioUsuario);
    var servicioCatalogo = new ServicioCatalogo(repositorio);
    var servicioPrestamo = new ServicioPrestamo(servicioCatalogo, servicioUsuario, repositorioPrestamos, servicioMultas);


    var libroMenu = new LibroMenu(servicioCatalogo);
    var usuarioMenu = new UsuarioMenu(servicioUsuario);
    var prestamoMenu = new PrestamoMenu(servicioPrestamo, servicioCatalogo, servicioUsuario);
    var menuPrincipal = new MenuPrincipal(libroMenu, usuarioMenu, prestamoMenu);

    menuPrincipal.Ejecutar();
    */
