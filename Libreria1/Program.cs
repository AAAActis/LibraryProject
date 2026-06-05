using Libreria1.Interfaces;
using Libreria1.Repositories;
using Libreria1.Services; // Ajustá los namespaces si difieren
using Libreria1.Application.Services;
using Libreria1.Domain.Entities;
using Libreria1.Application.Interfaces;
using Microsoft.OpenApi;
using Libreria1.Application.DTOs;
using Libreria1.API.Controllers;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();


//se habilita el middleware de manejo de excepciones personalizado para toda la aplicación
app.UseMiddleware<Libreria1.Presentation.Middleware.ExceptionHandlerMiddLeware>();


// 1. Habilitar controladores
builder.Services.AddControllers();

// 2. Inyección de Repositorios (Singleton: los datos viven mientras la API esté prendida)
builder.Services.AddSingleton<IRepositorio<Libro>, RepositorioEnMemoria<Libro>>();
builder.Services.AddSingleton<IRepositorio<Usuario>, RepositorioEnMemoria<Usuario>>();
builder.Services.AddSingleton<IRepositorio<Prestamo>, RepositorioEnMemoria<Prestamo>>();
builder.Services.AddSingleton<IRepositorio<Multa>, RepositorioMultas>();

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
var app = builder.Build();

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
