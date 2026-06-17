using System;  
using System.Collections.Generic;
using Libreria1.Domain.Entities;
namespace Libreria1.Application.Interfaces;
public interface IServicioMulta
{
    //Calcula la multa basada en la fecha de devolución y la fecha de préstamo.
    Multa CalcularMulta(Prestamo prestamo);
    //Aplica la multa al usuario correspondiente.
    List<Multa> ObtenerMultasPorUsuario(int nroSocio);

    Multa ObtenerMultaPorPrestamo(Guid prestamoId);
}