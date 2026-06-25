using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;
using Libreria1.Application.Services; 
using Libreria1.Domain.Entities;
using Libreria1.Domain.Exceptions;
using Libreria1.Interfaces;
using Libreria1.Application.Interfaces;

namespace Libreria1.Tests;

public class ServicioPrestamosTests
{
    private readonly Mock<ICatalogo<Libro>> _mockCatalogo;
    private readonly Mock<IUsuarios> _mockUsuarios;
    private readonly Mock<IRepositorio<Prestamo, Guid>> _mockPrestamoRepo;
    private readonly Mock<IServicioMulta> _mockServicioMultas;
    private readonly ServicioPrestamo _servicioPrestamo;

    public ServicioPrestamosTests()
    {
        _mockCatalogo = new Mock<ICatalogo<Libro>>();
        _mockUsuarios = new Mock<IUsuarios>();
        _mockPrestamoRepo = new Mock<IRepositorio<Prestamo, Guid>>();
        _mockServicioMultas = new Mock<IServicioMulta>();

        _servicioPrestamo = new ServicioPrestamo(
            _mockCatalogo.Object,
            _mockUsuarios.Object,
            _mockPrestamoRepo.Object,
            _mockServicioMultas.Object
        );
    }

    [Fact]
    public void PrestarLibro_ConLibroDisponible_LlamaAgregarUnaVez()
    {
        // AAA: ARRANGE
        var nroSocio = 1001;
        var isbn = "123-456";

        var libroMock = new Libro(isbn, "Cien Años", "Gabo", 1967, 400); 
        var usuarioMock = new Usuario("Lauti", "Test", "lauti@test.com");

        _mockUsuarios.Setup(u => u.BuscarPorNumeroSocio(It.IsAny<int>())).Returns(usuarioMock);
        _mockCatalogo.Setup(c => c.BuscarLibroPorIsbn(It.IsAny<string>())).Returns(libroMock);
        
        //Simulamos que la base de datos de préstamos está vacía para que pase la validación
        _mockPrestamoRepo.Setup(r => r.ObtenerTodos()).Returns(new List<Prestamo>());

        // AAA: ACT
        _servicioPrestamo.PrestarLibro(nroSocio, isbn);

        // AAA: ASSERT
        _mockPrestamoRepo.Verify(r => r.Agregar(It.IsAny<Prestamo>()), Times.Once);
    }

    [Fact]
    public void PrestarLibro_ConLibroNoDisponible_LanzaLibroNoDisponibleException()
    {
        // AAA: ARRANGE
        var nroSocio = 1001;
        var isbn = "123-456";

        var libroMock = new Libro(isbn, "Cien Años", "Gabo", 1967, 400);
        libroMock.MarcarPrestado(); // Forzamos a estar No Disponible

        var usuarioMock = new Usuario("Lauti", "Test", "lauti@test.com");

        _mockUsuarios.Setup(u => u.BuscarPorNumeroSocio(It.IsAny<int>())).Returns(usuarioMock);
        _mockCatalogo.Setup(c => c.BuscarLibroPorIsbn(It.IsAny<string>())).Returns(libroMock);
        _mockPrestamoRepo.Setup(r => r.ObtenerTodos()).Returns(new List<Prestamo>());

        // AAA: ACT & ASSERT
        Assert.Throws<LibroNoDisponibleException>(() => 
            _servicioPrestamo.PrestarLibro(nroSocio, isbn)
        );
    }

    [Fact]
    public void PrestarLibro_ConLimiteAlcanzado_LanzaLimitePrestamosAlcanzadoException()
    {
        // AAA: ARRANGE
        var isbn = "123-456";

        //El libro que el usuario INTENTA pedir (este está nuevo y DISPONIBLE)
        var libroParaPrestar = new Libro(isbn, "Cien Años", "Gabo", 1967, 400);
        var usuarioMock = new Usuario("Lauti", "Test", "lauti@test.com");

        var nroSocio = usuarioMock.NroSocio;

        _mockUsuarios.Setup(u => u.BuscarPorNumeroSocio(It.IsAny<int>())).Returns(usuarioMock);
        _mockCatalogo.Setup(c => c.BuscarLibroPorIsbn(It.IsAny<string>())).Returns(libroParaPrestar);

        // 2. Creamos OTROS libros ficticios para rellenar el historial del usuario
        var libroViejo1 = new Libro("111", "Libro 1", "Autor", 2000, 100);
        var libroViejo2 = new Libro("222", "Libro 2", "Autor", 2000, 100);
        var libroViejo3 = new Libro("333", "Libro 3", "Autor", 2000, 100);

        // 3. Simulamos que el usuario ya tiene 3 préstamos activos con ESOS otros libros
        var prestamosActivos = new List<Prestamo>
        {
            new Prestamo(libroViejo1, usuarioMock), 
            new Prestamo(libroViejo2, usuarioMock), 
            new Prestamo(libroViejo3, usuarioMock)  
        };
        
        _mockPrestamoRepo.Setup(r => r.ObtenerTodos()).Returns(prestamosActivos);

        // AAA: ACT & ASSERT
        // Como el libro nuevo sí está disponible, la lógica avanzará y chocará contra el límite de 3.
        Assert.Throws<LimitePrestamosAlcanzadoException>(() => 
            _servicioPrestamo.PrestarLibro(nroSocio, isbn)
        );
    }
}