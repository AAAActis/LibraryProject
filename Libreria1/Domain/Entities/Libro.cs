//Responsabilidad unica: Representar un libro en el sistema de biblioteca.
using System;
using System.Collections.Generic;
using System.Linq;
using Libreria1.Application.Interfaces;

namespace Libreria1.Domain.Entities;
public class Libro : IEntidad<Guid>
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Isbn {get; private set;}
    public string Titulo {get; private set;}
    public string Autor {get; private set;}
    public bool EstaDisponible {get; set;}


    public Libro(string isbn, string titulo, string autor)
    {
        Id = Guid.NewGuid();
        Isbn = isbn;
        Titulo = titulo;
        Autor = autor;
        EstaDisponible = true;
    }

    public void MarcarPrestado()
    {
        EstaDisponible = false;
    }

    public void MarcarDevuelto()
    {
        EstaDisponible = true;
    }
}