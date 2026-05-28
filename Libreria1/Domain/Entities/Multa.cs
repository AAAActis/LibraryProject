public class Multa
{
    /// Responsabilidad única: Representa una penalización económica por la devolución tardía de un préstamo y calcula su costo.
    private Prestamo _prestamoId {get;}
    private int _diasRetraso {get;}
    private decimal _montoMulta {get; set;}
    private DateTime _fechaGenerada {get; set;}
    private const decimal _tarifaDiaria = 50.0m; // Ejemplo de tarifa diaria para la multa

    public Prestamo PrestamoId => _prestamoId;
    public int DiasRetraso => _diasRetraso;
    public decimal MontoMulta => _montoMulta;
    public DateTime FechaGenerada => _fechaGenerada;

    public Multa(Prestamo prestamoId, int diasRetraso)
    {
        _prestamoId = prestamoId;
        _diasRetraso = diasRetraso;
        _fechaGenerada = DateTime.Now;
        // Calcula el monto de la multa basado en los días de retraso y la tarifa diaria.
        _montoMulta = CalcularMontoMulta(diasRetraso);
    }

    private decimal CalcularMontoMulta(int diasRetraso)
    {
        return diasRetraso * _tarifaDiaria;
    }

    public bool EstaVencida()
    {
        return DateTime.Now > _fechaGenerada.AddDays(30); // Ejemplo de que la multa vence después de 30 días
    }

    public void ObtenerDetallesMulta()
    {
        Console.WriteLine($"Multa generada para el préstamo del libro '{_prestamoId.titulo}' por el usuario '{_prestamoId.usuarioAsignado.nombre}'.");
        Console.WriteLine($"Dias de retraso: {_diasRetraso}, Monto de la multa: {_montoMulta:C}, Fecha de generación: {_fechaGenerada}");
    }

}