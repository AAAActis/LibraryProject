using System;
using Libreria1.Application;

namespace Libreria1.Application
{
    public class Presenter : IPresenter
    {
        public void MostrarMensaje(string m)
        {
            Console.WriteLine(m);
        }

        public void MostrarTitulo(string m)
        {
            int ancho = m.Length;
            string tituloDecorado = new string('=', ancho + 4) + "\n";
            tituloDecorado += $"| {m} |\n";
            tituloDecorado += new string('=', ancho + 4);
            Console.WriteLine(tituloDecorado);
        } 

        public void MostrarMenuPrincipal()
        {
            MostrarTitulo("Sistema de Gestión de Biblioteca");
            Console.WriteLine("Seleccione una opción:");
            Console.WriteLine("1. Tareas de Libro");
            Console.WriteLine("2. Tareas de Usuario");
            Console.WriteLine("3. Tareas de Préstamo");
            Console.WriteLine("0. Salir");
        }

        public void MostrarTituloLibro()
    {
        MostrarTitulo("Tareas de Libro");
        Console.WriteLine("1. Agregar libro");
        Console.WriteLine("2. Buscar libro por ISBN");
        Console.WriteLine("3. Listar todos los libros");
        Console.WriteLine("4. Eliminar libro");
        Console.WriteLine("0. Volver al menú principal");
    
    }

    public void MostrarTituloUsuario()
    {
        MostrarTitulo("Tareas de Usuario");
        Console.WriteLine("1. Registrar usuario");
        Console.WriteLine("2. Buscar usuario por ID");
        Console.WriteLine("0. Volver al menú principal");
    }
    public void MostrarTituloPrestamo()
    {
        MostrarTitulo("Tareas de Préstamo");
        Console.WriteLine("1. Prestar libro");
        Console.WriteLine("2. Devolver libro");
        Console.WriteLine("3. Listar préstamos activos");
    }

          
}
