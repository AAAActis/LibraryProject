using System;
using System.Collections.Generic;
using System.Linq;
using Libreria1.Interfaces;
using Libreria1.Repositories;

public class RepositorioMultas : RepositorioEnMemoria<Multa>
{
    // Este repositorio se encarga exclusivamente de gestionar las multas, utilizando la implementación en memoria para almacenamiento temporal.
    public IEnumerable<Multa> ObtenerMultasPorPrestamo(Guid prestamoId)
    {
        return ObtenerTodos().Where(m => m.PrestamoAsignado.Id == prestamoId);
    }

    public IEnumerable<Multa> ObtenerMultasVencidas()
    {
        // Reutilizamos el ObtenerTodos() de la clase base y filtramos
        return ObtenerTodos().Where(m => m.EstaVencida());
    }
}  