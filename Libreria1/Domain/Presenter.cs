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
    }
}
