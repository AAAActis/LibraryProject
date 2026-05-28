using System;
public interface IServicioMulta
{
    //Calcula la multa basada en la fecha de devolución y la fecha de préstamo.
    Multa CalcularMulta(Prestamo prestamo);
    
    //Aplica la multa al usuario correspondiente.
    List<Multa> ObtenerMultasPorUsuario(Usuario usuario);
}