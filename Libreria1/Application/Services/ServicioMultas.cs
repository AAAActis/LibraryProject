using System;
using System.Linq;
using System.Collections.Generic;
using Libreria1.Interfaces;

public class ServicioMultas : IServicioMulta
{
    private readonly IRepositorio<Multa> repositorioMultas;

    public ServicioMultas(IRepositorio<Multa> repositorioMultas)
    {
        this.repositorioMultas = repositorioMultas;
    }

    public Multa CalcularMulta (Prestamo prestamo)
    {
        //verificar si el prestamo esta vencido antes de calcular multa
        if (prestamo == null)
        {
            throw new PrestamoNoEncontradoException();
        }
        // calcular multa
        var diasRetraso = (DateTime.Now - prestamo.FechaPrestamo).Days - 30;
        if (diasRetraso > 0)
        {
            var multa = new Multa(prestamo, diasRetraso);
            repositorioMultas.Agregar(multa);
            return multa;
        }
        return null; // no hay multa si no hay retraso
    }
    
    public List<Multa> ObtenerMultasPorUsuario(Usuario usuario)
    {
        return repositorioMultas.ObtenerTodos()
        .Where(m => m.PrestamoAsignado.UsuarioAsignado.Id == usuario.Id)
        .ToList();  
    }

}