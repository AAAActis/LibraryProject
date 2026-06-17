//Responsabilidad unica: Representar un libro en el sistema de biblioteca.
using System;
using System.Collections.Generic;
using System.Linq;
using Libreria1.Application.Interfaces;

namespace Libreria1.Domain.Entities;
public class Libro : IEntidad<string>
{
    public string Id => Isbn; // Mapeo directo a la PK real
    public string Isbn { get; private set; }
    public string Titulo { get; private set; }
    public string Autor { get; private set; }
    public int AñoPublicacion { get; private set; }
    public int CantPaginas { get; private set; }
    public bool EstaDisponible { get; set; }


    public Libro(string isbn, string titulo, string autor, int añoPublicacion, int cantPaginas)
    {
         // El ID es el ISBN, que es único por definición
        Isbn = isbn;
        Titulo = titulo;
        Autor = autor;
        AñoPublicacion = añoPublicacion;
        CantPaginas = cantPaginas;
        EstaDisponible = true;
    }

    // Constructor de sobrecarga para mapeos básicos (ej. historial de préstamos)
public Libro(string isbn, string titulo, string autor)
{
    Isbn = isbn;
    Titulo = titulo;
    Autor = autor;
    // Valores por defecto para que no estalle la lógica de negociod
    AñoPublicacion = 0;
    CantPaginas = 0;
    EstaDisponible = false; 
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