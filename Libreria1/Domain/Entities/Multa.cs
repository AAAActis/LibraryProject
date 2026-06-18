using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Libreria1.Application.Interfaces;

namespace Libreria1.Domain.Entities;
public class Multa : IEntidad<Guid>
{
    /// Responsabilidad única: Representa una penalización económica por la devolución tardía de un préstamo y calcula su costo.
    
    [Key]
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid PrestamoId { get; private set; }
    [ForeignKey("PrestamoId")]
    public Prestamo PrestamoAsignado { get; private set; } 
    public int DiasRetraso { get; private set; }
    public decimal MontoMulta { get; private set; }
    public DateTime FechaGenerada { get; private set; }


    private const decimal TarifaDiaria = 50.0m;

    protected Multa() { } // Constructor protegido para EF Core

    public Multa(Prestamo prestamoId, int diasRetraso)
    {
        PrestamoAsignado = prestamoId;
        DiasRetraso = diasRetraso;
        FechaGenerada = DateTime.Now;
        // Calcula el monto de la multa basado en los días de retraso y la tarifa diaria.
        MontoMulta = CalcularMontoMulta(diasRetraso);
    }

    private decimal CalcularMontoMulta(int diasRetraso)
    {
        return diasRetraso * TarifaDiaria;
    }

    public bool EstaVencida()
    {
        return DateTime.Now > FechaGenerada.AddDays(30); // Ejemplo de que la multa vence después de 30 días
    }

    public void ObtenerDetallesMulta()
    {
        Console.WriteLine($"Multa generada para el préstamo del libro '{PrestamoAsignado.LibroPrestado.Titulo}' por el usuario '{PrestamoAsignado.UsuarioAsignado.Nombre}'.");
        Console.WriteLine($"Dias de retraso: {DiasRetraso}, Monto de la multa: {MontoMulta:C}, Fecha de generación: {FechaGenerada}");
    }

}