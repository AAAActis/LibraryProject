using System;
using Libreria1.Services;
using Libreria1.Interfaces;
using Libreria1.Repositories;
public class Presenter : IPresenter
{
       public void MostrarMensaje(string m)
        {
            Console.WriteLine(m);
        }
    
        public void MostrarTitulo(string m)
        {
            m.ToArray();
            int ancho = m.Count();

            string tituloDecorado = new string('=', ancho + 4) + "\n";
            tituloDecorado += $"| {m} |\n";
            tituloDecorado += new string('=', ancho + 4);
            Console.WriteLine(tituloDecorado);
        } 
          
}