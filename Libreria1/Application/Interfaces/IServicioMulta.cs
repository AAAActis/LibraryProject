using System;
public interface IServicioMulta
{
    //Calcula la multa basada en la fecha de devolución y la fecha de préstamo.
    decimal CalcularMulta(DateTime fechaDevolucion, DateTime fechaPrestamo);
    
    //Aplica la multa al usuario correspondiente.
    void AplicarMulta(Guid userId, decimal montoMulta);
}