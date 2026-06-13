using System;
using System.Collections.Generic;
using System.Linq;
using Libreria1.Interfaces;
using Libreria1.Repositories;
using Libreria1.Domain.Entities;
using Libreria1.Application.Interfaces;

    // Este repositorio se encarga exclusivamente de gestionar las multas, utilizando la implementación en memoria para almacenamiento temporal.
    public class RepositorioMultas : RepositorioEnMemoria<Multa, Guid>
    {
        public IEnumerable<Multa> ObtenerMultasPorPrestamo(Guid prestamoId)
        {
            return ObtenerTodos().Where(m => m.PrestamoAsignado.Id == prestamoId);
        }

        public IEnumerable<Multa> ObtenerMultasVencidas()
        {
            return ObtenerTodos().Where(m => m.EstaVencida());
        }
    }
