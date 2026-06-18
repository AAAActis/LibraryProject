using System;
using System.Linq;
using System.Collections.Generic;
using Libreria1.Interfaces;
using Libreria1.Domain.Entities;
using Libreria1.Application.Interfaces;

public class ServicioMultas : IServicioMulta
{
    private readonly IRepositorio<Multa, Guid> _repositorioMultas;

public ServicioMultas(IRepositorio<Multa, Guid> repositorioMultas)
    {
        this._repositorioMultas = repositorioMultas;
    }

    public Multa? CalcularMulta (Prestamo prestamo)
    {
       if (prestamo == null) throw new PrestamoNoEncontradoException("Préstamo no encontrado");
    
    if (prestamo.FechaDevolucion.HasValue && DateTime.Now > prestamo.FechaDevolucion.Value)
    {
        var diasRetraso = (DateTime.Now - prestamo.FechaDevolucion.Value).Days;
        if (diasRetraso > 0)
        {
            // Respeta tu diseño: le pasás el objeto entero
            var multa = new Multa(prestamo, diasRetraso);
            
            _repositorioMultas.Agregar(multa);
            return multa;
        }
    }
    return null;
}
    
    public List<Multa> ObtenerMultasPorUsuario(int nroSocio)
    {
        return _repositorioMultas.ObtenerTodos()
        .Where(m => m.PrestamoAsignado.UsuarioAsignado.NroSocio == nroSocio)
        .ToList();  
    }

    public Multa ObtenerMultaPorPrestamo(Guid prestamoId)
    {
        return _repositorioMultas.ObtenerTodos()
        .FirstOrDefault(m => m.PrestamoAsignado.Id == prestamoId);
    }

}