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

    public Multa CalcularMulta (Prestamo prestamo)
    {
        //verificar si el prestamo esta vencido antes de calcular multa
        if (prestamo == null)
        {
            throw new PrestamoNoEncontradoException("Prestamo no encontrado");  
        }
        // calcular multa
        var diasRetraso = (DateTime.Now - prestamo.FechaPrestamo).Days - 30;
        if (diasRetraso > 0)
        {
            var multa = new Multa(prestamo, diasRetraso);
            _repositorioMultas.Agregar(multa);
            return multa;
        }
        return null; // no hay multa si no hay retraso
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