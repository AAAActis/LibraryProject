using System.Linq;

namespace Libreria1.Domain.Menus
{
    public abstract class MenuBase : IMenu
    {

        public abstract void Ejecutar();

        protected void MostrarTitulo(string texto)
        {
            Console.WriteLine(texto);
        }

        protected void MostrarMensaje(string texto)
        {
            Console.WriteLine(texto);
        }

        protected void EsperarContinuar()
        {
            Console.WriteLine();
            Console.WriteLine("Presione Enter para continuar...");
            Console.ReadLine();
        }

        protected bool ValidarIsbn13(string? isbn, out string isbnVal)
        {
            isbnVal = (isbn ?? string.Empty).Trim();
            return isbnVal.Length == 13 && isbnVal.All(char.IsDigit);
        }
    }
}
