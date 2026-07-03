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
using System.Data.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>(); 
var cadena = builder.Configuration.GetConnectionString("Libreria") ?? throw new InvalidOperationException("Falta la cadena de conexión en appsettings.json");

// 1. Habilitar controladores
builder.Services.AddControllers();

// Agregamos la cadena de conexión a tu Docker
//var connectionString = "Host=localhost;Port=5432;Database=libreria;Username=postgres;Password=1234;";
// 2. Inyección de Repositorios (Singleton: los datos viven mientras la API esté prendida)
builder.Services.AddScoped<IRepositorio<Libro, string>, RepositorioLibroEF>();
builder.Services.AddScoped<IRepositorio<Usuario, Guid>, RepositorioUsuariosEF>();
builder.Services.AddScoped<IRepositorio<Multa, Guid>, RepositorioMultasEF>(); // Multas se guardan en memoria porque son temporales y no críticas
builder.Services.AddScoped<IRepositorio<Prestamo, Guid>, RepositorioPrestamosEF>();

// 3. Inyección de Servicios (Scoped: nacen y mueren con cada petición HTTP)
builder.Services.AddScoped<ICatalogo<Libro>, ServicioCatalogo>();
builder.Services.AddScoped<IUsuarios, ServicioUsuario>();
builder.Services.AddScoped<IServicioMulta, ServicioMultas>();
builder.Services.AddScoped<ServicioPrestamo>();
builder.Services.AddScoped<IServicioToken, ServicioToken>();


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



builder.Services.AddCors(options =>
{
    options.AddPolicy("DesarrolloLocal", policy =>
    {
        policy.WithOrigins("http://localhost:5000", "https://localhost:7001")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddHttpsRedirection(options =>
{
    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
    options.HttpsPort = 7001; // Forzamos el salto a tu puerto seguro
});

builder.Services.AddDbContext<LibreriaContext>(options =>
{
    //Usa PostgreSQL y lee la cadena de conexión de appsettings.json
    options.UseNpgsql(builder.Configuration.GetConnectionString("Libreria"))
           //Activa el log de SQL en la consola para ver qué hace EF Core por detrás
           .LogTo(Console.WriteLine, LogLevel.Information);
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            // Aquí lee la clave secreta desde tu appsettings.json
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt")["Key"] ?? throw new InvalidOperationException("Falta la clave secreta"))),
            ValidateIssuer = false, // Cambiar a true si defines un Issuer
            ValidateAudience = false, // Cambiar a true si defines un Audience
            ClockSkew = TimeSpan.Zero // Evita el margen de gracia de 5 min al vencer el token
        };
    });


var app = builder.Build();

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseCors("DesarrolloLocal");

app.UseAuthentication();
app.UseAuthorization();

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
